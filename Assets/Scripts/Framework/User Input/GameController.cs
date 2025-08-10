using UnityEngine;
using System.Collections;
using Framework.Delegates;

namespace Framework.UserInput
{
	public abstract class GameController : MonoBehaviour
	{
		#region Class members
		[System.NonSerialized]
		public Transform controlledTransform;

		public SimpleDelegate aButtonDownDelegate;
		public SimpleDelegate bButtonDownDelegate;
		public SimpleDelegate xButtonDownDelegate;
		public SimpleDelegate yButtonDownDelegate;
		public SimpleDelegate l1ButtonDownDelegate;
		public SimpleDelegate l2ButtonDownDelegate;
		public SimpleDelegate r1ButtonDownDelegate;
		public SimpleDelegate r2ButtonDownDelegate;
		public SimpleDelegate startButtonDownDelegate;
		public SimpleDelegate backButtonDownDelegate;

		public SimpleDelegate aButtonUpDelegate;
		public SimpleDelegate bButtonUpDelegate;
		public SimpleDelegate xButtonUpDelegate;
		public SimpleDelegate yButtonUpDelegate;
		public SimpleDelegate l1ButtonUpDelegate;
		public SimpleDelegate l2ButtonUpDelegate;
		public SimpleDelegate r1ButtonUpDelegate;
		public SimpleDelegate r2ButtonUpDelegate;
		public SimpleDelegate startButtonUpDelegate;
		public SimpleDelegate backButtonUpDelegate;
		#endregion

		#region Class  accessors
		private bool _inputEnabled = true;
		public bool inputEnabled
		{
			get { return _inputEnabled;}
			set {_inputEnabled = value;}
		}
		#endregion

		#region Class functions
		public abstract void SetControlledTransform (Transform t);
		public abstract bool IsStickActive (int stick);
		public abstract float GetSpeed (int stick);
		public abstract float GetAngle (int stick);
		public abstract float GetRawHorizontalAxis (int stick);
		public abstract float GetRawVerticalAxis   (int stick);
		public abstract bool IsRunningInRightPlatform ();
		public abstract void DoUpdate ();
		public abstract void ResetController ();
		#endregion
	}
}
