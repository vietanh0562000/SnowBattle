using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Transform extensions.
/// 
/// By Jorge L. Chávez Herrera.
/// 
/// Extensions methods for Transform component.
/// </summary>
public static class TransformExtensions
{
	/// <summary>
	/// Sets the parent for the transform and resets all transformations
	/// </summary>
	/// <param name="t">T.</param>
	/// <param name="parent">Parent.</param>
	static public void ParentReset (this Transform t, Transform parent)
	{
		t.SetParent (parent);
		t.ResetTransformation ();
	}

	/// <summary>
	/// Resets the local position, rotation & scale of the transformation.
	/// </summary>
	/// <param name="t">T.</param>
	static public void ResetTransformation (this Transform t)
	{
		t.localPosition = Vector3.zero;
		t.localRotation = Quaternion.identity;
		t.localScale = new Vector3(1, 1, 1);
	}

	/// <summary>
	/// Destroys all children of this transform.
	/// </summary>
	/// <param name="t">T.</param>
	static public void DestroyChildren (this Transform t)
	{
		while (t.childCount > 0) 
		{
			GameObject.DestroyImmediate (t.GetChild (0).gameObject);
		}
	}
}
