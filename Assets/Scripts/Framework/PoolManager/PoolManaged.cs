using UnityEngine;
using System.Collections;
using Framework.Delegates;

namespace Framework.Pooling
{
	/// <summary>
	/// PoolManaged.
	/// Attach this component to a gameObject to be managed by the PoolManager.
	/// 
	/// by Jorge L. Chávez H.
	/// </summary>
	public class PoolManaged : MonoBehaviour
	{
	    #region Class members
	    public int instanceCount = 1;
	    [System.NonSerialized] public float spawnTime;       // Holds the last time at wich the gameObject was spawned.
	    [System.NonSerialized] public PrefabPool prefabPool; // Holds a reference to the pool from wich this gameObject was pooled.
	    public SimpleDelegate onInstantiate;              	 // This delegate will be called by the PoolManager only when the game object gets instantiated.
	    public SimpleDelegate onSpawn;                       // This delegate will be called by the PoolManager everytime the gameObject is spawned.
	    public SimpleDelegate onDespawn;                 	 // This delegate will be called by the PoolManager everytime the gameObject is despawned.

	    [System.NonSerialized] public GameObject spawner;    // Holds a reference to a gameObject responsable for spawning this gameObject.
	    #endregion

	    #region Class functions
		public void Despawn (float delay)
		{
			Invoke ("Despawn", delay);
		}
	    /// <summary>
	    /// Despawn this instance.
	    /// </summary>
	    public void Despawn ()
	    {
	        // Cancel all invokes & stop coroutines before making this instance inactive
	        MonoBehaviour[] allScipts = GetComponentsInChildren<MonoBehaviour>();

	        for (int i = 0; i < allScipts.Length; i++)
	        {
	            if (allScipts[i] != null)
	            {
	                allScipts[i].StopAllCoroutines();
	                allScipts[i].CancelInvoke();
	            }
	        }

			prefabPool.Despawn (this);
	    }
	    #endregion
	}
}