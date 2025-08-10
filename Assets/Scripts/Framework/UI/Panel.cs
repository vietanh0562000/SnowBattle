using System.Collections;
using System.Collections.Generic;
using Framework.Utils;
using UnityEngine;

namespace Framework.UI
{
    /// <summary>
    /// Panel.
    /// By Jorge L. Chávez Herrera
    ///
    /// Privides basic functionality for UI panels.
    /// </summary>
    public class Panel : OptimizedGameObject
    {
        #region Class members
        public bool startsHidden; // Determines whether this panel will start hidden.

        private bool visible;     // Current visible state for this panel.

        private Tween[] childTweens;
        #endregion

        #region Class accessors
        #endregion

        #region Base class overrides
        #endregion

        #region Class implementation

        private void TweenUpdate(float time)
        {
            foreach (Tween tween in childTweens)
                tween.UpdateTween (time);
        }

        /// <summary>
        /// Shows the panel.
        /// </summary>
        public void Show(float delay = 0)
        {
            //UpdateInfo();
            visible = true;
            gameObject.SetActive (visible);
            //StartCoroutine(ShowCoroutine(delay));
        }
        /*
        private IEnumerator ShowCoroutine(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            // Freeze time
            if (freezeTime == true)
            {
                timeScaleStack.Push(Time.timeScale);
                Time.timeScale = 0;
            }

            cachedCanvasGroup.interactable = false;

            if (onBeginShow != null)
                onBeginShow();

            if (showHideCoroutine != null)
                StopCoroutine(showHideCoroutine);

            showHideCoroutine = TweenUtils.TweenFloat(0, 1, duration, 0, ETweenType.easeOut, TweenUpdate, EndShowTweenDelegate);
            StartCoroutine(showHideCoroutine);
        }

        /// <summary>
        /// Hides the panel.
        /// </summary>
        public void Hide(float delay = 0)
        {
            if (visible == true)
            {
                StartCoroutine(HideCoroutine(delay));
            }
        }

        private IEnumerator HideCoroutine(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);

            visible = false;

            cachedCanvasGroup.interactable = false;

            if (onBeginHide != null)
                onBeginHide();

            // Hide animation
            if (showHideCoroutine != null)
                StopCoroutine(showHideCoroutine);

            showHideCoroutine = TweenUtils.TweenFloat(1, 0, duration, 0, ETweenType.easeOut, TweenUpdate, EndHideTweenDelegate);
            StartCoroutine(showHideCoroutine);
        }

        protected virtual void TweenUpdate(float value)
        {
            cachedRectTransform.anchoredPosition3D = Vector3.Lerp(outPosition, inPosition, value);
            cachedRectTransform.localEulerAngles = Vector3.Lerp(outRotation, inRotation, value);
            cachedRectTransform.localScale = Vector3.Lerp(outScale, inScale, value);
            cachedCanvasGroup.alpha = Mathf.Lerp(outFade, inFade, value);
        }

        protected virtual void EndShowTweenDelegate()
        {
            cachedCanvasGroup.interactable = true;

            if (onEndShow != null)
                onEndShow();
        }

        protected virtual void EndHideTweenDelegate()
        {

            cachedCanvasGroup.interactable = true;

            if (onEndHide != null)
                onEndHide();

            // Unfreeze time if needed
            if (freezeTime == true)
            {
                Time.timeScale = timeScaleStack.Pop();
            }

            gameObject.SetActive(false);
        }*/
        #endregion

        #region MonoBehaviour events

        virtual protected void Awake()
        {
            visible = !startsHidden;

            childTweens = GetComponentsInChildren<Tween>();
            gameObject.SetActive(visible);
            TweenUpdate(0);
        }

        #endregion

        #region Interface implementation
        #endregion

        #region Nested classes
        #endregion
    }
}
