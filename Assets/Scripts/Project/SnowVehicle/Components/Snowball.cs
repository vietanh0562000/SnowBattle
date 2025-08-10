using System;
using UnityEngine;
using Framework.Pooling;
using Framework.Utils;


public class Snowball : OptimizedGameObject
{
    #region Class members
    [SerializeField] private PoolManaged explosionPrefab;
    [SerializeField] private Transform meshTransform;
    [NonSerialized] public SnowVehicle snowVehicle;
    [NonSerialized] public bool isAttached;

    private Vector3 prevPosition;
    private static PrefabPool explosionPool;
    private float size;
    #endregion

    #region Class accessors
    public float Size
    {
        get { return cachedTransform.lossyScale.magnitude * 0.25f;}
    }
    #endregion

    #region MonoBehaviour overrides
    private void Awake()
    {
        explosionPool = PoolManager.CreatePoolForPrefab(explosionPrefab);
        cachedPoolManaged.onSpawn += OnSpawn;
        cachedPoolManaged.onDespawn += OnDespawn;
    }

    private void Update()
    {
        // Rotate according to x,z velocity.
        Vector3 delta = Vector3.Scale(cachedTransform.position - prevPosition, new Vector3(1,0,1));

        float rotX = delta.magnitude * Time.deltaTime * 3000;   
        meshTransform.Rotate(rotX, 0, 0, Space.Self);
        prevPosition = cachedTransform.position;

        // Scale according to x,z velocity.
        if (snowVehicle.cachedCharacterController.isGrounded && isAttached)
        {
            //size = Mathf.Clamp01(size + (delta.magnitude * (snowVehicle.snowballGrowSpeed) * Time.deltaTime));
            size = Mathf.Clamp01(size + (snowVehicle.snowballGrowSpeed * snowVehicle.acceleration.Value * Time.deltaTime));
            float scale = size * GameManager.Settings.data.snowballMaxSize;

            cachedTransform.localScale = Vector3.one * scale;
            cachedTransform.localPosition = new Vector3(0, scale / 2.0f, 0.5f + (scale / 2.0f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore collisions with ground.
        if (other.CompareTag("Ground"))
            return;
        
        // Ge destroyed on collision with pit.
        if (other.CompareTag("Pit"))
        {
            //GetDestroyed();
            cachedPoolManaged.Despawn();
            return;
        }

        // Ignore collisions with snow vehicle spawner.
        if (other.gameObject == cachedPoolManaged.spawner)
            return;
        
        // Find out if collided with another snowball
        if (other.CompareTag("Snowball"))
        {
            Snowball otherSnowball = other.GetComponent<Snowball>();
            TakeSnowballHit(otherSnowball);
            return;
        }

        // Hitting other snow vehicle
        if (other.CompareTag("Player") || other.CompareTag("NPC"))
        {
            SnowVehicle otherSnowVehicle = other.GetComponent<SnowVehicle>();
            otherSnowVehicle.TakePerfectHit(this);

            // Despawn this snowball.
            cachedPoolManaged.Despawn();
        }
    }
    #endregion

    #region Class implentation

    private void TakeSnowballHit(Snowball otherSnowball)
    { 
        if (cachedTransform.parent)
            GetComponentInParent<SnowVehicle>().TakeSnowballHit(otherSnowball);
       
        cachedPoolManaged.Despawn();
    }

    #endregion

    #region Class implentation
    private void OnSpawn()
    {
        size = 0;
        cachedTransform.localScale = Vector3.zero;
        cachedRigidbody.linearVelocity = Vector3.zero;
        prevPosition = cachedTransform.position;
        isAttached = true;
    }

    private void OnDespawn()
    {
        // Spawn smaller snowball particles.
        PoolManaged explosion = explosionPool.SpawnAt(cachedTransform.position, Quaternion.identity, null, gameObject);
        explosion.GetComponent<ParticleSystem>().Play();

        // Notify spawner snow vehicle this snowball has been destroyed.
        if (snowVehicle.snowball == this)
            snowVehicle.snowball = null;
    }
    #endregion
}
