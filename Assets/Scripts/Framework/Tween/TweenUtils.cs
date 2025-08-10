using System.Collections;
using UnityEngine;
using Framework.Delegates;


namespace Framework.Tweening
{
	public enum ETweenType { NoEase, EaseOut, EaseIn, EaseInOut, BounceOut};

	/// <summary>
	/// TweenUtils.
	/// By Jorge L. Chávez Herrera.
	/// 
	/// Defines coroutines useful for tweening Float, Vector2 & Vector3 values.
	/// Delegates for Update & tween end can be provided. 
	/// Speficy the use of Uncaled or Scaled time.
	/// </summary>
	public static class TweenUtils
	{
		#region Class members
		static public AnimationCurve noEaseCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0, 0.75f, 0.75f), new Keyframe(1, 1) });
		static public AnimationCurve easeOutCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0, 0.75f, 0.75f), new Keyframe(1, 1) });
		static public AnimationCurve easeInCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1, 1.5f, 1.5f) });
		static public AnimationCurve easeInOutCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0, 0.75f, 0.75f), new Keyframe(1, 1) });
		static public AnimationCurve bounceCurve = new AnimationCurve (new Keyframe[] { new Keyframe (0, 0, 4, 4), new Keyframe (0.5f, 1f), new Keyframe (1, 0, -4, -4)});
        static public AnimationCurve bounceCurve3 = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0, 4, 4), new Keyframe(0.5f, 2f), new Keyframe(1, 1, -4, -4) });

        static public AnimationCurve bounceOutCurve2 = new AnimationCurve(new Keyframe[]
        {
            new Keyframe (0,0,0.07789838f,0.07789838f,0,0.5f),
            new Keyframe (0.5f,1,3.917495f,-3.185179f,0.1066752f,0.5112603f),
            new Keyframe (0.65f,0.75f,0,0,0,0),
            new Keyframe (0.8f,1,3.463799f,-1.871237f,0.5f,0.5f),
            new Keyframe (0.9f,0.9f,0,0,0.15f,0.15f),
            new Keyframe (1.0f,1,2.012381f,0,0.4891294f,0),
        });

        static public AnimationCurve bounceOutCurve = new AnimationCurve(new Keyframe[]
        {
            new Keyframe (0.000000f,0.000000f,10.710800f,10.710800f,0.000000f,0.206804f),
            new Keyframe (0.625000f,1.000000f,-4.990141f,3.466648f,0.320586f,0.428737f),
            new Keyframe (0.760677f,1.250000f,0.002363f,0.002363f,0.326222f,0.371945f),
            new Keyframe (0.900000f,1.000000f,-3.057821f,3.515377f,0.482728f,0.362515f),
            new Keyframe (0.950000f,1.101887f,0.000000f,0.000000f,0.603669f,0.241969f),
            new Keyframe (1.000000f,1.000000f,-5.013115f,0.000000f,0.352130f,0.000000f),
        });

        static private AnimationCurve[] animationCurves = new AnimationCurve[] { noEaseCurve, easeOutCurve, easeInCurve, easeInOutCurve, bounceOutCurve };
		#endregion

		#region Class implementation
		public delegate void UpdateFloatDelegate (float value);
		public delegate void UpdateVector2Delegate (Vector2 value);
		public delegate void UpdateVector3Delegate (Vector3 value);

		/// <summary>
        /// Tweens a float.
        /// </summary>
        /// <returns>The float.</returns>
        /// <param name="mono">Mono.</param>
        /// <param name="startValue">Start value.</param>
        /// <param name="endValue">End value.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="delay">Delay.</param>
        /// <param name="tweenType">Tween type.</param>
        /// <param name="updateFloatDelegate">Update float delegate.</param>
        /// <param name="tweenEndDelegate">Tween end delegate.</param>
        /// <param name="unscaledTime">If set to <c>true</c> unscaled time.</param>
        static public IEnumerator TweenFloat (MonoBehaviour mono, float startValue, float endValue, float duration, float delay, ETweenType tweenType, UpdateFloatDelegate updateFloatDelegate, SimpleDelegate tweenEndDelegate = null, bool unscaledTime = true)
		{
            IEnumerator coroutine = TweenFloat (startValue, endValue, duration, delay, tweenType, updateFloatDelegate, tweenEndDelegate, unscaledTime);
            mono.StartCoroutine (coroutine);
            return coroutine;
		}


		/// <summary>
        /// Tweens a float.
        /// </summary>
        /// <returns>The float.</returns>
        /// <param name="startValue">Start value.</param>
        /// <param name="endValue">End value.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="delay">Delay.</param>
        /// <param name="tweenType">Tween type.</param>
        /// <param name="updateFloatDelegate">Update float delegate.</param>
        /// <param name="tweenEndDelegate">Tween end delegate.</param>
        /// <param name="unscaledTime">If set to <c>true</c> unscaled time.</param>
		static public IEnumerator TweenFloat (float startValue, float endValue, float duration, float delay, ETweenType tweenType, UpdateFloatDelegate updateFloatDelegate, SimpleDelegate tweenEndDelegate = null, bool unscaledTime = true)
		{
			yield return new WaitForSeconds (delay);

			AnimationCurve animationCurve = animationCurves[(int)tweenType];

            if (duration > 0)
            {
                for (float t = 0; t <= duration; t += unscaledTime == true ? Time.unscaledDeltaTime : Time.deltaTime)
                {
                    updateFloatDelegate(Mathf.LerpUnclamped(startValue, endValue, animationCurve.Evaluate(t / duration)));
                    yield return null;
                }
            }

			updateFloatDelegate (endValue);

			if (tweenEndDelegate != null)
				tweenEndDelegate ();
		}

        /// <summary>
        /// Tweens a Vector2.
        /// </summary>
        /// <returns>The vector2.</returns>
        /// <param name="mono">Mono.</param>
        /// <param name="startValue">Start value.</param>
        /// <param name="endValue">End value.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="delay">Delay.</param>
        /// <param name="tweenType">Tween type.</param>
        /// <param name="updateFloatDelegate">Update float delegate.</param>
        /// <param name="tweenEndDelegate">Tween end delegate.</param>
        /// <param name="unscaledTime">If set to <c>true</c> unscaled time.</param>
        static public IEnumerator TweenVector2 (MonoBehaviour mono, Vector2 startValue, Vector2 endValue, float duration, float delay, ETweenType tweenType, UpdateVector2Delegate updateFloatDelegate, SimpleDelegate tweenEndDelegate = null, bool unscaledTime = true)
        {
            IEnumerator coroutine = TweenVector2 (startValue, endValue, duration, delay, tweenType, updateFloatDelegate, tweenEndDelegate, unscaledTime);
            mono.StartCoroutine(coroutine);
            return coroutine;
        }

		/// <summary>
		/// Tweens a Vector2.
		/// </summary>
		/// <returns>The vector2.</returns>
		/// <param name="startValue">Start value.</param>
		/// <param name="endValue">End value.</param>
		/// <param name="duration">Duration.</param>
		/// <param name="delay">Delay.</param>
		/// <param name="tweenType">Tween type.</param>
		/// <param name="updateVector2Delegate">Update vector2 delegate.</param>
		/// <param name="tweenEndDelegate">Tween end delegate.</param>
		/// <param name="unscaledTime">If set to <c>true</c> unscaled time.</param>
		static public IEnumerator TweenVector2 (Vector2 startValue, Vector2 endValue, float duration, float delay, ETweenType tweenType, UpdateVector2Delegate updateVector2Delegate, SimpleDelegate tweenEndDelegate = null, bool unscaledTime = true)
		{
			yield return new WaitForSeconds (delay);

			AnimationCurve animationCurve = animationCurves[(int)tweenType];

            if (duration > 0)
            {
                for (float t = 0; t <= duration; t += unscaledTime == true ? Time.unscaledDeltaTime : Time.deltaTime)
                {
                    updateVector2Delegate(Vector2.LerpUnclamped(startValue, endValue, animationCurve.Evaluate(t / duration)));
                    yield return null;
                }
            }

			updateVector2Delegate (endValue);

			if (tweenEndDelegate != null)
				tweenEndDelegate ();
		}

        /// <summary>
        /// Tweens a Vector3.
        /// </summary>
        /// <returns>The vector3.</returns>
        /// <param name="mono">Mono.</param>
        /// <param name="startValue">Start value.</param>
        /// <param name="endValue">End value.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="delay">Delay.</param>
        /// <param name="tweenType">Tween type.</param>
        /// <param name="updateFloatDelegate">Update float delegate.</param>
        /// <param name="tweenEndDelegate">Tween end delegate.</param>
        /// <param name="unscaledTime">If set to <c>true</c> unscaled time.</param>
        static public IEnumerator TweenVector3 (MonoBehaviour mono, Vector3 startValue, Vector3 endValue, float duration, float delay, ETweenType tweenType, UpdateVector3Delegate updateFloatDelegate, SimpleDelegate tweenEndDelegate = null, bool unscaledTime = true)
        {
            IEnumerator coroutine = TweenVector3 (startValue, endValue, duration, delay, tweenType, updateFloatDelegate, tweenEndDelegate, unscaledTime);
            mono.StartCoroutine(coroutine);
            return coroutine;
        }

		/// <summary>
		/// Tweens a Vector3.
		/// </summary>
		/// <returns>The vector3.</returns>
		/// <param name="startValue">Start value.</param>
		/// <param name="endValue">End value.</param>
		/// <param name="duration">Duration.</param>
		/// <param name="delay">Delay.</param>
		/// <param name="tweenType">Tween type.</param>
		/// <param name="updateVector3Delegate">Update vector3 delegate.</param>
		/// <param name="tweenEndDelegate">Tween end delegate.</param>
		/// <param name="unscaledTime">If set to <c>true</c> unscaled time.</param>
		static public IEnumerator TweenVector3 (Vector3 startValue, Vector3 endValue, float duration, float delay, ETweenType tweenType, UpdateVector3Delegate updateVector3Delegate, SimpleDelegate tweenEndDelegate = null, bool unscaledTime = true)
		{
			if (delay > 0)
				yield return new WaitForSeconds (delay);

			AnimationCurve animationCurve = animationCurves[(int)tweenType];

            if (duration > 0)
            {
                for (float t = 0; t <= duration; t += unscaledTime == true ? Time.unscaledDeltaTime : Time.deltaTime)
                {
                    updateVector3Delegate(Vector3.LerpUnclamped(startValue, endValue, animationCurve.Evaluate(t / duration)));
                    yield return null;
                }
            }

			updateVector3Delegate (endValue);

			if (tweenEndDelegate != null)
				tweenEndDelegate ();
		}
		#endregion
	}
}
