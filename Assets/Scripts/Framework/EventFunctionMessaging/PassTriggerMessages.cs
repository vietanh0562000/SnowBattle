using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.EventFunctionMessaging
{
	/// <summary>
	/// Pass trigger messages.
	/// Passes MonoBehaviour trigger calls triggered by UnityPhisycs to the target GameObject.
	/// 
	/// By Jorge L. Chávez Herrera
	/// </summary>
	public class PassTriggerMessages : MonoBehaviour 
	{
		#region Class members
		public GameObject target;
		#endregion

		#region MonoBehaviour overrides
		/// <summary>
		/// Passes the trigger enter event call to the target gameObject.
		/// </summary>
		/// <param name="collision">Collision.</param>
		private void OnTriggerEnter (Collider collider)
		{
			target.SendMessage ("OnTriggerEnter", collider, SendMessageOptions.DontRequireReceiver);
		}

		/// <summary>
		/// Passes the trigger stay event call to the target gameObject.
		/// </summary>
		/// <param name="collision">Collision.</param>
		private void OnTriggerStay (Collider collider)
		{
			target.SendMessage ("OnTriggerStay", collider, SendMessageOptions.DontRequireReceiver);
		}

		/// <summary>
		/// Passes the trigger exit event call to the target gameObject.
		/// </summary>
		/// <param name="collision">Collision.</param>
		private void OnTriggerExit (Collider collider)
		{
			target.SendMessage ("OnTriggerExit", collider, SendMessageOptions.DontRequireReceiver);
		}
		#endregion
	}
}
