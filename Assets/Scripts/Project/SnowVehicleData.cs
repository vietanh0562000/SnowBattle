using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowVehicleData : ScriptableObject
{
    #region Class members
    public string snowVehicleName;
    public float punchForce = 1;
    public float speed = 1;
    public float snowBallGrowSpeed = 1;
    public int price = 1000;
    public int powerIncrease;
    public int speedIncrease;

    public GameObject model;
    #endregion
}
