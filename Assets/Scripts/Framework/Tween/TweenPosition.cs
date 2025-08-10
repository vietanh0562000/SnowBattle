using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Tweening;
using Framework.Delegates;
using Framework.Utils;


namespace Framework.Tweening
{
	/// <summary>
	/// Tween position.
	///
	/// By Jorge L. Chávez Herrera.
	/// </summary>
	public class TweenPosition : OptimizedGameObject 
	{
		#region class members
		public bool autoPlay;
		public Vector3 from;
		public Vector3 to;
		public float duration = 0.25f;
		public float delay = 0;
		public ETweenType tweenType;
		public bool loop = false;
		#endregion

		#region MoniBehaviour overrides
		private void Start ()
		{
			if (autoPlay == true)
				Play ();
		}
		#endregion

		#region Class Implementation
		public void Play (bool reverse = true)
		{
			StopAllCoroutines ();

			if (reverse == false) 
			{
				StartCoroutine (TweenUtils.TweenFloat (0, 1, duration, delay, ETweenType.EaseOut, TweenUpdate, TweenEnd));
			} 
			else 
			{
				StartCoroutine (TweenUtils.TweenFloat (1, 0, duration, delay, ETweenType.EaseOut, TweenUpdate, TweenEnd));
			}
		}

		protected virtual void TweenUpdate (float value)
		{
			cachedRectTransform.anchoredPosition3D = Vector3.Lerp (from, to, value);
		}

		protected virtual void TweenEnd() {
			if (loop) {
				Play ();
			}
		}
		#endregion
	}
}