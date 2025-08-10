using UnityEngine;
using System.Collections.Generic;

namespace Framework.Avatar
{
    /// <summary>
    /// Avatar Skin Variations Renderer.
    /// Implements variations for all kinds of renderers.
    /// 
    /// By Jorge L. Chávez Herrera.
    /// </summary>
    public class AvatarSkinVariationsRenderer : AvatarSkinVariationsBase
    {
        #region Class members
        public GameObject[] gameObjects;
        public Transform variationOffPivot;
        public AvatarSkinVariationsRenderer parentVariation;
        [SerializeField] private AvatarSkinVariationsRenderer colorLink;
        public string parentPivotName;
        public Transform constraint;
        public Vector3 rotationOffset;

        private List<AvatarSkinVariationsRenderer> childrenVariations = new List<AvatarSkinVariationsRenderer>(); 
        private List<AvatarSkinVariationsRenderer> colorDependants = new List<AvatarSkinVariationsRenderer>(); 
        private Transform parentPivotTransform;
        #endregion

        #region Class accesors
        override public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                SetColor(_color);

                // Apply color to all dependants
                for (int i = 0; i < colorDependants.Count; i++)
                    colorDependants[i].Color = _color;
            }
        }

        override public int Count
        {
            get { return (int)gameObjects.Length; }
        }
        #endregion

        #region MonoBehaviour overrides
        private void Awake()
        {
            if (parentVariation)
                parentVariation.AddChildren(this);

            if (colorLink)
                colorLink.AddColorDependant(this);
        }

        private void LateUpdate()
        {
            if (parentVariation != null && constraint != null && Variation > -1 && parentPivotTransform != null)
            {
                gameObjects[Variation].transform.localPosition = parentPivotTransform.localPosition;
                gameObjects[Variation].transform.localRotation = parentPivotTransform.localRotation * Quaternion.Euler(rotationOffset);
                gameObjects[Variation].transform.localScale = parentPivotTransform.localScale;
            }
        }
        #endregion

        #region Base class overrides
        override protected void Toggle(int v)
        {
            // Toggle GameObject.
            for (int i = 0; i < gameObjects.Length; i++)
                gameObjects[i].SetActive(i == v);

            if (parentVariation != null && constraint != null && v > -1)
            {
                gameObjects[v].transform.SetParent(constraint, false);
                // Find parent pivot's transform
                if (parentVariation.Variation > -1)
                    parentPivotTransform = parentVariation.gameObjects[parentVariation.Variation].transform.Find(parentPivotName);
                else
                    parentPivotTransform = parentVariation.variationOffPivot;
            }

            for (int i = 0; i < childrenVariations.Count; i++)
                childrenVariations[i].Toggle(childrenVariations[i].Variation);

            if (v > -1)
                SetColor(Color);
        }

        override protected void SetColor(Color color)
        {
            // Set renderers color.
            if (Variation < gameObjects.Length && Variation > -1)
            {
                Renderer[] renderers = gameObjects[Variation].GetComponentsInChildren<Renderer>(true);

                for (int r = 0; r < renderers.Length; r++)
                {
                    for (int m = 0; m < renderers[r].materials.Length; m++)
                        renderers[r].materials[m].color = color;
                }
            }
        }

        public void AddChildren(AvatarSkinVariationsRenderer child)
        {
            childrenVariations.Add(child);
        }

        public void AddColorDependant(AvatarSkinVariationsRenderer child)
        {
            colorDependants.Add(child);
        }
        #endregion
    }
}
