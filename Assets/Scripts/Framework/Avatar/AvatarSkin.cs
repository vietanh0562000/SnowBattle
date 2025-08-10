using System.Collections.Generic;
using UnityEngine;
using System;
using Framework.Utils;


namespace Framework.Avatar
{
    /// <summary>
    /// Avatar skin.
    /// Defines a data structure for holding avatar skins.
    /// Avatar skins aere just a collection of AvatarFeatures.
    /// 
    /// By Jorge L. Chávez Herrera.
    /// </summary>
    public class AvatarSkin : ScriptableObject
    {
        #region Class members
        public List<AvatarSkinFeature> features;
        #endregion

        #region Class accessors
        /// <summary>
        /// Gets the default skin loaded from resources.
        /// </summary>
        /// <value>The default.</value>
        static public AvatarSkin Default
        {
            get { return Resources.Load<AvatarSkin>("Avatar Skin"); }
        }
        #endregion

        #region Class implementation

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        public void Init()
        {
            for (int i = 0; i < features.Count; i++)
                features[i].Init();
        }

        /// <summary>
        /// Gets the serialized features.
        /// </summary>
        /// <returns>The serialized features.</returns>
        public string GetSerializedFeatures()
        {
            List<Color32> color32List = new List<Color32>();

            // Pack skin each skin feature into a 4 byte Color32 struct, color RGB
            // values are converted from floats while apha is used to store
            // the varation index.
            foreach (AvatarSkinFeature avatarFeatureData in features)
            {
                Color32 packedSkinFeature = avatarFeatureData.Color;
                // Bytes won't accept negative values, to store the aproppiate value
                // for indexes (which can be -1 if feature is off) we simply increment,
                // then drecemnet at unpack time.
                packedSkinFeature.a = (byte)(avatarFeatureData.VariationIndex + 1);
                color32List.Add(packedSkinFeature);
            }
            
            byte[] byteArray = Conversion.Color32ListToByteArray(color32List);

            return Convert.ToBase64String(byteArray);
        }

        /// <summary>
        /// Sets the serialized features.
        /// </summary>
        /// <param name="serializedFeatures">Serialized features.</param>
        public void SetSerializedFeatures(string serializedFeatures)
        {
            byte[] byteArray = Convert.FromBase64String(serializedFeatures);
            List<Color32> color32List = Conversion.Color32ListFromByteArray(byteArray);

            // Unpack skin features from a 4 byte Color32 struct, color RGB
            // values are converted from bytes to floats while variation
            // index is rtreived from the color alpha value.
            for (int i = 0; i < features.Count; i++)
            {
                Color32 packedSkinFeature = color32List[i];
                // Bytes won't accept negative values, to store the aproppiate value
                // for indexes (which can be -1 if feature is off) we simply increment,
                // then drecemnet at unpack time.
                features[i].VariationIndex = packedSkinFeature.a - 1;
                packedSkinFeature.a = 1;
                features[i].Color = packedSkinFeature;
            }
        }

        /// <summary>
        /// Gets the feature specified by name.
        /// </summary>
        /// <returns>The feature.</returns>
        /// <param name="name">Name.</param>
        public AvatarSkinFeature GetFeature (string name)
        {
            foreach (AvatarSkinFeature feature in features)
                if (feature.name == name)
                    return feature;

            return null;
        }

        /// <summary>
        /// Gets the index of the feature.
        /// </summary>
        /// <returns>The feature index.</returns>
        /// <param name="name">Name.</param>
        public int GetFeatureIndex(string name)
        {
            for (int i = 0; i < features.Count; i++)
                if (features[i].name == name)
                    return i;

            return -1;
        }
       
        #endregion
    }
}