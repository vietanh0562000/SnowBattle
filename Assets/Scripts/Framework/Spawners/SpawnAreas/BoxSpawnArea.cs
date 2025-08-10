using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawnArea : SpawnArea {

    #region Class members
    [SerializeField] private Vector3 size = Vector3.one;
    #endregion

    public void OnDrawGizmos()
    {
        Gizmos.matrix = cachedTransform.localToWorldMatrix;
        // Draw inner spawn radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(Vector3.zero, size);
    }

    override public Vector3 GetSpawnPoint()
    {
        Vector3 pos = new Vector3(
            Random.Range(-size.x / 2, size.x / 2),
            Random.Range(-size.y / 2, size.y / 2),
            Random.Range(-size.z / 2, size.z / 2)
            );

        return cachedTransform.TransformPoint(pos);

    }

}
