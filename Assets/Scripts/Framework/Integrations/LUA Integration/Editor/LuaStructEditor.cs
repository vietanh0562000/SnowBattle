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
	[CustomEditor(typeof(LuaStruct))]
	public class LuaStructEditor : Editor 
	{
		#region Editor overrides
		override public void OnInspectorGUI ()
		{
			NciteEditorTools.DrawNciteLogo ();
			NciteEditorTools.BlockHeader ("Lua Struct");

			LuaStruct luaStuct = target as LuaStruct;

			Variable.VariableListGUIInspector (luaStuct.members);

			if (GUI.changed)
				EditorUtility.SetDirty(luaStuct);
		}
		#endregion
	}
}
#endif