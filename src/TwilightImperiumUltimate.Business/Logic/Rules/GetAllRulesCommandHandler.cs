namespace TwilightImperiumUltimate.Business.Logic.Rules;

public class GetAllRulesCommandHandler(
    IRuleRepository ruleRepository,
    IMapper mapper)
    : IRequestHandler<GetAllRulesCommand, ItemListDto<RuleDto>>
{
    private readonly IRuleRepository _ruleRepository = ruleRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<RuleDto>> Handle(GetAllRulesCommand request, CancellationToken cancellationToken)
    {
        var rules = await _ruleRepository.GetAllRules(cancellationToken);

        var rulesDto = _mapper.Map<List<RuleDto>>(rules);

        return new ItemListDto<RuleDto>(rulesDto);
    }
}
