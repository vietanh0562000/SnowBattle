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
public static class Vector3Extensions
{
    /*
    static public Vector3 PolarOffset (float distance, float height, float angle)
    {
        return new Vector3 (Mathf.Sin (angle * Mathf.Deg2Rad) * -distance, height, Mathf.Cos (angle * Mathf.Deg2Rad) * distance);
    }*/

    /// <summary>
    /// Returns true if any value on this vector is Nan.
    /// </summary>
    /// <returns><c>true</c>, if nan was ised, <c>false</c> otherwise.</returns>
    /// <param name="v">V.</param>
    static public bool IsNan (this Vector3 v)
    {
        return float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z);
    }

    static public Vector3 FromSpherical (float radius, float angle, float elevation)
    {
        /*
        Vector3 ret;
        float a = radius * Mathf.Cos (elevation);
        ret.x = a * Mathf.Cos (polar);
        ret.y = radius * Mathf.Sin (elevation);
        ret.z = a * Mathf.Sin (polar);*/

        return new Vector3(Mathf.Sin (angle * Mathf.Deg2Rad) * -radius, elevation, Mathf.Cos(angle * Mathf.Deg2Rad) * radius);;
    }

    /*
    /// <summary>
    /// Sets the parent for the transform and resets all transformations
    /// </summary>
    /// <param name="t">T.</param>
    /// <param name="parent">Parent.</param>
    static public void ParentReset(this Transform t, Transform parent)
    {
        t.SetParent(parent);
        t.ResetTransformation();
    }

    /// <summary>
    /// Resets the local position, rotation & scale of the transformation.
    /// </summary>
    /// <param name="t">T.</param>
    static public void ResetTransformation(this Transform t)
    {
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// Destroys all children of this transform.
    /// </summary>
    /// <param name="t">T.</param>
    static public void DestroyChildren(this Transform t)
    {
        while (t.childCount > 0)
        {
            GameObject.DestroyImmediate(t.GetChild(0).gameObject);
        }
    }*/
}
