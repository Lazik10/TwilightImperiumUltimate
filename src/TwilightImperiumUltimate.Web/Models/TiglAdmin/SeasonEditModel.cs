using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;

namespace TwilightImperiumUltimate.Web.Models.TiglAdmin;

public class SeasonEditModel(SeasonDto dto)
{
    private readonly SeasonDto _original = dto;

    public int SeasonNumber { get; set; } = dto.SeasonNumber;

    public string SeasonName { get; set; } = dto.Name;

    public bool IsActive { get; set; } = dto.IsActive;

    public DateTime StartDateValue { get; set; } = dto.StartDate.ToDateTime(TimeOnly.MinValue);

    public DateTime EndDateValue { get; set; } = dto.EndDate.ToDateTime(TimeOnly.MinValue);

    public bool IsEditing { get; set; }

    public DateOnly StartDate => DateOnly.FromDateTime(StartDateValue);

    public DateOnly EndDate => DateOnly.FromDateTime(EndDateValue);

    public void Reset()
    {
        SeasonName = _original.Name;
        IsActive = _original.IsActive;
        StartDateValue = _original.StartDate.ToDateTime(TimeOnly.MinValue);
        EndDateValue = _original.EndDate.ToDateTime(TimeOnly.MinValue);
    }
}
