using System.Collections;
using System.Collections.Generic;
using Framework.Utils;
using UnityEngine;


/// <summary>
/// Player Controller.
/// 
/// Defines behaviour for player controller.
/// 
/// By Jorge L. Chávez Herrera.
/// </summary>
public class PlayerController : SnowVehicle
{
    #region Class members
    [SerializeField] private Renderer[] renderers;
    private Vector3 prevMousePos;
    private Vector3 direction;

    static public Color[] colors = {Color.red, new Color(1, .5f, 0), new Color (1,0.75f,0), Color.green, new Color(0,1,1),
        Color.blue, new Color(1,0,1)};
    #endregion

    #region Class accessors
    private static PlayerController _instance;
    static public PlayerController Instance
    {
        get { return _instance; }    
    }
    #endregion

    #region MonoBehaviour events
    override protected void Awake()
    {
        base.Awake();
        cachedFSM.AddState("Idle", new SV_PlayerStateIdle(this));
        cachedFSM.ReplaceState("Patrol", new SV_PlayerStatePatrol(this));
    }
    #endregion

    #region Base class overrides
    override protected void GatherSettings()
    {
        // Gather player settings.
        speed = GameManager.Settings.data.playerSpeed;
        punchForce = GameManager.Settings.data.playerPunchForce;
        perfectPunchForce = GameManager.Settings.data.playerPerfectPunchForce;
        angle = new AngleSDInterpolator(1.0f / GameManager.Settings.data.playerTurnSpeed);
        snowballGrowSpeed = GameManager.Settings.data.playerSnowballGrowSpeed;
        killSizeIncrement = GameManager.Settings.data.playerKillSizeIncrement;
        killPunchForceIncrement = GameManager.Settings.data.playerKillPunchForceIncrement;
        killSpeedIncrement = GameManager.Settings.data.playerKillSpeedIncrement;
        killSnowBallGrowSpeedIncrement = GameManager.Settings.data.playerKillSnowballGrowSpeedIncrement;
    }

    override protected void OnSpawn()
    {
        _instance = this;

        base.OnSpawn();
        SetUserName(GameManager.Instance.GetUserName());
        Camera.main.GetComponent<CameraController>().SetTarget(cachedTransform);
        cachedFSM.SetState("Idle");
    }

    override public void GetKilled()
    {
        cachedFSM.SetState("Drowned");
        GameManager.Instance.Invoke("EndGame", 1);
    }
    #endregion

    #region Class implementattion
    /// <summary>
    /// Sets the color.
    /// </summary>
    /// <param name="index">Index.</param>
    public void SetColor(int index)
    {
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.SetColor("_Color", colors[index]);
    }

    /// <summary>
    /// Sets the name of the user.
    /// </summary>
    /// <param name="userName">User name.</param>
    public void SetUserName(string userName)
    {
        infoLabels.SetName(userName, false);
    }
    #endregion
}
