using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Pooling
{
	/// Timed Despawn
	/// Despawns a PoolManaged gameObject after the specified time.
	/// By Jorge L. Chávez Herrera.
	[RequireComponent (typeof (PoolManaged))]
	public class TimedDespawn : MonoBehaviour 
	{
		#region Class members
		public float despawnDelay = 1;
		#endregion

		#region MonoBehaviour overrides
		private void Awake () 
		{
			GetComponent<PoolManaged> ().onSpawn += DespawnAfterDelay;
		}
		#endregion

		#region Class implementation
		private void DespawnAfterDelay ()
		{
			GetComponent<PoolManaged> ().Invoke ("Despawn", despawnDelay);
		}
		#endregion
	}
}
