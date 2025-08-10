using System.Collections;
using UnityEngine;
using Framework.AIModels;
using Framework.Tweening;

public class SV_StateFrozen : StateBaseGeneric<SnowVehicle>
{
    #region Class members
    private float freezeTime;
    #endregion

    #region Super class overrides
    public SV_StateFrozen(SnowVehicle inOwner) : base(inOwner) { }

    public override void OnEnter()
    {
        if (owner.snowball)
            owner.snowball.cachedPoolManaged.Despawn();
        
        owner.immunity = float.MaxValue;
        owner.acceleration.targetValue = 0;
        owner.angle.ResetAndTarget(owner.angle.Value, owner.angle.Value);
        owner.StartCoroutine(FreezeCoroutine(GameManager.Settings.data.freezeTime));
        freezeTime = GameManager.Settings.data.freezeTime;

        owner.infoLabels.ShowBuffInfo(true, Color.cyan, owner.freezeIcon);
    }
    #endregion

    #region CLass implementation
    IEnumerator FreezeCoroutine(float duration)
    {
        TweenUtils.TweenFloat(owner, 0, 1, 0.5f, 0, ETweenType.EaseInOut, IceScaleUpdate, null);
        yield return new WaitForSeconds(duration - 1);
        TweenUtils.TweenFloat(owner, 1, 0, 1, 0, ETweenType.EaseInOut, IceScaleUpdate, null);
        owner.cachedFSM.SetState("Patrol");
    }

    private void IceScaleUpdate(float iceScale)
    {
        owner.ice.localScale = Vector3.one * iceScale;
        owner.cachedCharacterController.radius = Mathf.Lerp(0.9f, 1.5f, iceScale);
    }

    public override void OnUpdate()
    {
        freezeTime -= Time.deltaTime;
        owner.infoLabels.SetBuffProgress(freezeTime / GameManager.Settings.data.freezeTime);
    }

    public override void OnExit()
    {
        owner.immunity = 2;
        owner.acceleration.targetValue = 1;
        owner.infoLabels.ShowBuffInfo(false, Color.cyan, owner.freezeIcon);
    }
    #endregion
}
