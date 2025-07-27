namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public class EndSeasonCommandHandler : IRequestHandler<EndSeasonCommand, bool>
{
    public Task<bool> Handle(EndSeasonCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
