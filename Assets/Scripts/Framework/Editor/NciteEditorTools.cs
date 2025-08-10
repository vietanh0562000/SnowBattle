using UnityEngine;
using UnityEditor;
using System.Collections;

public static class NciteEditorTools 
{
	public static void ContactSupportWithSubject (string subject) 
	{
		string url = "mailto:support@ncite.mx?subject=" + EscapeURL(subject);
		Application.OpenURL(url);
	}

	static string EscapeURL (string url)
	{
		return WWW.EscapeURL(url).Replace("+","%20");
	}

	private static Texture2D _NciteLogo = null;

	public static Texture2D NciteLogo 
	{
		get 
		{
			if (_NciteLogo == null) 
			{
				_NciteLogo =  Resources.Load("Ncite Logo") as Texture2D;
			} 

			return _NciteLogo;
		}
	}


	public static void DrawNciteLogo () 
	{

		GUIStyle s =  new GUIStyle();
		GUIContent content =  new GUIContent (NciteLogo, "Visit site");

		bool click = GUILayout.Button(content, s);
		if(click) {
			Application.OpenURL("https://ncite.mx/");
		}
	}

	public static void BlockHeader (string header) 
	{
		EditorGUILayout.Space();
		EditorGUILayout.HelpBox(header, MessageType.None);
		EditorGUILayout.Space();
	}
}