using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.SliceDraft;

public class DraftSlicesService(
    ISystemTilesForMapSetupProvider systemTilesForMapSetupProvider,
    ISliceSystemTilePreparer sliceSystemTilePreparer,
    IMiltyDraftSliceBalancer miltyDraftSliceBalancer,
    IMiltyDraftSystemTileSetter miltyDraftSystemTileSetter,
    ILogger<DraftSlicesService> logger)
    : IDraftSlicesService
{
    private readonly ISystemTilesForMapSetupProvider _systemTilesForMapSetupProvider = systemTilesForMapSetupProvider;
    private readonly ISliceSystemTilePreparer _sliceSystemTilePreparer = sliceSystemTilePreparer;
    private readonly IMiltyDraftSliceBalancer _miltyDraftSliceBalancer = miltyDraftSliceBalancer;
    private readonly IMiltyDraftSystemTileSetter _miltyDraftSystemTileSetter = miltyDraftSystemTileSetter;
    private readonly ILogger<DraftSlicesService> _logger = logger;

    public async Task<List<Slice>> DraftSlices(
        SliceDraftRequest request,
        CancellationToken cancellationToken)
    {
        var systemTilesForSlices = await _systemTilesForMapSetupProvider.GetSystemTilesForSlices(request.GameVersions, cancellationToken);
        var systemTilesForSliceRedistribution = new SystemTilesForSlices();

        if (request.PreviewSlices)
        {
            return GetPreviewSlices(request.NumberOfSlices, systemTilesForSlices);
        }

        if (systemTilesForSlices.BlueTiles.Count < request.NumberOfSlices * 3
            || systemTilesForSlices.RedTiles.Count < request.NumberOfSlices * 2)
        {
            _logger.LogError("Not enough system tiles to draft slices, generating preview slices");
            return GetPreviewSlices(request.NumberOfSlices, systemTilesForSlices);
        }

        var preparedSystemTiles = await _sliceSystemTilePreparer.PrepareSystemTilesForSlices(systemTilesForSlices, systemTilesForSliceRedistribution, request);

        var slices = await CreateBalancedSlices(request.NumberOfSlices, preparedSystemTiles, systemTilesForSlices);

        slices = await _miltyDraftSystemTileSetter.SetSystemTilesForSlices(preparedSystemTiles, slices);

        return slices;
    }

    private List<Slice> GetPreviewSlices(int numberOfSlices, SystemTilesForMapSetup systemTilesForSlices)
    {
        var slices = new List<Slice>();

        for (int i = 0; i < numberOfSlices; i++)
        {
            var slice = new Slice() { Id = i };
            slice.DraftedSystemTiles.Add(systemTilesForSlices.EmptyHomeSystemPlaceholder);
            slice.DraftedSystemTiles.Add(systemTilesForSlices.EmptySystemPlaceholder);
            slice.DraftedSystemTiles.Add(systemTilesForSlices.EmptySystemPlaceholder);
            slice.DraftedSystemTiles.Add(systemTilesForSlices.EmptySystemPlaceholder);
            slice.DraftedSystemTiles.Add(systemTilesForSlices.EmptySystemPlaceholder);
            slice.DraftedSystemTiles.Add(systemTilesForSlices.EmptySystemPlaceholder);

            slices.Add(slice);
        }

        return slices;
    }

    private async Task<List<Slice>> CreateBalancedSlices(
        int numberOfSlices,
        SystemTilesForSlices systemTilesforSlices,
        SystemTilesForMapSetup systemTilesForMapSetup)
    {
        var slices = GenerateRequiredNumberOfSlices(numberOfSlices);

        slices = InitializeHomePositions(slices, systemTilesForMapSetup.EmptyHomeSystemPlaceholder);
        slices = await _miltyDraftSliceBalancer.BalanceSlices(systemTilesforSlices, slices);

        return slices;
    }

    private List<Slice> GenerateRequiredNumberOfSlices(int numberOfSlices)
    {
        var slices = new List<Slice>();

        for (int i = 0; i < numberOfSlices; i++)
        {
            var slice = new Slice() { Id = i };

            slices.Add(slice);
        }

        return slices;
    }

    private List<Slice> InitializeHomePositions(List<Slice> slices, SystemTile emptyHomePositionPlaceholder)
    {
        for (int i = 0; i < slices.Count; i++)
        {
            slices[i].DraftedSystemTiles.Add(emptyHomePositionPlaceholder);
        }

        return slices;
    }
}
