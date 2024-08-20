using TwilightImperiumUltimate.Core.Entities.Website;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class WebsitesData
{
    internal static List<Website> Websites => new List<Website>()
    {
        new Website()
        {
            Id = 1,
            Title = "Fantasy Flight Games",
            Description = string.Empty,
            WebsitePath = "https://www.fantasyflightgames.com/en/products/#/universe/twilight-imperium-universe",
        },
        new Website()
        {
            Id = 2,
            Title = "Twilight Imperium Wiki",
            Description = string.Empty,
            WebsitePath = "https://twilight-imperium.fandom.com/wiki/Twilight_Imperium_Wiki",
        },
        new Website()
        {
            Id = 3,
            Title = "Twilight Imperium 4th Edition Rules Reference",
            Description = string.Empty,
            WebsitePath = "https://www.tirules.com",
        },
        new Website()
        {
            Id = 4,
            Title = "Board Game Geek",
            Description = string.Empty,
            WebsitePath = "https://boardgamegeek.com/boardgame/233078/twilight-imperium-fourth-edition",
        },
        new Website()
        {
            Id = 5,
            Title = "Reddit",
            Description = string.Empty,
            WebsitePath = "https://www.reddit.com/r/twilightimperium/",
        },
        new Website()
        {
            Id = 6,
            Title = "SCPT Podcasts",
            Description = string.Empty,
            WebsitePath = "https://www.podbean.com/user-mEJPMtyOuVj",
        },
        new Website()
        {
            Id = 7,
            Title = "Twilight Wars",
            Description = string.Empty,
            WebsitePath = "https://www.twilightwars.com/games",
        },
        new Website()
        {
            Id = 8,
            Title = "Twilight Imperium Assistant",
            Description = string.Empty,
            WebsitePath = "https://ti-assistant.com",
        },
        new Website()
        {
            Id = 9,
            Title = "Online Match Stats",
            Description = string.Empty,
            WebsitePath = "https://lookerstudio.google.com/reporting/3b435bf2-2100-488c-a424-130f1d22ebb0/page/pE58B",
        },
        new Website()
        {
            Id = 10,
            Title = "Keegan Map Generator",
            Description = string.Empty,
            WebsitePath = "https://keeganw.github.io/ti4/",
        },
        new Website()
        {
            Id = 11,
            Title = "TI4 Lab",
            Description = string.Empty,
            WebsitePath = "https://ti4-lab.fly.dev/",
        },
        new Website()
        {
            Id = 12,
            Title = "Twilight Imperium 4 Balanced Map Generator",
            Description = string.Empty,
            WebsitePath = "https://ti4-map-generator.derekpeterson.ca",
        },
        new Website()
        {
            Id = 13,
            Title = "TI4 Map Lab",
            Description = string.Empty,
            WebsitePath = "https://joepinion.github.io/ti4-map-lab/",
        },
        new Website()
        {
            Id = 14,
            Title = "Milty Draft Generator",
            Description = string.Empty,
            WebsitePath = "https://milty.shenanigans.be",
        },
        new Website()
        {
            Id = 15,
            Title = "Milty Draft Map Generator",
            Description = string.Empty,
            WebsitePath = "https://miltydraft.com/goodtimes",
        },
        new Website()
        {
            Id = 16,
            Title = "TI4 Cartographer",
            Description = string.Empty,
            WebsitePath = "https://acodcha.github.io/ti4-cartographer/",
        },
        new Website()
        {
            Id = 17,
            Title = "Color Picker",
            Description = string.Empty,
            WebsitePath = "https://www.ti.vetinari.net",
        },
        new Website()
        {
            Id = 18,
            Title = "Twilight Imperium 4th Color Assigner",
            Description = string.Empty,
            WebsitePath = "https://brilleslange.github.io",
        },
        new Website()
        {
            Id = 19,
            Title = "Card Generator",
            Description = string.Empty,
            WebsitePath = "https://ti4-card-images.appspot.com/static/card.html",
        },
    };
}
