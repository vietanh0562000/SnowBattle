using System;
using UnityEngine;

namespace Framework.Avatar
{
    /// <summary>
    /// Avatar Skin Feature.
    /// Defines a data structure for holding an avatar skin feature.
    /// Avatar Skin Features are containers holding the index of a variation a color
    /// and settings to deal with the implementation on 3D meshes or 2D Spine models.
    /// 
    /// By Jorge L. Chávez Herrera.
    /// </summary>
    [System.Serializable]
    public class AvatarSkinFeature : ScriptableObject
    {
        #region Class members
        [SerializeField] private int _vartiationIndex = 0;      // Selected variation index.
        [SerializeField] private Color _color = default(Color); // Color tint to apply to this feature.

        public string[] slotNames;            // The names of slots contained in this feature.
        public Sprite[] icons;                // Sprites used as thumbnails for variation selection UI buttons.
        public Sprite[] masks;                // Sprites masks used to tint variation selection UI buttons.
        public bool allowSetColor = true;     // Allow set color for this variation.
        public bool allowSetVariation = true; // Allow set variation for this variation. 
        public bool allowToggleOff = false;   // Enables the ability to toggle off this feature (accessories).
        public int hueMin = 0;                // MNinimum hue range for color customization.
        public int hueMax = 360;              // Maximum hue range for color customization.
        public int hueSteps = 12;             // Number of steps in hue variations for this feature.
        public float saturationMin = 0;       // Minimum saturarion range for color customization.
        public float saturationMax = 1;       // Maximum saturarion range for color customization.
        public float valueMin = 0;            // Minimum value range for color customization.
        public float valueMax = 1;            // Maximum value range for color customization.
        public int saturationValueSteps = 6;  // Number of steps in saturation variations for this feature.
        public int[] hueAdjustmemts;          // Hue adjustments fir micro managing hue steps.
        public bool hasGrayscale;             // Has a grayscale in palette.
        public float grayMin = 0;             // Minimum gray value for color customization.
        public float grayMax = 1;             // Maximum gray value for color customization.
        public AvatarSkinFeature colorLink;   // Hook to link the color of this feature to another one.

		public int[] prices;				  // Prices for individual items

        // Callbacks
        public Action<int> OnVariationIndexChange;
        public Action<Color> OnColorChange;
        #endregion

        #region Class accsessors
        /// <summary>
        /// Gets or sets the index of the variation.
        /// </summary>
        /// <value>The index of the variation.</value>
        public int VariationIndex
        {
            get
            { return _vartiationIndex; }

            set
            {
                _vartiationIndex = value;

                if (OnVariationIndexChange != null)
                    OnVariationIndexChange(_vartiationIndex);
            }
        }

        /// <summary>
        /// Gets or sets the color of the variation.
        /// </summary>
        /// <value>The color.</value>
        public Color Color
        {
            get { return _color; }

            set
            {
                SetColor(value);
            }
        }

        /// <summary>
        /// Sets the color.
        /// </summary>
        /// <param name="color">Color.</param>
        private void SetColor(Color color)
        {
            _color = color;

            if (OnColorChange != null)
                OnColorChange(_color);
        }

        #endregion

        #region Scriptable object events
        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Init()
        {
            if (colorLink != null)
                colorLink.OnColorChange += SetColor;
        }
        #endregion
    } 
}
