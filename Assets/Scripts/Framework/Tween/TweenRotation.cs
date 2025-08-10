using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework.Tweening
{
	public class TweenRotation : MonoBehaviour 
	{
		public Vector3 speed;

		private void Update ()
		{
			transform.Rotate (new Vector3 (speed.x * Time.deltaTime, speed.y * Time.deltaTime, speed.z * Time.deltaTime));
		}
	}
}
