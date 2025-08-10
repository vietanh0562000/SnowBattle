using System.Collections;
using System.Collections.Generic;
using Framework.Utils;
using UnityEngine;


namespace Framework.UserInput
{
	/// <summary>
	/// KeyboardController.
	/// Keyboard only Game Controller.
	/// 
	/// Create by Jorge L. Chavez Herrera.
	/// </summary>
	public class KeyboardController : GameController 
	{
		#region Class members
		private Vector3 targetWorldPos;
		private float speed;
		#endregion

		#region GameController overrides
		/// <summary>
		/// Sets the controlled transform.
		/// </summary>
		/// <param name="t">T.</param>
		override public void SetControlledTransform (Transform  t)
		{
			controlledTransform = t;
		}

		/// <summary>
		/// Determines whether this instance is stick active the specified stick.
		/// </summary>
		/// <returns><c>true</c> if this instance is stick active the specified stick; otherwise, <c>false</c>.</returns>
		/// <param name="stick">Stick.</param>
		override public bool IsStickActive (int stick)
		{
			return Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.DownArrow);
		}

		/// <summary>
		/// Gets the speed.
		/// </summary>
		/// <returns>The speed.</returns>
		/// <param name="stick">Stick.</param>
		override public float GetSpeed (int stick)
		{
			return IsStickActive (0) == true ? 1 : 0; 
		}

		/// <summary>
		/// Gets the angle.
		/// </summary>
		/// <returns>The angle.</returns>
		/// <param name="stick">Stick.</param>
		override public float GetAngle (int stick)
		{
			return VectorTools.GetAngleXZ (targetWorldPos, controlledTransform.position);
		}

		/// <summary>
		/// Gets the raw horizontal axis.
		/// </summary>
		/// <returns>The raw horizontal axis.</returns>
		/// <param name="stick">Stick.</param>
		override public float GetRawHorizontalAxis (int stick)
		{
			if ( (targetWorldPos - controlledTransform.position).magnitude > 0.25f)
				return (targetWorldPos - controlledTransform.position).normalized.x;
			
			return 0;
		}

		/// <summary>
		/// Gets the raw vertical axis.
		/// </summary>
		/// <returns>The raw vertical axis.</returns>
		/// <param name="stick">Stick.</param>
		override public float GetRawVerticalAxis (int stick)
		{
			if ( (targetWorldPos - controlledTransform.position).magnitude > 0.25f)
				return (targetWorldPos - controlledTransform.position).normalized.z;

			return 0;
		}

		/// <summary>
		/// Determines whether this instance is running in right platform.
		/// </summary>
		/// <returns><c>true</c> if this instance is running in right platform; otherwise, <c>false</c>.</returns>
		override public bool IsRunningInRightPlatform ()
		{
			return Application.isMobilePlatform == false;
		}

		/// <summary>
		/// Updates this instance
		/// </summary>
		override public void DoUpdate ()
		{
			// Allow multiple direction keys
			Vector3 dir = Vector3.zero;

			// Left
			if (Input.GetKey (KeyCode.LeftArrow))
				dir += new Vector3 (-1, 0, 0);

			// Right
			if (Input.GetKey (KeyCode.RightArrow))
				dir += new Vector3 (1, 0, 0);
		
			// Up
			if (Input.GetKey (KeyCode.UpArrow))
				dir += new Vector3 (0, 0, 1);

			// Down
			if (Input.GetKey (KeyCode.DownArrow))
				dir += new Vector3 (0, 0, -1);

			if (controlledTransform != null)
				targetWorldPos = controlledTransform.position + dir;

			// A button
			if (Input.GetKeyDown (KeyCode.Z)) 
			{
				if (aButtonDownDelegate != null)
					aButtonDownDelegate ();
			}

			if (Input.GetKeyUp (KeyCode.Z)) 
			{
				if (aButtonUpDelegate != null)
					aButtonUpDelegate ();
			}

			// B button
			if (Input.GetKeyDown (KeyCode.X)) 
			{
				if (bButtonDownDelegate != null)
					bButtonDownDelegate ();
			}

			if (Input.GetKeyUp (KeyCode.X)) 
			{
				if (bButtonUpDelegate != null)
					bButtonUpDelegate ();
			}
		}

		override public void ResetController () {}
		#endregion
	}
}
