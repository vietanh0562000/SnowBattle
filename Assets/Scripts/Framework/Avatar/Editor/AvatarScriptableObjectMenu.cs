using UnityEditor;
using Framework.EditorUtils;
using Framework.Avatar;

/// <summary>
/// AvatarScriptableObjectMenu.
/// 
/// Editor classe to add menu creation options to inherited sciptable object types.
/// 
/// By Jorge L. Chávez Herrera.
/// </summary>
public class AvatarScriptableObjectMenu
{
	#region Class implementation
    [MenuItem("Assets/Create/Ncite/Avatar/Avatar Skin Feature")]
    static public void CreateAvatarSkinFeature()
    {
        ScriptableObjectUtility.CreateAsset<AvatarSkinFeature>();
    }

	[MenuItem("Assets/Create/Ncite/Avatar/Avatar Skin")]
	static public void CreateAvatarSkin ()
	{
		ScriptableObjectUtility.CreateAsset<AvatarSkin>();
	}
	#endregion
}