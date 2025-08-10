using System;
using System.Collections;
using System.Collections.Generic;
using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class DailyBonusPanel : PopupPanel
{
    #region Class mebers
    [SerializeField] private DailyBonusButton[] daylyBonusButtons;
    #endregion

    #region Super class overrides
    public override void UpdateInfo()
    {
        // Set button values & states
        for (int i = 0; i < daylyBonusButtons.Length; i++)
        {
            EDailyBonusState buttonState = EDailyBonusState.NotReady;

            if (i < GameManager.DailyBonusIndex)
                buttonState = EDailyBonusState.Collected;

            if (i == GameManager.DailyBonusIndex)
                buttonState = EDailyBonusState.Collectable;

            daylyBonusButtons[i].SetInfo(GameManager.Settings.data.dailyBonusValues[i], buttonState);
        }
    }

    #endregion

    #region Class implementation
    public void Claim()
    {
        GameManager.Funds += GameManager.Settings.data.dailyBonusValues[GameManager.DailyBonusIndex];
        GameManager.LastDailyBonusCollectionDay = DateTime.Now.DayOfYear;
        GameManager.DailyBonusIndex++;

        if (GameManager.DailyBonusIndex > 4)
            GameManager.DailyBonusIndex = 0;

        Hide();
    }
    #endregion
}
