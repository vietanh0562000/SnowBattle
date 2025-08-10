using UnityEngine;
using System.Collections;


namespace Framework.Utils
{
	/// <summary>
	/// DontDontDestroy.
	/// Marks the gameObject to not be destroyed on scene load.
	/// Use it only on hierachy roots.
	/// 
	/// By Jorge L.Chávez Herrera.
	/// </summary>
	public class DontDestroy : MonoBehaviour 
	{
		#region MonoBehaviour overrides
		private void Awake () 
		{
			DontDestroyOnLoad (this.gameObject);
		}
		#endregion
	}
}
