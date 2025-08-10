using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AIModels;

public class SV_NPCStatePatrol : StateBaseGeneric<SnowVehicle>
{
    #region Class members
    private const float EDGE_AWARENESS_DISTANCE = 2;
    #endregion

    #region Supaer class overrides
    public SV_NPCStatePatrol(SnowVehicle inOwner) : base(inOwner) { }

    public override void OnEnter()
    {
        owner.acceleration.targetValue = 1;
    }

    public override void OnUpdate()
    {
        owner.UpdateSnowBall();

        // If we are to close to an edge, switch to chase state to prevent falling down.
        if (NoGroundOnPoint(owner.cachedTransform.TransformPoint(new Vector3(0, 0, EDGE_AWARENESS_DISTANCE))))
            owner.cachedFSM.SetState("Chase");

        if (time > 5)
            owner.cachedFSM.SetState("Chase");
;    }
    #endregion

    #region Class implementation

    private bool NoGroundOnPoint(Vector3 worldPoint)
    {
        RaycastHit hit;

        if (Physics.Raycast(worldPoint, Vector3.down, out hit, Mathf.Infinity))
        {
            return hit.collider.CompareTag("Pit") || hit.collider.CompareTag("Ice");
        }

        return false;
    }
    #endregion
}
