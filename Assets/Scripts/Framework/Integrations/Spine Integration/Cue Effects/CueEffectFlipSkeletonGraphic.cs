#if NCITE_SPINE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class CueEffectFlipSkeletonGraphic : MonoBehaviour, ICueEffect
{
	#region Class members
	public bool flipX;
	public bool flipY;

	public SkeletonGraphic skeletonGraphic;
	#endregion

	#region MonoBehaviour overrides
	private void Awake ()
	{
		if (skeletonGraphic == null)
			skeletonGraphic = GetComponent<SkeletonGraphic> ();
	}
	#endregion

	#region ICueEffect implementation

	public void CueIn ()
	{
		if (flipX == true)
			skeletonGraphic.Skeleton.FlipX = !skeletonGraphic.Skeleton.FlipX;

		if (flipY == true)
			skeletonGraphic.Skeleton.FlipY = !skeletonGraphic.Skeleton.FlipY;
	}

	public void CueOut (){}

	#endregion
}
#endif
