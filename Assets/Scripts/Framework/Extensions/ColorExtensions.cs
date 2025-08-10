using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Color extensions.
/// 
/// By Jorge L. Chávez Herrera.
/// 
/// Extensions methods for Color struct.
/// </summary>
public static class ColorExtensions
{
    // Converts to RGBA color to HSVA color.
    static public HSVColor ToHSVColor(this Color color)
    {
        return HSVColor.FromColor (color);
    }
}
