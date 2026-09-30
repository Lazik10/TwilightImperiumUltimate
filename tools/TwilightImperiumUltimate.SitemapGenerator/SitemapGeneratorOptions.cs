namespace TwilightImperiumUltimate.SitemapGenerator;

internal sealed class SitemapGeneratorOptions
{
    required public string RepositoryRoot { get; init; }

    public Uri? ApiBaseUrl { get; init; }

    public static SitemapGeneratorOptions Parse(string[] arguments)
    {
        var repositoryRoot = GetArgument(arguments, "--repository-root")
            ?? throw new ArgumentException("The --repository-root argument is required.");
        var apiBaseUrl = GetArgument(arguments, "--api-base-url");

        return new SitemapGeneratorOptions
        {
            RepositoryRoot = Path.GetFullPath(repositoryRoot),
            ApiBaseUrl = string.IsNullOrWhiteSpace(apiBaseUrl) ? null : new Uri(apiBaseUrl, UriKind.Absolute),
        };
    }

    private static string? GetArgument(IReadOnlyList<string> arguments, string name)
    {
        for (var index = 0; index < arguments.Count - 1; index++)
        {
            if (string.Equals(arguments[index], name, StringComparison.Ordinal))
                return arguments[index + 1];
        }

        return null;
    }
}
