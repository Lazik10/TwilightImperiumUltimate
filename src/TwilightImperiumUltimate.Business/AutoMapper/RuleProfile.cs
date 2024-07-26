using TiRule = TwilightImperiumUltimate.Core.Entities.Rules.Rule;

namespace TwilightImperiumUltimate.Business.AutoMapper;


internal class RuleProfile : Profile
{
    public RuleProfile()
    {
        CreateMap<TiRule, RuleDto>()
            .ConstructUsing(x => new RuleDto(
                x.Id,
                x.RuleCategory,
                x.Content));
    }
}
