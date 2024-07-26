namespace TwilightImperiumUltimate.Web.Models.Account;

public interface IErrorResponse
{
    string? Type { get; set; }

    string? Title { get; set; }

    int? Status { get; set; }

    string? Detail { get; set; }

    string? Instance { get; set; }

    IReadOnlyDictionary<string, List<string>>? Errors { get; set; }

    string? AdditionalProp1 { get; set; }

    string? AdditionalProp2 { get; set; }

    string? AdditionalProp3 { get; set; }
}
