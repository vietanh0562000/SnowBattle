using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.AIModels;
using Framework.Tweening;

public class SV_StateDrowned : StateBaseGeneric<SnowVehicle>
{
    public SV_StateDrowned(SnowVehicle inOwner) : base(inOwner) { }

    public override void OnEnter()
    {
        owner.ice.gameObject.SetActive(true);
        owner.ice.localScale = Vector3.one;
       
        TweenUtils.TweenFloat (owner, owner.size, 0, 1, 2, ETweenType.EaseOut, 
                               (float value) => { owner.size = value; }, 
                                null);
        
        owner.StartCoroutine(ExitCoroutine());
    }

    private IEnumerator ExitCoroutine()
    {
        yield return new WaitForSeconds(2);
        owner.cachedPoolManaged.Despawn();

        if (owner.CompareTag("Player") == false && owner.cachedPoolManaged.prefabPool.spawnedCount == 0)
            GameManager.Instance.EndGame();
    }
}
