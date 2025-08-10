using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Framework.Delegates;
using Framework.Utils;

namespace Framework.Gameplay
{
	/// <summary>
	/// Base camera.
	/// By Jorge L. Chávez Herrera.
	/// 
	/// Defines basic camera functionality for fading, shaking & off center.
	/// </summary>
    [RequireComponent(typeof(Camera))]
	public class BaseCamera : OptimizedGameObject 
	{	
		#region Class membersa
		public Color fadeColor;
		public Material fadeMaterial;
		public float fade = 1;
		[System.NonSerialized]
		public float fadeDuration;
		
		public Vector2 viewOffset;
		
		const float MAX_SHAKE_TIME = 0.05f;
		[System.NonSerialized] public float shakeIntensity = 0; // Holds the intensity for camera shaking
		private float shakeTime = 0;
		private Quaternion shakeRotation, newShakeRotation;
		
		public SimpleDelegate afterLateUpdateDelegate; /// Will be called after LateUpdate
		static public bool isFading;
		#endregion
		
		#region MonoBehaviour overrides
        virtual protected IEnumerator Start ()
		{
            yield return new WaitForEndOfFrame();
			FadeIn ();
		}

		/// <summary>
		/// Sets de off center projection matrix & other settings, overriders must always call base
		/// </summary>
		virtual protected void LateUpdate ()
		{	
			float top = cachedCamera.nearClipPlane * Mathf.Tan (cachedCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
			float bottom = -top; 
			float left = bottom * cachedCamera.aspect; 
			float right = top * cachedCamera.aspect; 
			
			Vector2 offset = new Vector2(viewOffset.x * (right - left), viewOffset.y * (top-bottom));
			
			cachedCamera.projectionMatrix = PerspectiveOffCenter (left + offset.x, right + offset.x, bottom + offset.y, top + offset.y, cachedCamera.nearClipPlane, cachedCamera.farClipPlane);
			
			// Apply shake
			shakeTime+= Time.unscaledDeltaTime;
			
			if (shakeTime > MAX_SHAKE_TIME)
			{
				newShakeRotation= GetShakeRotation();
				shakeTime = 0;
			}
			
			shakeRotation = Quaternion.Slerp (shakeRotation, newShakeRotation, Time.unscaledDeltaTime * 100);
			cachedTransform.rotation*= Quaternion.Slerp (Quaternion.identity, shakeRotation, shakeIntensity);
			
			
			if (afterLateUpdateDelegate != null)
				afterLateUpdateDelegate ();
		}

		/// <summary>
		/// Renders a screen aligned quad for the fade effect
		/// </summary>
		private IEnumerator OnPostRender() 
		{

			yield return new WaitForEndOfFrame();

			// Draw a quad over the whole screen for fading
            if (fadeMaterial && fade > 0) 
			{
				fadeMaterial.SetColor("_Color", new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1) * Mathf.SmoothStep(0,1,fade));
				fadeMaterial.SetPass(0);

				GL.PushMatrix ();

				GL.Viewport (cachedCamera.pixelRect);
				GL.LoadOrtho();
				GL.LoadIdentity();

				GL.Begin( GL.QUADS );	
				GL.Vertex3( 1, 0, 0);
				GL.Vertex3( 1, 1, 0);
				GL.Vertex3( 0, 1, 0);
				GL.Vertex3( 0, 0, 0 );
				GL.End();

				GL.PopMatrix ();
			}	
		}
		#endregion
		
		#region Class implementation
		public bool IsOffScreen (Vector3 worldPosition, float margin = 0)
		{
			Vector2 viewportPoint = cachedCamera.WorldToViewportPoint (worldPosition);	
			return !(viewportPoint.x > 0 - margin && viewportPoint.x < 1 + margin && viewportPoint.y > 0 - margin && viewportPoint.y < 1 + margin); 
		}
		
		private Quaternion GetShakeRotation () 
		{
			return Quaternion.Euler (Random.onUnitSphere);
		}
			
		public void SetFade (float fadeValue)
		{
			fade = fadeValue;
		}
		
		/// <summary>
		/// Fades the camera out.
		/// </summary>
		public void FadeOut (float duration = 0.25f, float delay = 0) 
		{	
			StopAllCoroutines ();
			fadeDuration = duration;
			StartCoroutine (FadeCoroutine (0, 1, duration, delay));
		}
		
		/// <summary>
		/// Fades the camera in.
		/// </summary>
		public void FadeIn (float duration = 0.25f, float delay = 0) 
		{
			StopAllCoroutines ();
			fadeDuration = duration;
			StartCoroutine (FadeCoroutine(1, 0, duration, delay));
		}

		/// <summary>
		/// interpolates fade variable with the specified time
		/// </param>
		IEnumerator FadeCoroutine (float from, float to, float time, float delay) 
		{	
			yield return new WaitForSeconds (delay);

			isFading = true;

			for (float i = 0; i < time; i+= Time.deltaTime) 
			{
				fade = Mathf.Lerp (from, to, i  / time);
		
				yield return null;
			}

			fade = to;
			isFading = false;
		}

		/// <summary>
		/// Starts the camera shake coroutine.
		/// </summary>
		/// <param name="duration">Duration in seconds that the shake will last</param>
		public void Shake (float duration)
		{
			StartCoroutine (ShakeCoroutine (duration));
		}

		/// <summary>
		/// Actual camera shake function
		/// </summary>
		/// <param name="duration">Duration in seconds that the shake will last.</param>
		IEnumerator ShakeCoroutine (float duration)
		{
			shakeIntensity = 1;
			yield return new WaitForSecondsRealtime (duration);
			
			for (float t = 0; t <= 1; t+= Time.unscaledDeltaTime * 2) 
			{
				shakeIntensity = 1-t;
				yield return null;
			}
			
			shakeIntensity = 0;
		}

		/// <summary>
		/// Crates an of center perspective projection matrix.
		/// </summary>
		/// <returns>The off center.</returns>
		/// <param name="left">Left.</param>
		/// <param name="right">Right.</param>
		/// <param name="bottom">Bottom.</param>
		/// <param name="top">Top.</param>
		/// <param name="near">Near.</param>
		/// <param name="far">Far.</param>
		static Matrix4x4 PerspectiveOffCenter (float left, float right, float bottom, float top, float near, float far) 
		{
			float x = 2.0F * near / (right - left);
			float y = 2.0F * near / (top - bottom);
			float a = (right + left) / (right - left);
			float b = (top + bottom) / (top - bottom);
			float c = -(far + near) / (far - near);
			float d = -(2.0F * far * near) / (far - near);
			float e = -1.0F;
			Matrix4x4 m = new Matrix4x4();
			
			m[0, 0] = x; m[0, 1] = 0; m[0, 2] = a; m[0, 3] = 0;
			m[1, 0] = 0; m[1, 1] = y; m[1, 2] = b; m[1, 3] = 0;
			m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = c; m[2, 3] = d;
			m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = e; m[3, 3] = 0;
			
			return m;
		}
		#endregion
	}
}