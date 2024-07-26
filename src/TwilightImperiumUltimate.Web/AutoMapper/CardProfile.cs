using TwilightImperiumUltimate.Contracts.DTOs.Card;

namespace TwilightImperiumUltimate.Web.AutoMapper;

public class CardProfile : Profile
{
    public CardProfile()
    {
        CreateMap<BaseCardDto, CardModel>();
        CreateMap<PromissoryNoteCardDto, PromissoryNoteCardModel>();
    }
}
