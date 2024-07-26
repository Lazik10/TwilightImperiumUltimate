namespace TwilightImperiumUltimate.Business.Logic.Factions;

public class GetFactionByIdQuery(int id) : IRequest<FactionDto>
{
    public int Id { get; set; } = id;
}
