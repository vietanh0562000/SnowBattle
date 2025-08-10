#if NCITE_SPINE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


namespace Framework.Spine
{
	[System.Serializable]
	public class SpineAnimationSegment
	{
		[SpineAnimation]
		public string animation;
		public int repeat = 1;
		public float duration = 0;
		public float hold;

		public SpineAnimationSegment()
		{
			repeat = 1;
			duration = 0;
		}
	}

	/// <summary>
	/// Spine animation player.
	/// Component prividing an interface to Spine animation
	/// 
	/// By Jorge L. Chávez Herrera
	/// </summary>
	public class SpineAnimationSequencer : SpineAnimationPlayerBase
	{
		#region Class members
		public SkeletonGraphic skeletonGraphic;

		public bool autoStart;
		public bool loop;
		public List<SpineAnimationSegment> animationSegments  = new List<SpineAnimationSegment> ();
		#endregion

		#region MonoBehaviour opverrides
		private void Awake ()
		{
			if (skeletonGraphic == null)
				skeletonGraphic =  GetComponentInChildren<SkeletonGraphic> ();
		}

		private void OnEnable ()
		{
			if (autoStart == true)
				StartCoroutine (PlaySequence ());
		}
		#endregion

		#region Class implementation
		private IEnumerator PlaySequence ()
		{
			float time = 0;

			Loop:
				for (int i = 0; i < animationSegments.Count; i++) 
				{
					for (int t = 0; t < animationSegments [i].repeat; t++)
					{
					
						skeletonGraphic.AnimationState.SetAnimation (0, animationSegments [i].animation, false);	
						float duration = GetAninmationDuration (animationSegments [i].animation) + animationSegments [i].hold;

						if (animationSegments [i].duration > 0)
							duration = animationSegments [i].duration;

						// Wit for the animation to end
						for (float d = 0; d < duration; d += Time.deltaTime) 
						{
							yield return null;
						}
					}
				}

			if (loop == true)
				goto Loop;
		}

		public float GetAninmationDuration (string animationName)
		{
			return skeletonGraphic.SkeletonDataAsset.GetSkeletonData (true).FindAnimation (animationName).Duration;
		}
		
		#endregion
	}
}
#endif