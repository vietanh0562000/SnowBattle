using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Framework.Gameplay;

namespace Framework.Utils
{
	public class Loader 
	{
		#region Class implementation
		static public void LoadScene (string name, MonoBehaviour mono)
		{
			mono.StartCoroutine (LoadSceneCoroutine(name));
		}

		static private IEnumerator LoadSceneCoroutine (string name)
		{
			BaseCamera baseCamera = Camera.main.GetComponent<BaseCamera> ();

			if (baseCamera != null) 
			{
				baseCamera.FadeOut (0.25f);
				yield return new WaitForSeconds (baseCamera.fadeDuration);
			}

			LoadingPanel.sceneToLoad = name;
			SceneManager.LoadScene ("Loader");
		}
		#endregion
	}
}
