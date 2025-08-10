using System.Collections;
using System.Collections.Generic;
using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : PopupPanel
{
    #region Class members
    [SerializeField] private Slider sensitivitySlider;


    private float sesitivity;
    #endregion

    #region Super class overrides
    public override void UpdateInfo()
    {
        sensitivitySlider.value = Mathf.InverseLerp(1, 3, GameManager.Settings.data.playerTurnSpeed);
    }
    #endregion

    #region Class implementation
    public void SetSensitivity(float value)
    {
        sesitivity = value;
        GameManager.Settings.data.playerTurnSpeed = Mathf.Lerp(1, 3, value);
    }

    public void ResetProgress()
    {
        GameManager.Instance.ResetProgress();
    }

    public void PPOnClick()
    {
        GameManager.Instance.OpenPP();
    }
    #endregion
}
