using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Framework.Localization 
{
	/// SpreadsheetData 
	/// Representation for a Google Doc spreadsheet.
	/// By Jorge L. Chávez Herrera. 
	public class SpreadsheetData : ScriptableObject 
	{
		#region Nested Classes
		[System.Serializable]
		public class Cell
		{
			public string key;
			public int sheet;
			public string column;
			public int row;
			public string value;

			public Cell (string key, int sheet, string column, int row)
			{
				this.key = key;
				this.sheet = sheet;
				this.column = column;
				this.row = row;
			}
		}
		#endregion

		#region Class members
		public string googleDocKey;
		public List<Cell> cells = new List<Cell> ();
		#endregion
	}
}

	/*

	/// <summary>
	/// Spreadsheet data.
	/// Representation for a Google Doc spreadsheet.
	/// 
	/// By Jorge L. Chávez Herrera.
	/// </summary>
	public class SpreadsheetData  
	{
		private  Dictionary<int, Dictionary<string, string>> _worksheets =  new Dictionary<int, Dictionary<string, string>>();

		/// <summary>
		/// Sets string data for the specified cell at worsheetIndex, row, col
		/// </summary>
		/// <param name="worksheetIndex">Worksheet index.</param>
		/// <param name="row">Row.</param>
		/// <param name="col">Col.</param>
		/// <param name="data">Data.</param>
		public void SetData (int worksheetIndex, uint row, uint col, string data) 
		{

			Dictionary<string, string> cells = null;

			if(_worksheets.ContainsKey(worksheetIndex)) 
			{
				cells = _worksheets[worksheetIndex];
			}
			else 
			{
				cells =  new Dictionary<string, string>();
				_worksheets.Add (worksheetIndex, cells);
			}

			string cellKey = row.ToString() + ":" + col.ToString();

			if (cells.ContainsKey(cellKey)) 
			{
				cells[cellKey] = data;
			} 
			else 
			{
				cells.Add(cellKey, data);
			}

		}

		/// <summary>
		/// Gets string data from the specified cell at worsheetIndex, row, col
		/// </summary>
		/// <returns>The data.</returns>
		/// <param name="worksheetIndex">Worksheet index.</param>
		/// <param name="row">Row.</param>
		/// <param name="col">Col.</param>
		public string GetData(int worksheetIndex, int row, int col) 
		{
			Dictionary<string, string> cells = null;

			if(_worksheets.ContainsKey(worksheetIndex)) 
			{
				cells = _worksheets[worksheetIndex];

				string cellKey = row.ToString() + ":" + col.ToString();

				if(cells.ContainsKey(cellKey)) 
				{
					return cells[cellKey];
				}
			}

			return string.Empty;
		}

		/// <summary>
		/// Gets or sets all worksheets on this spread sheet.
		/// </summary>
		/// <value>The worksheets.</value>
		public Dictionary<int, Dictionary<string, string>> worksheets 
		{
			get 
			{
				return _worksheets;
			}
			set 
			{
				_worksheets = value;
			}
		}
	}
}
*/
