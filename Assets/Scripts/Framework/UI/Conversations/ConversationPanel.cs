using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;


namespace Framework.UI
{
	/// <summary>
	/// Conversation panel.
	/// By Jorge L. Chávez Herrera.
	/// 
	/// Defines a panel showing text & character portraits 
	/// for conversations to unfold the story of the game.
	/// </summary>
	public class ConversationPanel : PanelBase 
	{
		[System.Serializable]
		/// <summary>
		/// Class facilitates connection with widgets
		/// </summary>
		public class WidgetGruoup
		{
			public CanvasGroup groupCanvasGroup;
			public RectTransform textPanelRoot;
			public Image portrait;
			[System.NonSerialized]
			public Vector3 savedPostraitScale;
			public Text nameLabel;
			public Text textLabel;
			public Button continueButton;
			public Button closeButton;
		}
		#region Class members
		public float textWritingDuration = 1;
		public WidgetGruoup[] widgetGroups;
		private int currentWidgetGroupIndex;
		private int conversationNodeIndex;
		private Conversation currentConversation;
		private bool hudWasActive;
		#endregion

		#region MonoBehaviour overrides
		override protected void Awake ()
		{
            base.Awake(); // Just added and has not been tested.
			// Save portrait scales fot flexibility
			foreach (WidgetGruoup wg in widgetGroups) 
			{
				wg.savedPostraitScale = wg.portrait.rectTransform.localScale;
			}

			onBeginShow += LowerMusicVolumeAndHideHUD;
			onBeginHide += RaiseMusicVolumeAndRestureHUD;
		}
		#endregion

		#region Class implementattion
		private void LowerMusicVolumeAndHideHUD ()
		{ /*
			// Hide HUD if Needed
			hudWasActive = UIManager.Instance.hudPanel.gameObject.activeSelf;

			if (hudWasActive == true)
				UIManager.Instance.hudPanel.gameObject.SetActive (false);
			
			MusicManager.Instance.FadeTo (1, 0.15f);*/
		}

		private void RaiseMusicVolumeAndRestureHUD ()
		{
			/*
			MusicManager.Instance.FadeTo (1, MusicManager.Instance.startVolume);
			UIManager.Instance.hudPanel.gameObject.SetActive (hudWasActive);
			*/
		}

		public void StartConversation (Conversation conversation)
		{
			foreach (WidgetGruoup wg in widgetGroups) 
			{
				wg.groupCanvasGroup.gameObject.SetActive (false);
				wg.groupCanvasGroup.alpha = 1;
			}
			
			currentConversation = conversation;
			currentWidgetGroupIndex = conversation.startFlipped == true ? 1 : 0;
			conversationNodeIndex = 0;

			Show ();


				
			SetNodeInfo (currentWidgetGroupIndex, conversationNodeIndex);
		}
			
		private void SetNodeInfo (int widgetGroupIndex, int conversationNodeIndex)
		{
			WidgetGruoup wg = widgetGroups [widgetGroupIndex % 2];
			Conversation.Node node = currentConversation.nodes [conversationNodeIndex];

			wg.groupCanvasGroup.gameObject.SetActive (true);
			wg.portrait.sprite = node.converser.GetSpriteForMood (node.mood);
			wg.portrait.rectTransform.offsetMax = node.converser.offset;
			wg.portrait.rectTransform.offsetMin = node.converser.offset;
			wg.textPanelRoot.offsetMax = node.textPanelOffset;
			wg.textPanelRoot.offsetMin = node.textPanelOffset;

			wg.portrait.rectTransform.localScale = Vector3.Scale (wg.savedPostraitScale, node.converser.scale);
			wg.nameLabel.text = node.converser.GetName ();

			// Send node messages
			node.SendMessages ();

			// Playback audio
			if (node.audioClip != null) 
			{
				cachedAudioSource.clip = node.audioClip;
				cachedAudioSource.Play ();
				StartCoroutine (ShowText (wg.textLabel, node.GetText (), node.audioClip.length - 0.25f));
			}
			else
				StartCoroutine (ShowText (wg.textLabel, node.GetText (), textWritingDuration));

			wg.continueButton.gameObject.SetActive (conversationNodeIndex < currentConversation.nodes.Count - 1);
			wg.closeButton.gameObject.SetActive (conversationNodeIndex > currentConversation.nodes.Count -2);
		}
			
		private IEnumerator ShowText (Text label, string text, float duration)
		{
			// Clear text label
			label.text = "";

			// Reveal text character by character 
			for (float t = 0; t <= duration; t += Time.unscaledDeltaTime) 
			{
				float nt = t / duration;

				string part1 = text.Remove ((int)(nt * text.Length));
				string part2 = text.Remove (0, part1.Length);
				label.text = part1 + "<color=#ffffff00>" + part2 + "</color>";
				yield return null;
			}

			label.text = text;
		}

		private IEnumerator FadeWidgetGruoup (WidgetGruoup wg, float start, float end, float delay, float duration)
		{
			wg.groupCanvasGroup.alpha = start;
				
			yield return new WaitForSecondsRealtime (delay);

			for (float t = 0; t <= duration; t += Time.unscaledDeltaTime) 
			{
				float nt = t / duration;
				wg.groupCanvasGroup.alpha = Mathf.Lerp (start, end, nt);
				yield return null;
			}

			wg.groupCanvasGroup.alpha = end;
		}

		/// <summary>
		/// Shows next conversation node
		/// </summary>
		public void Next ()
		{
			StopAllCoroutines ();
			cachedAudioSource.Stop ();

			widgetGroups [currentWidgetGroupIndex % 2].groupCanvasGroup.gameObject.SetActive (false);

			if (currentConversation.nodes [conversationNodeIndex].converser != currentConversation.nodes [conversationNodeIndex + 1].converser) 
			{
				StartCoroutine (FadeWidgetGruoup (widgetGroups [currentWidgetGroupIndex % 2], 1, 0, 0, 0.25f));
				currentWidgetGroupIndex++;
				StartCoroutine (FadeWidgetGruoup (widgetGroups [currentWidgetGroupIndex % 2], 0, 1, 0, 0.25f));
			}
			
			conversationNodeIndex++;
			SetNodeInfo (currentWidgetGroupIndex, conversationNodeIndex);
		}
		#endregion
	}
}
