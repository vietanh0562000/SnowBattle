using Framework.Utils;
using UnityEngine;
using System.Collections;
using System;
//using Facebook.Unity;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// Game manager.
/// 
/// Implements general game flow.
/// 
/// By Jorge L. Chávez Herrera.
/// </summary>
public class GameManager : MonoSingleton<GameManager>
{
    #region Class members
    // a bool to update the bonus power & speed at game start
    private static bool hasUpdatedAfterStartUp;
    private const string PREF_LAST_SAVED_POWER = "POWER";
    private const string PREF_LAST_SAVED_SPEED = "SPEED";

    private const string PREF_LVL = "SAVED_LVL";
    private const string PREF_RANKING = "SAVED_RANKING";
    private const string PREF_GAMEPLAY_V = "GAMEPLAY_V";
    private const string PREF_FUNDS = "FUNDS";
    private const string PREF_SNOW_VEHICLE_INDEX = "SNOW_VEHICLE_INDEX";
    private const string PREF_BOUGHT_SNOW_VEHICLES = "BOUGHT_SNOW_VEHICLES";

    private const string PREF_LAST_DAILY_BONUS_COLLECTION_DAY = "LAST_DAILY_BONUSY_COLLECTION_DAY";
    private const string PREF_DAILY_BONUS_INDEX = "DAILY_BONUS_INDEX";
    private string PREF_buffDuration = "PREF_buffDuration";

    [Header("Version Link")]
    public string stateLink = "https://www.dropbox.com/s/c5sprzk49c8p1of/SnowBattle_GamePlay_Versions.txt?raw=1";
    public bool MeDebugging;
    [Header("Privacy Policy")]
    public string PPlink = "";
    [Header("Win Coins Award")]
    public int fundsIncrease = 250;
    
    [Header("Settings")]
    [SerializeField] private GameSettings settings;
    [Header("Levels")]
    public int levelCount;

    private Level currentLevel;
    private int levelIndex = 1;
    private int ranking;
    private List<int> boughtSnowVehicles = new List<int>() { 0 };
    #endregion

    #region Class accessors
    static public GameSettings Settings
    {
        get { return Instance.settings; }
    }

    static public int LastDailyBonusCollectionDay
    {
        get { return PlayerPrefs.GetInt(PREF_LAST_DAILY_BONUS_COLLECTION_DAY, 0); }
        set { PlayerPrefs.SetInt(PREF_LAST_DAILY_BONUS_COLLECTION_DAY, value); }
    }

    static public int DailyBonusIndex
    {
        get { return PlayerPrefs.GetInt(PREF_DAILY_BONUS_INDEX, 0); }
        set { PlayerPrefs.SetInt(PREF_DAILY_BONUS_INDEX, value); }
    }

    static public int Funds
    {
        get { return PlayerPrefs.GetInt(PREF_FUNDS, 0); }
        set { PlayerPrefs.SetInt(PREF_FUNDS, value); }
    }

    static public int SnowVehicleIndex
    {
        get { return PlayerPrefs.GetInt(PREF_SNOW_VEHICLE_INDEX, 0); }
        set { PlayerPrefs.SetInt(PREF_SNOW_VEHICLE_INDEX, value); }
    }
    #endregion

    #region MonoBehaviour events
    // NEVER OVERRIDE AWAKE (See MonoSingleton.cs).
    public override void Init()
    {
        Application.targetFrameRate = 60;

        if (!MeDebugging)
        {
            if (PlayerPrefs.HasKey(PREF_buffDuration))
            {
                settings.ApplyGameSettingsValues();
            }
            StartCoroutine(CheckNewValues());
        }

        // Get saved bough snow vehicles.
        if (PlayerPrefs.HasKey(PREF_BOUGHT_SNOW_VEHICLES))
        {
            string serializedData = PlayerPrefs.GetString(PREF_BOUGHT_SNOW_VEHICLES);

            string[] split = serializedData.Split(',');

            boughtSnowVehicles = new List<int>() { 0 };

            for (int i = 0; i < split.Length; i++)
            {
                if (string.IsNullOrEmpty(split[i]) == false)
                    boughtSnowVehicles.Add(int.Parse(split[i]));
            }
        }
    }

    private IEnumerator Start()
    {
        levelIndex = PlayerPrefs.GetInt(PREF_LVL, 1);
        ranking = PlayerPrefs.GetInt(PREF_RANKING, 0);

        LoadLevel(levelIndex);

        // Allow the player to claim the daily bonus.
        if (DateTime.Now.DayOfYear - LastDailyBonusCollectionDay >= 1)
        {
            // More than one day passed since last claim.
            if (DateTime.Now.DayOfYear - LastDailyBonusCollectionDay > 1)
            {
                // Reset counter day
                DailyBonusIndex = 0;
            }

            UIManager.Instance.dailyBonusPanel.Show();

            // Wait until daily bonus panel is closed.
            while (UIManager.Instance.dailyBonusPanel.gameObject.activeSelf)
            {
                yield return null;
            }
        }

        UIManager.Instance.mainManuPanel.Show();
        UIManager.Instance.titlePanel.Show();
    }
    #endregion

    #region Class implementation
    /// <summary>
    /// Starts the game.
    /// </summary>
    public void StartGame()
    {
        StartLevel(levelIndex);
        /*GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start,
            "Level " + levelIndex);*/
    }

    /// <summary>
    /// Ends the game.
    /// </summary>
    public void EndGame()
    {
        // Stop all
        SnowVehicle[] snowVehicles = FindObjectsOfType<SnowVehicle>();
        foreach (SnowVehicle snowVehicle in snowVehicles)
            snowVehicle.cachedFSM.SetState(null);

        Time.timeScale = 0;

        NPCController[] npcs = FindObjectsOfType<NPCController>();

        // Show Victory or game over panel
        if (npcs.Length == 0)
            UIManager.Instance.victoryPanel.Show();
        else
        {
            UIManager.Instance.gameOverPanel.Show();
            UIManager.Instance.titlePanel.Show();
        }

        // Hide HUD
        UIManager.Instance.HUDPanel.Hide();
    }

    /// <summary>
    /// Restarts the game on tghe current level.
    /// </summary>
    public void Retry()
    {
        currentLevel.CleanUp();
        LoadLevel(levelIndex);
        StartLevel(levelIndex);
        /*GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start,
            "Level " + levelIndex);*/
    }

    /// <summary>
    /// Continues the game to the next level.
    /// </summary>
    public void Continue()
    {
        currentLevel.CleanUp();

        levelIndex++;
        if (levelIndex > levelCount)
            levelIndex = 1;

        LoadLevel(levelIndex);
        StartLevel(levelIndex);
        /*GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start,
            "Level " + levelIndex);*/
        PlayerPrefs.SetInt(PREF_LVL, levelIndex);
        //Debug.Log("SAVED IN PLAYER PREFS level Index: " + levelIndex);
    }

    /// <summary>
    /// Goes to Main menu.
    /// </summary>
    public void MainMenu()
    {
        currentLevel.CleanUp();
        UIManager.Instance.gameOverPanel.Hide();
        UIManager.Instance.mainManuPanel.Show(0.25f);
        LoadLevel(levelIndex);
    }

    /// <summary>
    /// Starts the specified level.
    /// </summary>
    /// <param name="index"></param>
    private void StartLevel(int index)
    {
        StartCoroutine(StartLevelCoroutine(index));
    }

    private IEnumerator StartLevelCoroutine(int index)
    {
        // UI cleanup
        Time.timeScale = 1;
        UIManager.Instance.titlePanel.Hide();
        UIManager.Instance.gameOverPanel.Hide();
        UIManager.Instance.mainManuPanel.Hide();

        // Start
        UIManager.Instance.HUDPanel.Show();
        yield return new WaitForSecondsRealtime(1);
        currentLevel.npcSpawner.maxActiveCount = Settings.data.npcCount;
        currentLevel.npcSpawner.SpawnMax();
        yield return new WaitForSecondsRealtime(3);
        PlayerController.Instance.cachedFSM.SetState("Patrol");
        currentLevel.npcSpawner.SendMessageToAllActive("Patrol");
        yield return new WaitForSecondsRealtime(10);
        currentLevel.buffSpawner.AutoSpawn = true;
    }

    private void LoadLevel(int index)
    {
        StartCoroutine(LoadLevel(string.Format("Level {0:00}", index)));
    }

    private IEnumerator LoadLevel(string levelName)
    {
        if (currentLevel != null)
        {
            if (currentLevel.freezeZoneSpanwer)
                currentLevel.freezeZoneSpanwer.DespawnAll();
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        currentLevel = FindObjectOfType<Level>();

        if (currentLevel.freezeZoneSpanwer != null)
            currentLevel.freezeZoneSpanwer.SpawnMax();

        currentLevel.playerSpawner.Spawn(1);
        PlayerController.Instance.cachedFSM.SetState("Idle");

        if (!hasUpdatedAfterStartUp)
        {
            if (PlayerPrefs.HasKey(PREF_LAST_SAVED_SPEED))
            {
                UpdateSnowVehicleForceAndSpeed(PlayerPrefs.GetInt(PREF_LAST_SAVED_POWER),
            PlayerPrefs.GetInt(PREF_LAST_SAVED_SPEED));
                hasUpdatedAfterStartUp = true;
            }
        }
    }

    public void SetUserName(string userName)
    {
        PlayerPrefs.SetString("UserName", userName);
    }

    public string GetUserName()
    {
        return PlayerPrefs.GetString("UserName", "");
    }

    public void BuySnowVehicle(int index, int price)
    {
        Funds -= price;
        boughtSnowVehicles.Add(index);

        string serializedData = "";

        for (int i = 0; i < boughtSnowVehicles.Count; i++)
            serializedData = boughtSnowVehicles[i].ToString() + ",";

        PlayerPrefs.SetString(PREF_BOUGHT_SNOW_VEHICLES, serializedData);
        Debug.Log(serializedData);

        SnowVehicleIndex = index;
    }

    public bool SnowVehicleIsBought(int index)
    {
        return boughtSnowVehicles.Contains(index);
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(PREF_LVL);
        PlayerPrefs.DeleteKey(PREF_RANKING);
        levelIndex = 1;
        ranking = 0;
        currentLevel.CleanUp();
        LoadLevel(levelIndex);
    }

    public void IncrementRanking()
    {
        ranking++;
        PlayerPrefs.SetInt(PREF_RANKING, ranking);
    }

    public int GetRanking()
    {
        return ranking;
    }
    #endregion

    public void LvlFailedAnalytics()
    {
        /*GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail,
            "Level " + levelIndex);*/
    }

    public void LvlCompleteAnalytics()
    {
        /*GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete,
            "Level " + levelIndex);*/
        LogAchievedLevelEvent((levelIndex + 1).ToString());
        Funds += fundsIncrease;
    }

    public void UpdateSnowVehicleForceAndSpeed(int powerToAdd, int speedToAdd)
    {
        if (powerToAdd == 5)
        {
            Settings.data.npcPunchForce = 14.5f;
            Settings.data.npcPerfectPunchForce = 21.5f;
        }
        if (powerToAdd == 7)
        {
            Settings.data.npcPunchForce = 14.5f;
            Settings.data.npcPerfectPunchForce = 21.5f;
        }
        if (powerToAdd == 7)
        {
            Settings.data.npcPunchForce = 14.5f;
            Settings.data.npcPerfectPunchForce = 21.5f;
        }
        if (powerToAdd == 10)
        {
            Settings.data.npcPunchForce = 15f;
            Settings.data.npcPerfectPunchForce = 22f;
        }
        if (powerToAdd == 15)
        {
            Settings.data.npcPunchForce = 15f;
            Settings.data.npcPerfectPunchForce = 22f;
        }
        if (powerToAdd == 20)
        {
            Settings.data.npcPunchForce = 15f;
            Settings.data.npcPerfectPunchForce = 22f;
        }
        if (powerToAdd == 25)
        {
            Settings.data.npcPunchForce = 15.5f;
            Settings.data.npcPerfectPunchForce = 22.5f;
        }
        if (powerToAdd == 30)
        {
            Settings.data.npcPunchForce = 16f;
            Settings.data.npcPerfectPunchForce = 23f;
        }

        PlayerPrefs.SetInt(PREF_LAST_SAVED_POWER, powerToAdd);
        PlayerPrefs.SetInt(PREF_LAST_SAVED_SPEED, speedToAdd);
    }

    IEnumerator CheckNewValues()
    {
        Debug.Log("Started Checking Value coroutine!");
        WWW GettingDataTextFileWWW = new WWW(stateLink);
        yield return GettingDataTextFileWWW;
        string resultBrut = GettingDataTextFileWWW.text;
        string[] result = resultBrut.Split(';');
        int version = Int32.Parse(result[0]);
        if (!PlayerPrefs.HasKey(PREF_GAMEPLAY_V))
        {
            // Apply Settings + SAVE THE PREF VERSION
            PlayerPrefs.SetInt(PREF_GAMEPLAY_V, version);
            settings.UpdateGameSettingsValues(resultBrut);

        }
        else // CHECK IF WE UPDATED THE VERSION
        {
            int currentV = PlayerPrefs.GetInt(PREF_GAMEPLAY_V);
            if (version > currentV)
            {
                // Apply Settings + UPDATE THE PREF VERSION
                Debug.Log("[UPDATE] - New GAMEPLAY VERSION Detected : " + version);
                PlayerPrefs.SetInt(PREF_GAMEPLAY_V, version);
                settings.UpdateGameSettingsValues(resultBrut);
            }
        }
    }

    public void NoAdsIAPPurchased()
    {
        /*GameAnalytics.NewBusinessEvent("usd", 2, "No ADS", "com.removingads",
            "cartType");*/
    }

    public void OpenPP()
    {
        Application.OpenURL(PPlink);
    }

    void LogAchievedLevelEvent(string level)
    {
        /*var parameters = new Dictionary<string, object>();
        parameters[AppEventParameterName.Level] = level;
        FB.LogAppEvent(
            AppEventName.AchievedLevel, null,
            parameters
        );*/
    }
}
