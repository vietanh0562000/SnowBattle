using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AIModels;

public class SV_PlayerStateIdle : StateBaseGeneric<SnowVehicle>
{
    #region Class members
    private Vector3 prevMousePosition;
    #endregion

    #region Class implementattion
    public override void OnEnter()
    {
        owner.acceleration.InstantValue = 0;
        owner.angle.ResetAndTarget(180, 180, 0.125f);
    }

    public SV_PlayerStateIdle(SnowVehicle inOwner) : base(inOwner) { }

    public override void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
            prevMousePosition = Input.mousePosition;
        
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - prevMousePosition;
            owner.angle.targetValue += -delta.x * Time.deltaTime * owner.speed;
        }

        prevMousePosition = Input.mousePosition;
    }

    public override void OnExit()
    {
        owner.angle.smoothTime = 1.0f / GameManager.Settings.data.playerTurnSpeed;
    }
    #endregion
}
