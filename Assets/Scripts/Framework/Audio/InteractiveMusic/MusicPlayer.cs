using UnityEngine;
using System.Collections;
using Framework.Delegates;
using UnityEngine.Audio;
using Framework.Utils;
using System;
using Framework.Tweening;

namespace Framework.Audio
{
    /// <summary>
    /// Music Player.
    /// 
    /// Defines a music player capable of mixing loops from and song in realtime.
    /// Uses double buffering to playback a new loop while the current one is fading out.
    /// For this player to work effectively loops must be separated and have a tail.
    /// 
    /// By Jorge L. Chávez Herrera.
    /// </summary>
    public class MusicPlayer : MonoSingleton<MusicPlayer>
    {
        #region Class members
        [SerializeField]
        private InteractiveMusic music;         // The InteractiveMusic scriptable object to be played.
        [SerializeField]
        AudioMixerGroup _output;                // The AudioMixerGroup to play music through.
        [SerializeField][Range(0, 1)]
        private float _volume = 1;              // Music volume.
        [SerializeField]
        private bool autoPlay = true;           // Start playback automatically

        private AudioSource[] audioSources;     // Array of audio sources (It will always have 2.
        private AudioSource currentAudioSource; // Current audio source used for playback.

        [System.NonSerialized]
        public int currentSegmentIndex = 0;     // Index of the currently playing music segment.
        private int nextSegmentIndex = 0;       // Index of the next music segment to play. 

        public SimpleDelegate<float, float> onPlayhead1Update; // Delegate called when playhead for 1st buffer changes.
        public SimpleDelegate<float, float> onPlayhead2Update; // Delegate called when playhead for 1st buffer changes.

        private int lastPlayedSegmentIndex;
        #endregion

        #region Class accessors
        /// <summary>
        /// Gets or sets the output audio mixer group.
        /// </summary>
        /// <value>The output.</value>
        public AudioMixerGroup Output
        {
            get { return _output; }
            set
            {
                _output = value;
                audioSources[0].outputAudioMixerGroup = _output;
                audioSources[1].outputAudioMixerGroup = _output;
            }
        }
        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>The volume.</value>
        public float Volume
        {
            get { return _volume; }
            set 
            {
                _volume = value;
                audioSources[0].volume = _volume; 
                audioSources[1].volume = _volume; 
            }
        }
        #endregion

        #region MonoBehaviour events
        private void Awake ()
        {
            // Setup 2 audio sources for double buffered playback.
            audioSources = new AudioSource[2];

            audioSources[0] = gameObject.AddComponent<AudioSource>();
            audioSources[1] = gameObject.AddComponent<AudioSource>();

            Output = _output;
        }

        private IEnumerator Start ()
        {
            // This is needed in order for music to play at first update and not
            // before as it happens as with regular AudioSources.
            yield return new WaitForSeconds (2);

            if (autoPlay == true)
                Play();
        }

        private void Update ()
        {
            if (onPlayhead1Update != null)
                onPlayhead1Update (audioSources[0].time / audioSources[0].clip.length, audioSources[0].volume); 

            if (onPlayhead2Update != null)
                onPlayhead1Update (audioSources[1].time / audioSources[1].clip.length, audioSources[1].volume);
        }
        #endregion

        #region Base class overrides
        #endregion

        #region Class implementation
        /// <summary>
        /// Starts music playback.
        /// </summary>
        public void Play ()
        {
            if (music != null)
            {
                currentSegmentIndex = nextSegmentIndex = 0;
                StartCoroutine (PlaySegmentCoroutine (0, currentSegmentIndex));
            }
        }

        public void PlayMusic (InteractiveMusic music)
        {
            this.music = music;
            Play ();
        }

        /// <summary>
        /// Pauses music playback.
        /// </summary>
        public void Pause ()
        {
            for (int i = 0; i < audioSources.Length; i++)
                audioSources[i].Pause ();  
        }


        public void UnPause ()
        {
            for (int i = 0; i < audioSources.Length; i++)
                audioSources[i].UnPause ();
        }

        /// <summary>
        /// Stops music playback.
        /// </summary>
        public void Stop (float fadeOutDuration = 0)
        {
            if (fadeOutDuration > 0.0f)
            {
                // Stop with fade.
                StopAllCoroutines ();
                StartCoroutine (FadeAudioSource (currentAudioSource, 1, 0, fadeOutDuration, ()=> { StopAudioSources(); }));
            }
            else
            {
                // Stop with no fade.
                StopAudioSources ();
            }
        }

        private void StopAudioSources ()
        {
            StopAllCoroutines();
            for (int i = 0; i < audioSources.Length; i++)
                audioSources[i].Stop (); 
        }

        /// <summary>
        /// Sets the index of the next segment.
        /// </summary>
        /// <param name="index">Index.</param>
        public void SetNextSegmentIndex (int index)
        {
            if (music != null)
            {
                if (index < music.segments.Count)
                    nextSegmentIndex = index;
                else
                {
                    nextSegmentIndex = music.segments.Count - 1;
                    Debug.LogWarning("Segment index out of range.");
                }
            }
            else
                Debug.LogWarning("No music assigned.");
        }

        private IEnumerator PlaySegmentCoroutine (int audioSourceIndex, int segmentIndex)
        {
            
            // Playback current segment.
            currentAudioSource = audioSources[audioSourceIndex];

            currentSegmentIndex = segmentIndex;
            currentAudioSource.time = (60.0f / music.bpm) * music.segments [currentSegmentIndex].startBeat;

            // Prevent segment to repeat by going one segment backwards
            int nonRepeatedSegmentIndex = currentSegmentIndex;

            if (nonRepeatedSegmentIndex == lastPlayedSegmentIndex && nonRepeatedSegmentIndex > 0)
                nonRepeatedSegmentIndex--;

            currentAudioSource.Play (music.segments[nonRepeatedSegmentIndex].audioClip);
            lastPlayedSegmentIndex = nonRepeatedSegmentIndex;

            // Wait until playback finishes.
            yield return new WaitForSecondsRealtime ((60.0f / music.bpm) * music.segments[nonRepeatedSegmentIndex].length);

            float fadeLength = (60.0f / music.bpm) * music.segments [nonRepeatedSegmentIndex].fadeLength;

            // Playback next segment.
            StartCoroutine (PlaySegmentCoroutine ((audioSourceIndex + 1) % 2, nextSegmentIndex));

            // Fade out current segment.
            yield return StartCoroutine (FadeAudioSource (audioSources[audioSourceIndex], 1, 0, fadeLength));
        }

        private IEnumerator FadeAudioSource (AudioSource audioSource, float startVolume, float endVolume, float duration, Action endAction = null)
        {
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                float nt = t / duration;
                audioSource.volume = Mathf.Lerp (startVolume, endVolume, nt);
                yield return null;
            }

            audioSource.volume = endVolume;

            if (endAction != null)
                endAction ();
        }
        #endregion

        #region Interface implementation
        #endregion

        #region Nested classes
        #endregion
    }
}
