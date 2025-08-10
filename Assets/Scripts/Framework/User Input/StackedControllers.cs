using UnityEngine;
using System.Collections;
using Framework.Delegates;

namespace Framework.UserInput
{
	public class StackedControllers : GameController
	{
		#region CLass members
		public GameController[] controllers;
		#endregion

		#region MonoBehaviour poverrides
		private void Update ()
		{
			if (inputEnabled == true) 
			{
				foreach (GameController gameController in controllers) 
				{
					if (gameController.IsRunningInRightPlatform () == true) 
					{
						gameController.DoUpdate ();
					}
				}
			}
		}
		#endregion

		#region GameController overrides
		/// <summary>
		/// Sets the controlled transform.
		/// </summary>
		/// <param name="t">T.</param>
		override public void SetControlledTransform (Transform  t)
		{
			for (int i = 0; i < controllers.Length; i++)
				controllers[i].SetControlledTransform (t);	
		}

		/// <summary>
		/// Determines whether this instance is stick active the specified stick.
		/// </summary>
		/// <returns><c>true</c> if this instance is stick active the specified stick; otherwise, <c>false</c>.</returns>
		/// <param name="stick">Stick.</param>
		override public bool IsStickActive (int stick)
		{
			// Must not be implemented 	
			return false;
		}
		
		/// <summary>
		/// Gets the speed.
		/// </summary>
		/// <returns>The speed.</returns>
		/// <param name="stick">Stick.</param>
		override public float GetSpeed (int stick)
		{
			float ret = 0;

			for (int i = 0; i < controllers.Length; i++)
				if (controllers [i].IsRunningInRightPlatform () == true && controllers [i].IsStickActive (stick))
					ret = controllers [i].GetSpeed (stick);
			
			return ret;
		}
		
		/// <summary>
		/// Gets the angle.
		/// </summary>
		/// <returns>The angle.</returns>
		/// <param name="stick">Stick.</param>
		override public float GetAngle (int stick)
		{
			float ret = 0;
			
			for (int i = 0; i < controllers.Length; i++)
				if (controllers [i].IsRunningInRightPlatform () == true && controllers [i].IsStickActive (stick))
					ret = controllers [i].GetAngle (stick);
			
			return ret;
		}

		/// <summary>
		/// Gets the raw horizontal axis.
		/// </summary>
		/// <returns>The raw horizontal axis.</returns>
		/// <param name="stick">Stick.</param>
		override public float GetRawHorizontalAxis (int stick)
		{
			float ret = 0;

			for (int i = 0; i < controllers.Length; i++)
				if (controllers [i].IsRunningInRightPlatform () == true && controllers [i].IsStickActive (stick))
					ret = controllers [i].GetRawHorizontalAxis (stick);

			return ret;
		}

		/// <summary>
		/// Gets the raw vertical axis.
		/// </summary>
		/// <returns>The raw vertical axis.</returns>
		/// <param name="stick">Stick.</param>
		override public float GetRawVerticalAxis (int stick)
		{
			float ret = 0;

			for (int i = 0; i < controllers.Length; i++)
				if (controllers [i].IsRunningInRightPlatform () == true && controllers [i].IsStickActive (stick))
					ret = controllers [i].GetRawVerticalAxis (stick);

			return ret;
		}

		/// <summary>
		/// Determines whether this instance is running in right platform.
		/// </summary>
		/// <returns><c>true</c> if this instance is running in right platform; otherwise, <c>false</c>.</returns>
		override public bool IsRunningInRightPlatform ()
		{
			// Must not be implemented 	
			return false;
		}

		/// <summary>
		/// Updates this instance
		/// </summary>
		override public void DoUpdate () 
		{
			// Must not be implemented 	
		}
		#endregion

		#region Class functions
		public void SetAButtonDownDelegate (SimpleDelegate del) 
		{
			if (inputEnabled == true) 
			{
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].aButtonDownDelegate = del;
			}
		}

		public void SetBButtonDownDelegate (SimpleDelegate del) 
		{
			if (inputEnabled == true) 
			{
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].bButtonDownDelegate = del;
			}
		}

		public void SetXButtonDownDelegate (SimpleDelegate del) 
		{
			if (inputEnabled == true) 
			{
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].xButtonDownDelegate = del;
			}
		}

		public void SetYButtonDownDelegate (SimpleDelegate del) 
		{
			if (inputEnabled == true) 
			{
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].yButtonDownDelegate = del;
			}
		}

		public void SetL1ButtonDownDelegate (SimpleDelegate del) 
		{
			if (inputEnabled == true) 
			{
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].l1ButtonDownDelegate = del;
			}
		}

		public void SetL2ButtonDownDelegate (SimpleDelegate del) 
		{
			if (inputEnabled == true) 
			{
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].l2ButtonDownDelegate = del;
			}
		}

		public void SerR1ButtonDownDelegate (SimpleDelegate del) 
		{
			if (inputEnabled == true) 
			{
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].r1ButtonDownDelegate = del;
			}
		}

		public void SetR2ButtonDownDelegate (SimpleDelegate del) 
		{
		
			if (inputEnabled == true) {
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].r2ButtonDownDelegate = del;
			}
		}

		public void SetStartButtonDownDelegate (SimpleDelegate del) 
		{
			if (inputEnabled == true) 
			{
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].startButtonDownDelegate = del;
			}
		}

		public void SetBackButtonDownDelegate (SimpleDelegate del) 
		{
			if (inputEnabled == true) 
			{
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].backButtonDownDelegate = del;
			}
		}
		 // Button Up
		public void SetAButtonUpDelegate (SimpleDelegate del) 
		{
			if (inputEnabled == true) 
			{
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].aButtonUpDelegate = del;
			}
		}
		
		public void SetBButtonUpDelegate (SimpleDelegate del) 
		{
			if (inputEnabled == true) 
			{
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].bButtonUpDelegate = del;
			}
		}
		
		public void SetXButtonUpDelegate (SimpleDelegate del) 
		{
			if (inputEnabled == true) 
			{
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].xButtonUpDelegate = del;
			}
		}
		
		public void SetYButtonUpDelegate (SimpleDelegate del) 
		{
			if (inputEnabled == true) 
			{
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].yButtonUpDelegate = del;
			}
		}
		
		public void SetL1ButtonUpDelegate (SimpleDelegate del) 
		{
			if (inputEnabled == true) 
			{
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].l1ButtonUpDelegate = del;
			}
		}
		
		public void SetL2ButtonUpDelegate (SimpleDelegate del) 
		{
			if (inputEnabled == true) 
			{
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].l2ButtonUpDelegate = del;
			}
		}
		
		public void SerR1ButtonUpDelegate (SimpleDelegate del) 
		{
			if (inputEnabled == true) 
			{
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].r1ButtonUpDelegate = del;
			}
		}
		
		public void SetR2ButtonUpDelegate (SimpleDelegate del) 
		{
			if (inputEnabled == true) 
			{
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].r2ButtonUpDelegate = del;
			}
		}
		
		public void SetStartButtonUpDelegate (SimpleDelegate del) 
		{
			if (inputEnabled == true) 
			{
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].startButtonUpDelegate = del;
			}
		}
		
		public void SetBackButtonUpDelegate (SimpleDelegate del) 
		{
			if (inputEnabled == true) 
			{
				for (int i = 0; i < controllers.Length; i++)
					controllers [i].backButtonUpDelegate = del;
			}
		}

		override public void ResetController () 
		{
			for (int i = 0; i < controllers.Length; i++)
				controllers [i].ResetController ();
		}
		#endregion
	}
}

