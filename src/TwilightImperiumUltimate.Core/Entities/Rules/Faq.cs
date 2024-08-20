using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Rules;

public class Faq : IEntity
{
    public int Id { get; set; }

    public string ComponentName { get; set; } = string.Empty;

    public string QuestionEnglish { get; set; } = string.Empty;

    public string AnswerEnglish { get; set; } = string.Empty;

    public string QuestionCzech { get; set; } = string.Empty;

    public string AnswerCzech { get; set; } = string.Empty;

    public FaqStatus FaqStatus { get; set; }
}
