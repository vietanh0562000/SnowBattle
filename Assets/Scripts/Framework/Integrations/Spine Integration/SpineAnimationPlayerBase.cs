using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineAnimationPlayerBase : MonoBehaviour 
{
	public void Stop ()
	{
		StopAllCoroutines ();
	}
}
