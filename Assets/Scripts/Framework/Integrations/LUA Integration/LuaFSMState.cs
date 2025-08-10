#if NCITE_LUA
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using Framework.AIModels;


namespace Framework.Lua
{
	/// <summary>
	/// LuaState
	/// 
	/// Provides a interface between FSM states and Lua scripts
	/// 
	/// by Jorge L. Chávez H.
	/// </summary>
	[System.Serializable]
	public class LuaFSMState : StateBase 
	{
		#region Class members
		private Script script;
		private DynValue onEnterFunc;
		private DynValue onUpdateFunc;
		private DynValue onFixedUpdateFunc;
		private DynValue onLateUpdateFunc;
		private DynValue onExitFunc;

		private DynValue onCollisionEnterFunc;
		private DynValue onCollisionStayFunc;
		private DynValue onCollisionExitFunc;

		private DynValue onTriggerEnterFunc;
		private DynValue onTriggerStayFunc;
		private DynValue onTriggerExitFunc;

		private DynValue onCollisionEnter2DFunc;
		private DynValue onCollisionStay2DFunc;
		private DynValue onCollisionExit2DFunc;

		private DynValue onTriggerEnter2DFunc;
		private DynValue onTriggerStay2DFunc;
		private DynValue onTriggerExit2DFunc;
		#endregion

		#region StateBase overrides
		public LuaFSMState (Script script)
		{
			this.script = script;

			// Cache lua functions
			onEnterFunc = script.Globals.Get ("OnEnter");
			onUpdateFunc = script.Globals.Get ("OnUpdate");
			onFixedUpdateFunc = script.Globals.Get ("OnFixedUpdate");
			onLateUpdateFunc = script.Globals.Get ("OnLateUpdate");
			onExitFunc = script.Globals.Get ("OnExit");

			onCollisionEnterFunc = script.Globals.Get ("OnCollisionEnter");
			onCollisionStayFunc = script.Globals.Get ("OnCollisionStay");
			onCollisionExitFunc = script.Globals.Get ("OnCollisionExit");

			onTriggerEnterFunc = script.Globals.Get ("OnTriggerEnter");
			onTriggerStayFunc = script.Globals.Get ("OnTriggerStay");
			onTriggerExitFunc = script.Globals.Get ("OnTriggerExit");

			onCollisionEnter2DFunc = script.Globals.Get ("OnCollisionEnter2D");
			onCollisionStay2DFunc = script.Globals.Get ("OnCollisionStay2D");
			onCollisionExit2DFunc = script.Globals.Get ("OnCollisionExit2D");

			onTriggerEnter2DFunc = script.Globals.Get ("OnTriggerEnter2D");
			onTriggerStay2DFunc = script.Globals.Get ("OnTriggerStay2D");
			onTriggerExit2DFunc = script.Globals.Get ("OnTriggerExit2D");

			// Execute Init function
			DynValue initFunc = script.Globals.Get ("Init");

			if (initFunc != DynValue.Nil)
				script.Call (initFunc);
		}

		override public void OnEnter() 
		{ 
			if (onEnterFunc != DynValue.Nil)
				script.Call (onEnterFunc);
		}

		override public void OnUpdate() 
		{
			if (onUpdateFunc != DynValue.Nil)
				script.Call (onUpdateFunc);
		}
			
		override public void OnFixedUpdate()
		{
			if (onFixedUpdateFunc != DynValue.Nil)
				script.Call (onFixedUpdateFunc);
		}
			
		override public void OnLateUpdate()
		{
			if (onLateUpdateFunc != DynValue.Nil)
				script.Call (onLateUpdateFunc);
		}
			
		override public void OnExit() 
		{
			if (onExitFunc != DynValue.Nil)
				script.Call (onExitFunc);
		}

		override public void OnCollisionEnter (Collision collision) 
		{
			if (onCollisionEnterFunc != DynValue.Nil)
				script.Call (onCollisionEnterFunc, collision);
		}

		override public void OnCollisionStay (Collision collision) 
		{
			if (onCollisionStayFunc != DynValue.Nil)
				script.Call (onCollisionStayFunc, collision);
		}

		override public void OnCollisionExit (Collision collision) 
		{
			if (onCollisionExitFunc != DynValue.Nil)
				script.Call (onCollisionExitFunc, collision);
		}

		override public void OnTriggerEnter (Collider collider) 
		{
			if (onTriggerEnterFunc != DynValue.Nil)
				script.Call (onTriggerEnterFunc, collider);
		}

		override public void OnTriggerStay (Collider collider) 
		{ 
			if (onTriggerStayFunc != DynValue.Nil)
				script.Call (onTriggerStayFunc, collider);
		}

		override public void OnTriggerExit (Collider collider) 
		{
			if (onTriggerExitFunc != DynValue.Nil)
				script.Call (onTriggerExitFunc, collider);
		}

		override public void OnCollisionEnter2D (Collision2D collision2D) 
		{
			if (onCollisionEnter2DFunc != DynValue.Nil)
				script.Call (onCollisionEnter2DFunc, collision2D);
		}

		override public void OnCollisionStay2D (Collision2D collision2D) 
		{
			if (onCollisionStay2DFunc != DynValue.Nil)
				script.Call (onCollisionStay2DFunc, collision2D);
		}

		override public void OnCollisionExit2D (Collision2D collision2D) 
		{
			if (onCollisionExit2DFunc != DynValue.Nil)
				script.Call (onCollisionExit2DFunc, collision2D);
		}

		override public void OnTriggerEnter2D (Collider2D collider2D) 
		{
			if (onTriggerEnter2DFunc != DynValue.Nil)
				script.Call (onTriggerEnter2DFunc, collider2D);
		}

		override public void OnTriggerStay2D (Collider2D collider2D) 
		{ 
			if (onTriggerStay2DFunc != DynValue.Nil)
				script.Call (onTriggerStay2DFunc, collider2D);
		}

		override public void OnTriggerExit2D (Collider2D collider2D) 
		{
			if (onTriggerExit2DFunc != DynValue.Nil)
				script.Call (onTriggerExit2DFunc, collider2D);
		}
		#endregion

		#region Class implementation
		/// <summary>
		/// Gets the binded Lua script.
		/// </summary>
		/// <returns>The script.</returns>
		public Script GetScript ()
		{
			return script;
		}

		/// <summary>
		/// Calls a function on the binded Lua script.
		/// </summary>
		/// <returns>The function.</returns>
		/// <param name="functionName">Function name.</param>
		/// <param name="args">Arguments.</param>
		public DynValue CallFunction (string functionName, params object[] args)
		{
			DynValue func = GetScript ().Globals.Get (functionName);
			return script.Call (func, args);
		}

		/// <summary>
		/// Gets a global variable from the binded Lua script.
		/// </summary>
		/// <returns>The script global.</returns>
		/// <param name="name">Name.</param>
		public DynValue GetScriptGlobal (string name)
		{
			return script.Globals.Get (name);
		}

		/// <summary>
		/// Sets the value for a global variable in the script.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		public void SetGlobalValue (string name, object value)
		{
			script.Globals [name] = value;
		}
		#endregion
	}
}
#endif
