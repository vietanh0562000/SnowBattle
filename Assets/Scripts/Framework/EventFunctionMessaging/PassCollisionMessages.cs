using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework.EventFunctionMessaging
{
	/// <summary>
	/// PassCollisionMessages.cs
	/// Passes MonoBehaviour collision triggered by UnityPhisycs to the target GameObject.
	/// 
	/// By Jorge L. Chávez Herrera
	/// </summary>
	public class PassCollisionMessages : MonoBehaviour 
	{
		#region Class members
		public GameObject target;
		#endregion

		#region MonoBehaviour overrides
		/// <summary>
		/// Passes the collision enter event call to the target gameObject.
		/// </summary>
		/// <param name="collision">Collision.</param>
		private void OnCollisionEnter (Collision collision)
		{
			target.SendMessage ("OnCollisionEnter", collision, SendMessageOptions.DontRequireReceiver);
		}
			
		/// <summary>
		/// Passes the collision stay event call to the target gameObject.
		/// </summary>
		/// <param name="collision">Collision.</param>
		private void OnCollisionStay (Collision collision)
		{
			target.SendMessage ("OnCollisionStay", collision, SendMessageOptions.DontRequireReceiver);
		}

		/// <summary>
		/// Passes the collision exit event call to the target gameObject.
		/// </summary>
		/// <param name="collision">Collision.</param>
		private void OnCollisionExit (Collision collision)
		{
			target.SendMessage ("OnCollisionExit", collision, SendMessageOptions.DontRequireReceiver);
		}
		#endregion
	}
}
