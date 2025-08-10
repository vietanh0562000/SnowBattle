using System.Collections;
using System.Collections.Generic;
using Framework.Utils;
using UnityEngine;
using System;


public class GameSettings : ScriptableObject
{
    #region Class members
    public Data data;
    #endregion

    #region Class implementation
    public string GetJsonData()
    {
        return JsonUtility.ToJson(data);
    }

    public void SetJsonData(string serializedJsonData)
    {
        data = JsonUtility.FromJson<Data>(serializedJsonData);
    }
    #endregion

    [System.Serializable]
    public class Data
    {
        #region Class members
        [Header("Player")]
        public float playerSpeed = 5;
        public float playerTurnSpeed = 2f;
        public float playerSnowballGrowSpeed = 0.3f;
        public float playerPunchForce = 10f;
        public float playerPerfectPunchForce = 20;

        [Header("Player Leveling Up")]
        public float playerKillSizeIncrement = 0.1f;
        public float playerKillPunchForceIncrement = 0.1f;
        public float playerKillSpeedIncrement = 0.1f;
        public float playerKillSnowballGrowSpeedIncrement = 0.1f;

        [Header("NPC")]
        public float npcSpeed = 5;
        public float npcTurnSpeed = 5;
        public float npcSnowballGrowSpeed = 0.15f;
       
        public float npcPunchForce = 12f;
        public float npcPerfectPunchForce = 20;

        [Header("NPC Leveling Up")]
        public float npcKillSizeIncrement = 0.1f;
        public float npcKillPunchForceIncrement = 0.1f;
        public float npcKillSpeedIncrement = 0.1f;
        public float npcKillSnowballGrowSpeedIncrement = 0.1f;
        public int npcCount = 10;

        [Header("Snowball")]
        public float snowballSpeed = 7.5f;
        public float snowballMaxSize = 2;

        [Header("Punch")]
        public float punchYAmplitude = 0.1f;

        [Header("Buffs")]
        public float buffDuration = 5;
        public float speedBuffMultiplier = 2;
        public float punchForceBuffMultiplier = 2;
        public float sizeBuffMultiplier = 2;

        [Header("Traps")]
        public float freezeTime = 7.5f;

        [Header("Daily Bonus")]
        public int[] dailyBonusValues;

        [Header("Debug & Check")]
        public bool CheckGameplayVersionValues = false;
    }
    #endregion

    #region Playerprefs Variables
    private string PREF_playerSpeed = "PREF_playerSpeed";
    private string PREF_playerTurnSpeed = "PREF_playerTurnSpeed";
    private string PREF_playerSnowballGrowSpeed = "PREF_playerSnowballGrowSpeed";
    private string PREF_playerPunchForce = "PREF_playerPunchForce";
    private string PREF_playerPerfectPunchForce = "PREF_playerPerfectPunchForce";


    private string PREF_npcSpeed = "PREF_npcSpeed";
    private string PREF_npcTurnSpeed = "PREF_npcTurnSpeed";
    private string PREF_npcSnowballGrowSpeed = "PREF_npcSnowballGrowSpeed";
    private string PREF_npcPunchForce = "PREF_npcPunchForce";
    private string PREF_npcPerfectPunchForce = "PREF_nPerfectPunchForce";
    private string PREF_npcCount = "PREF_npcCount";


    private string PREF_snowballSpeed = "PREF_snowballSpeed";
    private string PREF_snowballMaxSize = "PREF_snowballMaxSize";


    private string PREF_punchForce = "PREF_punchForce";
    private string PREF_perfectPunchForce = "PREF_perfectPunchForce";
    private string PREF_punchYAmplitude = "PREF_punchYAmplitude";


    private string PREF_killSizeIncrement = "PREF_killSizeIncrement";
    private string PREF_killPunchForceIncrement = "PREF_killPunchForceIncrement";
    private string PREF_killSpeedIncrement = "PREF_killSpeedIncrement";


    private string PREF_buffDuration = "PREF_buffDuration";
    private string PREF_speedBuffMultiplier = "PREF_speedBuffMultiplier";
    private string PREF_punchForceBuffMultiplier = "PREF_punchForceBuffMultiplier";
    private string PREF_sizeBuffMultiplier = "PREF_sizeBuffMultiplier";


    private string PREF_freezeTime = "PREF_freezeTime";
    #endregion

    public void ApplyGameSettingsValues()
    {
        // APPLY SETTINGS THAT ARE SAVED IN THE PLAYER PREFS

        data.playerSpeed = float.Parse(PlayerPrefs.GetString(PREF_playerSpeed));
        data.playerTurnSpeed = float.Parse(PlayerPrefs.GetString(PREF_playerTurnSpeed));
        data.playerSnowballGrowSpeed = float.Parse(PlayerPrefs.GetString(PREF_playerSnowballGrowSpeed));

        data.npcSpeed = float.Parse(PlayerPrefs.GetString(PREF_npcSpeed));
        data.npcTurnSpeed = float.Parse(PlayerPrefs.GetString(PREF_npcTurnSpeed));
        data.npcSnowballGrowSpeed = float.Parse(PlayerPrefs.GetString(PREF_npcSnowballGrowSpeed));
        data.npcCount = Int32.Parse(PlayerPrefs.GetString(PREF_npcCount));

        data.snowballSpeed = float.Parse(PlayerPrefs.GetString(PREF_snowballSpeed));
        data.snowballMaxSize = float.Parse(PlayerPrefs.GetString(PREF_snowballMaxSize));

        data.playerPunchForce = float.Parse(PlayerPrefs.GetString(PREF_punchForce));
        data.playerPerfectPunchForce = float.Parse(PlayerPrefs.GetString(PREF_perfectPunchForce));
        data.punchYAmplitude = float.Parse(PlayerPrefs.GetString(PREF_punchYAmplitude));

        data.npcKillSizeIncrement = float.Parse(PlayerPrefs.GetString(PREF_killSizeIncrement));
        data.npcKillPunchForceIncrement = float.Parse(PlayerPrefs.GetString(PREF_killPunchForceIncrement));
        data.npcKillSpeedIncrement = float.Parse(PlayerPrefs.GetString(PREF_killSpeedIncrement));

        data.buffDuration = float.Parse(PlayerPrefs.GetString(PREF_buffDuration));
        data.speedBuffMultiplier = float.Parse(PlayerPrefs.GetString(PREF_speedBuffMultiplier));
        data.punchForceBuffMultiplier = float.Parse(PlayerPrefs.GetString(PREF_punchForceBuffMultiplier));
        data.sizeBuffMultiplier = float.Parse(PlayerPrefs.GetString(PREF_sizeBuffMultiplier));

        data.freezeTime = float.Parse(PlayerPrefs.GetString(PREF_freezeTime));

        /*
        if (CheckGameplayVersionValues)
        {
            Debug.Log("APPLIED VALUES:");

            Debug.Log("Player Speed:" + data.playerSpeed);
            Debug.Log("Player Turn Speed:" + data.playerTurnSpeed);
            Debug.Log("Player Snow Ball Grow Speed:" + data.playerSnowballGrowSpeed);
            Debug.Log("Player Punch Force:" + data.playerPunchForce);
            Debug.Log("Perfect Punch Force:" + data.playerPerfectPunchForce);

            Debug.Log("Npc Speed:" + data.npcSpeed);
            Debug.Log("Npc Turn Speed:" + data.npcTurnSpeed);
            Debug.Log("Npc Snow Ball Grow Speed:" + data.npcSnowballGrowSpeed);
            Debug.Log("Npc Punch Force:" + data.npcPunchForce);
            Debug.Log("Npc Perfect Punch Force:" + data.npcPerfectPunchForce);

            Debug.Log("Npc Count:" + data.npcCount);

            Debug.Log("Snowball Speed:" + data.snowballSpeed);
            Debug.Log("Snowball Max Size:" + data.snowballMaxSize);

            Debug.Log("Punch Y Amplitude:" + data.punchYAmplitude);

            Debug.Log("Kill Size Increment:" + data.killSizeIncrement);
            Debug.Log("Kill Punch Force Increment:" + data.killPunchForceIncrement);
            Debug.Log("Kill Speed Increment:" + data.killSpeedIncrement);

            Debug.Log("Buff Duration:" + data.buffDuration);
            Debug.Log("Speed Buff Multiplier:" + data.speedBuffMultiplier);
            Debug.Log("Punch Force Buff Multiplier:" + data.punchForceBuffMultiplier);
            Debug.Log("Size Buff Multiplier:" + data.sizeBuffMultiplier);

            Debug.Log("Freeze Time:" + data.freezeTime);
        }*/
    }

    public void UpdateGameSettingsValues(string result)
    {
        // UPDATE THE SAVED SETTINGS IN PLAYER PREFS + APPLY THEM (PREVIOUS FUNCTION)

        string[] resultat = result.Split(';');

        PlayerPrefs.SetString(PREF_playerSpeed , resultat [1]);
        PlayerPrefs.SetString(PREF_playerTurnSpeed, resultat[2]);
        PlayerPrefs.SetString(PREF_playerSnowballGrowSpeed, resultat[3]);


        PlayerPrefs.SetString(PREF_npcSpeed, resultat[4]);
        PlayerPrefs.SetString(PREF_npcTurnSpeed, resultat[5]);
        PlayerPrefs.SetString(PREF_npcSnowballGrowSpeed, resultat[6]);
        PlayerPrefs.SetString(PREF_npcCount, resultat[7]);


        PlayerPrefs.SetString(PREF_snowballSpeed, resultat[8]);
        PlayerPrefs.SetString(PREF_snowballMaxSize, resultat[9]);
        

        PlayerPrefs.SetString(PREF_punchForce, resultat[10]);
        PlayerPrefs.SetString(PREF_perfectPunchForce, resultat[11]);
        PlayerPrefs.SetString(PREF_punchYAmplitude, resultat[12]);


        PlayerPrefs.SetString(PREF_killSizeIncrement, resultat[13]);
        PlayerPrefs.SetString(PREF_killPunchForceIncrement, resultat[14]);
        PlayerPrefs.SetString(PREF_killSpeedIncrement, resultat[15]);


        PlayerPrefs.SetString(PREF_buffDuration, resultat[16]);
        PlayerPrefs.SetString(PREF_speedBuffMultiplier, resultat[17]);
        PlayerPrefs.SetString(PREF_punchForceBuffMultiplier, resultat[18]);
        PlayerPrefs.SetString(PREF_sizeBuffMultiplier, resultat[19]);


        PlayerPrefs.SetString(PREF_freezeTime, resultat[20]);

        ApplyGameSettingsValues();

    }
}
