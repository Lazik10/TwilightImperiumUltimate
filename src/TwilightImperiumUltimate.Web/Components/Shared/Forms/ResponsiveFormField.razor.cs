using System.Linq.Expressions;

namespace TwilightImperiumUltimate.Web.Components.Shared.Forms;

public partial class ResponsiveFormField<TValue>
{
    /// <summary>
    /// Gets or sets the id shared with the caller's own input element (for the label's `for`
    /// attribute). The caller must set the same id on their rendered input.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Id { get; set; } = string.Empty;

    [Parameter]
    [EditorRequired]
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the field expression used to render a validation message for this field
    /// (e.g. <c>() =&gt; Model.Email</c>). When null, no validation message is rendered.
    /// </summary>
    [Parameter]
    public Expression<Func<TValue>>? For { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a visual "required" indicator is shown next to
    /// the label. Purely presentational -- actual required validation is still owned by the
    /// model's FluentValidation validator.
    /// </summary>
    [Parameter]
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the label text is centered instead of the default
    /// left alignment, so callers can flexibly match either style.
    /// </summary>
    [Parameter]
    public bool CenterLabel { get; set; }

    [Parameter]
    public string? HelpText { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the field's input content, typically a Radzen input component bound to the
    /// model property named in <see cref="For"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the validation display mode cascaded down from the enclosing
    /// <see cref="ResponsiveForm"/>. Defaults to <see cref="Enums.ValidationDisplay.Field"/> when
    /// this field is used outside a <see cref="ResponsiveForm"/> (no cascaded value available).
    /// </summary>
    [CascadingParameter]
    private Enums.ValidationDisplay? CascadedValidationDisplay { get; set; }

    private bool ShowFieldValidationMessage =>
        (CascadedValidationDisplay ?? Enums.ValidationDisplay.Field) == Enums.ValidationDisplay.Field;

    private string GetValidationMessageId() => $"{Id}-validation";

    private string GetHelpTextId() => $"{Id}-help";

    private string GetLabelCssClass() =>
        CenterLabel ? "responsive-form-field-label responsive-form-field-label-centered" : "responsive-form-field-label";
}
