using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;

public static class FileUtils
{
	public static void CreateFolder(string folderPath) 
	{
		if (!IsFolderExists (folderPath)) 
		{
			Directory.CreateDirectory (GetFullPath (folderPath));
			AssetDatabase.Refresh();
		}
	}

	public static bool IsFolderExists(string folderPath) 
	{
		if (folderPath.Equals (string.Empty)) 
		{
			return false;
		}

		return Directory.Exists (GetFullPath(folderPath));
	}

	private static string GetFullPath (string srcName) 
	{
		if (srcName.Equals (string.Empty))
		{
			return Application.dataPath;
		}

		if (srcName [0].Equals ('/')) {
			srcName.Remove(0, 1);
		}

		return Application.dataPath + "/" + srcName;
	}

}
#endif
