using UnityEngine;
using System.Collections;
using Framework.Pooling;

namespace Framework.Utils
{
	/// <summary>
	/// Auto despawn shuriken.
	/// Despawns this GameObject if TrailRenderer has no more vertices.
	/// </summary>
    [RequireComponent (typeof(PoolManaged))]
	public class AutoDespawnTrailRenderer : OptimizedGameObject
	{
		#region Class members
        private TrailRenderer[] trailRenderers;
		#endregion

		#region MonoBehaviour overrides
	    private void Awake()
	    {
            trailRenderers = GetComponentsInChildren<TrailRenderer>();
	    }

		private void OnEnable()
		{
			StopAllCoroutines ();
			StartCoroutine(CheckIfAlive());
		}

		#endregion

		#region Class implementattion
		// We only check if TrailRenderers are still alive every half of a seccond to save computing power.
	    IEnumerator CheckIfAlive()
	    {
	        while (true)
	        {
	            yield return new WaitForSeconds(0.5f);

	            int aliveTrailRenderers = 0;

                foreach (TrailRenderer tr in trailRenderers)
	            {
                    if (tr.positionCount > 0)
                        aliveTrailRenderers++;
	            }

                if (aliveTrailRenderers == 0)
	            {
	                cachedPoolManaged.Despawn();
	                break;
	            }
	        }
	    }
		#endregion
	}
}