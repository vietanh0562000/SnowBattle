using UnityEngine;
using UnityEditor;
using System.Collections;
using Framework.EditorUtils;

namespace Framework.Localization 
{
	public class EditorMenu : EditorWindow 
	{
		#if UNITY_EDITOR
		[MenuItem("Window/Ncite/Google Doc Localization")]

		static void OpenLocalizationSettings() 
		{
			UnityEditor.Selection.activeObject = LocalizationSettings.Instance;
		}

		#region Class implementation
		[MenuItem("Assets/Create/NciteFramework/Localization/SpreadSheetData")]
		static public void CreateSpreadSheetData()
		{
			ScriptableObjectUtility.CreateAsset<SpreadsheetData>();
		}
		#endregion
		#endif
	}
}

