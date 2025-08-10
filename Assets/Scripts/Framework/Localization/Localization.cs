using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Delegates;

namespace Framework.Localization
{
	/// <summary>
	/// Localization.
	/// 
	/// Main class for language localization services.
	/// The system works by downloading Google spreadsheets with localization keys & translations & making them 
	/// availabe as dictionaries to Unity components.
	/// 
	/// By Jorge L. Chávez Herrera.
	/// </summary>
	public class Localization
	{
		#region Class members
		static public SimpleDelegate onLanguageChanged;
		static private string currentLanguage = "ES";
		#endregion
	
		#region Class accessors

		/// <summary>
		/// Gets the localization data.
		/// </summary>
		/// <value>The localization data.</value>
		private static SerializableDictionary<string, SerializableDictionary<string,SerializableDictionary<string,string>>> _localizationData;
		private static SerializableDictionary<string, SerializableDictionary<string,SerializableDictionary<string,string>>> localizationData
		{
			get 
			{
				if (_localizationData == null)
					_localizationData = LoadLocalizationData ();
						
				return _localizationData;
			}
		}
		#endregion

		#region Class implementation
		/// <summary>
		/// Switchs the language.
		/// </summary>
		/// <param name="languageCode">Language code.</param>
		static public void SwitchLanguage (string languageCode)
		{
			currentLanguage = languageCode;

			if (onLanguageChanged != null)
				onLanguageChanged ();
		}

		/// <summary>
		/// Gets a localized string.
		/// </summary>
		/// <returns>The localized string.</returns>
		/// <param name="key">Key.</param>
		/// <param name="sheet">Sheet.</param>
		static public string GetLocalizedString (string key, string sheet)
		{
			if (localizationData.ContainsKey (sheet))
				if (localizationData[sheet].ContainsKey (currentLanguage))
					if (localizationData[sheet][currentLanguage].ContainsKey (key))
						return localizationData[sheet][currentLanguage][key];

			return "";
			//return key + " ?";
		}

		/// <summary>
		/// Loads the localization data, returns nested dictionaries for Localization sheets, languages & keys.
		/// </summary>
		/// <returns>The localization data.</returns>
		static private SerializableDictionary<string, SerializableDictionary<string,SerializableDictionary<string,string>>> LoadLocalizationData()
		{
			SerializableDictionary<string, SerializableDictionary<string,SerializableDictionary<string,string>>> result = new SerializableDictionary<string, SerializableDictionary<string,SerializableDictionary<string,string>>> ();

			// Read Json data from file
			string json = Resources.Load<TextAsset>("LocalizationData").text;
			SerializableDictionary<string,string> serializedDict = JsonUtility.FromJson<SerializableDictionary<string,string>>(json);

			result = new SerializableDictionary<string, SerializableDictionary<string,SerializableDictionary<string,string>>> ();

			// Deserialize sheets within data
			foreach (KeyValuePair<string,string> kv in serializedDict) 
			{
				SerializableDictionary<string, string> dict = JsonUtility.FromJson<SerializableDictionary<string,string>> (kv.Value); 

				SerializableDictionary<string, SerializableDictionary<string, string>> languagesDict = new SerializableDictionary<string, SerializableDictionary<string, string>> ();

				result.Add (kv.Key, languagesDict);

				// Deserialize languages within a sheet dict
				foreach (KeyValuePair<string,string> kv2 in dict) 
				{
					// Deserialize language dict a column at a time.
					SerializableDictionary<string, string> languageDict = JsonUtility.FromJson<SerializableDictionary<string,string>> (kv2.Value); 

					languagesDict.Add (kv2.Key, languageDict);
				}
			}

			return result;
		}


		static public void DumpSheetKeys (string sheet)
		{
			if (localizationData.ContainsKey (sheet)) 
			{
				foreach (string key in localizationData[sheet].Keys)
					Debug.Log (key);
			}
		}

		/// <summary>
		/// Returns all keys of a sheet.
		/// </summary>
		/// <param name="sheet">Sheet.</param>
		static public List<string> GetSheetKeys (string sheet)
		{
			List<string> sheetKeys = new List<string> ();

			if (localizationData.ContainsKey (sheet)) 
			{
				if (localizationData[sheet].ContainsKey (currentLanguage))
					foreach (string key in localizationData[sheet][currentLanguage].Keys)
						sheetKeys.Add (key);
			}

			return sheetKeys;
		}
		#endregion
	}

	[System.Serializable]
	public struct LocalizationInfo
	{
		[Header("Localization Info")]
		public string key;
		public string sheet;
	}
}