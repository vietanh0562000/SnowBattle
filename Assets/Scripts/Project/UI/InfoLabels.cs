using System.Collections;
using System.Collections.Generic;
using Framework.Tweening;
using Framework.Utils;
using UnityEngine;
using UnityEngine.UI;

public class InfoLabels : OptimizedGameObject
{
    #region Class members
    [SerializeField] private TextAsset textAsset;
    [SerializeField] private Sprite[] flags;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Text cachedText;
    [SerializeField] private Image cachedImage;
    [SerializeField] private Transform buffInfoRoot;
    [SerializeField] private Image buffIcon;
    [SerializeField] private Slider buffSlider;
    [SerializeField] private Text killLabel;
    private Transform trackedTransform;

    static private string[] names;
    private static UniqueRandomSequence namesIndex;
    private static UniqueRandomSequence flagsIndex;
    #endregion

    #region MonoBehaviour events
    private void Awake()
    {
        if (names == null)
        {
            names = textAsset.text.Split('\n');

            namesIndex = new UniqueRandomSequence(names.Length);
            flagsIndex = new UniqueRandomSequence(flags.Length);
        }

        cachedPoolManaged.onSpawn += OnSpawn;
    }

    private void LateUpdate()
    {
        Vector3 labelPos = trackedTransform.position;
        Vector3 position = Camera.main.WorldToScreenPoint(labelPos);
        cachedTransform.position = position;
        cachedTransform.localPosition += offset;
    }
    #endregion

    #region Class implementation
    public void ShowBuffInfo(bool visible, Color color, Sprite icon)
    {
        if (visible)
        {
            buffInfoRoot.gameObject.SetActive(true);
            StartCoroutine(TweenUtils.TweenFloat(0, 1, 0.5f, 0, ETweenType.BounceOut, BuffInfoScaleUpdate, null));
        }
        else
            StartCoroutine(TweenUtils.TweenFloat(1, 0, 0.5f, 0, ETweenType.EaseIn, BuffInfoScaleUpdate, BuffInfoScaleEnd));

        if (icon)
        {
            buffIcon.color = color;
            buffIcon.sprite = icon;
            buffSlider.targetGraphic.color = color;
        }
    }

    private void BuffInfoScaleUpdate(float value)
    {
        buffInfoRoot.localScale = Vector3.one * value;
    }

    private void BuffInfoScaleEnd()
    {
        buffInfoRoot.gameObject.SetActive(false);
    }

    public void SetBuffProgress(float value)
    {
        buffSlider.value = value;
    }

    public void SetTrackedTransform(Transform trackedTransform)
    {
        this.trackedTransform = trackedTransform;
    }

    public void SetName(string name, bool showFlag = true)
    {
        cachedText.text = name;
        cachedImage.transform.parent.gameObject.SetActive(showFlag);

        if (showFlag)
            cachedText.rectTransform.anchoredPosition = new Vector2(30, 0);
        else
            cachedText.rectTransform.anchoredPosition = Vector2.zero;
    }

    public void SetKills(int kills)
    {
        killLabel.text = "+" + kills.ToString();

        if (gameObject.activeSelf)
        {
            TweenUtils.TweenFloat(this, 0, 1, 0.5f, 0, ETweenType.BounceOut, KillLabelUpdate, null);
            TweenUtils.TweenFloat(this, 1, 0, 0.5f, 3, ETweenType.EaseIn, KillLabelUpdate, null);
        }
    }

    private void KillLabelUpdate(float value)
    {
        killLabel.transform.localScale = Vector3.one * value;
    }
    #endregion

    #region PoolManaged delegates
    private void OnSpawn()
    {
        cachedImage.transform.parent.gameObject.SetActive(true);
        cachedText.text = names[namesIndex.nextValue];
        cachedImage.sprite = Random.value > 0.5f ? flags[flagsIndex.nextValue] : flags[151];
        buffInfoRoot.localScale = Vector3.zero;
        buffInfoRoot.gameObject.SetActive(false);
        killLabel.transform.localScale = Vector3.zero;
    }
    #endregion
}