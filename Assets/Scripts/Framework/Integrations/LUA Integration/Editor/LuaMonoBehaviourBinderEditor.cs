#if NCITE_LUA
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Framework.Lua
{
	/// <summary>
	/// Lua MonoBehaviour binder editor.
	/// Unity Editor for LuaMonoBehaviourBinder class.
	/// 
	/// By Jorge  L. Chávez Herrera.
	/// </summary>
	[CustomEditor(typeof(LuaMonoBehaviourBinder))]
	public class LuaMonoBehaviourBinderEditor : Editor 
	{
		#region Editor overrides
		override public void OnInspectorGUI ()
		{
			NciteEditorTools.DrawNciteLogo ();
			NciteEditorTools.BlockHeader ("Lua MonoBehaviour Binder");

			LuaMonoBehaviourBinder luaMonoBehaviourBinder = target as LuaMonoBehaviourBinder;
			LuaScriptBinder.GUIInspector (luaMonoBehaviourBinder.luaScriptBinder);

			if (GUI.changed)
				EditorUtility.SetDirty(luaMonoBehaviourBinder);
		}
		#endregion
	}
}
#endif