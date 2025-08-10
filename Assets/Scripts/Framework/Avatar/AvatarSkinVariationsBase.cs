using UnityEngine;
using System.Collections;


namespace Framework.Avatar
{
    /// <summary>
    /// Avatar Skin Vriations Base.
    /// Base class for implementing hooks between avatar skins and actual 
    /// 2D or 3D models. 
    /// 
    /// By Jorge L. Chávez Herrera.
    /// </summary>
    public class AvatarSkinVariationsBase : MonoBehaviour
    {
        #region Class accesors
        /// <summary>
        /// Gets or sets the vriation.
        /// </summary>
        /// <value>The vriation.</value>
        protected int _variation;
        virtual public int Variation
        {
            get
            {
                return _variation;
            }
            set
            {
                _variation = value;
                Toggle(_variation);
            }
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        protected Color _color;
        virtual public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                SetColor(_color);
            }
        }

        /// <summary>
        /// Gets variations count.
        /// </summary>
        /// <value>The count.</value>
        virtual public int Count
        {
            get
            {
                return 0;
            }
        }
        #endregion

        #region Class implementation
        virtual protected void Toggle(int v) { }

        virtual protected void SetColor(Color color) { }
        #endregion
    }
}