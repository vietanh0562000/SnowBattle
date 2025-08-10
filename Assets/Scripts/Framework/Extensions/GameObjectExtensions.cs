using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameObject extensions.
/// 
/// Extension methods for GameObjects.
/// By Jorge L. Chávez Herrera.
/// </summary>
public static class GameObjectExtensions
{
    /// <summary>
    /// Sets the layer for the root GameObject and all of its childs.
    /// </summary>
    /// <param name="go">Go.</param>
    /// <param name="layer">Layer.</param>
    public static void SetRootLayer(this GameObject go, int layer)
    {
        go.layer = layer;

        Transform[] childs = go.GetComponentsInChildren<Transform>(true);

        foreach (Transform t in childs)
        t.gameObject.layer = layer;
	}
}
