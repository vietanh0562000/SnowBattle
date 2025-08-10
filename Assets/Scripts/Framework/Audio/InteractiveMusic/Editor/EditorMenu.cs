using UnityEngine;
using UnityEditor;
using System.Collections;
using Framework.EditorUtils;

namespace Framework.Audio 
{
	public class EditorMenu : EditorWindow 
	{
		#if UNITY_EDITOR
      
		#region Class implementation
		[MenuItem("Assets/Create/NciteFramework/Audio/InteractiveMusic")]
		static public void CreateSpreadSheetData()
		{
            ScriptableObjectUtility.CreateAsset<InteractiveMusic>();
		}
		#endregion

		#endif
	}
}

