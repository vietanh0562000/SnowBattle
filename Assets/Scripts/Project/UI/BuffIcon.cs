using Framework.Utils;
using UnityEngine;
using UnityEngine.UI;


public class BuffIcon : OptimizedGameObject
{
    #region Class members
    [SerializeField] private Vector3 offset;
    [System.NonSerialized] public Image cachedImage;
    private Transform trackedTransform;
    #endregion

    #region MonoBehaviour events
    private void Awake()
    {
        cachedImage = GetComponent<Image>();
    }

    private void LateUpdate()
    {
        Vector3 iconPos = trackedTransform.TransformPoint(offset);
        Vector3 position = Camera.main.WorldToScreenPoint(iconPos);
        cachedTransform.position = position;
    }
    #endregion

    #region Class implementation
    public void SetTrackedTransform(Transform trackedTransform)
    {
        this.trackedTransform = trackedTransform;
    }

    public void SetIcon(Sprite icon)
    {
        cachedImage.sprite = icon;
    }
    #endregion
}
