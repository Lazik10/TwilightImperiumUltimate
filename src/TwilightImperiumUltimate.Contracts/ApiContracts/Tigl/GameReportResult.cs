namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;

public class GameReportResult
{
    public GameReportResult()
    {
        Success = true;
    }

    public GameReportResult(bool success, string errorTitle, string errorMessage)
    {
        Success = success;
        ErrorTitle = errorTitle;
        ErrorMessage = errorMessage;
    }

    public bool Success { get; set; }

    public string ErrorTitle { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;
}
