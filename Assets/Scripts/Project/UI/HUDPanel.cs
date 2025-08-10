using System.Collections;
using System.Collections.Generic;
using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class HUDPanel : PanelBase 
{
    #region Class members
    [Header("UI Links")]
    [SerializeField] private Animation gameStartPanelAnimation;
    [SerializeField] private Text killsLabel;
    public Transform nameLabelsRoot;
    #endregion

    #region Base class overrides
    public override void UpdateInfo()
    {
        PlayStartGameAnimation();
        SetKills(0);
    }
    #endregion

    #region Class implementation
    public void SetKills(int kills)
    {
        killsLabel.text = "kill: "+ kills.ToString();
    }

    private void PlayStartGameAnimation()
    {
        gameStartPanelAnimation.Rewind();
        gameStartPanelAnimation.Play();
    }
    #endregion
}
