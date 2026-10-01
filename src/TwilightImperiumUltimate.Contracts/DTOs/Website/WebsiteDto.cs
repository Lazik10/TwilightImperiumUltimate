namespace TwilightImperiumUltimate.Contracts.DTOs.Website;

public record WebsiteDto(int Id, string Title, string Description, string WebsitePath, byte[]? ImageData, string? ImageContentType);
