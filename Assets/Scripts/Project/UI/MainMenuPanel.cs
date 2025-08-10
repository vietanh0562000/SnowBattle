using Framework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : PanelBase
{
    #region Class members
    [Header("UI Links")]
    [SerializeField] private float playerRotationSpeed = 10;
    [SerializeField] InputField nameInputField;
    [SerializeField] private Image medalImage;
    [SerializeField] private TextMeshProUGUI rankingLabel;
    [Header("Sprites")]
    [SerializeField] public Sprite[] medalSprites;

    private Vector3 prevMousePosition;
    private int colorIndex = 0;

    private static string[] rankings = new string[]{"BRONZE", "SILVER", "GOLD", "MASTER","DIAMOND"};
    #endregion

    #region Base class overrides
    public override void UpdateInfo()
    {
        Time.timeScale = 1;
        nameInputField.text = GameManager.Instance.GetUserName();

        int index = Mathf.Min(GameManager.Instance.GetRanking(), 19);
        medalImage.sprite = medalSprites[index / 4];
        rankingLabel.text = rankings[index / 4] + " " + ((index % 4) + 1).ToString();
    }
    #endregion

    #region Class implementation
    public void Next()
    {
        colorIndex = (int)Mathf.Repeat(colorIndex + 1, PlayerController.colors.Length);
        PlayerController.Instance.SetColor(colorIndex);
    }

    public void Previous()
    {
        colorIndex = (int)Mathf.Repeat(colorIndex - 1, PlayerController.colors.Length);
        PlayerController.Instance.SetColor(colorIndex);
    }

    public void OnNameEdit(string text)
    {
        GameManager.Instance.SetUserName(text);
    }
    #endregion
}
