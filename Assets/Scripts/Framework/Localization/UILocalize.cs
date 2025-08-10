using UnityEngine;
using UnityEngine.UI;
using System;


namespace Framework.Localization
{
	/// <summary>
	/// User interface localize.
	/// Attach this component to any Text gameobject to have localized text.
	/// 
	/// By Jorge L. Chávez Herrera.
	/// </summary>
	public class UILocalize : MonoBehaviour
	{
		#region Class members
	    public string key;
	    public string sheet;
		#endregion

		#region MonoBehaviour overrides
		private void Start() 
		{
			UpdateLabel ();
			Localization.onLanguageChanged += UpdateLabel;
		}

		private void OnDestroy () 
		{
			Localization.onLanguageChanged -= UpdateLabel;
		}
		#endregion

		#region Class implementattion
		private void UpdateLabel () 
		{
			Text label = GetComponentInChildren<Text>();
			label.text = GetLocalizedString ();
		}

		public string GetLocalizedString ()
		{
			return Localization.GetLocalizedString (key, sheet);
		}
		#endregion
	}
}