using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Framework.UI;
using Framework.Utils;
using Framework.Gameplay;

/// LoadingPanel
/// Description
/// By Jorge L. Chávez Herrera.
public class LoadingPanel : PanelBase 
{
	#region Class members
	public Slider progressSlider;
	private FloatSDInterpolator progressInterpolator = new FloatSDInterpolator (0.25f);
	static public string sceneToLoad;
	private AsyncOperation async;
	private const float LOAD_READY_PERCENTAGE = 0.9f;
	#endregion

	#region MonoBehaviour overrides
	private void Start ()
	{
		LoadLevel (sceneToLoad);
		sceneToLoad = null;
	}

	private void Update ()
	{
		progressSlider.value = progressInterpolator.Value;
	}
	#endregion

	#region Class implementation
	public void LoadLevel (string levelToLoad)
	{
		progressInterpolator.InstantValue = 0;
		StartCoroutine (LoadLevelCoroutine (levelToLoad));
	}

	private IEnumerator LoadLevelCoroutine (string levelToLoad)
	{
		BaseCamera baseCamera = Camera.main.GetComponent<BaseCamera> ();
		// Just to show better behaviour while playing in the Unity editor.
		yield return null;

		// Fade in Loading Panel
		if (baseCamera != null) 
		{
			yield return new WaitForSeconds (baseCamera.fadeDuration);
		}

		// Load scene & update progress bar
		async = SceneManager.LoadSceneAsync (levelToLoad);
		async.allowSceneActivation = false;

		while (!async.isDone) 
		{
			progressInterpolator.targetValue = async.progress;

			// We use progress because async.done is set to true only after scene activation
			// but in this case async.allowSceneActivation is set to false so that will never happen.
			if (async.progress >= LOAD_READY_PERCENTAGE && async.allowSceneActivation == false) 
			{
				progressInterpolator.targetValue = 1f;
				yield return new WaitForSeconds (1);

				// Fade out Loader
				if (baseCamera != null) 
				{
					baseCamera.FadeOut (0.25f);
					yield return new WaitForSeconds (baseCamera.fadeDuration);
				}

				async.allowSceneActivation = true;
			}
			
			yield return null;
		}
	}
	#endregion
}