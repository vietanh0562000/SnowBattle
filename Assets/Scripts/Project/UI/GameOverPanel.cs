using System.Collections;
using System.Collections.Generic;
using Framework.Pooling;
using Framework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : PanelBase
{
    #region Class members
    [Header("UI Links")]
    [SerializeField] private Text rankLabel;
    [SerializeField] private Text killLabel;
    [SerializeField] private Image medalImage;
    [SerializeField] private TextMeshProUGUI rankingLabel;
    [Header("Sprites")]
    [SerializeField] public Sprite[] medalSprites;

    [SerializeField] Button continueButton;

    private static string[] rankings = new string[] { "BRONZE", "SILVER", "GOLD", "MASTER", "DIAMOND" };
    #endregion

    #region Class implementation
    public override void UpdateInfo()
    {
        int rank = FindObjectsOfType<NPCController>().Length + 1;
        int kill = PlayerController.Instance.kills;
        rankLabel.text = "Rank\n" + rank.ToString();
        killLabel.text = "Kill\n" + kill.ToString();

        continueButton.interactable = (rank == 1);

        if (continueButton.interactable)
        {
            GameManager.Instance.IncrementRanking();
            // save level complete in Analytics
            GameManager.Instance.LvlCompleteAnalytics();
        }
        else
        {
            // save level fail in Analytics
            GameManager.Instance.LvlFailedAnalytics();
        }

        int index = Mathf.Min(GameManager.Instance.GetRanking(), 19);
        medalImage.sprite = medalSprites[index / 4];
        rankingLabel.text = rankings[index / 4] + " " + ((index % 4) + 1).ToString();
    }

    private void OnEnable()
    {
        Debug.Log("GAME OVER PANEL ON!");
        StartCoroutine(AfterPanelShow());
    }

    public void Retry()
    {
        GameManager.Instance.Retry();
    }

    public void Continue()
    {
        GameManager.Instance.Continue();
    }

    public void MainMenu()
    {
        GameManager.Instance.MainMenu();
    }

    IEnumerator AfterPanelShow()
    {
        yield return new WaitForSecondsRealtime(.5f);
        yield return null;
    }

    #endregion
}
