using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpawnArea: SpawnArea
{
    #region Class members
    [SerializeField] private float innerSpawnRadius = 0;
    [SerializeField] private float outerSpawnRadius = 1;
    #endregion

    public void OnDrawGizmos()
    {
        Gizmos.matrix = cachedTransform.localToWorldMatrix;
        // Draw inner spawn radius
        Gizmos.color = Color.red;
        CustomGizmos.DrawLineCircle(Vector3.zero, 16, innerSpawnRadius);
        // Draw outer spawn radius
        Gizmos.color = Color.blue;
        CustomGizmos.DrawLineCircle(Vector3.zero, 16, outerSpawnRadius);
    }

    override public Vector3 GetSpawnPoint()
    {
        float angle = Random.value * 360;
        float distance = Random.Range(innerSpawnRadius, outerSpawnRadius);

        Vector3 pos = new Vector3(
            Mathf.Cos(angle * Mathf.PI / 180) * distance,
            0,
            Mathf.Sin(angle * Mathf.PI / 180) * distance
            );

        return cachedTransform.TransformPoint(pos);

    }
}
