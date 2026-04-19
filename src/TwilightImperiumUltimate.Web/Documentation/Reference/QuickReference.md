# 🎯 Quick Reference Card - Responsive Components

## 📝 Text Components

### ResponsiveLabel
```razor
<ResponsiveLabel 
    Text="Your Text"
    FontSize="md"           // xs, sm, base, md, lg, xl, 2xl, 3xl
    TextColor="TextColor.White"
    CenterText="true"
    Visible="true" />
```

### ResponsiveRadzenText
```razor
<ResponsiveRadzenText 
    Text="Your Text"
    FontSize="lg"
    TextColor="TextColor.White"
    TextStyle="TextStyle.H1"
    TagName="TagName.H1" />
```

---

## 📐 Layout Components

### ResponsiveContainer
```razor
<ResponsiveContainer 
    Type="centered"         // fluid, centered, narrow, wide
    Padding="md">           // none, xs, sm, md, lg, xl
    <!-- Your content -->
</ResponsiveContainer>
```

### ResponsiveVerticalSpacer
```razor
<!-- Using predefined size -->
<ResponsiveVerticalSpacer Size="lg" />  // xs, sm, md, lg, xl, 2xl, 3xl

<!-- Using custom height -->
<ResponsiveVerticalSpacer CustomHeight="2.5rem" />
```

### ResponsiveHorizontalSpacer
```razor
<!-- Using predefined size -->
<ResponsiveHorizontalSpacer Size="md" />

<!-- Using custom width -->
<ResponsiveHorizontalSpacer CustomWidth="1.5rem" />
```

---

## 🎛️ Control Components

### ResponsiveButton
```razor
<ResponsiveButton 
    Text="Click Me"
    OnClick="@HandleClick"
    ButtonStyle="ButtonStyle.Primary"
    Size="ButtonSize.Medium"
    Disabled="false" />
```

### ResponsiveDataGrid
```razor
<ResponsiveDataGrid TItem="MyModel"
                    Data="@myData"
                    AllowPaging="true"
                    PageSize="10">
    <Columns>
        <RadzenDataGridColumn TItem="MyModel" 
                              Property="Name" 
                              Title="Name" />
    </Columns>
</ResponsiveDataGrid>
```

---

## 📏 Size Scales

### Font Sizes
| Size | Range (px) | Usage |
|------|-----------|-------|
| `xs` | 10-12 | Very small text |
| `sm` | 12-14 | Small text |
| `base` | 14-16 | **Default** body |
| `md` | 16-18 | Medium text |
| `lg` | 18-24 | Subheadings |
| `xl` | 24-32 | Titles |
| `2xl` | 28-42 | Page titles |
| `3xl` | 32-48 | Hero text |

### Spacing Sizes
| Size | Value | Pixels | Usage |
|------|-------|--------|-------|
| `xs` | 0.25rem | 4px | Tight |
| `sm` | 0.5rem | 8px | Small |
| `md` | 1rem | 16px | **Default** |
| `lg` | 1.5rem | 24px | Medium |
| `xl` | 2rem | 32px | Large |
| `2xl` | 3rem | 48px | XL |
| `3xl` | 4rem | 64px | Huge |

---

## 🎨 CSS Variables

### Spacing
```css
var(--space-xs)    /* 4px */
var(--space-sm)    /* 8px */
var(--space-md)    /* 16px */
var(--space-lg)    /* 24px */
var(--space-xl)    /* 32px */
var(--space-2xl)   /* 48px */
var(--space-3xl)   /* 64px */
```

### Typography
```css
var(--font-size-xs)    /* 10-12px */
var(--font-size-sm)    /* 12-14px */
var(--font-size-base)  /* 14-16px */
var(--font-size-md)    /* 16-18px */
var(--font-size-lg)    /* 18-24px */
var(--font-size-xl)    /* 24-32px */
var(--font-size-2xl)   /* 28-42px */
var(--font-size-3xl)   /* 32-48px */
```

### Colors
```css
var(--color-white)
var(--color-black)
var(--color-blue)
var(--color-gray-dark)
var(--color-gray-medium)
```

### Layout
```css
var(--max-content-width)    /* 1920px */
var(--border-radius-sm)     /* 0.5rem */
var(--border-radius-md)     /* 0.625rem */
var(--shadow-text)          /* Text shadow definition */
```

---

## 🔄 Quick Conversion

### Pixels → Rem
```
Formula: pixels ÷ 16 = rem

Examples:
4px   → 0.25rem  → var(--space-xs)
8px   → 0.5rem   → var(--space-sm)
16px  → 1rem     → var(--space-md)
24px  → 1.5rem   → var(--space-lg)
32px  → 2rem     → var(--space-xl)
48px  → 3rem     → var(--space-2xl)
64px  → 4rem     → var(--space-3xl)
```

### Old Component → New Component
```
Label               → ResponsiveLabel
VerticalSpace       → ResponsiveVerticalSpacer
HorizontalSpace     → ResponsiveHorizontalSpacer
(inline styles)     → ResponsiveContainer
Button              → ResponsiveButton
DataGrid            → ResponsiveDataGrid
```

---

## 🎯 Common Patterns

### Page Title with Spacing
```razor
<ResponsiveLabel Text="Page Title" FontSize="2xl" />
<ResponsiveVerticalSpacer Size="2xl" />
```

### Section with Content
```razor
<ResponsiveContainer Type="centered" Padding="lg">
    <ResponsiveLabel Text="Section Title" FontSize="xl" />
    <ResponsiveVerticalSpacer Size="lg" />
    <ResponsiveRadzenText Text="Content..." FontSize="base" />
</ResponsiveContainer>
```

### Button Group
```razor
<ResponsiveButton Text="Save" OnClick="@Save" />
<ResponsiveHorizontalSpacer Size="sm" />
<ResponsiveButton Text="Cancel" OnClick="@Cancel" />
```

---

## 📱 Breakpoints

```css
/* Mobile */
@media screen and (max-width: 480px) { }

/* Tablet */
@media screen and (max-width: 768px) { }

/* Desktop */
@media screen and (min-width: 769px) { }
```

---

## ✅ Best Practices

1. ✅ **Always use semantic sizes** (`md`, `lg`, etc.) instead of custom values
2. ✅ **Wrap page content** in `ResponsiveContainer`
3. ✅ **Use CSS variables** in custom styles
4. ✅ **Test on mobile first**, then scale up
5. ✅ **Avoid inline pixel values** whenever possible

---

## ❌ Common Mistakes

1. ❌ `style="font-size: 18px;"` → ✅ `FontSize="lg"`
2. ❌ `Width="200"` → ✅ Remove (let component auto-size)
3. ❌ `<div style="height: 50px;">` → ✅ `<ResponsiveVerticalSpacer Size="3xl" />`
4. ❌ Fixed container widths → ✅ `ResponsiveContainer` with `Type`

---

## 🚀 Import Statements

```razor
@using TwilightImperiumUltimate.Web.Components.Shared.Text
@using TwilightImperiumUltimate.Web.Components.Shared.Layouts
@using TwilightImperiumUltimate.Web.Components.Shared.Controls
```

---

## 📚 Related Documentation

- **[Responsive Design Guide](../Instructions/ResponsiveDesignGuide.md)** - Full implementation guide
- **[Before/After Examples](../Examples/BeforeAfterExamples.md)** - Visual comparisons
- **[Refactoring Summary](../Instructions/RefactoringSummary.md)** - Implementation summary
- **[StyleCop Compliance](../Standards/StyleCopCompliance.md)** - Code quality standards
- **[Documentation Index](../README.md)** - All documentation

---

**Print this card and keep it handy while refactoring! 📄**
