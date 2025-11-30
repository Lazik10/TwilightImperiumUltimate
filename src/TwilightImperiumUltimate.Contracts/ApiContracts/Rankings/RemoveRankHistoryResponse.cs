namespace TwilightImperiumUltimate.Contracts.ApiContracts.Rankings;

public class RemoveRankHistoryResponse
{
    public bool Success { get; set; }

    public string? ErrorTitle { get; set; }

    public string? ErrorMessage { get; set; }
}