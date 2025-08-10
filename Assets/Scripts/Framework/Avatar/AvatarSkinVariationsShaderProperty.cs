using UnityEngine;
using System.Collections;


namespace Framework.Avatar
{
    /// <summary>
    /// Avatar variations shader property.
    /// Hook for avatar variations to shader properties.
    /// 
    /// By Jorge L. Chávez Herrera.
    /// </summary>
    public class AvatarSkinVariationsShaderProperty : AvatarSkinVariationsBase
    {
        #region Class members
        [SerializeField] private string propertyName;
        [SerializeField] private string colorPropertyName;
        [SerializeField] private int _count;
        [SerializeField] private int valueOffset;
        [SerializeField] Renderer[] renderers;
        #endregion

        #region Class accesors
        override public int Count
        {
            get
            {
                return _count;
            }
        }
        #endregion

        #region Class implementation
        override protected void Toggle(int v) 
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                for (int i = 0; i < renderers.Length; i++)
                {
                    Material[] materials = renderers[i].materials;

                    for (int m = 0; m < materials.Length; m++)
                        materials[m].SetFloat(propertyName, Variation + valueOffset);
                }
            }
        }

        override protected void SetColor(Color color)
        {
            if (!string.IsNullOrEmpty(colorPropertyName))
            {
                for (int i = 0; i < renderers.Length; i++)
                {
                    Material[] materials = renderers[i].materials;

                    for (int m = 0; m < materials.Length; m++)
                        materials[m].SetColor(colorPropertyName, color);
                }
            }
        }
        #endregion
    }
}