namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class FaqData
{
    internal static List<Faq> Faqs => new List<Faq>
    {
        new()
        {
            Id = 1,
            ComponentName = "TheArborec",
            QuestionEnglish = "Question in english",
            AnswerEnglish = "Answer in english",
            QuestionCzech = "Question in czech",
            AnswerCzech = "Answer in czech",
            FaqStatus = FaqStatus.Submitted,
        },
    };
}
