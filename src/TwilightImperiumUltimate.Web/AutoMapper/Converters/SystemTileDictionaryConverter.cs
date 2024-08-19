namespace TwilightImperiumUltimate.Web.AutoMapper.Converters;

public class SystemTileDictionaryConverter : IValueResolver<Dictionary<int, SystemTileDto>, Dictionary<int, SystemTileModel>, Dictionary<int, SystemTileModel>>
{
    public Dictionary<int, SystemTileModel> Resolve(Dictionary<int, SystemTileDto> source, Dictionary<int, SystemTileModel> destination, Dictionary<int, SystemTileModel> destMember, ResolutionContext context)
    {
        var result = new Dictionary<int, SystemTileModel>();
        foreach (var item in source)
        {
            result[item.Key] = context.Mapper.Map<SystemTileModel>(item.Value);
        }

        return result;
    }
}