using UnityEngine;
using Framework.Utils;


namespace Framework.UserInput
{
	/// <summary>
	/// Game Pad.
	/// Game Pad input method.
	/// 
	/// Create by Jorge L. Chavez Herrera.s
	/// </summary>
	public class GamePad : GameController
	{
		#region Subclasses
		// Holds required information to represent an analog stick
		private class AnalogStick
		{
			public string horizontalAxisName;
			public string verticalAxisName;
			public Vector2 axisValues;
			
			public AnalogStick (string horizontalAxisName, string verticalAxisName)
			{
				this.horizontalAxisName = horizontalAxisName;
				this.verticalAxisName = verticalAxisName;
			}
			
			/// <summary>
			/// Update the this analog stick based on Input axis info.
			/// </summary>
			/// <param name="touch">Touch.</param>
			public void Update ()
			{
				axisValues = new Vector2 (Input.GetAxis (horizontalAxisName), Input.GetAxis (verticalAxisName));
			}
			
			/// <summary>
			/// Gets a normalized speed value based on the distance between 2 the stick and its base
			/// </summary>
			/// <returns>The speed.</returns>
			public float GetSpeed ()
			{
				float dst = Vector2.Distance (Vector2.zero, axisValues);
				
				return dst;
			}
			
			/// <summary>
			/// Gets the joystick angle.
			/// </summary>
			/// <returns>The angle.</returns>
			public float GetAngle ()
			{
				return VectorTools.GetAngle (Vector2.zero,axisValues);
			}
		}
		#endregion
		
		#region Class members
		public string joystick1Name = "Joy1";
		public string joystick2Name = "Joy2";
		private AnalogStick[] analogSticks;
		#endregion
		
		#region MonoBehavior overrides
		void Awake ()
		{
			// Create 2 analog sticks side by side horizontally on screen
			analogSticks = new AnalogStick[] { 
				new AnalogStick (joystick1Name + "Horizontal", joystick1Name + "Vertical"),
				new AnalogStick (joystick2Name + "Horizontal", joystick2Name + "Vertical")
			};
		}
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
			if (stick > 1)
				return false;
			
			return analogSticks[stick].axisValues.magnitude != 0;
		}
		
		/// <summary>
		/// Gets the speed.
		/// </summary>
		/// <returns>The speed.</returns>
		/// <param name="stick">Stick.</param>
		override public float GetSpeed (int stick)
		{
			if (stick > 1)
				return 0;
			
			return analogSticks[stick].GetSpeed ();
		}
		
		/// <summary>
		/// Gets the angle.
		/// </summary>
		/// <returns>The angle.</returns>
		/// <param name="stick">Stick.</param>
		override public float GetAngle (int stick)
		{
			if (stick > 1)
				return 0;
			
			return analogSticks[stick].GetAngle ();
		}

		/// <summary>
		/// Gets the raw horizontal axis.
		/// </summary>
		/// <returns>The raw horizontal axis.</returns>
		/// <param name="stick">Stick.</param>
		override public float GetRawHorizontalAxis (int stick)
		{
			return Input.GetAxisRaw (analogSticks[stick].horizontalAxisName);
		}

		/// <summary>
		/// Gets the raw vertical axis.
		/// </summary>
		/// <returns>The raw vertical axis.</returns>
		/// <param name="stick">Stick.</param>
		override public float GetRawVerticalAxis (int stick)
		{
			return Input.GetAxisRaw (analogSticks[stick].verticalAxisName);
		}

		/// <summary>
		/// Determines whether this instance is running in right platform.
		/// </summary>
		/// <returns><c>true</c> if this instance is running in right platform; otherwise, <c>false</c>.</returns>
		override public bool IsRunningInRightPlatform ()
		{
			return true;
		}

		/// <summary>
		/// Updates this instance
		/// </summary>
		override public void DoUpdate ()
		{
			for (int i = 0; i < analogSticks.Length; i++)
			{
				analogSticks[i].Update ();
			}

			// Buttons Down
			if (xButtonDownDelegate != null && Input.GetKeyDown ("joystick 1 button 0"))
				xButtonDownDelegate ();

			if (aButtonDownDelegate != null && Input.GetKeyDown ("joystick 1 button 1"))
				aButtonDownDelegate ();

			if (bButtonDownDelegate != null && Input.GetKeyDown ("joystick 1 button 2"))
				bButtonDownDelegate ();

			if (yButtonDownDelegate != null && Input.GetKeyDown ("joystick 1 button 3"))
				yButtonDownDelegate ();

			if (l1ButtonDownDelegate != null && Input.GetKeyDown ("joystick 1 button 4"))
				l1ButtonDownDelegate ();

			if (r1ButtonDownDelegate != null && Input.GetKeyDown ("joystick 1 button 5"))
				r1ButtonDownDelegate ();

			if (l2ButtonDownDelegate != null && Input.GetKeyDown ("joystick 1 button 6"))
				l2ButtonDownDelegate ();

			if (r2ButtonDownDelegate != null && Input.GetKeyDown ("joystick 1 button 7"))
				r2ButtonDownDelegate ();

			if (backButtonDownDelegate != null && Input.GetKeyDown ("joystick 1 button 8"))
				backButtonDownDelegate ();

			if (startButtonDownDelegate != null && Input.GetKeyDown ("joystick 1 button 9"))
				startButtonDownDelegate ();

			// Buttons Up
			if (xButtonUpDelegate != null && Input.GetKeyUp ("joystick 1 button 0"))
				xButtonUpDelegate ();

			if (aButtonUpDelegate != null && Input.GetKeyUp ("joystick 1 button 1"))
				aButtonUpDelegate ();

			if (bButtonUpDelegate != null && Input.GetKeyUp ("joystick 1 button 2"))
				bButtonUpDelegate ();

			if (yButtonUpDelegate != null && Input.GetKeyUp ("joystick 1 button 3"))
				yButtonUpDelegate ();

			if (l1ButtonUpDelegate != null && Input.GetKeyUp ("joystick 1 button 4"))
				l1ButtonUpDelegate ();

			if (r1ButtonUpDelegate != null && Input.GetKeyUp ("joystick 1 button 5"))
				r1ButtonUpDelegate ();

			if (l2ButtonUpDelegate != null && Input.GetKeyUp ("joystick 1 button 6"))
				l2ButtonUpDelegate ();

			if (r2ButtonUpDelegate != null && Input.GetKeyUp ("joystick 1 button 7"))
				r2ButtonUpDelegate ();

			if (backButtonUpDelegate != null && Input.GetKeyUp ("joystick 1 button 8"))
				backButtonUpDelegate ();

			if (startButtonUpDelegate != null && Input.GetKeyUp ("joystick 1 button 9"))
				startButtonUpDelegate ();
		}
			
		override public void ResetController () {}
		#endregion
	}
}