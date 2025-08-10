using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EDailyBonusState { Collectable, Collected, NotReady }

public class DailyBonusButton : MonoBehaviour
{
    #region Class members
    [SerializeField] private Button button;
    [SerializeField] private Text valueText;
    [SerializeField] private Image statusImage;
    [SerializeField] private Sprite checkSprite;
    [SerializeField] private Sprite lockSprite;

    private Color startColor;
    #endregion

    #region MonoBehaviour events
    private void Awake()
    {
        startColor = button.image.color;
    }
    #endregion

    #region Class implementation
    public void SetInfo(int value, EDailyBonusState state)
    {
        valueText.text = "+" + value;

        switch (state)
        {
            case EDailyBonusState.Collectable:
                statusImage.gameObject.SetActive(false);
                statusImage.sprite = null;
                button.interactable = true;
                button.image.color = startColor;
                break;
            case EDailyBonusState.Collected:
                statusImage.gameObject.SetActive(true);
                statusImage.sprite = checkSprite;
                button.interactable = false;
                button.image.color = startColor;
                break;
            case EDailyBonusState.NotReady:
                statusImage.gameObject.SetActive(true);
                statusImage.sprite = lockSprite;
                button.interactable = false;
                button.image.color = Color.grey;
                break;
        }
    }
    #endregion
}
