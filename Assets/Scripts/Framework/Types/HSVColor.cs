using UnityEngine;

/// <summary>
/// HSVColor.cs
/// 
/// Implements Hue, Saturation & Value color model & conversions to RGB.
/// 
/// By Jorge L. Chávez Herrera.
/// </summary>
[System.Serializable]
public struct HSVColor
{
    public float h;
    public float s;
    public float v;
    public float a;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="HSVColor"/> struct.
	/// </summary>
	/// <param name="h">The height.</param>
	/// <param name="s">S.</param>
	/// <param name="v">V.</param>
	public HSVColor (float h, float s, float v)
	{
        this.h = h;
        this.s = s;
        this.v = v;
        this.a = 1f;
    }
	
	/// <summary>
	/// Initializes a new instance of the <see cref="HSVColor"/> struct.
	/// </summary>
	/// <param name="h">The height.</param>
	/// <param name="s">S.</param>
	/// <param name="v">V.</param>
	/// <param name="a">The alpha component.</param>
    public HSVColor (float h, float s, float v, float a)
    {
        this.h = h;
        this.s = s;
		this.v = v;
        this.a = a;
    }

    /// <summary>
    /// Returns an instance of HSVColor from RGBA
    /// </summary>
    /// <returns>The RGB.</returns>
    /// <param name="r">The red component.</param>
    /// <param name="g">The green component.</param>
    /// <param name="b">The blue component.</param>
    /// <param name="a">The alpha component.</param>
	static public HSVColor FromRGBA(float r, float g, float b, float a)
	{
		Vector4 K = new Vector4 (0.0f, -1.0f / 3.0f, 2.0f / 3.0f, -1.0f);
		Vector4 p = g < b ? new Vector4 (b, g, K.w, K.z) : new Vector4 (g, b, K.x, K.y);
		Vector4 q = r < p.x ? new Vector4 (p.x, p.y, p.w, r) : new Vector4 (r, p.y, p.z, p.x);

    	float d = q.x - Mathf.Min(q.w, q.y);
    	float e = 1.0e-10f;
    	return new HSVColor (Mathf.Abs(q.z + (q.w - q.y) / (6.0f * d + e)) * 360, d / (q.x + e), q.x);
	}

	/// <summary>
	/// Returns an instance of HSVColor from Color
	/// </summary>
	/// <returns>The color.</returns>
	/// <param name="c">C.</param>
	static public HSVColor FromColor(Color c)
	{
		Vector4 K = new Vector4(0.0f, -1.0f / 3.0f, 2.0f / 3.0f, -1.0f);
		Vector4 p = c.g < c.b ? new Vector4(c.b, c.g, K.w, K.z) : new Vector4(c.g, c.b, K.x, K.y);
		Vector4 q = c.r < p.x ? new Vector4(p.x, p.y, p.w, c.r) : new Vector4(c.r, p.y, p.z, p.x);

    	float d = q.x - Mathf.Min(q.w, q.y);
    	float e = 1.0e-10f;
    	return new HSVColor(Mathf.Abs(q.z + (q.w - q.y) / (6.0f * d + e)) * 360, d / (q.x + e), q.x);
	}

	/// <summary>
	/// Converts to an RGB Color
	/// </summary>
	/// <returns>
	/// The color.
	/// </returns>
	public Color ToColor() 
	{
		int hi = (int)Mathf.Floor(h / 60.0f) % 6;
		float f = (h / 60.0f) - Mathf.Floor(h / 60.0f);

		float p = v * (1.0f - s);
		float q = v * (1.0f - (f * s));
		float t = v * (1.0f - ((1.0f - f) * s));

		Color result;

		switch (hi)
		{
		case 0:
			result = new Color(v, t, p, a);
			break;
		case 1:
			result = new Color(q, v, p, a);
			break;
		case 2:
			result = new Color(p, v, t, a);
			break;
		case 3:
			result = new Color(p, q, v, a);
			break;
		case 4:
			result = new Color(t, p, v, a);
			break;
		case 5:
			result = new Color(v, p, q, a);
			break;
		default:
			result = new Color(0, 0, 0, a);
			break;
		}
		return result;
	}

	override public string ToString()
	{
		return string.Format("h:{0} s:{1} v:{2}", h,s,v);
	}
}

public class HSVColorBuffer
{
	public HSVColor[] data;
	private readonly int height;
	private readonly int width;

	public HSVColorBuffer(int width, int height, Color[] colorData)
	{
		this.width = width;
		this.height = height;

		this.data = new HSVColor[colorData.Length];

		for (int i = 0; i < colorData.Length; i++)
			this.data[i] = HSVColor.FromColor(colorData[i]);
	}

	public HSVColor this[int x, int y]
	{
		get
		{
			return data[(y * width)+height];
		}
		set
		{
			data[(y * width) + x] = value;
		}
	}
}