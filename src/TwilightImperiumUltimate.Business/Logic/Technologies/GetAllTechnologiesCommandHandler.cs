namespace TwilightImperiumUltimate.Business.Logic.Technologies;

public class GetAllTechnologiesCommandHandler(
    ITechnologyRepository technologyRepository,
    IMapper mapper)
    : IRequestHandler<GetAllTechnologiesCommand, ItemListDto<TechnologyDto>>
{
    private readonly ITechnologyRepository _technologyRepository = technologyRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ItemListDto<TechnologyDto>> Handle(GetAllTechnologiesCommand request, CancellationToken cancellationToken)
    {
        var technologies = await _technologyRepository.GetAllTechnologies(cancellationToken);

        var technologiesDto = _mapper.Map<List<TechnologyDto>>(technologies);

        return new ItemListDto<TechnologyDto>(technologiesDto);
    }
}
