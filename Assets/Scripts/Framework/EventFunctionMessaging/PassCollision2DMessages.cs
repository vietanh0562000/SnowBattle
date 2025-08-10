using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework.EventFunctionMessaging
{
	/// <summary>
	/// PassCollision2DMessages.cs
	/// Passes MonoBehaviour collision 2D triggered by UnityPhisycs to the target GameObject.
	/// 
	/// By Jorge L. Chávez Herrera
	/// </summary>
	public class PassCollision2DMessages : MonoBehaviour 
	{
		#region Class members
		public GameObject target;
		#endregion

		#region MonoBehaviour overrides
		/// <summary>
		/// Passes the collision 2D enter event call to the target gameObject.
		/// </summary>
		/// <param name="collision">Collision.</param>
		private void OnCollisionEnter2D (Collision2D collision)
		{
			target.SendMessage ("OnCollisionEnter2D", collision, SendMessageOptions.DontRequireReceiver);
		}
			
		/// <summary>
		/// Passes the collision 2D stay event call to the target gameObject.
		/// </summary>
		/// <param name="collision">Collision.</param>
		private void OnCollisionStay2D (Collision2D collision)
		{
			target.SendMessage ("OnCollisionStay2D", collision, SendMessageOptions.DontRequireReceiver);
		}

		/// <summary>
		/// Passes the collision 2D exit event call to the target gameObject.
		/// </summary>
		/// <param name="collision">Collision.</param>
		private void OnCollisionExit2D (Collision2D collision)
		{
			target.SendMessage ("OnCollisionExit2D", collision, SendMessageOptions.DontRequireReceiver);
		}
		#endregion
	}
}
