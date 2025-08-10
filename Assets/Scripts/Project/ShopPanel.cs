using Framework.UI;
using Framework.Utils;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script template.
/// 
/// Defines a template with regions for C# scripts.
/// 
/// By Jorge L. Chávez Herrera.
/// </summary>
public class ShopPanel : PopupPanel
{
    #region Class members
    [Header("UI Links")]
    [SerializeField] private Text fundsLabel;
    [SerializeField] private TextMeshProUGUI powerIncreaseLabel;
    [SerializeField] private TextMeshProUGUI speedIncreaseLabel;
    [SerializeField] RawImage previewRawImage;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Text priceLabel;
    [SerializeField] private Button buyButton;
    [SerializeField] private Text buyButtonLabel;
    [SerializeField] private Button selectButton;
    [SerializeField] private Text selectButtonLabel;
    [SerializeField] private Camera previewCamera;
    [SerializeField] private Transform snowVehicleRoot;
    [SerializeField] private SnowVehicleData[] snowVehicleData;

    private FloatSDInterpolator index = new FloatSDInterpolator(0.125f);
    private const float SNOW_VEHICLE_SPANCING = 7.5f;
    #endregion

    #region Class accessors
    private RenderTexture _renderTexture;
    private RenderTexture RenderTexture
    {
        get
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(previewRawImage.rectTransform);
            Rect renderTextureScreenRect = previewRawImage.rectTransform.rect;

            if (_renderTexture == null && renderTextureScreenRect.width > 0 && renderTextureScreenRect.height > 0)
                _renderTexture = new RenderTexture((int)renderTextureScreenRect.width, (int)renderTextureScreenRect.height, 24);

            return _renderTexture;
        }
    }

    private int Index 
    {
        get { return (int)index.targetValue; }
    }
    #endregion

    #region MonoBehaviour events
    protected override void Awake()
    {
        base.Awake();
        // Create a render texture and assign it to the preview camera.
        previewRawImage.texture = RenderTexture;
        previewCamera.targetTexture = RenderTexture;

        for (int i = 0; i < snowVehicleData.Length; i++)
        {
            GameObject go = Instantiate(snowVehicleData[i].model);
            go.transform.SetParent(snowVehicleRoot, false);
            go.transform.localPosition = new Vector3(i * SNOW_VEHICLE_SPANCING, 0, 0);
            go.transform.localRotation = Quaternion.Euler(0, 180, 0);
            go.transform.localScale = Vector3.one;
            go.SetRootLayer(snowVehicleRoot.gameObject.layer);
        }

        UpdateInfo();
    }

    private void Update()
    {
        snowVehicleRoot.localPosition = new Vector3(-index.Value * SNOW_VEHICLE_SPANCING, 0, 0);
    }

    private void OnDestroy()
    {
        Destroy(_renderTexture);
    }
    #endregion

    #region Base class overrides
    public override void UpdateInfo()
    {
        fundsLabel.text = GameManager.Funds.ToString();

        // Buy button
        buyButton.gameObject.SetActive(!GameManager.Instance.SnowVehicleIsBought(Index));
        bool enoughFunds = GameManager.Funds >= snowVehicleData[Index].price;
        buyButton.interactable = enoughFunds;
        buyButtonLabel.text = enoughFunds ? "BUY" : "NOT ENOUGH FUNDS";

        // Select button
        selectButton.gameObject.SetActive(!buyButton.gameObject.activeSelf);
        selectButton.interactable = GameManager.SnowVehicleIndex != Index;
        selectButtonLabel.text = selectButton.interactable ? "SELECT" : "SELECTED";

        priceLabel.text = snowVehicleData[Index].price.ToString();
    }
    #endregion

    #region Class implementation
    public void Next()
    {
        index.targetValue = Mathf.Clamp(Index + 1, 0, snowVehicleData.Length-1);
        UpdateInfo();
        UpdatePowerInfo();
    }

    public void Previous()
    {
        index.targetValue = Mathf.Clamp(Index - 1, 0, snowVehicleData.Length-1);
        UpdateInfo();
        UpdatePowerInfo();
    }

    public void Buy()
    {
        GameManager.Instance.BuySnowVehicle(Index, snowVehicleData[Index].price);
        UpdateInfo();
        GameManager.Instance.UpdateSnowVehicleForceAndSpeed(snowVehicleData[Index].powerIncrease,
            snowVehicleData[Index].speedIncrease);
    }

    public void Select()
    {
        GameManager.SnowVehicleIndex = Index;
        UpdateInfo();
        GameManager.Instance.UpdateSnowVehicleForceAndSpeed(snowVehicleData[Index].powerIncrease,
            snowVehicleData[Index].speedIncrease);
    }
    #endregion

    #region Interface implementation
    #endregion

    #region Nested classes
    #endregion

    public void UpdatePowerInfo()
    {
        if (Index != 0)
        {
            powerIncreaseLabel.gameObject.SetActive(true);
            speedIncreaseLabel.gameObject.SetActive(true);
            powerIncreaseLabel.text = "+"+ snowVehicleData[Index].powerIncrease +"% POWER";
            speedIncreaseLabel.text = "+" + snowVehicleData[Index].speedIncrease + "% SPEED";
        }
        else
        {
            powerIncreaseLabel.gameObject.SetActive(false);
            speedIncreaseLabel.gameObject.SetActive(false);
        }
    }
}
