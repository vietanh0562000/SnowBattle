using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// AudioClipExtensions.cs
/// 
/// By Jorge L. Chávez Herrera.
/// </summary>
public static class AudioSourceExtensions
{
	static public void Play (this AudioSource audioSource, AudioClip audioClip, float volume = 1, float pitch = 1)
	{
		audioSource.clip = audioClip;
		audioSource.volume = volume;
        audioSource.pitch = pitch;
		audioSource.Play ();
	}

    static public void PlayDelayed (this AudioSource audioSource, AudioClip audioClip, float delay, float volume = 1, float pitch = 1)
	{
		audioSource.clip = audioClip;
		audioSource.volume = volume;
        audioSource.pitch = pitch;
		audioSource.PlayDelayed (delay);
	}

    static public void PlayClipAt (this AudioSource audioSource, AudioClip audioClip, Vector3 position, float volume = 1, float pitch = 1)
    {
        GameObject go = new GameObject("ClipAtPoint");
        go.transform.position = position;
        AudioSource newAudioSource = go.AddComponent<AudioSource>();
        newAudioSource.Play (audioClip, volume, pitch);
    }

    static private void CopyAudioSourceProperties (AudioSource source, AudioSource destination)
    {
        destination.clip = source.clip;
        destination.outputAudioMixerGroup = source.outputAudioMixerGroup;
        destination.mute = source.mute;
        destination.bypassEffects = source.mute;
        destination.bypassListenerEffects = source.mute;
        destination.bypassReverbZones = source.mute;
        destination.playOnAwake = source.mute;
        destination.loop = source.mute;
        destination.priority = source.priority;
        destination.volume = source.volume;
        destination.pitch = source.pitch;
        destination.panStereo = source.panStereo;
        destination.spatialBlend = source.spatialBlend;
        destination.reverbZoneMix = source.reverbZoneMix;
        destination.playOnAwake = source.playOnAwake;

        // TODO 
        // Implemente 3D sound settings
    }

}

