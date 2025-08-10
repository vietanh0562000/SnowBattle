using UnityEngine;

#if NCITE_LUA
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
#endif

#if NCITE_SPINE
using Framework.Spine;
#endif
using Framework.AIModels;
using Framework.Utils;
using Framework.Pooling;
using Framework.Gameplay;


#if NCITE_LUA
namespace Framework.Lua
{
	/// <summary>
	/// LUA script manager.
	/// Handles global housekeeping for LUA scripts.
	/// 
	/// by Jorge L. Chávez H.
	/// </summary>
	public class LUAScriptManager : MonoSingleton<LUAScriptManager> 
	{ 
		static private System.Type[] unityTypes = new System.Type[]
        {
			// Data types
			typeof (Vector2),
            typeof (Vector3),
           // typeof (Vector4),
            typeof (Quaternion),
            typeof (Rect),
            typeof (Color),
			// GameObject
			//typeof (Object),
			typeof (GameObject),
			// Transforms
			typeof (Transform),
			typeof (RectTransform),
			// Renderers
			//typeof (Sprite),
			//typeof (SpriteRenderer),
			//typeof (LineRenderer),
			typeof (Camera),
			// Physics
			typeof (Rigidbody),
			typeof (Rigidbody2D),
			typeof (ForceMode),
			typeof (ForceMode2D),
			// Collision & colliders
            typeof (Collider),
            typeof (Collision),
			//typeof (BoxCollider),
			//typeof (SphereCollider),
			//typeof (CapsuleCollider),
			//typeof (MeshCollider),
			//typeof (WheelCollider),
			// Collision2D & colliders 2D
			typeof (Collision2D),
			typeof (Collider2D),
			//typeof (BoxCollider2D),
			//typeof (CircleCollider2D),
			//typeof (EdgeCollider2D),
			//typeof (PolygonCollider2D),
			//typeof (CapsuleCollider2D),
			// Animation
			typeof (Animation),
			// Sound
			typeof (AudioClip),
			//typeof (AudioSource),
			//typeof (AudioListener),
			// Classes
			typeof (Time),
			typeof (Mathf),
			typeof (Random),
			typeof (Input),
			//typeof (Gyroscope),
			//typeof (KeyCode),
			//typeof (Screen),
			typeof (Debug),
			//typeof (EventSystem),
			//typeof (PointerEventData),
		};

		static private System.Type[] unityExtensionTypes = new System.Type[]
		{
			// Data types
			typeof (EventSystemExtensions),
			typeof (TransformExtensions),
		};

		static private System.Type[] engineTypes = new System.Type[]
		{
			// Data types
			typeof (UniqueRandomSequence),
			typeof (HSVColor),
			// Interpolators
			typeof (FloatSDInterpolator),
			typeof (AngleSDInterpolator),
			typeof (Vector2SDInterpolator),
			typeof (Vector3SDInterpolator),

			typeof (PoolManaged),
			//typeof (PoolManaged[]),
			typeof (PoolManager),
			typeof (PrefabPool),
			typeof (OptimizedGameObject),
			typeof (OptimizedLuaGameObject),
			typeof (LuaMonoBehaviourBinder),
			typeof (FSM),
			typeof (LuaFSMStateBinder),
			typeof (StateBase),
			typeof (LuaFSMState),
			typeof (MiniGameManager),
			//typeof (PanelBase),
			//typeof (PopupPanel),
			//typeof (SpawnerBase),
			//typeof (PointSpawner),
			//typeof (RadiusSpawner),
			typeof (BaseCamera),
			typeof (FeedbackManager),

#if NCITE_SPINE
			typeof (SpineAnimationPlayer),
			//typeof (Avatar2D),
			//typeof (Skeleton),
#endif
		};
			
		#region MonoSingleton overrides
		override public void Init () 
		{
			// Register custom classes
			UserData.RegisterAssembly ();
			HardwiredClasses.HardwireTypes.Initialize ();
        }
		#endregion

		#region Class implementattion
		/// <summary>
		/// Registers the all types on the privided script.
		/// </summary>
		/// <param name="script">Script.</param>
		static public void RegisterTypesOnScript (Script script)
		{
			// Register Unity types
			foreach (System.Type type in unityTypes)
				RegisterType (script, type);

			// Register Unity extension types
			foreach (System.Type type in unityExtensionTypes)
				RegisterExtensionType (script, type);

			// Register Custom engine types
			foreach (System.Type type in engineTypes)
				RegisterType (script, type);
		}
			
		static private void RegisterType (Script script, System.Type type)
		{
            if (UserData.IsTypeRegistered(type) == false)
            {
            	UserData.RegisterType (type); 
            }

			//FixUnityTypes ();
			//FixCustomTypes ();

			script.Globals [type.Name] = UserData.CreateStatic (type);
        }

		static private void RegisterExtensionType (Script script, System.Type type)
		{
			if (UserData.IsTypeRegistered (type) == false)
				UserData.RegisterExtensionType (type);
			else
				Debug.Log ("Etension Already registered");

			script.Globals [type.Name] = UserData.CreateStatic (type);
		}
		#endregion

		static private void FixUnityTypes ()
		{
			var descr = ((StandardUserDataDescriptor)(UserData.RegisterType<GameObject>()));
			descr.RemoveMember("StopAnimation");
			descr.RemoveMember("PlayAnimation");
			descr.RemoveMember("SampleAnimation");
			descr.RemoveMember("AddComponent");

			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<Vector2>()));
			descr.RemoveMember("op_Implicit");
			descr.RemoveMember("__toVector2");
			descr.RemoveMember("__toVector3");

			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<Vector3>()));
			descr.RemoveMember("op_Implicit");

			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<Color>()));
			descr.RemoveMember("op_Implicit");
			descr.RemoveMember("__toColor");

			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<Color>()));
			descr.RemoveMember("__toVector4");

			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<MonoBehaviour>()));
			descr.RemoveMember("runInEditMode");
			descr.RemoveMember("get_runInEditMode");
			descr.RemoveMember("set_runInEditMode");

			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<Input>()));
			descr.RemoveMember("IsJoystickPreconfigured"); 

			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<UnityEngine.UI.MaskableGraphic>()));
			descr.RemoveMember("ParentMaskStateChanged"); 
			descr.RemoveMember("OnRebuildRequested");
			descr.RemoveMember("runInEditMode");
			descr.RemoveMember("get_runInEditMode");
			descr.RemoveMember("set_runInEditMode");

			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<UnityEngine.UI.Image>()));
			descr.RemoveMember("ParentMaskStateChanged");
			descr.RemoveMember("OnRebuildRequested");
			descr.RemoveMember("runInEditMode");
			descr.RemoveMember("get_runInEditMode");
			descr.RemoveMember("set_runInEditMode");

			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<UnityEngine.UI.RawImage>()));
			descr.RemoveMember("ParentMaskStateChanged");
			descr.RemoveMember("OnRebuildRequested");
			descr.RemoveMember("runInEditMode");
			descr.RemoveMember("get_runInEditMode");
			descr.RemoveMember("set_runInEditMode");

			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<UnityEngine.UI.Text>()));
			descr.RemoveMember("ParentMaskStateChanged"); 
			descr.RemoveMember("OnRebuildRequested");
			descr.RemoveMember("runInEditMode");
			descr.RemoveMember("get_runInEditMode");
			descr.RemoveMember("set_runInEditMode");

			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<UnityEngine.UI.Outline>()));
			descr.RemoveMember("runInEditMode");
			descr.RemoveMember("get_runInEditMode");
			descr.RemoveMember("set_runInEditMode");
		}

		static private void FixCustomTypes ()
		{
			var descr = ((StandardUserDataDescriptor)(UserData.RegisterType<FSM>()));
			descr.RemoveMember("runInEditMode");
			descr.RemoveMember("get_runInEditMode");
			descr.RemoveMember("set_runInEditMode");
			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<PoolManager>()));
			descr.RemoveMember("runInEditMode");
			descr.RemoveMember("get_runInEditMode");
			descr.RemoveMember("set_runInEditMode");
			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<PoolManaged>()));
			descr.RemoveMember("runInEditMode");
			descr.RemoveMember("get_runInEditMode");
			descr.RemoveMember("set_runInEditMode");
			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<OptimizedGameObject>()));
			descr.RemoveMember("runInEditMode");
			descr.RemoveMember("get_runInEditMode");
			descr.RemoveMember("set_runInEditMode");
			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<OptimizedLuaGameObject>()));
			descr.RemoveMember("runInEditMode");
			descr.RemoveMember("get_runInEditMode");
			descr.RemoveMember("set_runInEditMode");
			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<LuaMonoBehaviourBinder>()));
			descr.RemoveMember("runInEditMode");
			descr.RemoveMember("get_runInEditMode");
			descr.RemoveMember("set_runInEditMode");
			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<LuaFSMStateBinder>()));
			descr.RemoveMember("runInEditMode");
			descr.RemoveMember("get_runInEditMode");
			descr.RemoveMember("set_runInEditMode");
			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<FSM>()));
			descr.RemoveMember("runInEditMode");
			descr.RemoveMember("get_runInEditMode");
			descr.RemoveMember("set_runInEditMode");
			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<MiniGameManager>()));
			descr.RemoveMember("runInEditMode");
			descr.RemoveMember("get_runInEditMode");
			descr.RemoveMember("set_runInEditMode");
			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<BaseCamera>()));
			descr.RemoveMember("runInEditMode");
			descr.RemoveMember("get_runInEditMode");
			descr.RemoveMember("set_runInEditMode");
			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<FeedbackManager>()));
			descr.RemoveMember("runInEditMode");
			descr.RemoveMember("get_runInEditMode");
			descr.RemoveMember("set_runInEditMode");
			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<Framework.UI.PopupPanel>()));
			Debug.Log (descr);
			descr.RemoveMember("runInEditMode");
			descr.RemoveMember("get_runInEditMode");
			descr.RemoveMember("set_runInEditMode");
			descr = ((StandardUserDataDescriptor)(UserData.RegisterType<Framework.Spine.SpineAnimationPlayer>()));
			descr.RemoveMember("runInEditMode");
			descr.RemoveMember("get_runInEditMode");
			descr.RemoveMember("set_runInEditMode");
		}
	}
		
}
#endif
