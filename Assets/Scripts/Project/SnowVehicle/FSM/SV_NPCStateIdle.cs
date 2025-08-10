using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AIModels;

public class SV_NPCStateIdle : StateBaseGeneric<SnowVehicle>
{
    #region Class members
    #endregion

    #region Class implementattion
    public SV_NPCStateIdle(SnowVehicle inOwner) : base(inOwner) { }

    public override void OnEnter()
    {
        owner.acceleration.targetValue = 0;
    }
    #endregion
}
