using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Extensions;

public static class SliceExtensions
{
    public static float SliceEvaluation(this Slice slice, SystemWeight systemWeight)
    {
        float evaluation = 0.0f;
        evaluation = slice.Positions.Sum(pos => pos.SystemTile switch
        {
            null => 0.0f,
            var systemTile => systemTile.GetValue(systemWeight),
        });

        evaluation += slice.DraftedSystemTiles.Sum(systemTile => systemTile.GetValue(systemWeight));

        return evaluation;
    }

    public static bool HasAnomaly(this Slice slice)
    {
        return slice.Positions.Any(pos => pos.SystemTile != null && pos.SystemTile.HasAnomaly);
    }

    public static string GetSliceLog(this Slice slice)
    {
        var idText = $"Slice {slice.Id}";
        var numberOfAssignedPositions = $"{slice.Positions.Count(x => x.SystemTile is not null)} assigned positions";
        var assignedSystemTiles = string.Join(", ", slice.Positions.Where(x => x.SystemTile is not null).Select(x => $"[{x.X},{x.Y}]:{x.SystemTile!.SystemTileCode}"));
        var unassignedSystemTiles = string.Join(", ", slice.Positions.Where(x => x.SystemTile is null).Select(x => $"[{x.X},{x.Y}]"));
        var draftedSystemTiles = string.Join(", ", slice.DraftedSystemTiles.Select(x => x.SystemTileCode));

        return string.Join("\n", $"{idText} {numberOfAssignedPositions}", $"Assigned: {assignedSystemTiles}", $"Unassigned: {unassignedSystemTiles}", $"Drafted: {draftedSystemTiles}");
    }

    public static string GetSlicesEvaluation(this List<Slice> slices, SystemWeight systemWeight)
    {
        return string.Join("\n", slices.Select(x => string.Join(" - ", $"Id: {x.Id}", $"Combined weight: {x.SliceEvaluation(systemWeight)}")));
    }

    public static string GetSlicesLog(this List<Slice> slices)
    {
        return string.Join("\n", slices.Select(x => x.GetSliceLog()));
    }
}
