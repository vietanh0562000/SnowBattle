using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Framework.UserInput;
using Framework.Delegates;
using Framework.Utils;

/// <summary>
/// MouseController.
/// Mouse only Game Controller.
/// 
/// Create by Jorge L. Chavez Herrera.
/// </summary>
public class MouseController : GameController 
{
	#region Class members
	private Vector3 targetWorldPos;
	private float lastClickTime;
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
		return Input.GetMouseButton (0);
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
		if ((targetWorldPos - controlledTransform.position).magnitude > 0.25f)
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
		if (Input.GetMouseButtonDown (0)) 
		{
			targetWorldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			targetWorldPos.y -= Camera.main.transform.position.y;

			if (Time.time - lastClickTime < 0.25f && aButtonDownDelegate != null && !EventSystem.current.IsPointerOverUIObject())
				aButtonDownDelegate ();
		}

		if (Input.GetMouseButtonDown (1)) 
		{
			if (bButtonDownDelegate != null)
				bButtonDownDelegate ();
		}

		if (Input.GetMouseButton (0)) 
		{
			targetWorldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			targetWorldPos.y -= Camera.main.transform.position.y;
		}

		if (Input.GetMouseButtonUp (0)) 
		{
			if (aButtonUpDelegate != null)
				aButtonUpDelegate ();

			lastClickTime = Time.time;
		}

		if (Input.GetMouseButtonUp (1)) 
		{
			if (bButtonUpDelegate != null)
				bButtonUpDelegate ();
		}
	}

	override public void ResetController () {}
	#endregion
}
