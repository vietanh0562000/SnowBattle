using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Framework.EditorUtils;


/// <summary>
/// Settings menu.
/// 
/// Editor class, adds menu option fro creating scriptable object settigs files.
/// 
/// By Jorge L. Chávez Herrera.
/// </summary>
public class SettingsMenu
{
    #region Class implementation

    [MenuItem("Assets/Create/SnowFight/Settings")]
    static public void CreateSettings()
    {
        ScriptableObjectUtility.CreateAsset<GameSettings>();
        Debug.Log("SettingsCreated!");
    }

    [MenuItem("Assets/Create/SnowFight/Create Snow Vehicle Data")]
    static public void CreateSnowVehicleData()
    {
        ScriptableObjectUtility.CreateAsset<SnowVehicleData>();
    }

    [MenuItem("Assets/Create/SnowFight/Clear Player Prefs")]
    static public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    #endregion
}
