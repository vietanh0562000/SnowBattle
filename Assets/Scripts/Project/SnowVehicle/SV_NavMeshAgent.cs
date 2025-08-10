using System.Collections;
using System.Collections.Generic;
using Framework.Utils;
using UnityEngine;
using UnityEngine.AI;

public class SV_NavMeshAgent : OptimizedGameObject
{
    #region Class members
    [System.NonSerialized]
    public Transform target;
    public float targetPositionUpdate = 0.5f;
    #endregion

    #region Class accessors
    protected NavMeshAgent _cachedNavMeshAgent;
    virtual public NavMeshAgent cachedNavMeshAgent
    {
        get
        {
            if (_cachedNavMeshAgent == null)
                _cachedNavMeshAgent = GetComponent<NavMeshAgent>();

            return _cachedNavMeshAgent;
        }
    }
    #endregion

    #region Monobehaviour events
    private void Awake()
    {
        cachedPoolManaged.onSpawn += OnSpawn;
    }

    private void OnDestroy()
    {
        cachedPoolManaged.onDespawn -= OnSpawn;
    }
    #endregion

    #region Class implementattion
    private void OnSpawn()
    {
        StartCoroutine(UpdateTargetPosition(targetPositionUpdate));
    }

    private IEnumerator UpdateTargetPosition(float delay)
    {
        while (true)
        {
            if (target != null)
            {
                cachedNavMeshAgent.SetDestination(target.position);
            }

            yield return new WaitForSeconds(delay);
        }
    }
    #endregion
}
