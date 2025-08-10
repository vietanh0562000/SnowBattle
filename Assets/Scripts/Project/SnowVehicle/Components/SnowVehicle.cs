using UnityEngine;
using Framework.Pooling;
using Framework.Utils;
using Framework.Tweening;
using System.Collections;
using MoreMountains.NiceVibrations;

/// <summary>
/// Snow Vehicle.
/// 
/// Defines a base behaviour for snow vehicles.
/// 
/// By Jorge L. Chávez Herrera.
/// </summary>
public class SnowVehicle : OptimizedGameObject
{
    #region Class members
    public Sprite freezeIcon;
    // Exposed.
    [SerializeField] private PoolManaged trailsPrefab;
    [SerializeField] private PoolManaged snowballPrefab;
    [SerializeField] private PoolManaged infoLabelsPrefab;

    [SerializeField] private Transform labelsPivot;
    [SerializeField] private Transform modelTransform;
    [SerializeField] private ParticleSystem perfectHitParticleSystem;
    public Transform ice;
    // Settings.
    [System.NonSerialized] public float size;
    [System.NonSerialized] public float speed;
    [System.NonSerialized] public float punchForce;
    [System.NonSerialized] public float perfectPunchForce;
    [System.NonSerialized] public float snowballGrowSpeed;
    [System.NonSerialized] public float killSizeIncrement;
    [System.NonSerialized] public float killPunchForceIncrement;
    [System.NonSerialized] public float killSpeedIncrement;
    [System.NonSerialized] public float killSnowBallGrowSpeedIncrement;
    // Stats
    [System.NonSerialized] public int kills;
    // Interpolators.
    public FloatSDInterpolator acceleration;
    public AngleSDInterpolator angle;
    public Vector3SDInterpolator knockBackForce;
    // Trails.
    public static PrefabPool trailsPool;
    public Trails trails;
    // Snowball.
    public static PrefabPool snowballPool;
    [System.NonSerialized] public Snowball snowball;
    // Name label.
    private static PrefabPool infoLabelsPool;
    [System.NonSerialized] public InfoLabels infoLabels;
    // Flags
    [System.NonSerialized] public float immunity;
    // Constants.
    private const float GRAVITY = 9.8f;
    // Flags.
    protected bool wasGrounded = false;
    protected float spawnSize = 1;

    private SnowVehicle _lastAttacker;
    private float lastAttackerClearTimer;
    #endregion

    #region Class accessors
    protected float SnowBallSize
    {
        get { return snowball != null ? snowball.Size : 0; }
    }

    public SnowVehicle LastAttacker
    {
        get { return _lastAttacker; }
        set 
        { 
            _lastAttacker = value;
            lastAttackerClearTimer = 3;
        }
    }
    #endregion

    #region MonoBehaviour events
    virtual protected void Awake()
    {
        cachedPoolManaged.onSpawn += OnSpawn;
        cachedPoolManaged.onDespawn += OnDespawn;

        trailsPool = PoolManager.CreatePoolForPrefab(trailsPrefab);
        snowballPool = PoolManager.CreatePoolForPrefab(snowballPrefab);
        infoLabelsPool = PoolManager.CreatePoolForPrefab(infoLabelsPrefab);

        acceleration = new FloatSDInterpolator(0.5f);
        angle = new AngleSDInterpolator(1);
        knockBackForce = new Vector3SDInterpolator(0.5f);

        cachedFSM.AddState("Patrol", new SV_NPCStatePatrol(this));
        cachedFSM.AddState("Frozen", new SV_StateFrozen(this));
        cachedFSM.AddState("Drowned", new SV_StateDrowned(this));
    }

    virtual protected void Update()
    {
        // Rotate 
        if (!cachedFSM.CurrentStateIs("Frozen"))
            cachedTransform.eulerAngles = new Vector3(0, angle.Value, 0);

        // Move
        Vector3 forwardVector = cachedCharacterController.isGrounded ? cachedTransform.forward : Vector3.zero;
        // Apply acceleration & speed
        forwardVector *= acceleration.Value * speed;
        // Apply knock back
        Vector3 moveDirection = forwardVector + knockBackForce.Value;
        // Apply gravity.
        if (!cachedFSM.CurrentStateIs("Drowned"))
            moveDirection.y = moveDirection.y - GRAVITY;
        // Move the character controller.
        cachedCharacterController.Move(moveDirection * Time.deltaTime);

        // Size
        cachedTransform.localScale = Vector3.one * size * spawnSize;

        // Just landed, spawn trails.
        if (cachedCharacterController.isGrounded && wasGrounded == false && trails == null)
            CreateTrails();

        // Just launched on air, stop creating trails.
        if (cachedCharacterController.isGrounded == false && trails != null)
            DetachTrails();

        immunity -= Time.deltaTime;

        wasGrounded = cachedCharacterController.isGrounded;

        // Clear last attacker
        lastAttackerClearTimer -= Time.deltaTime;
        if (lastAttackerClearTimer <= 0)
            _lastAttacker = null;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Ice"))
        {
            if (!cachedFSM.CurrentStateIs("Frozen") && immunity <= 0)
                cachedFSM.SetState("Frozen");
        }
    }
    #endregion

    #region Class implementation

    /// <summary>
    /// Gathers the settings from GameManager.
    /// </summary>
    virtual protected void GatherSettings()
    {}

    public void GrowUp(int count)
    {
        if (!gameObject.activeSelf) 
            return;

        TweenUtils.TweenFloat(this, size, size + (killSizeIncrement * count), 0.5f, 0, ETweenType.BounceOut, (value) => { size = value; }, null);
        punchForce += (killPunchForceIncrement * count);
        speed += (killSpeedIncrement * count);
        //snowballGrowSpeed += (killSnowBallGrowSpeedIncrement * count);
    }

    public void ApplyBuff(Buff buff)
    {
        infoLabels.ShowBuffInfo(true, buff.color, buff.icon);

        switch (buff.name)
        {
            case "Speed Buff": StartCoroutine(SpeedBuffCoroutine(GameManager.Settings.data.buffDuration)); break;
            case "Punch Force Buff": StartCoroutine(PunchForceBuffCoroutine(GameManager.Settings.data.buffDuration));break;
            case "Size Buff": StartCoroutine(SizeBuffCoroutine(GameManager.Settings.data.buffDuration));break;
        }   
    }

    private void BuffProgressUpdate(float value)
    {
        infoLabels.SetBuffProgress(value);
    }

    private IEnumerator SpeedBuffCoroutine(float duration)
    {
        float originalSpeed = speed;

        StartCoroutine(TweenUtils.TweenFloat(1, 0, GameManager.Settings.data.buffDuration, 0, ETweenType.NoEase, BuffProgressUpdate));

        for (float t = 0; t <= 0.5f; t+= Time.deltaTime)
        {
            float nt = t / 0.5f;
            speed = Mathf.SmoothStep(originalSpeed, originalSpeed * GameManager.Settings.data.speedBuffMultiplier, nt);
            yield return null;
        } 

        yield return new WaitForSeconds(duration-1);

        for (float t = 0; t <= 0.5f; t+= Time.deltaTime)
        {
            float nt = t / 0.5f;
            speed = Mathf.SmoothStep(originalSpeed * GameManager.Settings.data.speedBuffMultiplier, originalSpeed, nt);
            yield return null;
        } 

        speed = originalSpeed;
        infoLabels.ShowBuffInfo(false, Color.clear, null);
    }

    private IEnumerator PunchForceBuffCoroutine(float duration)
    {
        punchForce = GameManager.Settings.data.punchForceBuffMultiplier;
        yield return new WaitForSeconds(duration);
        punchForce = 1;
    }

    private IEnumerator SizeBuffCoroutine(float duration)
    {
        float originalSize = size;

        StartCoroutine(TweenUtils.TweenFloat(1, 0, GameManager.Settings.data.buffDuration, 0, ETweenType.NoEase, BuffProgressUpdate));

        for (float t = 0; t <= 0.5f; t+= Time.deltaTime)
        {
            float nt = t / 0.5f;
            size = Mathf.SmoothStep(originalSize, originalSize * GameManager.Settings.data.sizeBuffMultiplier, nt);
            size = Mathf.Clamp(size, 0, 2);
            yield return null;
        } 

        yield return new WaitForSeconds(duration-1);

        for (float t = 0; t <= 0.5f; t+= Time.deltaTime)
        {
            float nt = t / 0.5f;
            size = Mathf.SmoothStep(originalSize * GameManager.Settings.data.sizeBuffMultiplier, originalSize, nt);
            size = Mathf.Clamp(size, 0, 2);
            yield return null;
        } 

        size = originalSize;
        infoLabels.ShowBuffInfo(false, Color.clear, null);
    }

    /// <summary>
    /// Spawns a new snow ball.
    /// </summary>
    public void SpawnSnowball()
    {
        snowball = snowballPool.SpawnAt(
            Vector3.forward, Quaternion.identity, 
            cachedTransform, gameObject
        ).GetComponent<Snowball>();

        snowball.snowVehicle = this;
    }

    /// <summary>
    /// Fires the  current snowball.
    /// </summary>
    public void FireSnowball()
    {
        if (snowball == null)
            return;

        snowball.cachedTransform.SetParent (null, true);
        snowball.cachedRigidbody.linearVelocity = cachedTransform.forward * GameManager.Settings.data.snowballSpeed * size;
        snowball.isAttached = false;
        snowball = null;
    }

    // Takes a hit when a snowball touches directly this vehicle.
    public void TakePerfectHit(Snowball otherSnowball)
    {
        LastAttacker = otherSnowball.cachedPoolManaged.spawner.GetComponent<SnowVehicle>();

        if (!cachedFSM.CurrentStateIs("Frozen"))
            acceleration.ResetAndTarget(0, 1);
        else
            acceleration.ResetAndTarget(0, 0);

        KnockBack(otherSnowball.cachedTransform.forward, perfectPunchForce * otherSnowball.Size);

        EnablePerfectHitParticleEmission();
        Invoke("DisablePerfectHitParticleEmission", 0.5f);
        
        if (snowball)
            snowball.cachedPoolManaged.Despawn();

        if (CompareTag("Player"))
        {
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
            Camera.main.GetComponent<CameraController>().Shake(0.25f);
        }
    }

    // Takes a hit from a snowball to our own snowball.
    public void TakeSnowballHit(Snowball otherSnowball)
    { 
        LastAttacker = otherSnowball.cachedPoolManaged.spawner.GetComponent<SnowVehicle>();

        if (!cachedFSM.CurrentStateIs("Frozen")) 
            acceleration.ResetAndTarget(0, 1);
        else
            acceleration.ResetAndTarget(0, 0);

        // Other snowball is still being held by a snow vehicle.
        if (otherSnowball.cachedTransform.parent)
        {
            // Other vehicle's snowball is bigger size, apply full punch force.
            if (otherSnowball.Size > snowball.Size)
                KnockBack(otherSnowball.cachedTransform.forward, punchForce * otherSnowball.Size);
            // Other vehicle's snowball is equal size, apply half punch force.
            else if(Mathf.Abs(otherSnowball.Size - snowball.Size) < Mathf.Epsilon)
                KnockBack(otherSnowball.cachedTransform.forward, punchForce * otherSnowball.Size * 0.5f);
        }
        // Other snowball has been thrown away.
        else
        {
            // Take hit with full punch force.
            KnockBack(otherSnowball.cachedTransform.forward, punchForce * otherSnowball.Size);
        }
    }

    public void KnockBack(Vector3 direction, float force)
    {     
        // Apply hit force only if not knocking back already. 
        if (!cachedCharacterController.isGrounded)
            return;

        Quaternion lookRotaton = Quaternion.LookRotation(direction);
        // Rotate to the opposite direction of the force.
        angle.InstantValue = lookRotaton.eulerAngles.y + 180;
        // Set a value for Y equal to force to make it jump over the air. 
        direction.y = GameManager.Settings.data.punchYAmplitude; 

        knockBackForce.InstantValue = direction * force;
        knockBackForce.targetValue = Vector3.zero;
    }

    public void EnablePerfectHitParticleEmission(bool state)
    {
        var emission = perfectHitParticleSystem.emission;
        emission.enabled = state;
    }

    public void EnablePerfectHitParticleEmission()
    {
        if (!cachedFSM.CurrentStateIs("Frozen"))
            EnablePerfectHitParticleEmission(true);
    }

    private void DisablePerfectHitParticleEmission ()
    {
        EnablePerfectHitParticleEmission(false);
    }

    virtual public void GetKilled()
    {
        cachedFSM.SetState("Drowned");

        if (LastAttacker != null && LastAttacker.infoLabels != null)
        { 
            LastAttacker.kills += kills + 1;
            LastAttacker.infoLabels.SetKills(kills + 1);
            LastAttacker.GrowUp(kills + 1);
        }
  
        UIManager.Instance.HUDPanel.SetKills(PlayerController.Instance.kills);
    }

    virtual protected void OnSpawn()
    {
        GatherSettings();

        kills = 0;
        immunity = 0;
        size = 1;
        cachedTransform.localScale = Vector3.one * size;
        ice.localScale = Vector3.zero;
        acceleration.ResetAndTarget(0, 1);
        knockBackForce.ResetAndTarget(Vector3.zero, Vector3.zero);
        LastAttacker = null;
        DisablePerfectHitParticleEmission();

        wasGrounded = false;
        trails = null;

        infoLabels = infoLabelsPool.SpawnAt(Vector3.zero, Quaternion.identity, UIManager.Instance.HUDPanel.nameLabelsRoot, gameObject).GetComponent<InfoLabels>();
        infoLabels.name = "InfoLabel "+ name;
        infoLabels.SetTrackedTransform(labelsPivot);
    }

    virtual protected void OnDespawn()
    {
        if (snowball)
        {
            snowball.cachedPoolManaged.Despawn();
            snowball = null;
        }

        if (trails)
            DetachTrails();

        Debug.Log("Despawning" + name);

        infoLabels.cachedPoolManaged.Despawn();
        infoLabels = null;
    }

    private void CreateTrails()
    {
        trails = trailsPool.SpawnAt(Vector3.zero, Quaternion.identity, cachedTransform, gameObject).GetComponent<Trails>();
    }

    private void DetachTrails()
    {
        trails.cachedTransform.SetParent(null, true);
        trails.SetEmiting(false);
        trails = null;
    }

    public void UpdateSnowBall()
    {
        if (cachedCharacterController.isGrounded)
        {
            if (snowball == null)
                SpawnSnowball();
        }
    }

    #endregion
}
