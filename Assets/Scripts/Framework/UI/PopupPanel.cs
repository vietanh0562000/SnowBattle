using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Framework.UI
{
	/// <summary>
	/// PopupPanel.
	/// By Jorge L. Chávez Herrera
	///
	/// Privides functionality for animated popup panels.
	/// Derive all panels form PanelBase.
	public class PopupPanel : PanelBase
	{
	    #region Class members
	    public RectTransform panelRoot;
	    public MaskableGraphic dimmer;
	    public float dimmerFade = 0.75f;
	    #endregion

		#region OptimizedGameObject overrides
		override public RectTransform cachedRectTransform
		{
			get
			{
				return panelRoot;
			}
		}
		#endregion

	    #region PanelBase overrides
		protected override void TweenUpdate (float value)
		{
			base.TweenUpdate (value);

			if (dimmer != null)
			{
				dimmer.color = new Color (0, 0, 0, Mathf.Lerp (0, dimmerFade, value));
				dimmer.gameObject.SetActive (value != 0);
			}
		}
	    #endregion
	}
}
