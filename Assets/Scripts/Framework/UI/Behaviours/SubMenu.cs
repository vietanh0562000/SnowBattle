using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.Tweening;

namespace Framework.UI
{
	[RequireComponent(typeof(LayoutElement))]
	[RequireComponent(typeof(VerticalLayoutGroup))]
	/// <summary>
	/// SubMenu.
	/// </summary>
	public class SubMenu : MonoBehaviour
	{
	    #region Class members
	    public float transitionDuration = 0.25f;
	    private LayoutElement layoutElement;
	    private bool expanded = false;
	    private float uncollapsedHeight;
	    #endregion

	    #region MonoBehaviour overrides
	    private void Awake()
	    {
	        layoutElement = GetComponent<LayoutElement>();
	        GetComponent<VerticalLayoutGroup>().enabled = true;
	    }

	    private void Start()
	    {
	        Canvas.ForceUpdateCanvases();
	        uncollapsedHeight = GetComponent<RectTransform>().sizeDelta.y;
	        GetComponent<VerticalLayoutGroup>().enabled = false;
	        layoutElement.minHeight = 0;
	    }
	    #endregion

	    #region Class implementation
	    public void Toggle()
	    {
	        if (expanded == false)
	            Expand();
	        else
	            Collapse();
	    }

	    public void Expand()
	    {
	        expanded = true;
	        StopAllCoroutines();
	        StartCoroutine(TweenUtils.TweenFloat(layoutElement.minHeight, uncollapsedHeight, transitionDuration, 0, ETweenType.EaseInOut, UpdateLayoutElementMinHeight));  
	    }

	    public void Collapse()
	    {
	        expanded = false;
	        StopAllCoroutines();
	        StartCoroutine(TweenUtils.TweenFloat(layoutElement.minHeight, 0, transitionDuration, 0, ETweenType.EaseInOut, UpdateLayoutElementMinHeight));
	    }



	    private void UpdateLayoutElementMinHeight(float value)
	    {
	        layoutElement.minHeight = value;
	    } 
	    #endregion
	}
}
