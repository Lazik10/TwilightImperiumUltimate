using Microsoft.AspNetCore.Components.Forms;

namespace TwilightImperiumUltimate.Web.Components.Shared.Forms;

public partial class ResponsiveForm : IDisposable
{
    private bool _disposed;
    private bool _hasValidationErrors;
    private EditContext? _editContext;

    /// <summary>
    /// Gets or sets the bound form model. A new <see cref="EditContext"/> is created whenever
    /// this instance changes.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public object Model { get; set; } = default!;

    /// <summary>
    /// Gets or sets the field content, typically a set of <see cref="ResponsiveFormField{TValue}"/>
    /// instances. Rendered inside a <see cref="Layouts.ResponsiveGridContainer"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets content that replaces the default submit button, for forms that need
    /// additional or different actions (e.g. multiple buttons, secondary links).
    /// </summary>
    [Parameter]
    public RenderFragment? FooterContent { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the form passes validation on submit.
    /// </summary>
    [Parameter]
    public EventCallback<EditContext> OnValidSubmit { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the form fails validation on submit.
    /// </summary>
    [Parameter]
    public EventCallback<EditContext> OnInvalidSubmit { get; set; }

    /// <summary>
    /// Gets or sets the number of field columns on desktop. Passed straight through to the
    /// underlying <see cref="Layouts.ResponsiveGridContainer"/>.
    /// </summary>
    [Parameter]
    public int Columns { get; set; } = 1;

    [Parameter]
    public int TabletColumns { get; set; }

    [Parameter]
    public int MobileColumns { get; set; }

    [Parameter]
    public string Gap { get; set; } = "var(--space-lg)";

    /// <summary>
    /// Gets or sets a value indicating whether a validation summary is shown above the fields
    /// once the form has failed validation.
    /// </summary>
    [Parameter]
    public bool ShowValidationSummary { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the form is currently submitting. While true, the
    /// submit button is disabled and shows <see cref="SubmittingText"/> instead of
    /// <see cref="SubmitText"/>. The caller owns this flag (matches the existing
    /// _isSubmitting/_loggingIn pattern used across this app's pages) so the actual async submit
    /// logic stays in the page, not in this generic shell.
    /// </summary>
    [Parameter]
    public bool IsSubmitting { get; set; }

    /// <summary>
    /// Gets or sets an additional caller-controlled condition that disables the submit button,
    /// independent of <see cref="IsSubmitting"/>.
    /// </summary>
    [Parameter]
    public bool SubmitDisabled { get; set; }

    [Parameter]
    public string SubmitText { get; set; } = "Submit";

    [Parameter]
    public string SubmittingText { get; set; } = "Submit";

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    private bool IsSubmitDisabled => IsSubmitting || SubmitDisabled;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing && _editContext is not null)
            _editContext.OnValidationStateChanged -= HandleValidationStateChanged;

        _disposed = true;
    }

    protected override void OnParametersSet()
    {
        if (_editContext is null || !ReferenceEquals(_editContext.Model, Model))
        {
            if (_editContext is not null)
                _editContext.OnValidationStateChanged -= HandleValidationStateChanged;

            _editContext = new EditContext(Model);
            _editContext.OnValidationStateChanged += HandleValidationStateChanged;
        }
    }

    private void HandleValidationStateChanged(object? sender, ValidationStateChangedEventArgs e)
    {
        _hasValidationErrors = _editContext?.GetValidationMessages().Any() == true;
        InvokeAsync(StateHasChanged);
    }

    private string GetGridCssClass() =>
        Columns == 1 ? "responsive-form-grid responsive-narrow-column" : "responsive-form-grid";

    private async Task HandleValidSubmit(EditContext context)
    {
        if (OnValidSubmit.HasDelegate)
            await OnValidSubmit.InvokeAsync(context);
    }

    private async Task HandleInvalidSubmit(EditContext context)
    {
        if (OnInvalidSubmit.HasDelegate)
            await OnInvalidSubmit.InvokeAsync(context);
    }
}
