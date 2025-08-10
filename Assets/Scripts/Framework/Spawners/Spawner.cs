using UnityEngine;
using Framework.Utils;
using Framework.Pooling;
using System.Collections;

namespace Framework.Spawning
{
    /// <summary>
    /// Spawner.
    /// Implements spawning for PoolManaged prefabs.
    /// Add additional objects attaching any SpawnerArea derived script to
    /// define the spawning areas.
    /// 
    /// By Jorge L. Chávez Herrera.
    /// 
    /// </summary>
    public class Spawner : OptimizedGameObject
    {
        #region Class members
        [Header("Spawned Prefabs")]
        public PoolManaged[] poolManagedPrefabs;
        public bool randomizePrefabSelection;
        [Header("Spawning Settings")]
        public string groundTag = "Ground";
        [SerializeField] protected float spawnSeparationRadius = 1;
        [SerializeField] protected float minSpawnDelay = 0;
        [SerializeField] protected float maxSpawnDelay = 0;
        [SerializeField] protected bool _autoSpawn = false;
        public int maxActiveCount = 0;

        [SerializeField] protected Vector3 minSpawnRotation;
        [SerializeField] protected Vector3 maxSpawnRotation;

        [Header("Spawn Areas")]
        public SpawnArea[] spawnAreas;
        private int currentSpanAreaIndex;

        private UniqueRandomSequence randomPoolIndex;
        private int poolIndex;
        private PrefabPool[] prefabPools;
        #endregion

        #region Clas accessors
        public bool AutoSpawn
        {
            get { return _autoSpawn; }
            set
            {
                _autoSpawn = value;

                CancelInvoke();

                if (_autoSpawn)
                    SpawnMissing();
            }
        }
        #endregion

        #region MonoBehaviour overrides
        private void Awake()
        {
            //Debug.Log("SPAWNED IS LINKED TO : " + this.gameObject.name);

            randomPoolIndex = new UniqueRandomSequence(poolManagedPrefabs.Length);

            // Create prefab pools.
            prefabPools = new PrefabPool[poolManagedPrefabs.Length];

            for (int i = 0; i < poolManagedPrefabs.Length; i++)
                prefabPools[i] = PoolManager.CreatePoolForPrefab(poolManagedPrefabs[i]);

            // Create a default spawn area if none has been specified.
            if (spawnAreas == null || spawnAreas.Length == 0)
            {
                SpawnArea defaultSpawnArea = gameObject.AddComponent<SpawnArea>();
                spawnAreas = new SpawnArea[] { defaultSpawnArea };
            }
        }

        private void OnEnable()
        {
            if (_autoSpawn)
                SpawnMissing();
        }
        #endregion

        #region Class implementation
        /// <summary>
        /// Gets the next prefab to spawn, if randomizePrefabSelection is set to false
        /// it will return the next Prefab in the sequence.
        /// </summary>
        /// <returns>The next prefab to spawn.</returns>
        public PoolManaged GetNextPrefabToSpawn()
        {
            if (randomizePrefabSelection)
                return poolManagedPrefabs[randomPoolIndex.nextValue];

            poolIndex = (poolIndex + 1) % poolManagedPrefabs.Length;

            return poolManagedPrefabs[poolIndex];
        }

       

        /// <summary>
        /// Spawns a specified number of gameObjects.
        /// </summary>
        /// <param name="count"></param>
        public void Spawn(int count)
        {
            StartCoroutine(SpawnCoroutine(count));
        }

        /// <summary>
        /// Spawns the max number of active gameObjects for this spawner.
        /// </summary>
        public void SpawnMax()
        {
            Spawn(maxActiveCount);
        }

        private IEnumerator SpawnCoroutine(int count)
        {
            for (int i = 0; i < count; i++)
            {
                float spawnInterval = Random.Range(minSpawnDelay, maxSpawnDelay);
                PoolManaged prefabToSpawn = GetNextPrefabToSpawn();
                SpawnArea spawnArea = GetNextSpawArea();
                Vector3 pos;

                int tries = 0;
                do
                {
                    tries++;
                    pos = spawnArea.GetSpawnPoint();
                    if (tries > 100)
                        break;
                }
                while (CanSpawnAt(pos, spawnSeparationRadius) == false);

                //if (tries > 100)
                  //  Debug.LogError(name + " Failed to spawn at " + pos);

                if (maxActiveCount > 0)
                {
                    if (GetTotalSpawnedActiveCount() < maxActiveCount)
                        PoolManager.SpawnAt(prefabToSpawn, pos, Quaternion.Euler(GetSpawnRotation()), null, gameObject);
                }
                else
                    PoolManager.SpawnAt(prefabToSpawn, pos, Quaternion.Euler(GetSpawnRotation()), null, gameObject);

                if (spawnInterval > 0)
                    yield return new WaitForSecondsRealtime(spawnInterval);
            }
        }

        /// <summary>
        /// Despawns all GameObjects fro all SpawnPools.
        /// </summary>
        public void DespawnAll()
        {
            Debug.Log("LENGTH IS = " + poolManagedPrefabs.Length);
            for (int i = 0; i < poolManagedPrefabs.Length; i++)
                try
                {
                    prefabPools[i].DespawnAll();
                }
                catch
                {
                    Debug.Log("ERROR AT index : " + i);
                }
        }

        /// <summary>
        /// Gets the total number of active spawned objects.
        /// </summary>
        /// <returns>The tottal active.</returns>
        public int GetTotalActive()
        {
            int totalActive = 0;

            for (int i = 0; i < prefabPools.Length; i++)
                totalActive += prefabPools[i].spawnedCount;

            return totalActive;

        }

        /// <summary>
        /// Spawns  game objects until the number reaches maxActiveCount.
        /// </summary>
        private void SpawnMissing()
        {
            int missing = maxActiveCount - GetTotalActive();

            if (missing > 0)
                Spawn(missing);

            if (_autoSpawn)
            {
                float spawnInterval = Random.Range(minSpawnDelay, maxSpawnDelay);

                Invoke("SpawnMissing", spawnInterval);
            }
        }

        /// <summary>
        /// Returns wheter an object can be spawned at a certain point in the ground 
        /// by casting a sphere encapsulating the object's.
        /// Use it when you don't want spawned objects to overlap in the scene.
        /// </summary>
        /// <param name="worldPoint"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public bool CanSpawnAt(Vector3 worldPoint, float radius)
        {
            RaycastHit hit;

            if (string.IsNullOrEmpty(groundTag))
                return true;

            if (Physics.Raycast(new Ray(worldPoint + new Vector3(0, 100, 0), Vector3.down), out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag(groundTag) == true)
                {
                    if (Physics.SphereCast(new Ray(worldPoint + new Vector3(0, 100, 0), Vector3.down), radius, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.CompareTag(groundTag) == true)
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the next spawn area.
        /// </summary>
        /// <returns></returns>
        protected SpawnArea GetNextSpawArea()
        {
            SpawnArea ret = spawnAreas[currentSpanAreaIndex];
            currentSpanAreaIndex = (currentSpanAreaIndex + 1) % spawnAreas.Length;

            return ret;
        }

        /// <summary>
        /// Draws the gizmo spawn point.
        /// </summary>
        /// <param name="t"></param>
        protected virtual void DrawGizmoSpawPoint(Transform t) { }

        /// <summary>
        /// Gets the random rotation for spawning bassed on min & max spawn rotation settings.
        /// </summary>
        /// <returns></returns>
        protected Vector3 GetSpawnRotation()
        {
            return new Vector3(
                Random.Range(minSpawnRotation.x, maxSpawnRotation.x),
                Random.Range(minSpawnRotation.y, maxSpawnRotation.y),
                Random.Range(minSpawnRotation.z, maxSpawnRotation.z)
                );
        }

        /// <summary>
        /// Gets the total number of gameObjects still active from all spawn pools.
        /// </summary>
        /// <returns></returns>
        protected int GetTotalSpawnedActiveCount()
        {
            int ret = 0;

            for (int i = 0; i < prefabPools.Length; i++)
                ret += prefabPools[i].spawnedCount;

            return ret;
        }

        /// <summary>
        /// Sends a message to all active game objects spawned.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public void SendMessageToAllActive(string message, object data = null)
        {
            for (int i = 0; i < prefabPools.Length; i++)
                prefabPools[i].SendMessageToAllActive(message, data);
        }

        /// <summary>
        /// Invokes the specified function on all active game spawned objetcs.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="delay"></param>
        public void InvokeOnAllActive(string functionName, float delay)
        {
            for (int i = 0; i < prefabPools.Length; i++)
                prefabPools[i].InvokeOnAllActive(functionName, delay);
        }

        #endregion
    }

}
