#if NCITE_LUA
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Lua;

/// LuaStruct
/// Represents a data structure that can created and initialized in the Unity Editor & is sutable for Lua scripts.
/// By Jorge L. Chávez Herrera.
public class LuaStruct : ScriptableObject 
{
	#region Class members
	public List<Variable> members = new List<Variable> ();
	#endregion

	#region Class implementation
	#endregion
}
#endif