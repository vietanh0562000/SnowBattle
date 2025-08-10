using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Framework.UI
{
	/// <summary>
	/// SetTextFromBuildVersion.
	/// Sets the lavel text according to Application build version number.
	/// 
	/// By Jorge L. Chavez Herrera.
	/// </summary>
	[RequireComponent (typeof(Text))]
	public class SetTextFromBuildVersion : MonoBehaviour
	{
		#region MonoBehaviour overrides
		private void Start ()
	    {
	        GetComponent<Text>().text = "V " + Application.version;
		}
		#endregion
	}
}
