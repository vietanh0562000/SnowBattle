#if NCITE_LUA
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Serialization.Json;


namespace Framework.Lua
{
	/// <summary>
	/// Lua mono behaviour binder.
	/// Binds lua scripts to all Unit's momobehaviour functions
	/// </summary>
	public class LuaMonoBehaviourBinder : OptimizedLuaGameObject 
	{
		#region Class members
		public LuaScriptBinder luaScriptBinder;

		// We eil allow caching to speed critical function
		private DynValue fixedUpdateFunc;
		private DynValue onTriggerStayFunc;
		private DynValue onCollisionStayFunc;
		private DynValue onTriggerStay2DFunc;
		private DynValue onCollisionStay2DFunc;

		// To do: implement yield WaitForFixedUpdate

		//private DynValue onMouseDownFunc;

		private DynValue updateFunc;
		// To do: implement yield null
		// To do: implement yield WaitForSeconds
		// To do: implement yield WWW
		// To do: implement yield StartCoroutine

		private DynValue lateUpdateFunc;

		private DynValue onDrawGizmosFunc;

		// To do: implement yield WaitForEndOfFrame

		private DynValue onApplicationPauseFunc;

		private DynValue onDisableFunc;

		private DynValue onApplicationQuitFunc;
		private DynValue onDestroyFunc;
		#endregion

		#region MonoBehaviour overrides
		private void Awake ()
		{
			luaScriptBinder.Init ();

			// Add the this variable for easy access to the most commonly used components from the Lua script
			luaScriptBinder.GetScript ().Globals.Set ("owner", UserData.Create (this));
		
			// Cache the MonoBehaviour functions critical for speed
			fixedUpdateFunc = luaScriptBinder.GetScript ().Globals.Get ("FixedUpdate");
		
			onTriggerStayFunc = luaScriptBinder.GetScript ().Globals.Get ("OnTriggerStay");
			onCollisionStayFunc = luaScriptBinder.GetScript ().Globals.Get ("OnCollistionStay");

			onTriggerStay2DFunc = luaScriptBinder.GetScript ().Globals.Get ("OnTriggerStay2D");
			onCollisionStay2DFunc = luaScriptBinder.GetScript ().Globals.Get ("OnCollistionStay2D");

			// To do: implement yield WaitForFixedUpdate

			// onMouseDownFunc = luaScriptBinder.GetScript ().Globals.Get ("OnMouse");*/

			updateFunc = luaScriptBinder.GetScript ().Globals.Get ("Update");
			// To do: implement yield null
			// To do: implement yield WaitForSeconds
			// To do: implement yield WWW
			// To do: implement yield StartCoroutine

			lateUpdateFunc = luaScriptBinder.GetScript ().Globals.Get ("LateUpdate");

			// To do: implement yield WaitForEndOfFrame

			if (cachedPoolManaged != null) 
			{
				cachedPoolManaged.onSpawn += OnSpawn;
				cachedPoolManaged.onDespawn += OnDespawn;
			}

			// Execute Lua Awake
			CallMonoLuaFunction ("Awake");
		}

		public void CallMonoLuaFunction (string functionName, params object[] args)
		{
			DynValue func = luaScriptBinder.GetScript ().Globals.Get (functionName);

			if (func != DynValue.Nil)
				luaScriptBinder.GetScript ().Call (func, args);
		}
			
		private void OnEnable ()
		{
			CallMonoLuaFunction ("OnEnable");
		}
			
		private void Start() 
		{ 
			CallMonoLuaFunction ("Start");
		}

		private void FixedUpdate() 
		{
			if (fixedUpdateFunc != DynValue.Nil)
				luaScriptBinder.GetScript().Call (fixedUpdateFunc);
		}

		private void OnTriggerEnter (Collider collider) 
		{
			CallMonoLuaFunction ("OnTriggerEnter", collider);
		}

		private void OnTriggerStay (Collider collider) 
		{ 
			if (onTriggerStayFunc != DynValue.Nil)
				luaScriptBinder.GetScript().Call (onTriggerStayFunc, collider);
		}

		private void OnTriggerExit (Collider collider) 
		{
			CallMonoLuaFunction ("OnTriggerExit", collider);
		}

		private void  OnCollisionEnter (Collision collision)
		{
			CallMonoLuaFunction ("OnCollisionEnter", collision);
		}

		private void OnCollisionStay (Collision collision) 
		{
			if (onCollisionStayFunc != DynValue.Nil)
				luaScriptBinder.GetScript().Call (onCollisionStayFunc, collision);
		}

		private void OnCollisionExit (Collision collision) 
		{
			CallMonoLuaFunction ("OnCollisionExit", collision);
		}

		private void OnCollisionEnter2D (Collision2D collision2D)
		{
			CallMonoLuaFunction ("OnCollisionEnter2D", collision2D);
		}
			
		private void OnCollisionStay2D (Collision2D collision2D) 
		{
			if (onCollisionStay2DFunc != DynValue.Nil)
				luaScriptBinder.GetScript().Call ("OnCollisionStay2D", collision2D);
		}

		private void OnCollisionExit2D (Collision2D collision2D) 
		{
			CallMonoLuaFunction ("OnCollisionExit2D", collision2D);
		}

		private void  OnTriggerEnter2D(Collider2D collider2D)
		{
			CallMonoLuaFunction ("OnTriggerEnter2D", collider2D);
		}

		private void OnTriggerStay2D (Collider2D collider2D) 
		{
			if (onTriggerStay2DFunc != DynValue.Nil)
				luaScriptBinder.GetScript().Call ("OnTriggerStay2D", collider2D);
		}

		private void OnTriggerExit2D (Collider2D collider2D) 
		{
			CallMonoLuaFunction ("OnTriggerExit2D", collider2D);
		}

		/*
		private void OnMouseDown () 
		{
			CallMonoLuaFunction ("OnMouseDown");
		}*/
			
		private void Update ()
		{
			if (updateFunc != DynValue.Nil)
				luaScriptBinder.GetScript().Call (updateFunc);
		}

		private void LateUpdate ()
		{
			if (lateUpdateFunc != DynValue.Nil)
				luaScriptBinder.GetScript().Call (lateUpdateFunc);
		}

		private void OnBecameVisible ()
		{
			CallMonoLuaFunction ("OnBecameVisible");
		}

		private void OnBecameInvisible ()
		{
			CallMonoLuaFunction ("OnBecameInvisible");
		}


		private void OnDrawGizmos ()
		{
			// No need to implement, only useful on editor
		}

		private void OnGUI ()
		{
			CallMonoLuaFunction ("OnGUI");
		}

		// To do: implement yield WaitForEndOfFrame

		private void OnApplicationPause ()
		{
			CallMonoLuaFunction ("OnApplicationPause");
		}

		private void OnDisable ()
		{
			CallMonoLuaFunction ("OnDisable");
		}

		private void OnApplicationQuit ()
		{
			CallMonoLuaFunction ("OnApplicationQuit");
		}

		private void OnDestroy ()
		{
			CallMonoLuaFunction ("OnDestroy");
		}
		#endregion

		#region PoolManaged delegates
		private void OnSpawn ()
		{
			CallMonoLuaFunction ("OnSpawn");
		}

		private void OnDespawn ()
		{
			CallMonoLuaFunction ("OnDespawn");
		}
		#endregion

		#region Class implementation
		/// <summary>
		/// Calls a function with string arguments on the binded Lua script (Intented to be used from Unity Actions,
		/// separate arguments in the same string by commas.
		/// </summary>
		/// <param name="functionName">Function name.</param>
		public void CallFunctionWthStringArgs (string inString)
		{
			List<string> strings = new List<string>(inString.Split (','));

			if (strings.Count < 2) 
			{
				DynValue func = luaScriptBinder.GetScript ().Globals.Get (inString);
				luaScriptBinder.GetScript ().Call (inString);
			} 
			else 
			{
				string functionName = strings [0];
				DynValue func = luaScriptBinder.GetScript ().Globals.Get (functionName);
				strings.RemoveAt (0);

				object[] args = new object[strings.Count];

				for (int i = 0; i < strings.Count; i++)
					args [i] = strings[i]; 
				
				luaScriptBinder.GetScript ().Call (func, args);
			}
		}

		/// <summary>
		/// Calls a function on the binded Lua script.
		/// </summary>
		/// <returns>The function.</returns>
		/// <param name="functionName">Function name.</param>
		/// <param name="args">Arguments.</param>
		public DynValue CallFunction (string functionName, params object[] args)
		{
			DynValue func = luaScriptBinder.GetScript ().Globals.Get (functionName);
			return luaScriptBinder.GetScript ().Call (func, args);
		}

		/// <summary>
		/// Calls a function on the binded Lua script with a delay,
		/// delay value is the 1st argument.
		/// </summary>
		/// <param name="functionName">Function name.</param>
		/// <param name="args">Arguments.</param>
		public void CallFunctionWithDelay (string functionName, params object[] args)
		{
			StartCoroutine (CallFunctionWithDelayCoroutine (functionName, args));
		}

		IEnumerator CallFunctionWithDelayCoroutine (string functionName, params object[] args)
		{
			// Use the first array element as the delay parameter
			double delay = (double)args[0];

			// Remove first element of the args array
			List<object>argumentList = new List<object>(args);
			argumentList.RemoveAt (0);

			yield return new WaitForSeconds ((float)delay);

			DynValue func = luaScriptBinder.GetScript().Globals.Get (functionName);
			luaScriptBinder.GetScript().Call (func, argumentList.ToArray ());
		}

		/// <summary>
		/// Gets a global DynValue variable from the binded Lua script.
		/// </summary>
		/// <returns>The script global.</returns>
		/// <param name="name">Name.</param>
		public DynValue GetScriptGlobal (string name)
		{
			return luaScriptBinder.GetScript ().Globals.Get (name);
		}
		#endregion

	}
}
#endif
