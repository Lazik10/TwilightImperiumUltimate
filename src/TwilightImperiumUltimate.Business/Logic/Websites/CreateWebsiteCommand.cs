using TwilightImperiumUltimate.Contracts.ApiContracts;
using TwilightImperiumUltimate.Contracts.DTOs.Website;

namespace TwilightImperiumUltimate.Business.Logic.Websites;

public class CreateWebsiteCommand(string title, string description, string websitePath, byte[]? imageData, string? imageContentType)
    : IRequest<ApiResponse<WebsiteDto>>
{
    public string Title { get; } = title;

    public string Description { get; } = description;

    public string WebsitePath { get; } = websitePath;

    public byte[]? ImageData { get; } = imageData;

    public string? ImageContentType { get; } = imageContentType;
}
