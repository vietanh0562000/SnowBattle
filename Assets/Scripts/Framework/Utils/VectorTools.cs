using UnityEngine;
using System.Collections;

namespace Framework.Utils
{	
	public static class VectorTools 
	{
		/// <summary>
		/// Gets the angle betwen 2 vectors, with north as 0 & incrementing clockwise
		/// </summary>
		/// <returns>The angle.</returns>
		/// <param name="v1">V1.</param>
		/// <param name="v2">V2.</param>
		static public float GetAngle (Vector2 v1, Vector2 v2)
		{
			float r = Mathf.Atan2 (v2.x - v1.x, v2.y - v1.y);
			float a = (r / Mathf.PI) * 180;
			
			// We don't want negative values
			if (a < 0) 
				a+= 360;
			
			return a;
		}
			
		/// <summary>
		/// Gets the angle betwen 2 vectors
		/// </summary>
		/// <returns>The angle.</returns>
		/// <param name="v1">V1.</param>
		/// <param name="v2">V2.</param>
		public static float GetAngleXZ (Vector3 v1, Vector3 v2)
		{
			float r = Mathf.Atan2 (v2.x - v1.x, v2.z - v1.z);
			float a = (r / Mathf.PI) * 180;
			
			// We don't want negative values
			if (a < 0) 
				a+= 360;
			
			return a;
		}
		
		
		/// <summary>
		/// Returns a vector with the same derection from center but with a clamped the distance.
		/// </summary>
		/// <returns>The distance.</returns>
		/// <param name="center">Center.</param>
		/// <param name="other">Other.</param>
		/// <param name="distance">Distance.</param>
		static public Vector2 ClampDistance (Vector2 center, Vector2 other, float distance)
		{
			Vector2 ret = other;
			float dst = Vector2.Distance(center, other);
			
			if (dst > distance)
			{
				Vector2 vect = center - other;
				vect = vect.normalized;
				vect *= (dst - distance);
				other += vect;
				ret = other;
			}
			
			return ret;
		}
		
		/// <summary>
		/// Sphericals the offset.
		/// </summary>
		/// <returns>The offset.</returns>
		/// <param name="v">V.</param>
		static public Vector3 SphericalOffset (Vector3 v) 
		{
			return new Vector3 (Mathf.Sin(v.z * Mathf.Deg2Rad) * -v.x, v.y, Mathf.Cos(v.z * Mathf.Deg2Rad) * -v.x); 
		}
		
		/// <summary>
		/// Gets a random XZ point inside a circle.
		/// </summary>
		/// <returns>The XZ point inside circle.</returns>
		/// <param name="radius">Radius.</param>
		static public Vector3 RandomXZPointInsideCircle (float radius)
		{
			Vector3 rnd = Random.insideUnitCircle;
			
			return new  Vector3 (rnd.x * radius, 0, rnd.y * radius);
		}
	}
}
