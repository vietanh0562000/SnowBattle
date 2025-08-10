using System.Collections;
using System.Collections.Generic;
using Framework.Pooling;
using Framework.Tweening;
using Framework.Utils;
using UnityEngine;

public class Buff : OptimizedGameObject
{
    public Sprite icon;
    public Color color;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private PoolManaged pickUpFXPrefab;
    private PrefabPool pickUpFXPool;
    #region MonoBehaviour events

    private void Awake()
    {
        pickUpFXPool = PoolManager.CreatePoolForPrefab(pickUpFXPrefab);
        cachedPoolManaged.onSpawn = OnSpawn;
    }
    private void Update()
    {
        cachedTransform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Don't allow this buff to be picked up if not visible by camera.
        Vector3 viewportCoords = Camera.main.WorldToViewportPoint(cachedTransform.position);
        if (viewportCoords.x < 0 || viewportCoords.x > 1 || viewportCoords.y < 0 || viewportCoords.y > 1)
            return;

        if (other.CompareTag("Player") || other.CompareTag("NPC"))
        {
            SnowVehicle snowVehicle = other.GetComponent<SnowVehicle>();

            if (!snowVehicle.cachedFSM.CurrentStateIs("Frozen")) 
                snowVehicle.ApplyBuff(this);
            
            cachedPoolManaged.Despawn();

            pickUpFXPool.SpawnAt(cachedTransform.position + Vector3.up, Quaternion.identity, null).transform.localScale = Vector3.one * 2;
        }
    }

    #endregion

    #region Class implementation

    private void EnableCollider()
    {
        GetComponent<Collider>().enabled = true;
    }

    private void UpdateScale (float scale)
    {
        cachedTransform.localScale = Vector3.one * scale;
    }
    #endregion

    #region PoolManaged delegates

    private void OnSpawn()
    {
        GetComponent<Collider>().enabled = false;
        Invoke("EnableCollider", 1);
        cachedTransform.localScale = Vector3.zero;
        StartCoroutine(TweenUtils.TweenFloat(0, 1, 0.5f, 0, ETweenType.BounceOut, UpdateScale, null));
    }

    #endregion
}
