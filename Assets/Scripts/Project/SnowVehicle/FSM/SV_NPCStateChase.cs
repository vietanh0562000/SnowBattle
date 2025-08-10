using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Framework.AIModels;
using Framework.Utils;

public class SV_NPCStateChase : StateBaseGeneric<NPCController>
{
    #region Class members
    private static List<SnowVehicle> chosenTargets = new List<SnowVehicle>();
    private SnowVehicle target;
    #endregion

    #region Super class overrides
    public SV_NPCStateChase(NPCController inOwner) : base(inOwner) {}

    public override void OnEnter()
    {
        owner.acceleration.targetValue = 1;
        // Pick a target
        target = FindTarget();
        owner.navMeshAgent.cachedTransform.position = owner.cachedTransform.TransformPoint(Vector3.forward);
        owner.navMeshAgent.target = target.transform;
        chosenTargets.Add(target);
    }

    public override void OnExit()
    {
        chosenTargets.Remove(target);
    }

    public override void OnUpdate()
    {
        owner.UpdateSnowBall();

        // Chase NavMeshAent to avoid obstacles and find target
        owner.angle.targetValue = VectorTools.GetAngleXZ(owner.cachedTransform.position, owner.navMeshAgent.cachedTransform.position);
         
        // Find if we are facing the target in a range close enough to shoot the snowball
        Vector3 heading = owner.navMeshAgent.target.position - owner.cachedTransform.position;
        float dot = Vector3.Dot(heading.normalized, owner.cachedTransform.forward);

        if (owner.snowball != null && owner.snowball.Size > 0.6f && dot > 0.99f)
        {
            owner.FireSnowball();
            owner.cachedFSM.SetState("Patrol");
        }

        if (time > 10)
            owner.cachedFSM.SetState("Patrol");
    }
    #endregion


    #region Class implementattion
    private SnowVehicle FindTarget()
    {
        // Gather all available snow vehicles.
        List<SnowVehicle> snowVehicles = new List<SnowVehicle>(GameObject.FindObjectsOfType<SnowVehicle>());
        List<SnowVehicle> filteredSnowVehicles = new List<SnowVehicle>();

        // Filter ourselves && all other already chosen snow vehicles.
        for (int i = 0; i < snowVehicles.Count; i++)
        {
            if (snowVehicles[i] != (owner as SnowVehicle) && chosenTargets.Contains(snowVehicles[i]) == false)
                filteredSnowVehicles.Add(snowVehicles[i]);
        }

        // Find the first player snow vehicle
        foreach (SnowVehicle snowVehicle in filteredSnowVehicles)
        {
            if (snowVehicle.CompareTag("Player"))
            {
                return snowVehicle;
            }
        }

        // Find the first frozen snow vehicle
        foreach (SnowVehicle snowVehicle in filteredSnowVehicles)
        {
            if (snowVehicle.cachedFSM.CurrentStateIs("Frozen"))
            {
                return snowVehicle;
            }
        }

        // No frozen snow vehicles found, pick a random one.
        return filteredSnowVehicles[Random.Range(0, filteredSnowVehicles.Count)]; 
    }

    #endregion
}
