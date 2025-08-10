using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Framework.Localization;


/// SpreadsheetDataEditor
/// Editor for SpreadsheetData scirptable object files
/// By Jorge L. Chávez Herrera.
[InitializeOnLoad]
[CustomEditor(typeof(SpreadsheetData))]
public class SpreadsheetDataEditor : Editor 
{
	#region Editor overrides
	override public void OnInspectorGUI()
	{
		SpreadsheetData spreadsheetData = target as SpreadsheetData;
		SpreadsheetData.Cell cellToRemove = null;

		NciteEditorTools.DrawNciteLogo ();
		NciteEditorTools.BlockHeader ("Settings");

		spreadsheetData.googleDocKey =  EditorGUILayout.TextField ("Google Doc Key", spreadsheetData.googleDocKey);

		if (GUILayout.Button ("Add Cell")) 
		{
			spreadsheetData.cells.Add (new SpreadsheetData.Cell ("Default", 0, "A", 0));
		}

		for (int i = 0; i < spreadsheetData.cells.Count; i++)
		{
			EditorGUILayout.BeginHorizontal ();
			spreadsheetData.cells [i].key = EditorGUILayout.TextField (spreadsheetData.cells [i].key);
			spreadsheetData.cells [i].sheet = EditorGUILayout.IntField (spreadsheetData.cells [i].sheet, GUILayout.Width (25));
			spreadsheetData.cells [i].column = EditorGUILayout.TextField (spreadsheetData.cells [i].column.ToUpper (), GUILayout.Width (25));
			spreadsheetData.cells [i].row = EditorGUILayout.IntField (spreadsheetData.cells [i].row, GUILayout.Width (50));
			EditorGUILayout.TextField (spreadsheetData.cells [i].value, GUILayout.Width (100));

			GUI.backgroundColor = Color.red;
			if (GUILayout.Button ("x")) 
			{
				cellToRemove = spreadsheetData.cells [i];
			}
			GUI.backgroundColor = Color.white;

			EditorGUILayout.EndHorizontal ();
		}

		if (GUILayout.Button ("Download Cell Data")) 
		{
			GoogleSpreadsheetReader.DownloadRawCellData (spreadsheetData.googleDocKey, spreadsheetData.cells);
		}

		if (cellToRemove != null)
			spreadsheetData.cells.Remove (cellToRemove);
			
		if (GUI.changed)
			EditorUtility.SetDirty (spreadsheetData);
	}
	#endregion
}