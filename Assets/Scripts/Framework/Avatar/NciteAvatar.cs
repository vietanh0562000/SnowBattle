using System.Collections.Generic;
using Framework.Utils;
using UnityEngine;
using System;

namespace Framework.Avatar
{
    /// <summary>
    /// Ncite avatar.
    /// 
    /// Defines basic functionality for aplying avatar skins (both 2d and 3d).
    /// 
    /// By Jorge L. Chávez Herrera.
    /// </summary>
    public class NciteAvatar : OptimizedGameObject
    {
        #region Class members
        public AvatarSkin skin;
        public Action OnSkinUpdate;

        private Dictionary<string, AvatarSkinVariationsBase> variationsDict = new Dictionary<string, AvatarSkinVariationsBase>();
        #endregion

        #region MonoBehaviour overrides
        virtual protected void Awake()
        {
            skin.Init();

            // Create a dictionary containing all AvatarVariationBase derived components 
            // in this hierachy, they will be accessed by the matching names 
            // defined in AvatarfFeatureSettings slot names.
            AvatarSkinVariationsBase[] variations = GetComponentsInChildren<AvatarSkinVariationsBase>(true);

            for (int i = 0; i < variations.Length; i++)
                variationsDict.Add(variations[i].name, variations[i]);

            UpdateSkin();
        }
        #endregion

        #region Class implementation
        /// <summary>
        /// Sets the serialized skin.
        /// </summary>
        /// <param name="serializedSkin">Serialized skin.</param>
        public void SetSerializedSkin(string serializedSkin)
        {
            if (!string.IsNullOrEmpty(serializedSkin))
            {
                skin.SetSerializedFeatures(serializedSkin);
                UpdateSkin();
            }
        }

        /// <summary>
        /// Gets the serialized skin.
        /// </summary>
        /// <returns>The serialized skin.</returns>
        public string GetSerializedSkin()
        {
            return skin.GetSerializedFeatures();
        }

        /// <summary>
        /// Sets the variation index of the fature.
        /// </summary>
        /// <param name="feature">Feature.</param>
        /// <param name="variationIndex">Index.</param>
        public void SetFatureVariation(AvatarSkinFeature feature, int variationIndex)
        {
            feature.VariationIndex = variationIndex;

            for (int i = 0; i < feature.slotNames.Length; i++)
                if (variationsDict.ContainsKey(feature.slotNames[i]))
                    variationsDict[feature.slotNames[i]].Variation = variationIndex;
                else
                    Debug.LogWarning("Slot with name: " + feature.slotNames[i] + " was not found in the hierachy");
        }

        /// <summary>
        /// Sets the color of the feature.
        /// </summary>
        /// <param name="feature">Feature.</param>
        /// <param name="color">Color.</param>
        public void SetFeatureColor(AvatarSkinFeature feature, Color color)
        {
            feature.Color = color;

            for (int i = 0; i < feature.slotNames.Length; i++)
                if (variationsDict.ContainsKey(feature.slotNames[i]))
                    variationsDict[feature.slotNames[i]].Color = color;
                else
                    Debug.LogWarning("Slot with name: " + feature.slotNames[i] + " was not found in the hierachy");
        }

        /// <summary>
        /// Gets the feature specified by name.
        /// </summary>
        /// <returns>The feature.</returns>
        /// <param name="variationName">Variation name.</param>
        public AvatarSkinVariationsBase GetFeature(string variationName)
        {
            return variationsDict[variationName];
        }

        /// <summary>
        /// Updates the skin.
        /// </summary>
        public void UpdateSkin()
        {
            for (int i = 0; i < skin.features.Count; i++)
            {
                SetFatureVariation(skin.features[i], skin.features[i].VariationIndex);
                SetFeatureColor(skin.features[i], skin.features[i].Color);
            }

            if (OnSkinUpdate != null)
                OnSkinUpdate();
        }
        #endregion
    }
}
