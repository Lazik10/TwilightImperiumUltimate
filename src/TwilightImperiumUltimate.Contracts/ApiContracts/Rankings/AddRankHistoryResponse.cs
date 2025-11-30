namespace TwilightImperiumUltimate.Contracts.ApiContracts.Rankings;

public class AddRankHistoryResponse
{
    public bool Success { get; set; }

    public int? NewRankHistoryId { get; set; }

    public string? ErrorTitle { get; set; }

    public string? ErrorMessage { get; set; }
}