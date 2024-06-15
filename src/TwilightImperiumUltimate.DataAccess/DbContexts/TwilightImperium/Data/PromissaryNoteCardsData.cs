using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Enums.Cards;
using TwilightImperiumUltimate.DataAccess.Services;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class PromissaryNoteCardsData
{
    internal static List<PromissaryNoteCard> PromissaryNoteCards => GetPromissaryNoteCards();

    private static List<PromissaryNoteCard> GetPromissaryNoteCards()
    {
        CreateCardImagePathService createCardImagePathService = new();

        var promissaryNoteCards = new List<PromissaryNoteCard>()
        {
            new() { PromissaryNoteCardName = PromissoryNoteName.Stymie, Faction = FactionName.TheArborec },
            new() { PromissaryNoteCardName = PromissoryNoteName.WarFunding, Faction = FactionName.TheBaronyOfLetnev },
            new() { PromissaryNoteCardName = PromissoryNoteName.RaghsCall, Faction = FactionName.TheClanOfSaar },
            new() { PromissaryNoteCardName = PromissoryNoteName.FiresOfTheGashlai, Faction = FactionName.TheEmbersOfMuaat },
            new() { PromissaryNoteCardName = PromissoryNoteName.TradeConvoys, Faction = FactionName.TheEmiratesOfHacan },
            new() { PromissaryNoteCardName = PromissoryNoteName.MilitarySupport, Faction = FactionName.TheFederationOfSol },
            new() { PromissaryNoteCardName = PromissoryNoteName.CreussIff, Faction = FactionName.TheGhostsOfCreuss },
            new() { PromissaryNoteCardName = PromissoryNoteName.CyberneticEnhancements, Faction = FactionName.TheL1z1xMindnet },
            new() { PromissaryNoteCardName = PromissoryNoteName.PromiseOfProtection, Faction = FactionName.TheMentakCoalition },
            new() { PromissaryNoteCardName = PromissoryNoteName.GiftOfPrescience, Faction = FactionName.TheNaaluCollective },
            new() { PromissaryNoteCardName = PromissoryNoteName.Antivirus, Faction = FactionName.TheNekroVirus },
            new() { PromissaryNoteCardName = PromissoryNoteName.TekklarLegion, Faction = FactionName.SardakkNorr },
            new() { PromissaryNoteCardName = PromissoryNoteName.ResearchAgreement, Faction = FactionName.TheUniversitiesOfJolNar },
            new() { PromissaryNoteCardName = PromissoryNoteName.Acquiescence, Faction = FactionName.TheWinnu },
            new() { PromissaryNoteCardName = PromissoryNoteName.PoliticalFavor, Faction = FactionName.TheXxchaKingdom },
            new() { PromissaryNoteCardName = PromissoryNoteName.GreyfireMutagen, Faction = FactionName.TheYinBrotherhood },
            new() { PromissaryNoteCardName = PromissoryNoteName.SpyNet, Faction = FactionName.TheYssarilTribes },
            new() { PromissaryNoteCardName = PromissoryNoteName.StrikeWingAmbuscade, Faction = FactionName.TheArgentFlight },
            new() { PromissaryNoteCardName = PromissoryNoteName.DarkPact, Faction = FactionName.TheEmpyrean },
            new() { PromissaryNoteCardName = PromissoryNoteName.BloodPact, Faction = FactionName.TheEmpyrean },
            new() { PromissaryNoteCardName = PromissoryNoteName.ScepterOfDominion, Faction = FactionName.TheMahactGeneSorcerers },
            new() { PromissaryNoteCardName = PromissoryNoteName.BlackMarketForgery, Faction = FactionName.TheNaazRokhaAlliance },
            new() { PromissaryNoteCardName = PromissoryNoteName.TheCavalry, Faction = FactionName.TheNomad },
            new() { PromissaryNoteCardName = PromissoryNoteName.Terraform, Faction = FactionName.TheTitansOfUl },
            new() { PromissaryNoteCardName = PromissoryNoteName.Crucible, Faction = FactionName.TheVuilRaithCabal },
            new() { PromissaryNoteCardName = PromissoryNoteName.KeleresRider, Faction = FactionName.TheCouncilKeleres },
        };

        var updatedPromissaryNoteCards = promissaryNoteCards.Select((promissaryNoteCard, i) =>
        {
            promissaryNoteCard.Id = i + 1;
            promissaryNoteCard.Type = CardType.Promissary;
            promissaryNoteCard.ImagePath = createCardImagePathService.GetCardImagePath(promissaryNoteCard.PromissaryNoteCardName, promissaryNoteCard.Type);
            return promissaryNoteCard;
        }).ToList();

        return updatedPromissaryNoteCards;
    }
}
