namespace TwilightImperiumUltimate.DataAccess.Services;

public class CreateCardImagePathService : ICreateCardImagePathService
{
    private const string _basePath = "resources\\images";
    private const string _language = "{language}";

    public string GetCardImagePath<TEnumName, TEnumType>(TEnumName enumName, TEnumType cardType)
        where TEnumName : Enum
        where TEnumType : Enum
    {
        if (string.IsNullOrEmpty(enumName.ToString()))
            throw new ArgumentException("Enum name is a required parameter.");

        return Path.Combine(_basePath, _language, cardType.ToString(), $"{enumName}.webp");
    }
}
