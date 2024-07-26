namespace TwilightImperiumUltimate.Business.AutoMapperProfiles;

internal class CardProfile : Profile
{
    public CardProfile()
    {
        CreateMap<ActionCard, ActionCardDto>()
            .ConstructUsing(ac => new ActionCardDto(
                ac.Id,
                ac.Name,
                ac.Text,
                ac.Type,
                ac.GameVersion,
                ac.EnumName,
                ac.TimingWindow));

        CreateMap<AgendaCard, AgendaCardDto>()
            .ConstructUsing(ac => new AgendaCardDto(
                ac.Id,
                ac.Name,
                ac.Text,
                ac.Type,
                ac.GameVersion,
                ac.EnumName,
                ac.AgendaCardType));

        CreateMap<ExplorationCard, ExplorationCardDto>()
            .ConstructUsing(ec => new ExplorationCardDto(
                ec.Id,
                ec.Name,
                ec.Text,
                ec.Type,
                ec.GameVersion,
                ec.EnumName,
                ec.ExplorationPlanetTrait));

        CreateMap<FrontierCard, FrontierCardDto>()
            .ConstructUsing(fc => new FrontierCardDto(
                fc.Id,
                fc.Name,
                fc.Text,
                fc.Type,
                fc.GameVersion,
                fc.EnumName));

        CreateMap<ObjectiveCard, ObjectiveCardDto>()
            .ConstructUsing(oc => new ObjectiveCardDto(
                oc.Id,
                oc.Name,
                oc.Text,
                oc.Type,
                oc.GameVersion,
                oc.EnumName,
                oc.ObjectiveCardType,
                oc.TimingWindow));

        CreateMap<PromissoryNoteCard, PromissoryNoteCardDto>()
            .ConstructUsing(pnc => new PromissoryNoteCardDto(
                pnc.Id,
                pnc.Name,
                pnc.Text,
                pnc.Type,
                pnc.GameVersion,
                pnc.EnumName,
                pnc.Faction));

        CreateMap<RelicCard, RelicCardDto>()
            .ConstructUsing(rc => new RelicCardDto(
                rc.Id,
                rc.Name,
                rc.Text,
                rc.Type,
                rc.GameVersion,
                rc.EnumName));

        CreateMap<StrategyCard, StrategyCardDto>()
            .ConstructUsing(sc => new StrategyCardDto(
                sc.Id,
                sc.Name,
                sc.Text,
                sc.Type,
                sc.GameVersion,
                sc.EnumName,
                sc.InitiativeOrder,
                sc.PrimaryAbilityText,
                sc.SecondaryAbilityText));
    }
}
