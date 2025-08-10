#if NCITE_LUA
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Framework.Lua
{
	/// <summary>
	/// Lua script binder.
	/// Encapsulates an interface to Lua scripts providing functionality to declare & link values to variable members on scripts.
	/// 
	/// By Jorge L. Chávez Herrera.
	/// </summary>
	[System.Serializable]
	public class LuaScriptBinder 
	{
		#region Class members
		public TextAsset tablesTextAsset;
		public TextAsset scriptTextAsset; 
		// public Object luaScript;
		private Script script;
		public List<Variable> variables = new List<Variable> ();
		#endregion

		#region Class implementation
		public void Init ()
		{
			string scriptString = "";

			// Initialize a new script and add it to the loaded sripts dictionary
			script = new Script();
			LUAScriptManager.RegisterTypesOnScript (script);

			// Register variables for this scrip & create dynvalues for easy referencing from other scrpits.
			foreach (Variable variable in variables) 
			{
				switch (variable.type) 
				{
					case VariableType.boolType:   script.Globals.Set (variable.name, variable.SetDynValue (DynValue.NewBoolean (variable.boolValue))); 
					break;
					case VariableType.intType:    script.Globals.Set (variable.name, variable.SetDynValue (DynValue.NewNumber (variable.intValue))); 
					break;
					case VariableType.floatType:  script.Globals.Set (variable.name, variable.SetDynValue (DynValue.NewNumber (variable.floatValue))); 
					break;
					case VariableType.stringType: script.Globals.Set (variable.name, variable.SetDynValue (DynValue.NewString (variable.stringValue))); 
					break;
					case VariableType.objectType: 
						if (variable.objectValue != null) 
						{
							UserData.RegisterType (variable.objectValue.GetType ());
							script.Globals.Set (variable.name,variable.SetDynValue (UserData.Create (variable.objectValue)));
						}
						// Object type variable has no value yet, we will register it anyways but with a Nil value
						else 
						{
							script.Globals.Set (variable.name, variable.SetDynValue (DynValue.Nil));
						}
					break;
				}
			}

			// Register tables if any
			if (tablesTextAsset != null)
				scriptString += tablesTextAsset.text;

			scriptString += scriptTextAsset.text;

			script.DoString (scriptString);
		}

		public Script GetScript ()
		{
			// Safe check for scrpit execution order changes
			if (script == null)
				Init ();
			
			return script;
		}

		static public void RegisterVariablesOnScript (Script script, List<Variable> variables)
		{
			// Register variables for this scrip
			foreach (Variable variable in variables) 
			{
				switch (variable.type) 
				{
				case VariableType.boolType:   script.Globals.Set (variable.name, DynValue.NewBoolean (variable.boolValue)); 
					break;
				case VariableType.intType:    script.Globals.Set (variable.name, DynValue.NewNumber (variable.intValue)); 
					break;
				case VariableType.floatType:  script.Globals.Set (variable.name, DynValue.NewNumber (variable.floatValue)); 
					break;
				case VariableType.stringType: script.Globals.Set (variable.name, DynValue.NewString (variable.stringValue)); 
					break;
				case VariableType.objectType: 
					if (variable.objectValue != null) 
					{
						UserData.RegisterType (variable.objectValue.GetType ());
						script.Globals.Set (variable.name, UserData.Create (variable.objectValue));
					}
					// Object type variable has no value yet, we will register it anyways but with a Nil value
					else 
					{
						script.Globals.Set (variable.name, DynValue.Nil);
					}
					break;
				}
			}
		}

		#if UNITY_EDITOR
		static public void GUIInspector (LuaScriptBinder luaScriptBinder)
		{
			Variable variableToRemove = null;
			Variable variableToMoveUp = null;
			Variable variableToMoveDown= null;

			if (luaScriptBinder == null)
				return;

			luaScriptBinder.scriptTextAsset = EditorGUILayout.ObjectField ("Script Text Asset", luaScriptBinder.scriptTextAsset, typeof(TextAsset), false) as TextAsset;
			luaScriptBinder.tablesTextAsset = EditorGUILayout.ObjectField ("Tables Text Asset", luaScriptBinder.tablesTextAsset, typeof(TextAsset), false) as TextAsset;

			Variable.VariableListGUIInspector (luaScriptBinder.variables);
		}
		#endif

		#endregion
	}
}
#endif
