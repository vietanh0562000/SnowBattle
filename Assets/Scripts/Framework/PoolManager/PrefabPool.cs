using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Framework.Pooling
{
	/// <summary>
	/// PrefabPool.
	/// Provides functionality for managing a pool of prefab instances.
	/// 
	/// by Jorge L. Chávez H.
	/// </summary>
	public class PrefabPool
	{
	    #region Class members
	    private List<PoolManaged> spawned;   // Holds the spawned (currently active) gameObjects in this pool.
	    private List<PoolManaged> despawned; // Holds the despawned (currently active) gameObjects in this pool.
	    private PoolManaged prefab;          // Holds the source GameObject prefab for all instances in this pool.
	    #endregion

	    #region Class accessors
	    /// <summary>
	    /// Returns the allocated gameObject instance count.
	    /// </summary>
	    /// <value>
	    /// The allocated instances.
	    /// </value>
	    public int allocatedInstanceCount
	    {
	        get { return spawned.Count + despawned.Count; }
	    }

	    /// <summary>
	    /// Gets the spawned (currently active) gameObject count.
	    /// </summary>
	    /// <value>The spawned count.</value>
	    public int spawnedCount
	    {
	        get { return spawned.Count; }
	    }

	    /// <summary>
	    /// Gets the despawned (currently inactive) gameObject count.
	    /// </summary>
	    /// <value>The despawned count.</value>
	    public int despawnedCount
	    {
	        get { return despawned.Count; }
	    }
	    #endregion

	    #region Class functions
	    /// <summary>
	    /// Initializes a new instance of the <see cref=PrefabPool"/> class.
	    /// </summary>
	    /// <param name="poolManagedPrefab">Pool managed prefab.</param>
	    public PrefabPool (PoolManaged poolManagedPrefab)
	    {
	        spawned = new List<PoolManaged>();
	        despawned = new List<PoolManaged>();
	        prefab = poolManagedPrefab;
	        AllocateInstances (prefab.instanceCount);
	    }

	    /// <summary>
	    /// Allocates a number of gameObject instances for this pool.
	    /// </summary>
	    /// <param name="count">Count.</param>
	    public void AllocateInstances (int count)
	    {
	        for (int i = 0; i < count; i++)
	        {
	            PoolManaged instance = (GameObject.Instantiate(prefab.gameObject)).GetComponent<PoolManaged>();
	            instance.name = prefab.name;
	            instance.prefabPool = this;

	            // Execute instantiation delegate
	            if (instance.onInstantiate != null)
	                instance.onInstantiate();

	            // Deactivate gamObject & save it for later spawning
	            instance.gameObject.SetActive(false);
	            instance.transform.SetParent (PoolManager.Instance.transform, true);
	            despawned.Add (instance);
	        }
	    }

	    public PoolManaged Spawn(GameObject spawner = null)
	    {
	        PoolManaged instance;

	        if (despawned.Count == 0)
	        {
	            AllocateInstances (1);
	            Debug.LogWarning("No more instances available for: " + prefab.name + ", will allocate a new one, but better tweak the pool size.");
	        }

	        instance = despawned[0];

	        // Pass other relevant info to the newly spawned instance
	        instance.gameObject.SetActive (true);
	        instance.spawnTime = Time.time;
	        instance.spawner = spawner;

	        despawned.Remove(instance);
	        spawned.Add(instance);

	        if (instance.onSpawn != null)
	            instance.onSpawn ();

	        return instance;
	    }

	    /// <summary>
	    /// Spawn the specified position, rotation, parent and spawner.
	    /// </summary>
	    /// <param name="position">Position.</param>
	    /// <param name="rotation">Rotation.</param>
	    /// <param name="parent">Parent.</param>
	    /// <param name="spawner">Spawner.</param>
	    public PoolManaged SpawnAt (Vector3 position, Quaternion rotation, Transform parent, GameObject spawner = null)
	    {
	        PoolManaged instance;

	        if (despawned.Count == 0)
	        {
	            AllocateInstances(1);
	            Debug.LogWarning("No more instances available for: " + prefab.name + ", will allocate a new one, but better tweak the pool size.");
	        }

	        instance = despawned[0];
            despawned.Remove(instance);
            spawned.Add(instance);

	        // Setup instance transform
	        instance.transform.SetParent (parent, false);
	        instance.transform.localPosition = position;
	        instance.transform.localRotation = rotation;
            instance.transform.localScale = Vector3.one;
	
	        // Pass other relevant info to the newly spawned instance
	        instance.gameObject.SetActive (true);
	        instance.spawnTime = Time.time;
	        instance.spawner = spawner;

	        if (instance.onSpawn != null)
	            instance.onSpawn ();

	        return instance;
	    }

	    /// <summary>
	    // Despawns the gameOject
	    /// <summary>
	    public void Despawn(PoolManaged instance)
	    {
	        if (instance.onDespawn != null)
	            instance.onDespawn ();

	        instance.gameObject.SetActive(false);

            instance.transform.SetParent (PoolManager.Instance.transform, false);
	        spawned.Remove (instance);
	        despawned.Add (instance);
	    }

		/// <summary>
		/// Despawns all spawned gameobjects.
		/// </summary>
		public void DespawnAll ()
		{
			while (spawned.Count > 0) 
			{
				spawned [0].Despawn ();
			}
		}

	    /// <summary>
	    /// Destroy all allocated gameObject instances.
	    /// </summary>
	    public void Clear()
	    {
	        while (spawned.Count > 0)
	        {
	            if (spawned[0] != null && spawned[0].gameObject != null)
	                GameObject.Destroy(spawned[0].gameObject);

	            spawned.RemoveAt (0);
	        }

	        despawned.Clear ();
	    }

        /// <summary>
        /// Sends a message to all active game objects.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public void SendMessageToAllActive(string message, object data = null)
        {
            for (int i = 0; i < spawnedCount; i++)
                spawned[i].SendMessage(message, data);
        }

        /// <summary>
        /// Invokes the specified function on all active game objetcs.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="delay"></param>
        public void InvokeOnAllActive(string functionName, float delay)
        {
            for (int i = 0; i < spawnedCount; i++)
                spawned[i].Invoke(functionName, delay);
        }

        #endregion
    }
}
