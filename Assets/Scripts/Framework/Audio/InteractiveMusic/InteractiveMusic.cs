using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework.Audio
{
    /// <summary>
    /// Interactive Music Segment.
    /// 
    /// Defines a segmnet of audio with beat and tempo information in addition to 
    /// audio data.
    /// InteractiveMusicSegments are played back and mixed in realtime by the 
    /// Interactive Music Player.
    /// 
    /// By Jorge L. Chávez Herrera.
    /// </summary>
    public class InteractiveMusic : ScriptableObject
    {
        #region Class members
        public float bpm = 120;

        public List<Segment> segments = new List<Segment>();
        #endregion

        #region Class accessors
        #endregion

        #region MonoBehaviour events
        #endregion

        #region Base class overrides
        #endregion

        #region Class implementation
        #endregion

        #region Interface implementation
        #endregion

        #region Nested classes
        [System.Serializable]
        public class Segment
        {
            public AudioClip audioClip;
            public int startBeat = 0;
            public int length = 4;
            public float fadeLength = 1;
        }
        #endregion
    }
}
