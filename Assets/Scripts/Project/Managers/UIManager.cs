using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using Framework.Utils;

public class UIManager : MonoSingleton<UIManager> 
{
    public PanelBase titlePanel;
    public HUDPanel HUDPanel;
    public VictoryPanel victoryPanel;
    public MainMenuPanel mainManuPanel;
    public GameOverPanel gameOverPanel;
    public SettingsPanel settingsPanel;
    public DailyBonusPanel dailyBonusPanel;
}
