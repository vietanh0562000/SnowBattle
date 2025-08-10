using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PolarCoords
{
    #region Class memebers
    public float radius;
    public float angle;
    public float elevation;
    #endregion

    #region Class implementation
    /// <summary>
    /// Initializes a new instance of the <see cref="T:PolarCoords"/> class.
    /// </summary>
    /// <param name="radius">Radius.</param>
    /// <param name="angle">Angle.</param>
    /// <param name="elevation">Elevation.</param>
    public PolarCoords (float radius, float angle, float elevation)
    {
        this.radius = radius;
        this.angle = angle;
        this.elevation = elevation;
    }

    /// <summary>
    /// Returns a Vector3 with the converted cartesian coordinates.
    /// </summary>
    /// <returns>The cartesian.</returns>
    public Vector3 ToCartesian ()
    {
        return new Vector3 (Mathf.Sin (angle * Mathf.Deg2Rad) * -radius, Mathf.Sin(elevation * Mathf.Deg2Rad) * radius, Mathf.Cos (angle * Mathf.Deg2Rad) * radius);
    }

    public static PolarCoords SmoothDamp (PolarCoords current, PolarCoords target, ref PolarCoords currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
    {
        float radius = Mathf.SmoothDamp(current.radius, target.radius, ref currentVelocity.radius, smoothTime, deltaTime);
        float angle = Mathf.SmoothDampAngle(current.angle, target.angle, ref currentVelocity.angle, smoothTime, deltaTime);
        float elevation = Mathf.SmoothDampAngle(current.elevation, target.elevation, ref currentVelocity.elevation, smoothTime, deltaTime);

        return new PolarCoords(radius, angle, elevation);
    }

    public override string ToString()
    {
        return "(" + radius + "," + angle + "," + elevation + ")";
    }
    #endregion
}
