using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Localization
{
	/// <summary>
	/// Utility classes for deserializing Google spreadshet data.
	/// 
	/// By Jorge L. Chávez Herrera.
	/// </summary>
	[System.Serializable]
	public class SpreadsheetProperties
	{
		public string title;
	}

	[System.Serializable]
	public class GridProperties
	{
		public int rowCount;
		public int columnCount;
	}

	[System.Serializable]
	public class SheetProperties
	{
		public int sheetId;
		public string title;
		public int index;
		public string sheetType;
		public GridProperties gridProperties;
	}

	[System.Serializable]
	public class StringArrayWrapper
	{
		public string[] array;
	}

	[System.Serializable]
	public class SheetData
	{
		public string range;
		public string majorDimension;
		public StringArrayWrapper[] values;
	}

	[System.Serializable]
	public class Sheet
	{
		public SheetProperties properties;
	}

	[System.Serializable]
	public class WorksheetTemplate 
	{
		public string spreadsheetId;
		public SpreadsheetProperties properties;
		public Sheet[] sheets;
	}
}
