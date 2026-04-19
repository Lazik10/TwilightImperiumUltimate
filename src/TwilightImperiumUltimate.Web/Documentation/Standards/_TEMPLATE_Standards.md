# [Standard Name] - Coding Standards

> **Category:** Standards  
> **Last Updated:** [Date]  
> **Enforced By:** [Tool/Analyzer Name]

---

## 📋 Overview

Brief description of the coding standard and why it's important.

**This standard covers:**
- Aspect 1
- Aspect 2
- Aspect 3

---

## 📐 Rules & Guidelines

### **Rule 1: [Rule Name]**

**Description:** What this rule requires

**Applies to:**
- Code element type 1
- Code element type 2

**Example (Correct):**
```csharp
// Correct implementation
public class Example
{
    // Code following the rule
}
```

**Example (Incorrect):**
```csharp
// Incorrect implementation
public class Example
{
    // Code violating the rule
}
```

**Analyzer Rule:** `[Rule ID]` (e.g., SA1234)

---

### **Rule 2: [Rule Name]**

**Description:** What this rule requires

**Applies to:**
- Code element type 1
- Code element type 2

**Example (Correct):**
```csharp
// Correct implementation
```

**Example (Incorrect):**
```csharp
// Incorrect implementation
```

**Analyzer Rule:** `[Rule ID]`

---

## ✅ Compliance Checklist

When writing code, ensure:

- [ ] Requirement 1
- [ ] Requirement 2
- [ ] Requirement 3
- [ ] Requirement 4
- [ ] Requirement 5

---

## 🔧 Configuration

### **EditorConfig Settings:**

```ini
# Standard configuration
[*.cs]
rule_id = severity_level
```

### **Project File Settings:**

```xml
<PropertyGroup>
    <Setting>Value</Setting>
</PropertyGroup>
```

---

## 🎯 Code Templates

### **Template 1: [Element Type]**

```csharp
/// <summary>
/// [Description].
/// </summary>
public [Type] [Name]
{
    // Implementation
}
```

### **Template 2: [Element Type]**

```csharp
/// <summary>
/// [Description].
/// </summary>
/// <param name="paramName">[Parameter description].</param>
/// <returns>[Return value description].</returns>
public [ReturnType] [MethodName]([Type] paramName)
{
    // Implementation
}
```

---

## 🚨 Common Violations

### **Violation 1: [Description]**

**Error Message:** `[Analyzer error message]`

**Problem:**
```csharp
// Code causing violation
```

**Solution:**
```csharp
// Corrected code
```

---

### **Violation 2: [Description]**

**Error Message:** `[Analyzer error message]`

**Problem:**
```csharp
// Code causing violation
```

**Solution:**
```csharp
// Corrected code
```

---

## 📊 Enforcement

| Level | Meaning | Action |
|-------|---------|--------|
| **Error** | Must be fixed | Build fails |
| **Warning** | Should be fixed | Build succeeds with warnings |
| **Info** | Suggestion only | No build impact |

---

## 🔍 Validation

### **How to Check:**

1. Build the project
2. Review analyzer messages
3. Fix any violations
4. Rebuild to confirm

### **Automated Tools:**

- Tool 1: Description
- Tool 2: Description

---

## 📚 Related Standards

- [Related Standard 1](RelatedStandard1.md)
- [Related Standard 2](RelatedStandard2.md)

---

## 🔄 Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0.0 | YYYY-MM-DD | Initial version |

---

## 🔗 Additional Resources

- [Official Documentation](external-link)
- [Style Guide](external-link)
- [Main Index](../README.md)
