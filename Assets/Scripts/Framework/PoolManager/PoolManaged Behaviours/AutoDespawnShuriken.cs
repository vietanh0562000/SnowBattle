using UnityEngine;
using System.Collections;


namespace Framework.Utils
{
	/// <summary>
	/// Auto despawn shuriken.
	/// Despawns all this GameObject if all particle systems in hierachy are not alive
	/// </summary>
	public class AutoDespawnShuriken : OptimizedGameObject
	{
		#region Class members
	    private ParticleSystem[] particleSystems;
		#endregion

		#region MonoBehaviour overrides
	    private void Awake()
	    {
	        particleSystems = GetComponentsInChildren<ParticleSystem>();
			cachedPoolManaged.onSpawn += OnSpawn;
			cachedPoolManaged.onDespawn += OnDespawn;
	    }

		private void OnEnable()
		{
			StopAllCoroutines ();
			StartCoroutine(CheckIfAlive());
		}

		#endregion

		#region PoolManaged delegates
	    private void OnSpawn()
	    {
	        foreach (ParticleSystem ps in particleSystems)
	            ps.Play();
	    }

	    private void OnDespawn()
	    {
	        foreach (ParticleSystem ps in particleSystems)
	            ps.Stop();
	    }
		#endregion

		#region Class implementattion
		// We only check particle systems are still alive every half of a seccond to save computing power.
	    IEnumerator CheckIfAlive()
	    {
	        while (true)
	        {
	            yield return new WaitForSeconds(0.5f);

	            int aliveSystems = 0;
	            foreach (ParticleSystem ps in particleSystems)
	            {
	                if (ps.IsAlive())
	                    aliveSystems++;
	            }

	            if (aliveSystems == 0)
	            {
	                cachedPoolManaged.Despawn();
	                break;
	            }
	        }
	    }
		#endregion
	}
}