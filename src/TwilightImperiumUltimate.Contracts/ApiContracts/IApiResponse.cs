using TwilightImperiumUltimate.Contracts.DTOs;

namespace TwilightImperiumUltimate.Contracts.ApiContracts;

public interface IApiResponse<TResponseDto>
{
    bool Success { get; set; }

    TResponseDto? Data { get; set; }

    ProblemDetailsDto ProblemDetails { get; set; }
}
