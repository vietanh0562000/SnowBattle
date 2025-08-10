using System.Collections;
using System.Collections.Generic;
using Framework.Pooling;
using Framework.Tweening;
using Framework.Utils;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// NPC Controller.
/// 
/// Defines a behaviour for NPCs.
/// 
/// By Jorge L. Chávez Herrera.
/// </summary>
public class NPCController : SnowVehicle
{
    #region Class members
    public PoolManaged navMeshAgentPrefab;
    private static UniqueRandomSequence variation;
    private Variations cachedVariations;
    [System.NonSerialized]
    public SV_NavMeshAgent navMeshAgent;
    #endregion

    #region MonoBehaciour events
    protected override void Awake()
    {
        base.Awake();
        PoolManager.CreatePoolForPrefab(navMeshAgentPrefab);
        cachedFSM.AddState("Idle", new SV_NPCStateIdle(this));
        cachedFSM.AddState("Chase", new SV_NPCStateChase(this));
        cachedVariations = GetComponent<Variations>();
        variation = new UniqueRandomSequence(cachedVariations.Count);
    }
    #endregion

    #region Base class overrides
    override protected void GatherSettings()
    {
        // Gather player settings.
        speed = GameManager.Settings.data.npcSpeed;
        punchForce = GameManager.Settings.data.npcPunchForce;
        perfectPunchForce = GameManager.Settings.data.npcPerfectPunchForce;
        angle = new AngleSDInterpolator(1.0f / GameManager.Settings.data.npcTurnSpeed);
        snowballGrowSpeed = GameManager.Settings.data.npcSnowballGrowSpeed;
        killSizeIncrement = GameManager.Settings.data.npcKillSizeIncrement;
        killPunchForceIncrement = GameManager.Settings.data.npcKillPunchForceIncrement;
        killSpeedIncrement = GameManager.Settings.data.npcKillSpeedIncrement;
        killSnowBallGrowSpeedIncrement = GameManager.Settings.data.npcKillSnowballGrowSpeedIncrement;
    }

    override protected void OnSpawn()
    {
        base.OnSpawn();

        cachedVariations.Variation = variation.nextValue;
        float startDirection = cachedTransform.eulerAngles.y;
        angle.ResetAndTarget(startDirection, startDirection, 1.0f / GameManager.Settings.data.npcTurnSpeed);
        cachedFSM.SetState("Idle");
        // Setup NavMeshAgent
        navMeshAgent = PoolManager.SpawnAt(navMeshAgentPrefab, cachedTransform.position, cachedTransform.rotation).GetComponent<SV_NavMeshAgent>();
        navMeshAgent.cachedNavMeshAgent.speed = speed + 1;
        //TweenUtils.TweenFloat(this, .1f, 1, 0.5f, 0, ETweenType.BounceOut, (value) => { spawnSize = value; }, null);
    }

    protected override void OnDespawn()
    {
        cachedFSM.SetState(null);
        base.OnDespawn();
        navMeshAgent.GetComponent<PoolManaged>().Despawn();
    }
    #endregion

    #region Class implementation

    public void Patrol()
    {
        cachedFSM.SetState("Patrol");
    }
    #endregion
}
