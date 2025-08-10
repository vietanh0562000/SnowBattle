#if NCITE_LUA
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Framework.Lua
{
	/// <summary>
	/// Lua FSM state binder editor.
	/// Unity Editor for LuaFSMStateBinder class.
	/// 
	/// By Jorge  L. Chávez Herrera.
	/// </summary>
	[CustomEditor(typeof(LuaFSMStateBinder))]
	public class LuaFSMStateBinderEditor : Editor 
	{
		#region Editor overrides
		override public void OnInspectorGUI ()
		{
			NciteEditorTools.DrawNciteLogo ();
			NciteEditorTools.BlockHeader ("Lua FSM State Binder");

			LuaFSMStateBinder luaFSMStateBinder = target as LuaFSMStateBinder;
			LuaFSMStateBinder.LuaFSMStateScript luaFSMStateScriptToRemove = null; 

			if (GUILayout.Button ("Add State"))
				luaFSMStateBinder.luaFSMStateScripts.Add (new LuaFSMStateBinder.LuaFSMStateScript ());

			foreach (LuaFSMStateBinder.LuaFSMStateScript luaFSMStateScript in luaFSMStateBinder.luaFSMStateScripts) 
			{
				EditorGUILayout.BeginVertical ("button");

				EditorGUILayout.BeginHorizontal ();
				luaFSMStateScript.stateName = EditorGUILayout.TextField ("State Name", luaFSMStateScript.stateName);
			
				GUI.backgroundColor = Color.red;
				if (GUILayout.Button ("x", GUILayout.Width (20)))
					luaFSMStateScriptToRemove = luaFSMStateScript;
				GUI.backgroundColor = Color.white;

				EditorGUILayout.EndHorizontal ();

				LuaScriptBinder.GUIInspector (luaFSMStateScript.luaScriptBinder);

				EditorGUILayout.EndVertical ();
			}

			// Remove selected states
			if (luaFSMStateScriptToRemove != null) 
			{
				luaFSMStateBinder.luaFSMStateScripts.Remove (luaFSMStateScriptToRemove);
			}

			// House keeping
			luaFSMStateBinder.defaultState = EditorGUILayout.TextField ("Default State", luaFSMStateBinder.defaultState);

			if (GUI.changed)
				EditorUtility.SetDirty(luaFSMStateBinder);
		}
		#endregion
	}
}
#endif