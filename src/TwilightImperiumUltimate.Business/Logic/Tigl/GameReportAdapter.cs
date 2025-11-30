using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;

namespace TwilightImperiumUltimate.Business.Logic.Tigl;

public partial class EvaluateGameReportCommandHandler
{
    private sealed class GameReportAdapter : IGameReport
    {
        public GameReportAdapter(Core.Entities.Tigl.MatchReport mr)
        {
            GameId = mr.GameId;
            Score = mr.Score;
            Round = mr.Round;
            PlayerCount = mr.PlayerResults.Count;
            PlayerResults = new List<PlayerResult>();
            Source = mr.Source;
            StartTimestamp = mr.StartTimestamp;
            EndTimestamp = mr.EndTimestamp;
            League = mr.League;
            Events = new List<string>();
        }

        public string GameId { get; set; }

        public int Score { get; set; }

        public int Round { get; set; }

        public int PlayerCount { get; set; }

        public IReadOnlyCollection<PlayerResult> PlayerResults { get; set; }

        public ResultSource Source { get; set; }

        public long StartTimestamp { get; set; }

        public long EndTimestamp { get; set; }

        public TiglLeague League { get; set; }

        public List<string> Events { get; set; }
    }
}
