using TwilightImperiumUltimate.Contracts.DTOs;

namespace TwilightImperiumUltimate.Contracts.ApiContracts;

public class ApiResponse<TResponseDto> : IApiResponse<TResponseDto>
    where TResponseDto : class
{
    public bool Success { get; set; }

    public TResponseDto? Data { get; set; }

    public ProblemDetailsDto ProblemDetails { get; set; } = new();
}
