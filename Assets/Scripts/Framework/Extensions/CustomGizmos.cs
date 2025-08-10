using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gizmo extensions.
/// 
/// By Jorge L. Chávez Herrera.
/// 
/// Extensions methods for Gizmos.
/// </summary>
public static class CustomGizmos
{
    /// <summary>
    /// Draws a line circle.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="steps"></param>
    /// <param name="radius"></param>
    public static void DrawLineCircle(Vector3 position, int steps, float radius)
    {
        for (int i = 0; i < steps; i++)
        {
            float angle = i * (360.0f / steps);
            float nextAngle = (i + 1) * (360.0f / steps);

            Vector3 pos = new Vector3(Mathf.Cos(angle * Mathf.PI / 180) * radius, 0, Mathf.Sin(angle * Mathf.PI / 180) * radius);
            Vector3 nextPos = new Vector3(Mathf.Cos(nextAngle * Mathf.PI / 180) * radius, 0, Mathf.Sin(nextAngle * Mathf.PI / 180) * radius);
            Gizmos.DrawLine(position + pos, position + nextPos);
        }
    }
}
