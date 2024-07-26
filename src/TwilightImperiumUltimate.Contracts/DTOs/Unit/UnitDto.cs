using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Unit;

public record UnitDto(
    int Id,
    UnitName UnitName,
    UnitType UnitType,
    int Cost,
    int Combat,
    int Move,
    int Capacity);
