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
	public class CueEffectPlaySpineAnimationSequece : SpineAnimationPlayerBase, ICueEffect
	{
		#region Child classes
		[System.Serializable]
		public class SpineAnimationSegment
		{
			public string animation;
			public int repeat = 1;
			public float duration = 0;
			public float hold;

			public SpineAnimationSegment ()
			{
				repeat = 1;
				duration = 0;
			}
		}
		#endregion

		#region Class members
		public SkeletonGraphic skeletonGraphic;
		public bool loop;
		public List<SpineAnimationSegment> animationSegments  = new List<SpineAnimationSegment> ();
		#endregion

		#region Class implementation
		private IEnumerator PlaySequence ()
		{
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

		#region ICueEffect implementation

		public void CueIn ()
		{
			StopAllCoroutines ();

			SpineAnimationPlayerBase spineAnimationPlayerBase = skeletonGraphic.GetComponent<SpineAnimationPlayerBase>();

			if (spineAnimationPlayerBase != null)
				spineAnimationPlayerBase.Stop ();
			
			StartCoroutine (PlaySequence ());
		}

		public void CueOut () {}
		#endregion
	}
}
#endif