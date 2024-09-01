namespace TwilightImperiumUltimate.Business.Logic.MapArchive;

public class DeleteMapCommand : IRequest<bool>
{
    public int Id { get; set; }
}
