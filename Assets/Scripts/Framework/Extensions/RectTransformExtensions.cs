using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RectTransformExtensions.cs
/// 
/// By Jorge L. Chávez Herrera.
/// </summary>
public static class RectTransformExtensions
{
	static public Rect GetScreenRect (this RectTransform rectTransform)
	{
		Vector3[] worldCorners = new Vector3[4];

		rectTransform.GetWorldCorners(worldCorners);

		Rect result = new Rect(
			worldCorners[0].x,
			worldCorners[0].y,
			worldCorners[2].x - worldCorners[0].x,
			worldCorners[2].y - worldCorners[0].y);
		return result;
	}
}
