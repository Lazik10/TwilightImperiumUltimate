using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Unit;

public record UnitWithCountDto(
    int Id,
    UnitName UnitName,
    UnitType UnitType,
    int Cost,
    int Combat,
    int Move,
    int Capacity,
    int Count)
    : UnitDto(Id, UnitName, UnitType, Cost, Combat, Move, Capacity);
