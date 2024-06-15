using TwilightImperiumUltimate.Core.Entities.News;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class ArticlesData
{
    internal static List<NewsArticle> Articles => GetArticles();

    private static List<NewsArticle> GetArticles()
    {
        var newsArticles = new List<NewsArticle>()
        {
            new()
            {
                Title = "New Era - Twilight Imperium: Ultimate!",
                Content = "Welcome to the all-new 'Twilight Imperium: Ultimate' website<br/><br/>" +
                          "As an avid fan and a dedicated developer, I'm developing this website to elevate your Twilight Imperium gaming experience. Here, you'll find a host of new features, game-enhancing tools, and community-driven sections tailor-made for fans, by a fans.<br/><br/>" +
                          "While the site is a cosmic work in progress, I'm committed to constantly evolving it with more functionality and user-friendly enhancements. Your feedback is crucial in this journey! So, if you have any cool ideas, spot pesky bugs, or just want to share your thoughts, reach out through my socials. I'm all ears (and antennas!).<br/><br/>" +
                          "Stay tuned for regular updates on new releases and features. And, most importantly, I hope this site becomes a valuable part of your galactic conquests. May your strategies be sharp, and your gaming sessions epic!<br/><br/>" +
                          "Happy gaming,<br/>Lazik",
                CreatedAt = new DateOnly(2023, 06, 15),
                UpdatedAt = new DateOnly(2023, 06, 15),
                UserId = 1,
            },
            new()
            {
                Title = "First Steps - Launching Our First Feature!",
                Content = "Introducing the Game Section<br/><br/>" +
                            "<p>Exciting news for all Twilight Imperium enthusiasts! The first section of our website is now live, dedicated to the mesmerizing universe of Twilight Imperium 4th Edition, the PoK Expansion, and Codices 1-3.</p>" +
                            "<p>Here's what you can explore:</p>" +
                            "<ul>" +
                            "  <li><strong>Faction Insights:</strong> Dive deep into each faction, understanding their unique strengths and strategies.</li>" +
                            "  <li><strong>Card Compendium:</strong> Scroll through an extensive collection of cards, from political agendas to technological advancements.</li>" +
                            "</ul>" +
                            "<p>Your feedback is vital for our journey through the stars! If you have suggestions for enhancements, or if you encounter any cosmic bugs, your insights are invaluable. Help us improve by reporting issues directly on our GitHub:</p>" +
                            "<p><a href=\"https://github.com/Lazik10/TwilightImperiumUltimate/issues\" style=\"color: yellow\">GitHub Repository</a></p>",
                CreatedAt = new DateOnly(2023, 08, 24),
                UpdatedAt = new DateOnly(2023, 08, 24),
                UserId = 1,
            },
            new()
            {
                Title = "Exciting Update - Introducing the 'Tools' Section!",
                Content = "Discover the New 'Tools' Section<br/><br/>" +
                          "<p>Great news for Twilight Imperium Commanders! I am thrilled to announce the launch of the 'Tools' section, a brand-new feature designed to enhance your galactic conquests. This section is tailor-made to add a new level of depth and customization to your Twilight Imperium experience.</p>" +
                          "<p>Here’s what awaits you in the 'Tools' section:</p>" +
                          "<ul>" +
                          "  <li><strong>Faction Color Drafting:</strong> Choose your faction color with new drafting tool, adding a personal touch to your empire.</li>" +
                          "  <li><strong>Faction Drafting:</strong> Select from all 25 factions using intuitive drafting tool, ensuring a balanced and exciting game setup.</li>" +
                          "  <li><strong>Custom Map Generator:</strong> Create unique galactic maps for your games, with varied configurations to keep every session fresh and challenging.</li>" +
                          "  <li><strong>Custom Card Creation:</strong> Unleash your creativity by designing your own custom cards, adding a personalized flair to your game.</li>" +
                          "</ul>" +
                          "<p>Your strategic insights are invaluable to me! As always if you have any suggestions for improving the 'Tools' section, or if you discover any anomalies in the vast reaches of my digital galaxy, please let me know. Your feedback helps me continuously improve and expand our universe.</p>" +
                          "<p>Contribute your ideas and report issues on our GitHub:</p>" +
                          "<p><a href=\"https://github.com/Lazik10/TwilightImperiumUltimate/issues\" style=\"color: yellow\">GitHub Repository</a></p>",
                CreatedAt = new DateOnly(2023, 11, 05),
                UpdatedAt = new DateOnly(2023, 11, 05),
                UserId = 1,
            },
            new()
            {
                Title = "First Milestone - TI: Ultimate Test Website Launch!",
                Content = "Embarking on a New Journey: TI: Ultimate Test Site Goes Live!<br/><br/>" +
                          "<p>I am thrilled to announce a giant leap in my developing Twilight Imperium website journey – the launch of the TI: Ultimate test website! This is a pivotal moment, marking the first time my platform is open for interstellar explorers – like you.</p>" +
                          "<p><strong>What’s in Store?</strong></p>" +
                          "<ul>" +
                          "  <li>Experience the website’s core functionalities in their initial form.</li>" +
                          "  <li>Explore sections that are already operational, offering a glimpse into what the final universe will look like.</li>" +
                          "</ul>" +
                          "<p>This is just the beginning! The test site is your playground to experiment, explore, and provide feedback. Your insights and suggestions are more than just valuable – they are the fuel that will drive me forward in this cosmic journey.</p>" +
                          "<p>As one of my first volunteers, you have the unique opportunity to shape the future of the TI: Ultimate website. Encounter a bug? Have a suggestion? Your feedback is crucial to navigating this new frontier. Let’s create the ultimate Twilight Imperium digital experience together!</p>" +
                          "<p>Join me in this adventure! Looking forward to your feedback and participation!</p>",
                CreatedAt = new DateOnly(2023, 11, 14),
                UpdatedAt = new DateOnly(2023, 11, 14),
                UserId = 1,
            },
            new()
            {
                Title = "Tech Triumph - Launching the Technology Cards Compendium!",
                Content = "Unveiling the Technology Cards Compendium<br/><br/>" +
                          "<p>Dear Twilight Imperium Commanders, it's time to unveil our latest addition to the TI: Ultimate website - the comprehensive Technology Cards Compendium! This new section is a treasure trove for players seeking in-depth knowledge and strategies around the game’s technological advancements.</p>" +
                          "<ul>" +
                          "  <li><strong>Biotic:</strong> Explore the life-enhancing and ecosystem-related technologies that give your civilization a biological edge.</li>" +
                          "  <li><strong>Propulsion:</strong> Boost your fleets with advanced propulsion technologies, ensuring faster and more efficient interstellar travel.</li>" +
                          "  <li><strong>Cybernetic:</strong> Augment your capabilities with cybernetic enhancements, blending the best of technology and biology.</li>" +
                          "  <li><strong>Warfare:</strong> Gain insights into warfare technologies that can turn the tide of any battle in your favor.</li>" +
                          "  <li><strong>Unit Upgrades:</strong> Upgrade your units with cutting-edge advancements for superior performance in various aspects of the game.</li>" +
                          "  <li><strong>Faction Technologies:</strong> Discover unique technologies specific to each faction, adding a layer of depth and strategy to your gameplay.</li>" +
                          "</ul>" +
                          "<p>Your experience and feedback are pivotal in this galactic journey. If you have any suggestions or encounter any glitches in the cosmic matrix of my website, your input is invaluable. Together, let's continue enhancing the TI: Ultimate experience for every commander in the galaxy!</p>" +
                          "<p>Explore, experiment, and enjoy the new Technology Cards Compendium, and don't forget to share your thoughts and feedback with us.</p>" +
                          "<p>Here's to many more technological breakthroughs!</p>",
                CreatedAt = new DateOnly(2023, 11, 19),
                UpdatedAt = new DateOnly(2023, 11, 19),
                UserId = 1,
            },
        };

        return newsArticles;
    }
}
