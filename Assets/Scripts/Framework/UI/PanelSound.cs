using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.UI;
using Framework.Utils;

[RequireComponent (typeof(PanelBase))]
/// <summary>
/// Panel sound.
/// Implements AudioSource playback when showing or hiding panels.
/// 
/// By Jorge L. CHávez H.
/// </summary>
public class PanelSound : OptimizedGameObject 
{
	#region Class members
	[SerializeField]
	private AudioClip showSFX;
	[SerializeField]
	private float showSFXDelay;
	[SerializeField]
	private AudioClip hideSFX;
	[SerializeField]
	private float hideSFXDelay;
	#endregion 

	#region MonoBehaviour overrides
	private void Awake ()
	{
		GetComponent<PanelBase> ().onBeginShow += PlayShowSFX;
		GetComponent<PanelBase> ().onBeginHide += PlayHideSFX;
	}

	private void OnDestroy ()
	{
		GetComponent<PanelBase> ().onBeginShow -= PlayShowSFX;
		GetComponent<PanelBase> ().onBeginHide -= PlayHideSFX;
	}

	private void PlayShowSFX ()
	{
		cachedAudioSource.PlayDelayed (showSFX, showSFXDelay);
	}

	private void PlayHideSFX ()
	{
		cachedAudioSource.PlayDelayed (hideSFX, hideSFXDelay);
	}
	#endregion 
}
