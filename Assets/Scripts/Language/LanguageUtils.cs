using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Honeti;
using UnityEngine;

public static class LanguageUtils 
{
    public static string GetLanguageValue(string content)
    {
        if (string.IsNullOrEmpty(content))
            return string.Empty;
        if (!I18N.Instance)
            return string.Empty;
        if (I18N.Instance.HasKey(content))
            return I18N.Instance.getValue(content);
        else
        {
            return IsSnakeCase(content) ? ConvertSnakeCaseToTileCase(content) : content;
        }
    }

    public static bool IsSnakeCase(string content)
    {
        return content.Contains("_") && !content.Contains(" ");
    }

    public static string ConvertSnakeCaseToTileCase(string content)
    {
        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
        return ti.ToTitleCase(content.ToLower().Replace("_", " "));
    }
}