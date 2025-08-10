using Framework.Pooling;
using UnityEngine;

public class Pit : MonoBehaviour
{
    // Water splash
    #region Class members
    [SerializeField] private PoolManaged waterSplasPrefab;
    PrefabPool waterSplashPool;
    #endregion

    #region MonoBehaviour events
    private void Awake()
    {
        waterSplashPool = PoolManager.CreatePoolForPrefab(waterSplasPrefab);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("NPC"))
        {
            SnowVehicle snowVehicle = other.GetComponent<SnowVehicle>();
            // A snow vehicle has fallen in to the pit, kill it.
            snowVehicle.GetKilled();

            // Spawn water splash.
            Vector3 pos = snowVehicle.cachedTransform.position;
            pos.y = -1;
            PoolManaged pm = waterSplashPool.SpawnAt(pos, Quaternion.identity, null);
            pm.transform.localScale = Vector3.one * 2;
        }
    }
    #endregion

}
