#if NCITE_SPINE
using UnityEngine;
using System.Collections;
using Spine.Unity;


namespace Framework.Spine 
{
	public class SpineBlinkPlayer : MonoBehaviour 
	{
		const int BlinkTrack = 1;

		[SpineAnimation]
		public string blinkAnimation;
		public float minimumDelay = 0.15f;
		public float maximumDelay = 3f;

		private IEnumerator Start ()
		{
			SkeletonAnimation skeletonAnimation = GetComponent<SkeletonAnimation>(); 

			if (skeletonAnimation == null) 
				yield break;
			
			while (true) 
			{
				skeletonAnimation.AnimationState.SetAnimation(SpineBlinkPlayer.BlinkTrack, blinkAnimation, false);
				yield return new WaitForSeconds(Random.Range(minimumDelay, maximumDelay));
			}
		}
	}
}
#endif
