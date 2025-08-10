using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Framework.Delegates;
using Framework.Utils;

namespace Framework.UserInput
{
	/// <summary>
	/// MobieTouchController.
	/// On Screen Analog Sticks & buttons input method.
	/// 
	/// Create by Jorge L. Chavez Herrera.
	/// </summary>
	public class MobileTouchController : GameController
	{
		#region Nested classes
		// Holds required information to represent an UI analog stick
		private class AnalogStick
		{
			public int touchID = -1;
			public float alpha = 0;
			public float screenLeftLimit;
			public float screenRightLimit;
			public Image baseWidget;
			public RectTransform baseRectTransform;
			public Image stickWidget;
			public RectTransform stickRectTransform;
			public float lastTouchTime;
			public bool isDoubleTap;
			public SimpleDelegate doubleTapStartedDelegate;
			public SimpleDelegate doubleTapEndedDelegate;
			
			private float maxDistance;

			public AnalogStick (Image baseWidget, Image stickWidget, float minX, float maxX)
			{
				this.baseWidget = baseWidget;
				baseRectTransform = baseWidget.rectTransform;
				this.stickWidget = stickWidget;
				stickRectTransform = stickWidget.rectTransform;
				
				screenLeftLimit = minX;
				screenRightLimit = maxX;
				
				maxDistance = (baseRectTransform.rect.width - stickRectTransform.rect.width) / 2;
			}

			/// <summary>
			/// Sets the active state for this stick.
			/// </summary>
			/// <param name="enabled">If set to <c>true</c> enabled.</param>
			public void SetEnabled (bool enabled)
			{
				baseRectTransform.gameObject.SetActive (enabled);
			}
			
			/// <summary>
			/// Sets the alpha.
			/// </summary>
			/// <param name="alpha">Alpha.</param>
			public void SetAlpha (float alpha)
			{
				this.alpha = alpha;
				baseWidget.color = stickWidget.color = new Color (1,1,1, alpha);
			}
			
			/// <summary>
			/// Update the this analog stick based on Touch info.
			/// </summary>
			/// <param name="touch">Touch.</param>
			public void Update (Touch touch)
			{
				switch (touch.phase)
				{
					case TouchPhase.Began:
					if (touchID == -1 && (touch.position.x >= screenLeftLimit && touch.position.x <= screenRightLimit) /* &&EventSystem.IsPointerOverUIObject() == false*/)
						{
							touchID = touch.fingerId;
							baseRectTransform.position = Camera.main.ScreenToWorldPoint (touch.position);
							stickRectTransform.localPosition = Vector2.zero;

							// Double tap
							if (Time.time - lastTouchTime < 0.25f)
							{
								isDoubleTap = true;

								if (doubleTapStartedDelegate != null)
									doubleTapStartedDelegate ();
							}

							lastTouchTime = Time.time;
							SetAlpha (1);

						}
					break;
					
					case TouchPhase.Stationary:
					case TouchPhase.Moved:
						if (touchID == touch.fingerId)
						{
							stickRectTransform.position = Camera.main.ScreenToWorldPoint (touch.position);
							stickRectTransform.localPosition = VectorTools.ClampDistance (Vector2.zero, stickRectTransform.localPosition, maxDistance);
						}
					break;
						
					case TouchPhase.Ended:
						if (touchID == touch.fingerId)
						{
							if (isDoubleTap)
							{
								if (doubleTapEndedDelegate != null)
									doubleTapEndedDelegate ();

								isDoubleTap = false;
							}

							touchID = -1;
							stickRectTransform.localPosition = Vector2.zero;
						}
					break;
				}
			}
			
			/// <summary>
			/// Gets a normalized speed value based on the distance between 2 the stick and its base
			/// </summary>
			/// <returns>The speed.</returns>
			public float GetSpeed ()
			{
				float dst = Vector2.Distance (Vector2.zero, stickRectTransform.localPosition);
				
				return dst / maxDistance;
			}
			
			/// <summary>
			/// Gets the joystick angle.
			/// </summary>
			/// <returns>The angle.</returns>
			public float GetAngle ()
			{
				return VectorTools.GetAngle (Vector2.zero, stickRectTransform.localPosition);
			}

			/// <summary>
			/// Gets the raw horizontal axis.
			/// </summary>
			/// <returns>The raw horizontal axis.</returns>
			/// <param name="stick">Stick.</param>
			public float GetRawHorizontalAxis ()
			{
				return stickRectTransform.localPosition.x / ((baseRectTransform.rect.width * 0.5f) - (stickRectTransform.rect.width * 0.5f));
			}
				
			/// <summary>
			/// Gets the raw vertical axis.
			/// </summary>
			/// <returns>The raw vertical axis.</returns>
			/// <param name="stick">Stick.</param>
			public float GetRawVerticalAxis ()
			{
				return stickRectTransform.localPosition.y / ((baseRectTransform.rect.height * 0.5f) - (stickRectTransform.rect.height * 0.5f));
			}

			/// <summary>
			/// Forces the touch end.
			/// </summary>
			public void ForceTouchEnd ()
			{
				touchID = -1;
				stickRectTransform.localPosition = Vector2.zero;
			}
		}
		#endregion
		
		#region Class members
		public bool leftStickEnabled = true;
        public Image leftStickBaseImage;
        public Image leftStickImage;
		public bool rightStickEnabled = true;
        public Image rightStickBaseImage;
        public Image rightStickImage;

		public Button buttonA;	
        public Image buttonAImage;
		public Button buttonB;
        public Image buttonBImage;
		public Button buttonX;
        public Image buttonXImage;
		public Button buttonY;
        public Image buttonYImage;

		private List<AnalogStick> analogSticks = new List<AnalogStick>();
		private bool doubleTapped;
		private Touch doubleTappedTouch;
		private int doubleTapIndex;
		#endregion
		
		#region MonoBehavior overrides
		private void Awake ()
		{
			// Create 2 analog sticks side by side horizontally on screen
			if (leftStickEnabled == true)
				analogSticks.Add (new AnalogStick (leftStickBaseImage, leftStickImage, 0, Screen.width / 2));
			
			if (rightStickEnabled == true)
				analogSticks.Add (new AnalogStick (rightStickBaseImage, rightStickImage, Screen.width / 2, Screen.width));
		}

		private void Start ()
		{
			gameObject.SetActive (IsRunningInRightPlatform ());
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
			
			return analogSticks[stick].touchID != -1;
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
			if (stick > 1)
				return 0;

			return analogSticks[stick].GetRawHorizontalAxis ();
		}

		/// <summary>
		/// Gets the raw vertical axis.
		/// </summary>
		/// <returns>The raw vertical axis.</returns>
		/// <param name="stick">Stick.</param>
		override public float GetRawVerticalAxis   (int stick)
		{
			if (stick > 1)
				return 0;

			return analogSticks[stick].GetRawVerticalAxis ();
		}	

		/// <summary>
		/// Determines whether this instance is running in right platform.
		/// </summary>
		/// <returns><c>true</c> if this instance is running in right platform; otherwise, <c>false</c>.</returns>
		override public bool IsRunningInRightPlatform ()
		{
			return Application.isMobilePlatform == true && Application.isEditor == false;
		}

		/// <summary>
		/// Updates this instance
		/// </summary>
		override public void DoUpdate ()
		{
			for (int s = 0; s < analogSticks.Count; s++)
			{
				for (int t = 0; t < Input.touches.Length; t++)
				{
					if (analogSticks[s].touchID == -1 || analogSticks[s].touchID == Input.touches[t].fingerId)
						analogSticks[s].Update (Input.touches[t]);
				}

				if (Input.touches.Length == 0)
					analogSticks [s].ForceTouchEnd ();

				if (analogSticks[s].touchID == -1)
					analogSticks[s].SetAlpha (analogSticks[s].alpha - Time.unscaledDeltaTime);
			}
		}
		#endregion

		#region Class functions
		public void AButtonDownAction ()
		{
			if (aButtonDownDelegate != null)
				aButtonDownDelegate ();		
		}

		public void AButtonUpAction ()
		{
			if (aButtonUpDelegate != null)
				aButtonUpDelegate ();		
		}

		public void BButtonDownAction ()
		{
			if (bButtonDownDelegate != null)
				bButtonDownDelegate ();	
		}

		public void BButtonUpAction ()
		{
			if (bButtonUpDelegate != null)
				bButtonUpDelegate ();	
		}

		public void XButtonDownAction ()
		{
			if (xButtonDownDelegate != null)
				xButtonDownDelegate ();	
		}

		public void XButtonUpAction ()
		{
			if (xButtonUpDelegate != null)
				xButtonUpDelegate ();	
		}

		public void YButtonDownAction ()
		{
			if (yButtonDownDelegate != null)
				yButtonDownDelegate ();	
		}

		public void YButtonUpAction ()
		{
			if (yButtonUpDelegate != null)
				yButtonUpDelegate ();	
		}

		public void L1ButtonDownAction ()
		{
			if (l1ButtonDownDelegate != null)
				l1ButtonDownDelegate ();	
		}

		public void L1ButtonUpAction ()
		{
			if (l1ButtonUpDelegate != null)
				l1ButtonUpDelegate ();	
		}

		public void L2ButtonDownAction ()
		{
			if (l2ButtonDownDelegate != null)
				l2ButtonDownDelegate ();	
		}

		public void L2ButtonUpAction ()
		{
			if (l2ButtonUpDelegate != null)
				l2ButtonUpDelegate ();	
		}

		public void R1ButtonDownAction ()
		{
			if (r1ButtonDownDelegate != null)
				r1ButtonDownDelegate ();	
		}

		public void R1ButtonUpAction ()
		{
			if (r1ButtonUpDelegate != null)
				r1ButtonUpDelegate ();	
		}

		public void R2ButtonDownAction ()
		{
			if (r2ButtonDownDelegate != null)
				r2ButtonDownDelegate ();	
		}

		public void R2ButtonUpAction ()
		{
			if (r2ButtonUpDelegate != null)
				r2ButtonUpDelegate ();	
		}

		public void StartButtonDownAction ()
		{
			if (startButtonDownDelegate != null)
				startButtonDownDelegate ();	
		}

		public void StartButtonUpAction ()
		{
			if (startButtonUpDelegate != null)
				startButtonUpDelegate ();	
		}

		public void BackButtonDownAction ()
		{
			if (backButtonDownDelegate != null)
				backButtonDownDelegate ();	
		}

		public void BackButtonUpAction ()
		{
			if (backButtonUpDelegate != null)
				backButtonUpDelegate ();	
		}

		override public void ResetController () {}
		#endregion
	}
}