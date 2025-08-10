using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework.EventFunctionMessaging
{
	/// <summary>
	/// Pass renderer messages.
	/// Passes MonoBehaviour messages triggered by UnityRenderer to the target GameObject.
	/// 
	/// By Jorge L. Chávez Herrera
	/// </summary>
	public class PassRendererMessages : MonoBehaviour 
	{
		#region Class members
		public GameObject target;
		#endregion

		#region MonoBehaviour overrides
		/// <summary>
		/// Passes the became visible event call to the target gameObject.
		/// </summary>
		private void OnBecameVisible ()
		{

			target.SendMessage ("OnBecameVisible", SendMessageOptions.DontRequireReceiver);
		}

		/// <summary>
		/// Passes the became invisible event call to the target gameObject.
		/// </summary>
		private void OnBecameInvisible ()
		{
			target.SendMessage ("OnBecameInvisible", SendMessageOptions.DontRequireReceiver);
		}

		/// <summary>
		/// Passes the pre cull event call to the target gameObject.
		/// </summary>
		private void OnPreCull ()
		{
			target.SendMessage ("OnPreCull", SendMessageOptions.DontRequireReceiver);
		}

		/// <summary>
		/// Passes the pre render event call to the target gameObject.
		/// </summary>
		private void OnPreRender ()
		{
			target.SendMessage ("OnPreRender", SendMessageOptions.DontRequireReceiver);
		}

		/// <summary>
		/// Passes the post render event call to the target gameObject.
		/// </summary>
		private void OnPostRender ()
		{
			target.SendMessage ("OnPostRender", SendMessageOptions.DontRequireReceiver);
		}

		/// <summary>
		/// Passes the render object event call to the target gameObject.
		/// </summary>
		private void OnRenderObject ()
		{
			target.SendMessage ("OnRenderObject", SendMessageOptions.DontRequireReceiver);
		}
		#endregion
	}
}
