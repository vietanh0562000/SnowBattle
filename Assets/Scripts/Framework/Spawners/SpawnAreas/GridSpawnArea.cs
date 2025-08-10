using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Spawning
{
    public class GridSpawnArea : SpawnArea
    {
        #region Class members
        [SerializeField] private int columns = 1;
        [SerializeField] private int rows = 1;
        [SerializeField] private int slices = 1;

        [SerializeField] private Vector3 size;

        private Vector3[] positions;
        private int positionIndex;
        #endregion

        #region MonoBehaviour events
        private void Awake()
        {
            InitializePositions();
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Vector3 step = new Vector3(
                 columns > 1 ? 1.0f / (float)(columns - 1) : 0f,
                 rows > 1 ? 1.0f / (float)(rows - 1) : 0f,
                 slices > 1 ? 1.0f / (float)(slices - 1) : 0f
                 );

            Vector3 centerOffset = new Vector3(
               columns > 1 ? 0 : size.x / 2.0f,
               rows > 1 ? 0 : size.y / 2.0f,
               slices > 1 ? 0 : size.z / 2.0f
               );

            for (int s = 0; s < slices; s++)
                for (int c = 0; c < rows; c++)
                    for (int r = 0; r < columns; r++)
                    {
                        Vector3 pos = new Vector3(
                            Mathf.Lerp(-size.x / 2.0f, size.x / 2.0f, r * step.x) + centerOffset.x,
                            Mathf.Lerp(-size.y / 2.0f, size.y / 2.0f, c * step.y) + centerOffset.y,
                            Mathf.Lerp(-size.z / 2.0f, size.z / 2.0f, s * step.z) + centerOffset.z);

                        Gizmos.DrawWireCube(cachedTransform.TransformPoint(pos), Vector3.one);
                    }

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(cachedTransform.position, size);
        }
        #endregion

        #region Supert class overrides
        override public Vector3 GetSpawnPoint()
        {
            Vector3 ret = cachedTransform.TransformPoint(positions[positionIndex]);
            positionIndex = (positionIndex + 1) % positions.Length;

            return ret;
        }
        #endregion

        #region Class implementation
        private void InitializePositions()
        {
            positions = new Vector3[columns * rows * slices];

            int index = 0;

            Vector3 step = new Vector3(
                columns > 1 ? 1.0f / (float)(columns - 1) : 0f,
                rows > 1 ? 1.0f / (float)(rows - 1) : 0f,
                slices > 1 ? 1.0f / (float)(slices - 1) : 0f
                );

            Vector3 centerOffset = new Vector3(
               columns > 1 ? 0 : size.x / 2.0f,
               rows > 1 ? 0 : size.y / 2.0f,
               slices > 1 ? 0 : size.z / 2.0f
               );

            for (int s = 0; s < slices; s++)
                for (int c = 0; c < rows; c++)
                    for (int r = 0; r < columns; r++)
                    {
                        positions[index] = new Vector3(
                            Mathf.Lerp(-size.x / 2.0f, size.x / 2.0f, r * step.x) + centerOffset.x,
                            Mathf.Lerp(-size.y / 2.0f, size.y / 2.0f, c * step.y) + centerOffset.y,
                            Mathf.Lerp(-size.z / 2.0f, size.z / 2.0f, s * step.z) + centerOffset.z);
                        index++;
                    }
        }
        #endregion
    }
}

