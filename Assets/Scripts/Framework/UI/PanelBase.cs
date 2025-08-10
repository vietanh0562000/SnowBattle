using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.Tweening;
using Framework.Delegates;
using Framework.Utils;


namespace Framework.UI
{
	/// <summary>
	/// Panel Base.
	/// By Jorge L. Chávez Herrera
	///
	/// Privides basic functionality for animated UI panels.
	/// Derive all panels form PanelBase.
	/// </summary>
	[RequireComponent(typeof(CanvasGroup))]
	public class PanelBase : OptimizedGameObject
	{
	    #region Class members
	    public bool startsHidden;
		public bool freezeTime = false;
	    public Vector3 outPosition;
	    public Vector3 outRotation;
	    public Vector3 outScale = Vector3.one;
	    public float outFade = 1;
	    public Vector3 inPosition;
	    public Vector3 inRotation;
	    public Vector3 inScale = Vector3.one;
	    public float inFade = 1;
	    public float duration = 0.25f;

		public SimpleDelegate onBeginShow; // Executed before playing back the showing animation.
		public SimpleDelegate onEndShow;   // Executed after playing back the showing animation.
		public SimpleDelegate onBeginHide; // Executed before playing back the hiding animation.
		public SimpleDelegate onEndHide;   // Executed after playing back the hiding animation. 

	    protected bool visible;
		static private Stack<float> timeScaleStack = new Stack<float> ();

		private IEnumerator showHideCoroutine;
	    #endregion

		#region Class accessors
		/// <summary>
		/// Returns the cached canvas group component.
		/// </summary>
		/// <value>The cached canvas group.</value>
		private CanvasGroup _cachedCanvasGroup;
		public CanvasGroup cachedCanvasGroup
		{
			get 
			{
				if (_cachedCanvasGroup == null)
					_cachedCanvasGroup = GetComponent<CanvasGroup>();

				return _cachedCanvasGroup;
			}
		}
		#endregion

	    #region MonoBehaviour overrides
	    virtual protected void Awake ()
	    {
	        visible = !startsHidden;

            if (!visible)
            {
                TweenUpdate(0);
            }

			gameObject.SetActive (visible);
	    }
	    #endregion

	    #region Class implementation
	    /// <summary>
	    /// Show the panel.
	    /// </summary>
		public void Show (float delay = 0)
	    {
	        visible = true;
	        gameObject.SetActive (true);
			UpdateInfo();
			StartCoroutine (ShowCoroutine (delay));
	    }

		private IEnumerator ShowCoroutine (float delay)
		{
			yield return new WaitForSecondsRealtime (delay);
			// Freeze time
			if (freezeTime == true) 
			{
				timeScaleStack.Push (Time.timeScale);
				Time.timeScale = 0;
			}

			//cachedCanvasGroup.interactable = false;

			if (onBeginShow != null)
				onBeginShow ();

			if (showHideCoroutine != null)
				StopCoroutine (showHideCoroutine);

			showHideCoroutine = TweenUtils.TweenFloat (0, 1, duration, 0, ETweenType.EaseOut, TweenUpdate, EndShowTweenDelegate);
			StartCoroutine (showHideCoroutine);
		}

	    /// <summary>
	    /// Hides the panel.
	    /// </summary>
		public void Hide (float delay = 0)
	    {
			if (visible == true) 
			{
				StartCoroutine (HideCoroutine (delay));
			}
	    }

		private IEnumerator HideCoroutine (float delay)
		{
			yield return new WaitForSecondsRealtime (delay);

			visible = false;

			//cachedCanvasGroup.interactable = false;

			if (onBeginHide != null)
				onBeginHide ();

			// Hide animation
			if (showHideCoroutine != null)
				StopCoroutine (showHideCoroutine);

			showHideCoroutine = TweenUtils.TweenFloat (1, 0, duration, 0, ETweenType.EaseOut, TweenUpdate, EndHideTweenDelegate);
			StartCoroutine (showHideCoroutine);
		}

		protected virtual void TweenUpdate (float value)
		{
			cachedRectTransform.anchoredPosition3D = Vector3.Lerp (outPosition, inPosition, value);
			cachedRectTransform.localEulerAngles = Vector3.Lerp (outRotation, inRotation, value);
			cachedRectTransform.localScale = Vector3.Lerp (outScale, inScale, value);
			cachedCanvasGroup.alpha = Mathf.Lerp (outFade, inFade, value);
		}

		protected virtual void EndShowTweenDelegate ()
		{
			//cachedCanvasGroup.interactable = true;

			if (onEndShow != null)
				onEndShow ();
		}

		protected virtual void EndHideTweenDelegate ()
		{
			
			//cachedCanvasGroup.interactable = true;

			if (onEndHide != null)
				onEndHide ();

			// Unfreeze time if needed
			if (freezeTime == true) 
			{
				Time.timeScale = timeScaleStack.Pop ();
			}
			
			gameObject.SetActive(false);
		}
			
		/// <summary>
		/// Override this function to populate panel widgets before playing back the show animation.
		/// </summary>
		virtual public void UpdateInfo() {}
	    #endregion
	}
}
		