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
	public class TweenColor : OptimizedGameObject 
	{
		#region Class members
		public Color startColor = Color.white;
		public Color endColor = Color.black;
		public float duration;
		#endregion

		#region MonoBehaviour overrides
		private void Start () 
		{
			StartCoroutine (TweenUtils.TweenFloat (1, 0, duration, 0, ETweenType.NoEase, UpdateDelegate, TweenEndDelegate));
		}
		#endregion

		#region Class implementattion
		private void UpdateDelegate (float value)
		{
			cachedSpriteRenderer.color = Color.Lerp (startColor, endColor, value);
		}

		private void TweenEndDelegate ()
		{
			StartCoroutine (TweenUtils.TweenFloat (0, 1, duration, 0, ETweenType.NoEase, UpdateDelegate, Start));
		}
		#endregion
	}
}
