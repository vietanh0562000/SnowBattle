#if NCITE_SPINE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


namespace Framework.Spine
{
	/// <summary>
	/// Spine animation player.
	/// Component prividing an interface to Spine animation
	/// 
	/// By Jorge L. Chávez Herrera
	/// </summary>
	public class SpineAnimationPlayer : MonoBehaviour 
	{
		private SkeletonAnimation skeletonAnimation;
		private int trackIndex;

		public float timeScale
		{
			get { return skeletonAnimation.AnimationState.GetCurrent (trackIndex).TimeScale;}
			set { skeletonAnimation.AnimationState.GetCurrent (trackIndex).TimeScale = value;}
		}


		public string currentName
		{
			get { return skeletonAnimation.AnimationState.GetCurrent (trackIndex).ToString ();}
		}

		/*
		public TrackEntry GetAnimationState (int trackIndex)
		{
			return skeletonAnimation.AnimationState.GetCurrent (trackIndex);
		}*/

		private void Awake ()
		{
			skeletonAnimation = GetComponentInChildren<SkeletonAnimation> ();
		}

		public void Play (int tranckIndex, string animationName, float timeScale, bool loop)
		{
			this.trackIndex = tranckIndex;
			skeletonAnimation.AnimationState.SetAnimation (tranckIndex, animationName, loop).timeScale = timeScale;
		}	
	}
}
#endif