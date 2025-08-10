using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Utils;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace Framework.Localization
{
	/// <summary>
	/// Localization settings.
	/// Scriptable object defining the data structure needed to store localization settings.
	/// 
	/// By Jorge L. Chávez Herrera.
	/// </summary>
	public class LocalizationSettings : ScriptableObject 
	{
		#region Class members
		public string localizationDocKey;
		public string localizationFilePath = "Assets/Resources/LocalizationData.txt";

		private const string SETTINGS_ASSET_NAME = "LocalizationSettings";
		private const string LOCALIZATION_SETTINGS_PATH = "Resources/";
		#endregion

		#region Class accessors
		private static LocalizationSettings instance = null;
		public static LocalizationSettings Instance 
		{
			get 
			{
				if (instance == null) 
				{
					instance = Resources.Load (SETTINGS_ASSET_NAME) as LocalizationSettings;

					if (instance == null) 
					{
						// If not found, autocreate the asset object.
						instance = CreateInstance<LocalizationSettings>();

						#if UNITY_EDITOR
						FileUtils.CreateFolder (LOCALIZATION_SETTINGS_PATH);

						string fullPath = Path.Combine(Path.Combine("Assets", LOCALIZATION_SETTINGS_PATH), SETTINGS_ASSET_NAME + ".asset");
						AssetDatabase.CreateAsset(instance, fullPath);
						#endif
					}

				}
				return instance;
			}
		}
		#endregion
	}
}
