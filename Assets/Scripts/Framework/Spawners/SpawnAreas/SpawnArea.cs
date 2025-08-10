using Framework.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : OptimizedGameObject
{
    virtual public Vector3 GetSpawnPoint()
    {
        return cachedTransform.position;
    }
}
