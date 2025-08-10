using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.EventFunctionMessaging
{
	/// <summary>
	/// Pass trigger 2D messages.
	/// Passes MonoBehaviour trigger 2D calls triggered by UnityPhisycs to the target GameObject.
	/// 
	/// By Jorge L. Chávez Herrera
	/// </summary>
	public class PassTrigger2DMessages : MonoBehaviour 
	{
		#region Class members
		public GameObject target;
		#endregion

		#region MonoBehaviour overrides
		/// <summary>
		/// Passes the trigger 2D enter event call to the target gameObject.
		/// </summary>
		/// <param name="collision">Collision.</param>
		private void OnTriggerEnter2D (Collider2D collider)
		{
			target.SendMessage ("OnTriggerEnter", collider, SendMessageOptions.DontRequireReceiver);
		}

		/// <summary>
		/// Passes the trigger 2D stay event call to the target gameObject.
		/// </summary>
		/// <param name="collision">Collision.</param>
		private void OnTriggerStay2D (Collider2D collider)
		{
			target.SendMessage ("OnTriggerStay", collider, SendMessageOptions.DontRequireReceiver);
		}

		/// <summary>
		/// Passes the trigger 2D exit event call to the target gameObject.
		/// </summary>
		/// <param name="collision">Collision.</param>
		private void OnTriggerExit2D (Collider2D collider)
		{
			target.SendMessage ("OnTriggerExit", collider, SendMessageOptions.DontRequireReceiver);
		}
		#endregion
	}
}
