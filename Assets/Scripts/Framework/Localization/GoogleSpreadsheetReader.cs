#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEditor;

namespace Framework.Localization
{
	/// <summary>
	/// Google spreadsheet reader.
	/// 
	/// Provides functionality for downloading google spreadsheet data using gData API v4.
	/// To avoid problems with aotomatic nested dictionary selralization, all dictionaries are manually serialized
	/// before adding them to a parent one.
	/// 
	/// By Jorge L. Chávez Herrera.
	/// </summary>
	public class GoogleSpreadsheetReader
	{
		#region Class members
		public static string API_KEY ="AIzaSyBEH_oZtk8pxMJIRZx1W1TVjyeRKc3ja2c";
		public static string docKey = "10ToUEUOvhBXPNP8zTexNGzHf5uqT1dyH6sz_ZtcfFMg";

		public static string SHEETS_V4_API_START_URL = "https://sheets.googleapis.com/v4/spreadsheets/";
		public static string SHEETS_V4_API_URL_GET_PROPERTIES = "?includeGridData=false&key=";
		#endregion

		#region class implementation
		static private string GetValuesURL (string sheetTitle)
		{
			return "/values/" + sheetTitle + "!a1%3Az4096?key=";
		}

		/// <summary>
		/// Downloads the translations.
		/// </summary>
		/// <param name="docKey">Document key.</param>
		/// <param name="filePath">File path.</param>
		static public void DownloadTranslations (string docKey, string filePath)
		{
			float progress = 0f;
			WWW request = new WWW (SHEETS_V4_API_START_URL + docKey + SHEETS_V4_API_URL_GET_PROPERTIES + API_KEY);

			// Get the sheets catalog
			if (request.error == null)
			{
				while (!request.isDone) 
				{
					EditorUtility.DisplayProgressBar("Caching Document", "Geetind sheet catalog", progress);
					Thread.Sleep (100);
				}

				WorksheetTemplate worksheetTemplate = JsonUtility.FromJson<WorksheetTemplate> (request.text);

				// Create a dictionary for storing ditionaries for every sheet
				SerializableDictionary<string, string> sheetsDict = new SerializableDictionary<string, string> ();

				// Download cell data from each sheet
				foreach (Sheet sheet in worksheetTemplate.sheets) 
				// Sheet sheet = worksheetTemplate.sheets[0];
				{
					request = new WWW (SHEETS_V4_API_START_URL + docKey + GetValuesURL (sheet.properties.title) + API_KEY);

					if (request.error == null) 
					{
						progress = 0f;

						while (!request.isDone) 
						{
							EditorUtility.DisplayProgressBar ("Caching Document", "Getting values for sheet: " + sheet.properties.title, progress);
							Thread.Sleep (100);
							progress += 0.1f;
						}

						// Fix Json formatting since Unity won't serialize nested arrays
						string formattedText = request.text.Replace ("[", "{ \"" + "array" + "\"" + ": [");
						formattedText = formattedText.Replace ("]", "]}");


						formattedText = formattedText.Replace ("\"values\": { \"array\": [", "\"values\" : [");
						formattedText = formattedText.Remove (formattedText.Length - 2);

						// Deserialize re formatted Json
						SheetData sheetData = JsonUtility.FromJson<SheetData> (formattedText);

						if (sheetData != null) 
						{
							// Create languages dict
							SerializableDictionary<string, string> languagesDict = new SerializableDictionary<string, string> ();
							//sheetsDict.Add (sheet.properties.title, languagesDict);

							if (sheetData.values != null) 
							{
								// Extract the language codes from the first row starting con column 1
								for (int col = 1; col < sheetData.values [0].array.Length; col++) 
								{
									SerializableDictionary<string,string> languageDict = new SerializableDictionary<string, string> ();

									// Get the current language code
									string languageCode = sheetData.values [0].array [col];

									if (sheetData.values != null)
									{
										// Fill up current language dictionary with localization keys
										for (int row = 1; row < sheetData.values.Length; row++) 
										{
											if (sheetData.values[row].array != null)
											{
												if (col < sheetData.values [row].array.Length) 
												{
													string key = sheetData.values [row].array [0];
													string value = sheetData.values [row].array [col];
													languageDict.Add (key, value);
												}
												else 
												{
													if (sheetData.values [row].array.Length > 0)
													{
														// Add empty 
														string key = sheetData.values [row].array [0];
														languageDict.Add (key, "");
													}
												}
											}
										}	
									}
										
									// Serialize the dictionary containing all the keys & values for the current language code
									// & add it to the languages dictionary of the current page.
									string serializedLanguageDict = JsonUtility.ToJson (languageDict);
									languagesDict.Add (languageCode, serializedLanguageDict);
								}	
							}

							// Serialize the dictionary containing language codes for the current sheet 
							// & add it to the sheets dictionary.
							string serializedLanguagesDict = JsonUtility.ToJson (languagesDict);
							sheetsDict.Add (sheet.properties.title, serializedLanguagesDict);
						}
					} 
					else
						Debug.Log (request.error);
				}

				// Finally, serialize the dictionary containing all sheets & write the string to disk
				string serializeSheetsDict = JsonUtility.ToJson (sheetsDict);
				System.IO.File.WriteAllText (filePath, serializeSheetsDict);
				AssetDatabase.Refresh ();
			}
			EditorUtility.ClearProgressBar ();
		}

		/// <summary>
		/// Downloads the raw cell data.
		/// </summary>
		/// <param name="docKey">Document key.</param>
		/// <param name="cells">Cells.</param>
		static public void DownloadRawCellData (string docKey, List<SpreadsheetData.Cell> cells)
		{
			float progress = 0f;
			WWW request = new WWW (SHEETS_V4_API_START_URL + docKey + SHEETS_V4_API_URL_GET_PROPERTIES + API_KEY);

			// Get the sheets catalog
			if (request.error == null)
			{
				while (!request.isDone) 
				{
					EditorUtility.DisplayProgressBar("Caching Document", "Getting sheet catalog", progress);
					Thread.Sleep (100);
				}

				WorksheetTemplate worksheetTemplate = JsonUtility.FromJson<WorksheetTemplate> (request.text);

				// Create a dictionary for storing ditionaries for every sheet
				//SerializableDictionary<string, string> sheetsDict = new SerializableDictionary<string, string> ();

				int sheetIndex = 0;

				// Download cell data from each sheet
				foreach (Sheet sheet in worksheetTemplate.sheets) 
				{
					request = new WWW (SHEETS_V4_API_START_URL + docKey + GetValuesURL (sheet.properties.title) + API_KEY);

					if (request.error == null) 
					{
						progress = 0f;

						while (!request.isDone) 
						{
							EditorUtility.DisplayProgressBar ("Caching Document", "Getting values for sheet: " + sheet.properties.title, progress);
							Thread.Sleep (100);
							progress += 0.1f;
						}

						// Fix Json formatting since Unity won't serialize nested arrays
						string formattedText = request.text.Replace ("[", "{ \"" + "array" + "\"" + ": [");
						formattedText = formattedText.Replace ("]", "]}");


						formattedText = formattedText.Replace ("\"values\": { \"array\": [", "\"values\" : [");
						formattedText = formattedText.Remove (formattedText.Length - 2);

						// Deserialize re formatted Json
						SheetData sheetData = JsonUtility.FromJson<SheetData> (formattedText);

						if (sheetData != null) 
						{
							if (sheetData.values != null)
							{
								for (int i = 0; i < cells.Count; i++)
								{
									if (cells [i].sheet == sheetIndex) 
									{
										int columnIndex = char.ToUpper (cells [i].column[0]) - 65;

										string cellValue = "";

										if (cells [i].row-1 < sheetData.values.Length && columnIndex < sheetData.values[cells [i].row-1].array.Length)
											cellValue = sheetData.values[cells [i].row-1].array [columnIndex];

										cells [i].value = cellValue;
									}
								}
							}
						}
					} 
					else
						Debug.Log (request.error);

					sheetIndex++;
				}
			}
			EditorUtility.ClearProgressBar ();
		}
	}
	#endregion
}

#endif
