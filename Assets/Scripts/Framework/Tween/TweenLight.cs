using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Utils;


namespace Framework.Tweening
{
	/// <summary>
	/// TweenColor.cs
	/// Tweens renderer's main color from start to endColor.
	/// 
	/// By Jorge L. Chávez Herrera.
	/// </summary>
	public class TweenLight : OptimizedGameObject 
	{
		#region Class members
		public float startIntensity = 0;
		public float endIntensity = 1;
		public float duration;

        private Light cachedLight;
		#endregion

		#region MonoBehaviour overrides
		private void Awake ()
		{
			cachedLight = GetComponent<Light> ();
		}

		private void Start () 
		{
			StartCoroutine (TweenUtils.TweenFloat (1, 0, duration, 0, ETweenType.NoEase, UpdateDelegate, TweenEndDelegate));
		}
		#endregion

		#region Class implementattion
		private void UpdateDelegate (float value)
		{
			cachedLight.intensity = Mathf.Lerp (startIntensity, endIntensity, value);
		}

		private void TweenEndDelegate ()
		{
			StartCoroutine (TweenUtils.TweenFloat (0, 1, duration, 0, ETweenType.NoEase, UpdateDelegate, Start));
		}
		#endregion
	}
}
