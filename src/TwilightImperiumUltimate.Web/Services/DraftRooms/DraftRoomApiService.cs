using System.Globalization;
using System.Net;
using TwilightImperiumUltimate.Contracts.ApiContracts;
using TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DraftRooms;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Models.DraftRooms;
using TwilightImperiumUltimate.Web.Resources;
using TwilightImperiumUltimate.Web.Services.HttpClients;

namespace TwilightImperiumUltimate.Web.Services.DraftRooms;

/// <summary>
/// Implements the typed client workflow for draft rooms.
/// </summary>
public sealed class DraftRoomApiService(
    ITwilightImperiumApiHttpClient apiHttpClient,
    IGuestIdentityStore guestIdentityStore)
    : IDraftRoomApiService
{
    private const string ParticipantIdHeader = "X-Draft-Participant-Id";
    private const string ParticipantSecretHeader = "X-Draft-Participant-Secret";

    public async Task<DraftRoomApiResult<DraftResultDto>> GetResultAsync(string token, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);
        var (response, status) = await apiHttpClient.GetAsync<DraftResultDto>(
            $"{Paths.ApiPath_DraftRooms}/results/{Uri.EscapeDataString(token)}",
            cancellationToken: cancellationToken);
        return MapReadResult(response, status);
    }

    public async Task<DraftRoomApiResult<DraftResultDto>> GetRoomResultAsync(string code, CancellationToken cancellationToken = default)
    {
        var context = await this.GetRequestContextAsync(code, cancellationToken);
        var (response, status) = await apiHttpClient.GetAsync<DraftResultDto>(
            $"{FormatRoomPath(Paths.ApiPath_DraftRoomsByCode, context.Code)}/result",
            cancellationToken: cancellationToken,
            headers: context.Headers);
        return MapReadResult(response, status);
    }

    public async Task<DraftRoomApiResult<DraftResultHistoryDto>> GetMyResultsAsync(int page, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(page, 1);
        var (response, status) = await apiHttpClient.GetAsync<DraftResultHistoryDto>(
            $"{Paths.ApiPath_DraftRooms}/results/mine",
            $"?page={page.ToString(CultureInfo.InvariantCulture)}",
            cancellationToken);
        return MapReadResult(response, status);
    }

    public async Task<DraftRoomApiCommandResult> DeleteResultAsync(Guid id, string? code = null, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        var headers = code is null ? null : (await this.GetRequestContextAsync(code, cancellationToken)).Headers;
        var status = await apiHttpClient.SendAsync(HttpMethod.Delete, $"{Paths.ApiPath_DraftRooms}/results/{id:D}", headers, cancellationToken);
        return new DraftRoomApiCommandResult(MapStatusCode(status));
    }

    public async Task<DraftRoomApiResult<DraftRoomEntryDto>> CreateAsync(
        CreateDraftRoomFormModel model,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);

        var request = new CreateDraftRoomRequest(
            model.Name,
            model.Password,
            model.IsGuest ? model.DisplayName : null);
        var (response, statusCode) = await apiHttpClient
            .PostApiAsync<CreateDraftRoomRequest, DraftRoomEntryDto>(
                Paths.ApiPath_DraftRooms,
                request,
                cancellationToken)
            .ConfigureAwait(false);

        return MapResult(response, statusCode);
    }

    public async Task<DraftRoomApiResult<DraftRoomEntryDto>> JoinAsync(
        string code,
        string? password,
        string? displayName,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        var normalizedCode = code.Trim().ToUpperInvariant();
        var identity = password is null && displayName is null
            ? await guestIdentityStore
                .GetAsync(normalizedCode, cancellationToken)
                .ConfigureAwait(false)
            : null;
        IReadOnlyDictionary<string, string>? headers = identity is null
            ? null
            : new Dictionary<string, string>
            {
                [ParticipantIdHeader] = identity.ParticipantId.ToString(),
                [ParticipantSecretHeader] = identity.Secret,
            };

        var request = new JoinDraftRoomRequest(
            normalizedCode,
            identity is null ? password : null,
            identity is null ? displayName : null);
        var (response, statusCode) = await apiHttpClient
            .PostApiAsync<JoinDraftRoomRequest, DraftRoomEntryDto>(
                Paths.ApiPath_DraftRoomsJoin,
                request,
                cancellationToken,
                headers)
            .ConfigureAwait(false);

        return MapResult(response, statusCode);
    }

    public async Task<DraftRoomApiCommandResult> LinkAccountAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        var normalizedCode = code.Trim().ToUpperInvariant();
        var identity = await guestIdentityStore
            .GetAsync(normalizedCode, cancellationToken)
            .ConfigureAwait(false);
        if (identity is null)
        {
            return new DraftRoomApiCommandResult(DraftRoomApiResultKind.Failed);
        }

        var headers = new Dictionary<string, string>
        {
            [ParticipantIdHeader] = identity.ParticipantId.ToString(),
            [ParticipantSecretHeader] = identity.Secret,
        };
        var statusCode = await apiHttpClient
            .SendAsync(
                HttpMethod.Post,
                FormatRoomPath(Paths.ApiPath_DraftRoomsLinkAccount, normalizedCode),
                headers,
                cancellationToken)
            .ConfigureAwait(false);
        var result = new DraftRoomApiCommandResult(MapStatusCode(statusCode));
        if (result.IsSuccess)
        {
            await guestIdentityStore
                .RemoveAsync(normalizedCode, cancellationToken)
                .ConfigureAwait(false);
        }

        return result;
    }

    public async Task<DraftRoomApiResult<IReadOnlyList<MyDraftRoomDto>>> GetMyRoomsAsync(
        CancellationToken cancellationToken = default)
    {
        var (response, statusCode) = await apiHttpClient
            .GetAsync<IReadOnlyList<MyDraftRoomDto>>(
                Paths.ApiPath_DraftRoomsMine,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return MapReadResult(response, statusCode);
    }

    public async Task<DraftRoomApiResult<DraftRoomViewDto>> GetViewAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        var context = await this.GetRequestContextAsync(code, cancellationToken).ConfigureAwait(false);
        var (response, statusCode) = await apiHttpClient
            .GetAsync<DraftRoomViewDto>(
                FormatRoomPath(Paths.ApiPath_DraftRoomsByCode, context.Code),
                cancellationToken: cancellationToken,
                headers: context.Headers)
            .ConfigureAwait(false);

        if (IsSuccessStatusCode(statusCode) && response is not null)
        {
            return new DraftRoomApiResult<DraftRoomViewDto>(DraftRoomApiResultKind.Success, response);
        }

        var resultKind = MapStatusCode(statusCode);
        return new DraftRoomApiResult<DraftRoomViewDto>(
            resultKind == DraftRoomApiResultKind.Success
                ? DraftRoomApiResultKind.Failed
                : resultKind,
            null);
    }

    public async Task<DraftRoomApiResult<IReadOnlyList<ChatMessageDto>>> GetChatHistoryAsync(
        string code,
        long? afterId,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var query = BuildHistoryQuery(afterId, limit);
        var context = await this.GetRequestContextAsync(code, cancellationToken).ConfigureAwait(false);
        var endpointPath = $"{FormatRoomPath(Paths.ApiPath_DraftRoomsByCode, context.Code)}/chat";
        var (response, statusCode) = await apiHttpClient
            .GetAsync<IReadOnlyList<ChatMessageDto>>(
                endpointPath,
                query,
                cancellationToken,
                context.Headers)
            .ConfigureAwait(false);

        return MapReadResult(response, statusCode);
    }

    public async Task<DraftRoomApiResult<IReadOnlyList<DraftLogEntryDto>>> GetLogHistoryAsync(
        string code,
        long? afterId,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var query = BuildHistoryQuery(afterId, limit);
        var context = await this.GetRequestContextAsync(code, cancellationToken).ConfigureAwait(false);
        var endpointPath = $"{FormatRoomPath(Paths.ApiPath_DraftRoomsByCode, context.Code)}/log";
        var (response, statusCode) = await apiHttpClient
            .GetAsync<IReadOnlyList<DraftLogEntryDto>>(
                endpointPath,
                query,
                cancellationToken,
                context.Headers)
            .ConfigureAwait(false);

        return MapReadResult(response, statusCode);
    }

    public async Task<DraftRoomApiResult<ChatMessageDto>> SendChatAsync(
        string code,
        string text,
        CancellationToken cancellationToken = default)
    {
        var context = await this.GetRequestContextAsync(code, cancellationToken).ConfigureAwait(false);
        var request = new SendDraftRoomChatMessageRequest { Text = text };
        var endpointPath = $"{FormatRoomPath(Paths.ApiPath_DraftRoomsByCode, context.Code)}/chat";
        var (response, statusCode) = await apiHttpClient
            .PostApiAsync<SendDraftRoomChatMessageRequest, ChatMessageDto>(
                endpointPath,
                request,
                cancellationToken,
                context.Headers)
            .ConfigureAwait(false);

        if (IsSuccessStatusCode(statusCode) && response.Success && response.Data is not null)
        {
            return new DraftRoomApiResult<ChatMessageDto>(DraftRoomApiResultKind.Success, response.Data);
        }

        var resultKind = MapStatusCode(statusCode);
        return new DraftRoomApiResult<ChatMessageDto>(
            resultKind == DraftRoomApiResultKind.Success
                ? DraftRoomApiResultKind.Failed
                : resultKind,
            null);
    }

    public Task<DraftRoomApiCommandResult> AddSeatAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        return this.SendRoomCommandAsync(
            HttpMethod.Post,
            Paths.ApiPath_DraftRoomsSeats,
            code,
            null,
            cancellationToken);
    }

    public Task<DraftRoomApiCommandResult> RemoveSeatAsync(
        string code,
        Guid seatId,
        CancellationToken cancellationToken = default)
    {
        return this.SendRoomCommandAsync(
            HttpMethod.Delete,
            Paths.ApiPath_DraftRoomsSeat,
            code,
            seatId,
            cancellationToken);
    }

    public Task<DraftRoomApiCommandResult> ClaimSeatAsync(
        string code,
        Guid seatId,
        CancellationToken cancellationToken = default)
    {
        return this.SendRoomCommandAsync(
            HttpMethod.Post,
            Paths.ApiPath_DraftRoomsSeatClaim,
            code,
            seatId,
            cancellationToken);
    }

    public Task<DraftRoomApiCommandResult> UnclaimSeatAsync(
        string code,
        Guid seatId,
        CancellationToken cancellationToken = default)
    {
        return this.SendRoomCommandAsync(
            HttpMethod.Post,
            Paths.ApiPath_DraftRoomsSeatUnclaim,
            code,
            seatId,
            cancellationToken);
    }

    public Task<DraftRoomApiCommandResult> LeaveAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        return this.SendRoomCommandAsync(
            HttpMethod.Post,
            Paths.ApiPath_DraftRoomsLeave,
            code,
            null,
            cancellationToken);
    }

    public Task<DraftRoomApiCommandResult> KickAsync(
        string code,
        Guid participantId,
        CancellationToken cancellationToken = default)
    {
        return this.SendRoomCommandAsync(
            HttpMethod.Post,
            Paths.ApiPath_DraftRoomsParticipantKick,
            code,
            participantId,
            cancellationToken);
    }

    public async Task<DraftRoomApiCommandResult> SetRoleAsync(
        string code,
        Guid participantId,
        DraftRoomRole role,
        CancellationToken cancellationToken = default)
    {
        var context = await this.GetRequestContextAsync(code, cancellationToken).ConfigureAwait(false);
        var request = new SetParticipantRoleRequest(role);
        var (response, statusCode) = await apiHttpClient
            .PutAsync<SetParticipantRoleRequest, DraftRoomViewDto>(
                FormatRoomPath(Paths.ApiPath_DraftRoomsParticipantRole, context.Code, participantId),
                request,
                context.Headers,
                cancellationToken)
            .ConfigureAwait(false);

        return MapCommandResult(response.Success, statusCode);
    }

    public async Task<DraftRoomApiCommandResult> UpdateSettingsAsync(
        string code,
        string name,
        bool isLocked,
        CancellationToken cancellationToken = default)
    {
        var context = await this.GetRequestContextAsync(code, cancellationToken).ConfigureAwait(false);
        var request = new UpdateRoomSettingsRequest(name, isLocked);
        var (response, statusCode) = await apiHttpClient
            .PutAsync<UpdateRoomSettingsRequest, DraftRoomViewDto>(
                FormatRoomPath(Paths.ApiPath_DraftRoomsSettings, context.Code),
                request,
                context.Headers,
                cancellationToken)
            .ConfigureAwait(false);

        return MapCommandResult(response.Success, statusCode);
    }

    public async Task<DraftRoomApiCommandResult> ChangePasswordAsync(
        string code,
        string password,
        CancellationToken cancellationToken = default)
    {
        var context = await this.GetRequestContextAsync(code, cancellationToken).ConfigureAwait(false);
        var request = new ChangeRoomPasswordRequest(password);
        var (response, statusCode) = await apiHttpClient
            .PutAsync<ChangeRoomPasswordRequest, DraftRoomViewDto>(
                FormatRoomPath(Paths.ApiPath_DraftRoomsPassword, context.Code),
                request,
                context.Headers,
                cancellationToken)
            .ConfigureAwait(false);

        return MapCommandResult(response.Success, statusCode);
    }

    public Task<DraftRoomApiCommandResult> CloseAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        return this.SendRoomCommandAsync(
            HttpMethod.Delete,
            Paths.ApiPath_DraftRoomsByCode,
            code,
            null,
            cancellationToken);
    }

    public async Task<DraftRoomApiCommandResult> UpdateDraftSettingsAsync(
        string code,
        DraftSettingsDto settings,
        CancellationToken cancellationToken = default)
    {
        var context = await this.GetRequestContextAsync(code, cancellationToken);
        var (response, status) = await apiHttpClient.PutAsync<DraftSettingsDto, DraftRoomViewDto>(
            $"{FormatRoomPath(Paths.ApiPath_DraftRoomsByCode, context.Code)}/draft/settings",
            settings,
            context.Headers,
            cancellationToken);
        return MapCommandResult(response.Success, status);
    }

    public Task<DraftRoomApiCommandResult> StartDraftAsync(string code, CancellationToken cancellationToken = default) =>
        this.SendRoomCommandAsync(HttpMethod.Post, $"{Paths.ApiPath_DraftRoomsByCode}/draft/start", code, null, cancellationToken);

    public Task<DraftRoomApiCommandResult> ResetDraftAsync(string code, CancellationToken cancellationToken = default) =>
        this.SendRoomCommandAsync(HttpMethod.Post, $"{Paths.ApiPath_DraftRoomsByCode}/draft/reset", code, null, cancellationToken);

    public Task<DraftRoomApiCommandResult> UndoLastPickAsync(string code, CancellationToken cancellationToken = default) =>
        this.SendRoomCommandAsync(HttpMethod.Post, $"{Paths.ApiPath_DraftRoomsByCode}/draft/undo", code, null, cancellationToken);

    public async Task<DraftRoomApiCommandResult> ApplyDraftActionAsync(string code, DraftActionDto action, CancellationToken cancellationToken = default)
    {
        var context = await this.GetRequestContextAsync(code, cancellationToken);
        var (response, status) = await apiHttpClient.PostApiAsync<DraftActionDto, DraftRoomViewDto>(
            $"{FormatRoomPath(Paths.ApiPath_DraftRoomsByCode, context.Code)}/draft/actions",
            action,
            cancellationToken,
            context.Headers);
        return MapCommandResult(response.Success, status);
    }

    public async Task<DraftRoomApiCommandResult> DecideForAsync(string code, DecideForRequest request, CancellationToken cancellationToken = default)
    {
        var context = await this.GetRequestContextAsync(code, cancellationToken);
        var (response, status) = await apiHttpClient.PostApiAsync<DecideForRequest, DraftRoomViewDto>(
            $"{FormatRoomPath(Paths.ApiPath_DraftRoomsByCode, context.Code)}/draft/decide-for",
            request,
            cancellationToken,
            context.Headers);
        return MapCommandResult(response.Success, status);
    }

    public async Task<DraftRoomApiCommandResult> AssignSeatAsync(
        string code,
        Guid seatId,
        Guid participantId,
        CancellationToken cancellationToken = default)
    {
        var context = await this.GetRequestContextAsync(code, cancellationToken);
        var (response, status) = await apiHttpClient.PostApiAsync<AssignSeatRequest, DraftRoomViewDto>(
            $"{FormatRoomPath(Paths.ApiPath_DraftRoomsSeat, context.Code, seatId)}/assign",
            new AssignSeatRequest(participantId),
            cancellationToken,
            context.Headers);
        return MapCommandResult(response.Success, status);
    }

    private static DraftRoomApiResult<DraftRoomEntryDto> MapResult(
        ApiResponse<DraftRoomEntryDto> response,
        HttpStatusCode statusCode)
    {
        if ((int)statusCode is >= 200 and < 300
            && response.Success
            && response.Data is not null)
        {
            return new DraftRoomApiResult<DraftRoomEntryDto>(DraftRoomApiResultKind.Success, response.Data);
        }

        var resultKind = statusCode == HttpStatusCode.NotFound
            ? DraftRoomApiResultKind.NotFoundOrWrongPassword
            : MapStatusCode(statusCode);

        return new DraftRoomApiResult<DraftRoomEntryDto>(
            resultKind == DraftRoomApiResultKind.Success
                ? DraftRoomApiResultKind.Failed
                : resultKind,
            null);
    }

    private static DraftRoomApiResult<TResponse> MapReadResult<TResponse>(
        TResponse? response,
        HttpStatusCode statusCode)
        where TResponse : class
    {
        if (IsSuccessStatusCode(statusCode) && response is not null)
        {
            return new DraftRoomApiResult<TResponse>(
                DraftRoomApiResultKind.Success,
                response);
        }

        var resultKind = MapStatusCode(statusCode);
        return new DraftRoomApiResult<TResponse>(
            resultKind == DraftRoomApiResultKind.Success
                ? DraftRoomApiResultKind.Failed
                : resultKind,
            null);
    }

    private static string BuildHistoryQuery(long? afterId, int limit)
    {
        if (afterId is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(afterId));
        }

        if (limit is < 1 or > DraftRoomLimits.MaxHistoryPageSize)
        {
            throw new ArgumentOutOfRangeException(nameof(limit));
        }

        var query = $"?limit={limit.ToString(CultureInfo.InvariantCulture)}";
        if (afterId.HasValue)
        {
            query += $"&afterId={afterId.Value.ToString(CultureInfo.InvariantCulture)}";
        }

        return query;
    }

    private static string FormatRoomPath(string pathTemplate, string code)
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            pathTemplate,
            Uri.EscapeDataString(code));
    }

    private static string FormatRoomPath(string pathTemplate, string code, Guid resourceId)
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            pathTemplate,
            Uri.EscapeDataString(code),
            resourceId.ToString("D"));
    }

    private static DraftRoomApiCommandResult MapCommandResult(bool responseSucceeded, HttpStatusCode statusCode)
    {
        var resultKind = MapStatusCode(statusCode);
        if (resultKind == DraftRoomApiResultKind.Success && !responseSucceeded)
        {
            resultKind = DraftRoomApiResultKind.Failed;
        }

        return new DraftRoomApiCommandResult(resultKind);
    }

    private static DraftRoomApiResultKind MapStatusCode(HttpStatusCode statusCode)
    {
        return statusCode switch
        {
            HttpStatusCode.NotFound => DraftRoomApiResultKind.NotFound,
            HttpStatusCode.Forbidden => DraftRoomApiResultKind.Forbidden,
            HttpStatusCode.Conflict => DraftRoomApiResultKind.Conflict,
            HttpStatusCode.TooManyRequests => DraftRoomApiResultKind.Throttled,
            HttpStatusCode.BadRequest => DraftRoomApiResultKind.ValidationFailed,
            _ when IsSuccessStatusCode(statusCode) => DraftRoomApiResultKind.Success,
            _ => DraftRoomApiResultKind.Failed,
        };
    }

    private static bool IsSuccessStatusCode(HttpStatusCode statusCode)
    {
        return (int)statusCode is >= 200 and < 300;
    }

    private async Task<DraftRoomApiCommandResult> SendRoomCommandAsync(
        HttpMethod method,
        string pathTemplate,
        string code,
        Guid? resourceId,
        CancellationToken cancellationToken)
    {
        var context = await this.GetRequestContextAsync(code, cancellationToken).ConfigureAwait(false);
        var path = resourceId is null
            ? FormatRoomPath(pathTemplate, context.Code)
            : FormatRoomPath(pathTemplate, context.Code, resourceId.Value);
        var statusCode = await apiHttpClient
            .SendAsync(method, path, context.Headers, cancellationToken)
            .ConfigureAwait(false);

        return new DraftRoomApiCommandResult(MapStatusCode(statusCode));
    }

    private async Task<(string Code, IReadOnlyDictionary<string, string>? Headers)> GetRequestContextAsync(
        string code,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        var normalizedCode = code.Trim().ToUpperInvariant();
        var identity = await guestIdentityStore
            .GetAsync(normalizedCode, cancellationToken)
            .ConfigureAwait(false);
        IReadOnlyDictionary<string, string>? headers = identity is null
            ? null
            : new Dictionary<string, string>
            {
                [ParticipantIdHeader] = identity.ParticipantId.ToString(),
                [ParticipantSecretHeader] = identity.Secret,
            };

        return (normalizedCode, headers);
    }
}
