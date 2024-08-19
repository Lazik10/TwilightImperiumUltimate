using TwilightImperiumUltimate.Core.Entities.Website;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class WebsitesData
{
    internal static List<Website> Websites => new List<Website>()
    {
        new Website()
        {
           Title = "Fantasy Flight Games",
           Description = string.Empty,
           WebsitePath = "https://www.fantasyflightgames.com/en/products/#/universe/twilight-imperium-universe",
        },
        new Website()
        {
           Title = "Twilight Imperium Wiki",
           Description = string.Empty,
           WebsitePath = "https://twilight-imperium.fandom.com/wiki/Twilight_Imperium_Wiki",
        },
        new Website()
        {
            Title = "Twilight Imperium 4th Edition Rules Reference",
            Description = string.Empty,
            WebsitePath = "https://www.tirules.com",
        },
        new Website()
        {
           Title = "Board Game Geek",
           Description = string.Empty,
           WebsitePath = "https://boardgamegeek.com/boardgame/233078/twilight-imperium-fourth-edition",
        },
        new Website()
        {
            Title = "Reddit",
            Description = string.Empty,
            WebsitePath = "https://www.reddit.com/r/twilightimperium/",
        },
        new Website()
        {
            Title = "SCPT Podcasts",
            Description = string.Empty,
            WebsitePath = "https://www.podbean.com/user-mEJPMtyOuVj",
        },
        new Website()
        {
            Title = "Twilight Wars",
            Description = string.Empty,
            WebsitePath = "https://www.twilightwars.com/games",
        },
        new Website()
        {
            Title = "Twilight Imperium Assistant",
            Description = string.Empty,
            WebsitePath = "https://ti-assistant.com",
        },
        new Website()
        {
            Title = "Online Match Stats",
            Description = string.Empty,
            WebsitePath = "https://lookerstudio.google.com/reporting/3b435bf2-2100-488c-a424-130f1d22ebb0/page/pE58B",
        },
        new Website()
        {
            Title = "Keegan Map Generator",
            Description = string.Empty,
            WebsitePath = "https://keeganw.github.io/ti4/",
        },
        new Website()
        {
            Title = "TI4 Lab",
            Description = string.Empty,
            WebsitePath = "https://ti4-lab.fly.dev/",
        },
        new Website()
        {
            Title = "Twilight Imperium 4 Balanced Map Generator",
            Description = string.Empty,
            WebsitePath = "https://ti4-map-generator.derekpeterson.ca",
        },
        new Website()
        {
            Title = "TI4 Map Lab",
            Description = string.Empty,
            WebsitePath = "https://joepinion.github.io/ti4-map-lab/",
        },
        new Website()
        {
            Title = "Milty Draft Generator",
            Description = string.Empty,
            WebsitePath = "https://milty.shenanigans.be",
        },
        new Website()
        {
            Title = "Milty Draft Map Generator",
            Description = string.Empty,
            WebsitePath = "https://miltydraft.com/goodtimes",
        },
        new Website()
        {
            Title = "TI4 Cartographer",
            Description = string.Empty,
            WebsitePath = "https://acodcha.github.io/ti4-cartographer/",
        },
        new Website()
        {
            Title = "Color Picker",
            Description = string.Empty,
            WebsitePath = "https://www.ti.vetinari.net",
        },
        new Website()
        {
            Title = "Twilight Imperium 4th Color Assigner",
            Description = string.Empty,
            WebsitePath = "https://brilleslange.github.io",
        },
        new Website()
        {
            Title = "Card Generator",
            Description = string.Empty,
            WebsitePath = "https://ti4-card-images.appspot.com/static/card.html",
        },
    };
}
