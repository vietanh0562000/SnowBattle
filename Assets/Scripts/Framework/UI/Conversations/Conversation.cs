using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Framework.UI
{
	public enum EConverserMood {Neutral, Positive, Negative};

	public class Conversation : ScriptableObject 
	{
		[System.Serializable]
		public class ConversationMessage
		{
			public string objectName;
			public string message;
		}

		[System.Serializable]
		public class Node
		{
			public Converser converser;
			public AudioClip audioClip;
			public EConverserMood mood;
			public string textKey;
			public string textSheet = "CONVERSATIONS";
			public Vector3 textPanelOffset;

			public ConversationMessage[] messages;

			public string GetText ()
			{
				return Framework.Localization.Localization.GetLocalizedString (textKey, textSheet);
			}

			public void SendMessages ()
			{
				if (messages != null) 
				{
					for (int i = 0; i < messages.Length; i++) 
					{
						GameObject go = GameObject.Find (messages [i].objectName);

						if (go != null)
							go.SendMessage (messages [i].message, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
		}

		public bool startFlipped = false;
		public List<Node> nodes = new List<Node>();
	}
}
