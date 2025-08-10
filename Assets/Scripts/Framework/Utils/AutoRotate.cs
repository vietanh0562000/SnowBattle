using UnityEngine;
using System.Collections;

namespace Framework.Utils
{
    public class AutoRotate : OptimizedGameObject
    {
        #region Class members

        [SerializeField] private Vector3 speed;

        #endregion

        #region MonoBehaviour events

        private void Update()
        {
            cachedTransform.Rotate(speed.x * Time.deltaTime, speed.y * Time.deltaTime, speed.z * Time.deltaTime);
        }

        #endregion
    }
}