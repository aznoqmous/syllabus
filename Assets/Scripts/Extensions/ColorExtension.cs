using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ColorExtension
{
    public static ColorHSV ToHSV(this Color color)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);
        return new ColorHSV() { h = h, s = s, v = v, a = color.a };
    }
    
    public static Color HueRotate(this Color color, float value)
    {
        ColorHSV colorHsv = color.ToHSV();
        colorHsv.h += value;
        return colorHsv.ToRGB();
    }
    public static Color MoveSaturation(this Color color, float value)
    {
        ColorHSV colorHsv = color.ToHSV();
        colorHsv.s += value;
        return colorHsv.ToRGB();
    }
    public static Color MoveLightness(this Color color, float value)
    {
        ColorHSV colorHsv = color.ToHSV();
        colorHsv.v += value;
        return colorHsv.ToRGB();
    }

    public static Color SetHue(this Color color, float value)
    {
        ColorHSV colorHsv = color.ToHSV();
        colorHsv.h = value;
        return colorHsv.ToRGB();
    }

    public static Color SetLightness(this Color color, float value)
    {
        ColorHSV colorHsv = color.ToHSV();
        colorHsv.v = value;
        return colorHsv.ToRGB();
    }

    public static Color SetSaturation(this Color color, float value)
    {
        ColorHSV colorHsv = color.ToHSV();
        colorHsv.s = value;
        return colorHsv.ToRGB();
    }

}

public class ColorHSV
{
    float _h, _s, _v, _a;

    public float h
    {
        get { return _h; }
        set { _h = value % 1f;}
    }
    public float s
    {
        get { return _s; }
        set { _s = Mathf.Clamp(value, 0,1f); }
    }
    public float v
    {
        get { return _v; }
        set { _v = Mathf.Clamp(value, 0, 1f); }
    }
    public float a
    {
        get { return _a; }
        set { _a = Mathf.Clamp(value, 0,1f); }
    }

    public Color ToRGB()
    {
        return Color.HSVToRGB(h, s, v);
    }
}