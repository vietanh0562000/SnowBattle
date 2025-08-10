#if NCITE_LUA
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoonSharp.Interpreter;
using Framework.AIModels;


namespace Framework.Lua
{
	/// <summary>
	/// Lua FSM state binder.
	/// 
	/// Binds Lua scripts to FSM states.
	/// 
	/// By Jorge L. Chávez Herrera.
	/// </summary>
	[RequireComponent (typeof (FSM))]
	public class LuaFSMStateBinder : OptimizedLuaGameObject 
	{
		#region Class members
		[System.Serializable]
		public class LuaFSMStateScript
		{
			public string stateName;
			public LuaScriptBinder luaScriptBinder;
		}

		public List<LuaFSMStateScript> luaFSMStateScripts = new List<LuaFSMStateScript>();
		public string defaultState;
		#endregion

		#region MonoBehaviour overrides
		private void Awake ()
		{
			for (int i = 0; i < luaFSMStateScripts.Count; i++) {
				luaFSMStateScripts [i].luaScriptBinder.Init ();

				// Add the owner variable for easy access to the most commonly used components from the Lua script
				luaFSMStateScripts [i].luaScriptBinder.GetScript ().Globals.Set ("owner", UserData.Create (this));

				// Create state
				cachedFSM.AddState (luaFSMStateScripts [i].stateName, new LuaFSMState (luaFSMStateScripts [i].luaScriptBinder.GetScript ()));

				// Share variables on MonoBehaviour binded Lua script to this states
				LuaMonoBehaviourBinder luaMonoBehaviourBinder = GetComponent<LuaMonoBehaviourBinder> ();

				if (luaMonoBehaviourBinder != null) 
				{
					foreach (Variable variable in luaMonoBehaviourBinder.luaScriptBinder.variables)
						luaFSMStateScripts [i].luaScriptBinder.GetScript ().Globals.Set (variable.name, variable.GetDynValue ()); 
				}
			}
		}

		private void Start ()
		{
			// Switch to default state if any
			if (string.IsNullOrEmpty (defaultState) == false)
				cachedFSM.SetState (defaultState);
		}
		#endregion

		#region Class implementation
		/// <summary>
		/// Gets the state.
		/// </summary>
		/// <returns>The state.</returns>
		/// <param name="name">Name.</param>
		public LuaFSMState GetState (string name)
		{
			return cachedFSM.states [name] as LuaFSMState;
		}

		/*
		/// <summary>
		/// Plays an audio clip.
		/// </summary>
		/// <param name="audioClip">Audio clip.</param>
		public void PlayAudioClip (AudioClip audioClip)
		{
			cachedAudioSource.clip = audioClip;
			cachedAudioSource.Play ();
		}*/

		/// <summary>
		/// Calls a function on the binded Lua script with a delay,
		/// delay value is the 1st argument.
		/// </summary>
		/// <param name="functionName">Function name.</param>
		/// <param name="args">Arguments.</param>
		public void CallFunctionWithDelay (string functionName, params object[] args)
		{
			StartCoroutine (CallFunctionWithDelayCoroutine (cachedFSM.currentState.name, functionName, args));
		}

		IEnumerator CallFunctionWithDelayCoroutine (string stateName, string functionName, params object[] args)
		{
			Script script = (cachedFSM.states [stateName] as LuaFSMState).GetScript ();

			// Use the first array element as the delay parameter
			double delay = (double)args[0];

			// Remove first element of the args array
			List<object>argumentList = new List<object>(args);
			argumentList.RemoveAt (0);

			yield return new WaitForSeconds ((float)delay);

			DynValue func = script.Globals.Get (functionName);
			script.Call (func, argumentList.ToArray ());
		}
		#endregion
	}
}
#endif
