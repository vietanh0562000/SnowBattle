using UnityEngine;
using UnityEditor;
using Framework.EditorUtils;
using Framework.UI;

/// <summary>
/// ConversationMenu.cs
/// By Jorge L. Chávez Herrera.
/// 
/// Editor class, adds menu creation options for conversations & conversers.
/// 
/// </summary>
public class ConversationMenu
{
	#region Class implementation
	[MenuItem("Assets/Create/Conversations/Converser")]
	static public void CreateConverser()
	{
		ScriptableObjectUtility.CreateAsset<Converser>();
	}

	[MenuItem("Assets/Create/Conversations/Conversation")]
	static public void CreateConversation()
	{
		ScriptableObjectUtility.CreateAsset<Conversation>();
	}
	#endregion
}
