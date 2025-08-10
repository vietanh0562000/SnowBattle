using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework.UI
{
	/// <summary>
	/// Converser.
	/// ScriptableObject defines data & functionality for characters shown 
	/// in the conversation dialog.
	/// 
	/// By Jorge L. Chávez Herrera.
	/// </summary>
	public class Converser : ScriptableObject 
	{
		#region Class members
		public Sprite[] sprites;
		public string nameKey;
		public string nameSheet;
		public Vector3 scale = Vector3.one;
		public Vector3 offset;
		#endregion

		#region Class implementation
		/// <summary>
		/// Gets the name for this converser character.
		/// </summary>
		/// <returns>The name.</returns>
		public string GetName ()
		{
			return Framework.Localization.Localization.GetLocalizedString (nameKey, nameSheet);
		}

		/// <summary>
		/// Returns the sprite best suited for mood or neutral.
		/// </summary>
		/// <returns>The sprite for mood.</returns>
		/// <param name="mood">Mood.</param>
		public Sprite GetSpriteForMood (EConverserMood mood)
		{
			Sprite result = sprites [(int)EConverserMood.Neutral];

			if (sprites[(int)mood] != null)
				result = sprites[(int)mood];
			
			return result;
		}
		#endregion
	}
}
