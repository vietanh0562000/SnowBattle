using Framework.Pooling;
using Framework.Spawning;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Spawner playerSpawner;
    public Spawner npcSpawner;
    public Spawner buffSpawner;
    public Spawner freezeZoneSpanwer;

    public void CleanUp()
    {
        playerSpawner.DespawnAll();
        npcSpawner.DespawnAll();
        buffSpawner.DespawnAll();
        buffSpawner.AutoSpawn = false;
        if (freezeZoneSpanwer != null)
            freezeZoneSpanwer.DespawnAll();
        PoolManager.DespawnAll();
    }
}
