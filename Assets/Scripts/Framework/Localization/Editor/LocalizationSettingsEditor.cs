using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Framework.Localization 
{
	/// <summary>
	/// Localization settings editor.
	/// 
	/// Esitor script for editing localization settings.
	/// 
	/// By Jorge L. Chávez Herrera.
	/// </summary>
	[InitializeOnLoad]
	[CustomEditor(typeof(LocalizationSettings))]
	public class LocalizationSettingsEditor : Editor 
	{
		#region Editor overrides
		override public void OnInspectorGUI ()
		{
			NciteEditorTools.DrawNciteLogo ();
			NciteEditorTools.BlockHeader ("Localization Settings");

			LocalizationSettings.Instance.localizationDocKey = EditorGUILayout.TextField ("Localization Doc Key", LocalizationSettings.Instance.localizationDocKey);
			LocalizationSettings.Instance.localizationFilePath = EditorGUILayout.TextField ("Localization File Path", LocalizationSettings.Instance.localizationFilePath);


			if (GUILayout.Button ("Download Translations")) 
			{
				GoogleSpreadsheetReader.DownloadTranslations (LocalizationSettings.Instance.localizationDocKey,  LocalizationSettings.Instance.localizationFilePath);
			}

			if (GUI.changed)
				EditorUtility.SetDirty (LocalizationSettings.Instance);


		}
		#endregion
	}
}