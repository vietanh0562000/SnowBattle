using UnityEngine;
using System.Collections.Generic;
using Framework.Utils;

namespace Framework.Pooling
{
    /// <summary>
    /// PoolManager.
    /// Singleton manager class providing functionality for spawning / despawning pooled gameObjects.
    /// 
    /// by Jorge L. Chávez H.
    /// </summary>
    public class PoolManager : MonoSingleton<PoolManager>
    {
        #region Class members
        static public Dictionary<PoolManaged, PrefabPool> prefabPoolsDict = new Dictionary<PoolManaged, PrefabPool>(); // holds gameObject pools referenced by their PoolManaged component.
        #endregion

        #region MonoBehaviour overrides 
        /// <summary>
        /// Clear all spawn pools when destorying this instance.
        /// </summary>
        override public void OnDestroy()
        {
            base.OnDestroy();
            // Clear (true);
        }
        #endregion

        #region Class functions 
        /// <summary>
        /// Creates a gameObject pool for the specified prefab.
        /// </summary>
        /// <returns>The pool for prefab.</returns>
        /// <param name="prefab">Prefab.</param>
        static public PrefabPool CreatePoolForPrefab(PoolManaged prefab)
        {
            PrefabPool prefabPool = null;

            if (prefabPoolsDict.ContainsKey(prefab) == false)
            {
                prefabPool = new PrefabPool(prefab);
                prefabPoolsDict.Add(prefab, prefabPool);
            }
            else
                prefabPool = prefabPoolsDict[prefab];

            return prefabPool;
        }

        /// <summary>
        /// Spawns a pool managed prefab.
        /// </summary>
        /// <param name="prefab">Prefab.</param>
        /// <param name="spawner">Spawner.</param>
        static public PoolManaged Spawn(PoolManaged prefab, GameObject spawner = null)
        {
            return prefabPoolsDict[prefab].Spawn(spawner);
        }

        /// <summary>
        /// Spawns the prefab referenced by it's PoolManaged component.
        /// </summary>
        /// <param name="prefab">Prefab.</param>
        static public PoolManaged SpawnAt(PoolManaged prefab, Vector3 position, Quaternion rotation, Transform parent = null, GameObject spawner = null)
        {
            return prefabPoolsDict[prefab].SpawnAt(position, rotation, parent, spawner);
        }

        /// <summary>
        /// Despawn all gameObjects in the provided despawnList.
        /// </summary>
        /// <param name='poolName'>
        /// Pool name.
        /// </param>
        /// <param name='despawnList'>
        /// Despawn list.
        /// </param>
        static public void DespawnList(List<PoolManaged> despawnList)
        {
            for (int i = 0; i < despawnList.Count; i++)
            {
                if (despawnList[i].gameObject.activeSelf == true)
                    despawnList[i].Despawn();
            }

            despawnList.Clear();
        }

        /// <summary>
        /// Despawns all.
        /// </summary>
        static public void DespawnAll()
        {
            foreach (PrefabPool pool in prefabPoolsDict.Values)
                pool.DespawnAll();
        }

        /// <summary>
        /// Clears all spawn pools
        /// </summary>
        private void Clear(bool garbageCollect = false)
        {
            foreach (PrefabPool pool in prefabPoolsDict.Values)
                pool.Clear();

            prefabPoolsDict.Clear();

            if (garbageCollect)
                System.GC.Collect();
        }
        #endregion
    }
}