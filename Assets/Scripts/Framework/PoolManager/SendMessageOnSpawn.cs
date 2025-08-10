using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Pooling
{
	/// Timed Despawn
	/// Despawns a PoolManaged gameObject after the specified time.
	/// By Jorge L. Chávez Herrera.
	[RequireComponent (typeof (PoolManaged))]
	public class SendMessageOnSpawn : MonoBehaviour 
	{
		#region Class members
		public GameObject target;
		public string message;
		#endregion

		#region MonoBehaviour overrides
		private void Awake () 
		{
			GetComponent<PoolManaged> ().onSpawn += SendMessage;

			if (target == null)
				target = this.gameObject;
		}
		#endregion
		
		#region Class implementation
		private void SendMessage ()
		{
			target.SendMessage (message, SendMessageOptions.DontRequireReceiver);
		}
		#endregion
	}
}
