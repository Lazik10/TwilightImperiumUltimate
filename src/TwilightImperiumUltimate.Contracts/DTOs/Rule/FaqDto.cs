using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Rule;

public record FaqDto(
    int Id,
    string ComponentName,
    string QuestionEnglish,
    string AnswerEnglish,
    string QuestionCzech,
    string AnswerCzech,
    FaqStatus FaqStatus);
