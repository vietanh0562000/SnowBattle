using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AIModels;

public class SV_PlayerStatePatrol : StateBaseGeneric<SnowVehicle>
{
    private Vector3 prevMousePos;
    private Vector3 direction;

    public SV_PlayerStatePatrol(SnowVehicle inOwner) : base(inOwner) { }

    public override void OnEnter()
    {
        //owner.acceleration.targetValue = 1;
    }

    override public void OnUpdate()
    {
        owner.UpdateSnowBall();

        // User pressed the screen
        if (Input.GetMouseButtonDown(0))
        {
            owner.acceleration.targetValue = 1;
            prevMousePos = Input.mousePosition;
        }

        // User is still pressing the screen
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - prevMousePos;

            if (delta.magnitude > 1)
            {
                direction = delta.normalized;
                Quaternion lookRotaton = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
                owner.angle.targetValue = (Mathf.LerpAngle(
                    owner.angle.targetValue, 
                    lookRotaton.eulerAngles.y, 
                    owner.acceleration.Value));
            }
            else
                owner.angle.targetValue = owner.angle.Value;
        }

        // Fire snowballs.
        if (Input.GetMouseButtonUp(0))
        {
            owner.acceleration.targetValue = 0;

            if (owner.snowball)
                owner.FireSnowball();
        }

        prevMousePos = Input.mousePosition;
    }
}
