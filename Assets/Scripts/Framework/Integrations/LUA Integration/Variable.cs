#if NCITE_LUA
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
#if UNITY_EDITOR
using UnityEditor;
#endif


/// <summary>
/// Variable.
/// Defines a variable that can be exposed on Unity components & used by Lua scripts.
/// 
/// By Jorge L. CHávez Herrera.
/// </summary>
[System.Serializable]
public class Variable
{
	#region class members
	[SerializeField] private VariableType _type;  // The type for this variable, bool, int, float, string or Object.
	[SerializeField] private string _name;		  // The name for this variable.
	[SerializeField] private float  _numberValue; // Bool, int or float value.
	[SerializeField] private string _stringValue; // String value
	[SerializeField] private Object _objectValue; // Object value, can be anything in Unity GameObjects, components, etc.

	private DynValue dynValue = null;
	#endregion

	#region class accessors
	/// <summary>
	/// Gets the variable type.
	/// </summary>
	/// <value>The type.</value>
	public VariableType type
	{
		get { return _type; }
	}

	/// <summary>
	/// Gets the variable name.
	/// </summary>
	/// <value>The name.</value>
	public string name
	{
		get { return _name; }
		set { _name = value; }
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="Variable"/> bool value.
	/// </summary>
	/// <value><c>true</c> if bool value; otherwise, <c>false</c>.</value>
	public bool boolValue
	{
		get { return _numberValue == 0 ? false : true; }
		set { _numberValue = (value == false) ? 0 : 1; }  
	}

	/// <summary>
	/// Gets or sets the int value.
	/// </summary>
	/// <value>The int value.</value>
	public int intValue
	{
		get { return (int)_numberValue; }
		set { _numberValue = value; }  
	}

	/// <summary>
	/// Gets or sets the float value.
	/// </summary>
	/// <value>The float value.</value>
	public float floatValue
	{
		get { return _numberValue; }
		set { _numberValue = value; }  
	}

	/// <summary>
	/// Gets or sets the string value.
	/// </summary>
	/// <value>The string value.</value>
	public string stringValue
	{
		get { return _stringValue; }
		set { _stringValue = value; }  
	}

	/// <summary>
	/// Gets or sets the object value.
	/// </summary>
	/// <value>The object value.</value>
	public Object objectValue
	{
		get { return _objectValue; }
		set { _objectValue = value; }  
	}
	#endregion

	#region Class implementattion
	/// <summary>
	/// Initializes a new instance of the <see cref="Variable"/> class.
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="type">Type.</param>
	public Variable (string name, VariableType type)
	{
		this._name = name;
		this._type = type;
	}

	/// <summary>
	/// Sets the DynValue associated with this variable.
	/// </summary>
	/// <returns>The dyn value.</returns>
	/// <param name="dynValue">Dyn value.</param>
	public DynValue SetDynValue (DynValue dynValue)
	{
		this.dynValue = dynValue;
		return dynValue;
	}

	/// <summary>
	/// Gets the DynValue assosiated with this variable.
	/// </summary>
	/// <returns>The dyn value.</returns>
	public DynValue GetDynValue ()
	{
		return dynValue;
	}
		
	/// <summary>
	/// Creates a new boolean variable.
	/// </summary>
	/// <returns>The boolean variable.</returns>
	/// <param name="name">Name.</param>
	/// <param name="value">Value.</param>
	static public Variable NewBool (string name, bool value)
	{
		Variable ret = new Variable (name, VariableType.boolType);
		ret._numberValue = value == false ? 0 : 1;

		return ret;
	}

	/// <summary>
	///  Creates a new int variable.
	/// </summary>
	/// <returns>The integer variable.</returns>
	/// <param name="name">Name.</param>
	/// <param name="value">Value.</param>
	static public Variable NewInt (string name, int value)
	{
		Variable ret = new Variable (name, VariableType.intType);
		ret._numberValue = value;

		return ret;
	}

	/// <summary>
	///  Creates a new float variable.
	/// </summary>
	/// <returns>The float variable.</returns>
	/// <param name="name">Name.</param>
	/// <param name="value">Value.</param>
	static public Variable NewFloat (string name, float value)
	{
		Variable ret = new Variable (name, VariableType.floatType);
		ret._numberValue = value;

		return ret;
	}

	/// <summary>
	/// Creates a new string variable.
	/// </summary>
	/// <returns>The string.</returns>
	/// <param name="name">Name.</param>
	/// <param name="value">Value.</param>
	static public Variable NewString (string name, string value)
	{
		Variable ret = new Variable (name, VariableType.stringType);
		ret._stringValue = value;

		return ret;
	}

	/// <summary>
	/// Creates a new object variable.
	/// </summary>
	/// <returns>The object.</returns>
	/// <param name="name">Name.</param>
	/// <param name="value">Value.</param>
	static public Variable NewObject (string name, Object value)
	{
		Variable ret = new Variable (name, VariableType.objectType);
		ret._objectValue = value;

		return ret;
	}
	#endregion 

#if UNITY_EDITOR
	static public void VariableListGUIInspector (List<Variable> variables)
	{
		Variable variableToRemove = null;
		Variable variableToMoveUp = null;
		Variable variableToMoveDown = null;

		// Add variable of different type buttons
		EditorGUILayout.BeginHorizontal ();

		EditorGUILayout.LabelField ("Add variable:", GUILayout.Width (80));

		if (GUILayout.Button ("Bool"))
			variables.Add (Variable.NewBool ("unamed", false));

		if (GUILayout.Button ("Integer"))
			variables.Add (Variable.NewInt ("unamed", 0));

		if (GUILayout.Button ("Float"))
			variables.Add (Variable.NewFloat ("unamed", 0));

		if (GUILayout.Button ("String"))
			variables.Add (Variable.NewString ("unamed", ""));

		if (GUILayout.Button ("Object"))
			variables.Add (Variable.NewObject ("unamed", null));

		EditorGUILayout.EndHorizontal ();

		foreach (Variable variable in variables) 
		{
			EditorGUILayout.BeginVertical ("button");

			EditorGUILayout.BeginHorizontal ();

			// Draw move up/down arrows
			EditorGUILayout.BeginVertical (GUILayout.Width (20));

			GUI.enabled = variables.IndexOf (variable) > 0;

			GUI.backgroundColor = Color.green;
			if (GUILayout.Button ("↑"))
				variableToMoveUp = variable;

			GUI.enabled = variables.IndexOf (variable) < variables.Count-1;

			if (GUILayout.Button ("↓"))
				variableToMoveDown = variable;

			GUI.backgroundColor = Color.white;
			GUI.enabled = true;
			EditorGUILayout.EndVertical ();

			EditorGUILayout.BeginVertical ();

			EditorGUILayout.BeginHorizontal ();

			variable.name = EditorGUILayout.TextField ("Name", variable.name);
			GUI.backgroundColor = Color.red;
			if (GUILayout.Button ("x", GUILayout.Width (20)))
				variableToRemove = variable;
			GUI.backgroundColor = Color.white;
			EditorGUILayout.EndHorizontal ();

			switch (variable.type) 
			{
			case VariableType.boolType: variable.boolValue = EditorGUILayout.Toggle ("Bool value", variable.boolValue); break;
			case VariableType.intType: variable.intValue = EditorGUILayout.IntField ("Integer value", variable.intValue); break;
			case VariableType.floatType: variable.floatValue = EditorGUILayout.FloatField ("Float value", variable.floatValue); break;
			case VariableType.stringType: variable.stringValue = EditorGUILayout.TextField ("String value", variable.stringValue); break;
			case VariableType.objectType: variable.objectValue = ObjectField ("Object value", variable.objectValue); break;
			}

			EditorGUILayout.EndVertical ();

			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.EndVertical ();
		}

		// Housekeeping

		// Remove selected variables
		if (variableToRemove != null) 
		{
			variables.Remove (variableToRemove);
		}

		// Move variable up
		if (variableToMoveUp != null) 
		{
			int index = variables.IndexOf (variableToMoveUp);
			variables.Remove (variableToMoveUp);
			variables.Insert (index -1, variableToMoveUp);
			variableToMoveUp = null;
		}

		// Move variable down
		if (variableToMoveDown != null) 
		{
			int index = variables.IndexOf (variableToMoveDown);
			variables.Remove (variableToMoveDown);
			variables.Insert (index + 1, variableToMoveDown);
			variableToMoveDown = null;
		}
	}

	/// <summary>
	/// Objects the field.
	/// Draws an object field allowig top bind GameObjects or it's components the variable value.
	/// </summary>
	/// <returns>The field.</returns>
	/// <param name="label">Label.</param>
	/// <param name="value">Value.</param>
	static public Object ObjectField (string label, Object value)
	{
		Object ret = value;

		if (value == null || value.GetType () == typeof(GameObject)) 
		{
			ret = EditorGUILayout.ObjectField ("Object value", value, typeof (Object), true);

			if (value != null) 
			{
				// Get all components on this GameObject & create a popup menu to chose from
				int selected = 0;
				Component[] components = (value as GameObject).GetComponents<Component> ();

				// Build components menu
				List<string> options = new List<string> ();
				options.Add ("-- No component selected --");

				for (int i = 0; i < components.Length; i++) {
					options.Add (components [i].GetType ().Name);
				}

				// Slect from components
				selected = EditorGUILayout.Popup ("Components", selected, options.ToArray ());

				if (selected > 0) 
				{
					ret = components [selected - 1];
				}
			}
		}
		// Reference is component
		else 
		{
			if (value.GetType () == typeof(Component)) {
				GameObject go = (value as Component).gameObject;

				ret = EditorGUILayout.ObjectField ("Object value", ret, typeof(Object), false);

				List<Component> components = new List<Component> (go.GetComponents<Component> ());

				// Build components menu
				List<string> options = new List<string> ();
				options.Add ("-- No component selected --");

				for (int i = 0; i < components.Count; i++)
					options.Add (components [i].GetType ().Name);

				int selected = components.IndexOf (value as Component) + 1;
				selected = EditorGUILayout.Popup ("Components", selected, options.ToArray ());

				// Slect from components
				if (selected == 0)
					ret = go;
				else {
					ret = components [selected - 1];
				}
			} 
			else 
			{
				// Value is any other type derived from Object
				ret = EditorGUILayout.ObjectField ("Object value", ret, typeof(Object), false);
			}
		}

		return ret;
	}
#endif
}

public enum VariableType
{
	/// <summary>
	/// Boolean type.
	/// </summary>
	boolType,
	/// <summary>
	/// Integer type.
	/// </summary>
	intType,
	/// <summary>
	/// float type.
	/// </summary>
	floatType,
	/// <summary>
	/// String type.
	/// </summary>
	stringType,
	/// <summary>
	/// System.Object type.
	/// </summary>
	objectType
}



#endif
