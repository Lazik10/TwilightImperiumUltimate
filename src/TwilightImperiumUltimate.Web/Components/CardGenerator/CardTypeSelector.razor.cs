using Microsoft.AspNetCore.Components;
using System;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.CardGenerator;

public partial class CardTypeSelector
{
    [Parameter]
    public EventCallback<CardGenerationType> OnSelectedCardChange { get; set; } = default!;

    public CardGenerationType SelectedCardType { get; set; }

    public IReadOnlyCollection<KeyValuePair<CardGenerationType, string>> CardTypes { get; set; } = default!;

    public void SetCardType(CardGenerationType cardType)
    {
        SelectedCardType = cardType;
        if (OnSelectedCardChange.HasDelegate)
            OnSelectedCardChange.InvokeAsync(cardType);
    }

    protected override void OnInitialized()
    {
        CardTypes = Enum.GetValues(typeof(CardGenerationType))
                           .Cast<CardGenerationType>()
                           .Select(x => new KeyValuePair<CardGenerationType, string>(x, x.GetDisplayName()))
                           .ToList();
    }
}
