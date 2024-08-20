namespace TwilightImperiumUltimate.Business.Logic.Rules;

public class GetAllRulesQueryHandler(
    IRuleRepository ruleRepository,
    IMapper mapper)
    : IRequestHandler<GetAllRulesQuery, ItemListDto<RuleDto>>
{
    private readonly IRuleRepository _ruleRepository = ruleRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<RuleDto>> Handle(GetAllRulesQuery request, CancellationToken cancellationToken)
    {
        var rules = await _ruleRepository.GetAllRules(cancellationToken);

        var rulesDto = _mapper.Map<List<RuleDto>>(rules);

        return new ItemListDto<RuleDto>(rulesDto);
    }
}
