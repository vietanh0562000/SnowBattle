using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Utils;


/// PlayAudioOnSpawn
/// Description
/// By Jorge L. Chávez Herrera.
public class PlayAudioOnSpawn : OptimizedGameObject
{

	#region MonoBehaviour overrides
	private void Awake () 
	{
		cachedPoolManaged.onSpawn += PlayAudio;
	}
	#endregion

	#region Class implementation
	// Update is called once per frame
	private void PlayAudio () 
	{
		cachedAudioSource.Play ();
	}
	#endregion
}
