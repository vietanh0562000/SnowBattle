using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


namespace Framework.UI
{
	/// <summary>
	/// ButtonSound.
	/// Plays back an audioclip when a button is pressed.
	/// 
	/// By Jorge L. Chavez Herrera.
	/// </summary>
	public class ButtonSound : MonoBehaviour, IPointerDownHandler
	{
		#region Class members
		public AudioClip audioClip;
		public float volume = 1;
		public float delay = 0;
		#endregion
			
		#region Class accessors
		private AudioSource _cachedAudioSource;
		private AudioSource cachedAudioSource
		{
			get
			{
				if (_cachedAudioSource == null)
				{
					// Try finding an already exixting AudioSource
					_cachedAudioSource = GetComponent<AudioSource>();
						
					// If no AudioSource was found, add a new one
					if (_cachedAudioSource == null)
						_cachedAudioSource = gameObject.AddComponent<AudioSource>();
				}
					
				return _cachedAudioSource;
			}
		}
		#endregion
			
		#region IPointerDownHandler implementation
		// This fucntion is triggered by the Unity's event system
		public void OnPointerDown (PointerEventData eventData) 
		{
			Selectable selectable = GetComponent<Selectable>();
				
			if (selectable != null && selectable.interactable == true)
			{	
				cachedAudioSource.volume = volume;
				cachedAudioSource.clip = audioClip;
				cachedAudioSource.PlayDelayed ( delay);
			}
		}
		#endregion
	}
}
