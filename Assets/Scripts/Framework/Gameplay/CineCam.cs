using System.Collections;
using System.Collections.Generic;
using Framework.Utils;
using UnityEngine;

namespace Framework.Gameplay
{
    /// <summary>
    /// CineCam.
    /// 
    /// Defines a cinematic camera that is manipulated using polar coordinates 
    /// arount it's view target(s). 
    /// It has a set of interpolation functions for smooth transitions.
    /// 
    /// By Jorge L. Chávez Herrera.
    /// </summary>
    public class CineCam : BaseCamera
    {
        #region Class members
        // targets
        public GameObject[] targets;
        private Transform proxyTarget;
        private Vector3SDInterpolator proxyTargetPositionInterpolator = new Vector3SDInterpolator(0);

        // Target offset
        public Vector3 targetOffset;
        private Vector3SDInterpolator targetOffsetInterpolator = new Vector3SDInterpolator(0);

        // Polar position
        public PolarCoords position;
        private PolarSDInterpolator positionInterpolator = new PolarSDInterpolator(0);

        // Field of view
        private float fieldOfView = 60;
        private FloatSDInterpolator fieldOfViewInterpolator = new FloatSDInterpolator(0);

        // Singleton reference 
        static private CineCam _instance;

        // Camera shots
        private Dictionary<string, ShotData> shotsDictionary = new Dictionary<string, ShotData>();
        #endregion

        #region Class accessors
        /// <summary>
        /// Gets the camera target.
        /// </summary>
        /// <value>The target.</value>
        public GameObject Target
        {
            get { return proxyTarget.gameObject; }
        }

        static public CineCam Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets or sets the shot with the specified key.
        /// </summary>
        /// <param name="key">Key.</param>
        public ShotData this[string key]
        {
            get { return shotsDictionary[key]; }
            set
            {
                shotsDictionary[key] = value;
            }
        }
        #endregion

        #region MonoBehaviour events
        private void Awake()
        {
            _instance = this;

            // Create & initialize internal target.
            proxyTarget = new GameObject("Proxy Target").transform;
            proxyTarget.position = GetBounds(targets).center;

            // Initialize interpolators.
            targetOffsetInterpolator.InstantValue = targetOffset;
            positionInterpolator.InstantValue = position;
            fieldOfViewInterpolator.InstantValue = fieldOfView;
            proxyTargetPositionInterpolator.InstantValue = proxyTarget.position;
        }

        override protected void LateUpdate()
        {
            base.LateUpdate();

            // Update field of view.
            fieldOfView = fieldOfViewInterpolator.Value;
            cachedCamera.fieldOfView = fieldOfView;

            // Update target offset.
            targetOffset = targetOffsetInterpolator.Value;

            // Update target.
            Bounds targetBounds = GetBounds(targets);
            proxyTargetPositionInterpolator.targetValue = targetBounds.center;
            proxyTarget.position = proxyTargetPositionInterpolator.Value;

            // Update position.
            position = positionInterpolator.Value;

            // Multiple target filed of view computation override.
            if (targets != null && targets.Length > 1)
            {
                // Compute camera distance based of field of view to fit all targets in.
                float cameraDistance = 1.0f; // Constant factor
                Vector3 objectSizes = targetBounds.max - targetBounds.min;
                float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);
                float cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * cachedCamera.fieldOfView); // Visible height 1 meter in front
                float distance = cameraDistance * objectSize / cameraView; // Combined wanted distance from the object
                distance += 0.5f * objectSize; // Estimated offset from the center to the edge of the object

                // Override camera distance 
                proxyTarget.position = targetBounds.center;
                position.radius = distance;
            }

            // Update transform position
            Vector3 newPosition = proxyTarget.TransformPoint(targetOffset + position.ToCartesian());

            if (newPosition.IsNan() == false)
                cachedTransform.position = newPosition;

            // Update aiming to target + offset (Prevent Quaternions with rotation vector zero).
            Vector3 lookRotation = proxyTarget.TransformPoint(targetOffset) - cachedTransform.position;

            if (lookRotation != Vector3.zero && lookRotation.IsNan() == false)
                cachedTransform.rotation = Quaternion.LookRotation(lookRotation);
        }

        private void OnDestroy()
        {
            if (proxyTarget)
                Destroy(proxyTarget.gameObject);
        }
        #endregion

        #region Base class overrides
        #endregion

        #region Class implementation
        /// <summary>
        /// Gets the centre of multiple gameObject transforms.
        /// </summary>
        /// <returns>The bounds.</returns>
        /// <param name="gameObjects">Game objects.</param>
        private static Bounds GetBounds(GameObject[] gameObjects)
        {
            if (gameObjects == null || gameObjects.Length == 0)
                return new Bounds();

            if (gameObjects.Length == 1)
                return new Bounds(gameObjects[0].transform.position, Vector3.zero);

            Bounds bounds = new Bounds(gameObjects[0].transform.position, Vector3.zero);

            for (int i = 1; i < gameObjects.Length; i++)
                bounds.Encapsulate(gameObjects[i].transform.position);

            return bounds;
        }

        /// <summary>
        /// Moves smoothly to the new position with the specified duration.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="smoothTime">Duration.</param>
        public void MoveTo(PolarCoords position, float smoothTime)
        {
            positionInterpolator.targetValue = position;
            positionInterpolator.smoothTime = smoothTime;
        }

        /// <summary>
        /// Sets the target offset.
        /// </summary>
        /// <param name="targetOffset">Target offset.</param>
        /// <param name="smoothTime">Smooth time.</param>
        public void SetTargetOffset(Vector3 targetOffset, float smoothTime)
        {
            targetOffsetInterpolator.targetValue = targetOffset;
            targetOffsetInterpolator.smoothTime = smoothTime;
        }

        /// <summary>
        /// Zooms to the new FOV with the specified duration.
        /// </summary>
        /// <param name="fov">Fov.</param>
        /// <param name="smoothTime">Duration.</param>
        public void ZoomTo(float fov, float smoothTime)
        {
            fieldOfViewInterpolator.targetValue = fov;
            fieldOfViewInterpolator.smoothTime = smoothTime;
        }

        /// <summary>
        /// Adds a shot with no defined target.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="targetOffset">Target offset.</param>
        /// <param name="position">Position.</param>
        /// <param name="fieldOfView">Field of view.</param>
        public void AddShot(string name, Vector3 targetOffset, PolarCoords position, float fieldOfView)
        {
            shotsDictionary[name] = new ShotData(null, targetOffset, position, fieldOfView);
        }

        /// <summary>
        /// Adds a shot with a single target.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="target">Target.</param>
        /// <param name="targetOffset">Target offset.</param>
        /// <param name="position">Position.</param>
        /// <param name="fieldOfView">Field of view.</param>
        public void AddShot(string name, GameObject target, Vector3 targetOffset, PolarCoords position, float fieldOfView)
        {
            shotsDictionary[name] = new ShotData(new GameObject[] { target }, targetOffset, position, fieldOfView);
        }

        /// <summary>
        /// Adds a shot with multiple targets.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="targets">Targets.</param>
        /// <param name="targetOffset">Target offset.</param>
        /// <param name="position">Position.</param>
        /// <param name="fieldOfView">Field of view.</param>
        public void AddShot(string name, GameObject[] targets, Vector3 targetOffset, PolarCoords position, float fieldOfView)
        {
            shotsDictionary[name] = new ShotData(targets, targetOffset, position, fieldOfView);
        }

        /// <summary>
        /// Sets current shot by name.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="smoothTime">Duration.</param>
        /// <param name="delay">Delay.</param>
        public void SetShot(string name, float smoothTime, float delay)
        {
            if (shotsDictionary.ContainsKey(name))
                SetShotData(shotsDictionary[name], null, smoothTime, delay);
            else
                Debug.LogWarning(name + ": No such camera shot exists.");
        }

        /// <summary>
        /// Sets current shot by name using the specified target.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="newTarget">Target.</param>
        /// <param name="smoothTime">Duration.</param>
        /// <param name="delay">Delay.</param>
        public void SetShot(string name, GameObject newTarget, float smoothTime, float delay)
        {
            if (shotsDictionary.ContainsKey(name))
                SetShotData(shotsDictionary[name], new GameObject[] { newTarget }, smoothTime, delay);
            else
                Debug.LogWarning(name + ": No such camera shot exists.");
        }

        /// <summary>
        /// Sets a shot by its name using multiple targets.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="newTargets">New targets.</param>
        /// <param name="smoothTime">Duration.</param>
        /// <param name="delay">Delay.</param>
        public void SetShot(string name, GameObject[] newTargets, float smoothTime, float delay)
        {
            if (shotsDictionary.ContainsKey(name))
                SetShotData(shotsDictionary[name], newTargets, smoothTime, delay);
            else
                Debug.LogWarning(name + ": No such camera shot exists.");
        }

        /// <summary>
        /// Returns wheter the shot ispecified exists.
        /// </summary>
        /// <returns><c>true</c>, if shot was containsed, <c>false</c> otherwise.</returns>
        /// <param name="shotName">Shot name.</param>
        public bool ContainsShot(string shotName)
        {
            return shotsDictionary.ContainsKey(shotName);
        }

        /// <summary>
        /// Clears all shots.
        /// </summary>
        public void ClearShots()
        {
            shotsDictionary.Clear();
        }

        /// <summary>
        /// Sets the current shot data, if other than null, target array is overrided.
        /// Keep in mind that when mutiple targets are speficied, the radius value of  
        /// position is overrided to allow camera find the distance to targets based 
        /// on field of view. 
        /// </summary>
        /// <param name="shotData">Shot data.</param>
        /// <param name="newTargets">New targets.</param>
        /// <param name="smoothTime">Smooth time.</param>
        /// <param name="delay">Delay.</param>
        private void SetShotData (ShotData shotData, GameObject[] newTargets, float smoothTime, float delay)
        {
            StartCoroutine(SetShotDataCoroutine(shotData, newTargets, smoothTime, delay));
        }

        private IEnumerator SetShotDataCoroutine(ShotData shotData, GameObject[] newTargets, float newSmoothTime, float delay)
        {
            // Wait for delay.
            if (delay > 0)
                yield return new WaitForSeconds(delay);

            // Replace targets if new ones where specified.
            if (newTargets != null)
            {
                shotData.targets = newTargets;
                targets = shotData.targets;
            }

            // Update all interpolator smooth times.
            targetOffsetInterpolator.smoothTime = newSmoothTime;
            positionInterpolator.smoothTime = newSmoothTime;
            fieldOfViewInterpolator.smoothTime = newSmoothTime;
            proxyTargetPositionInterpolator.smoothTime = newSmoothTime;

            // Update all shot values.
            if (Mathf.Abs(newSmoothTime) < Mathf.Epsilon)
            {
                // Smooth time is 0, update values instantly.
                targetOffsetInterpolator.InstantValue = shotData.targetOffset;
                positionInterpolator.InstantValue = shotData.position;
                fieldOfViewInterpolator.InstantValue = shotData.fieldOfView;
                cachedCamera.fieldOfView = fieldOfViewInterpolator.Value;
            }
            else
            {
                // Smooth time is greater than 0, update target values for interpolation.
                targetOffsetInterpolator.targetValue = shotData.targetOffset;
                positionInterpolator.targetValue = shotData.position;
                fieldOfViewInterpolator.targetValue = shotData.fieldOfView;
            }
        }

        /// <summary>
        /// Creates a snapshot of the desired target(s).
        /// </summary>
        /// <returns>The snapshot.</returns>
        /// <param name="target">Target.</param>
        /// <param name="position">Position.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="fieldOfView">Field of view.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="layer">Layer.</param>
        public static Texture2D TakeSnapshot(GameObject target, PolarCoords position,
                                             Vector3 offset, float fieldOfView,
                                             int width, int height, string layer)
        {
            Debug.Log("Taking snapshot");
            // Save current active render texture & create a temporary one 
            // with the desirred dimensions.
            RenderTexture activeRenderTexture = RenderTexture.active;
            RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 24);
            RenderTexture.active = renderTexture;

            // Create & set up the camera
            GameObject cameraGO = new GameObject("Snapshot Camera");
            CineCam cineCam = cameraGO.AddComponent<CineCam>();
            cineCam.cachedCamera.targetTexture = renderTexture;
            cineCam.cachedCamera.fieldOfView = fieldOfView;
            cineCam.cachedCamera.nearClipPlane = 0.01f;
            cineCam.cachedCamera.cullingMask = LayerMask.GetMask(layer);
            cineCam.cachedCamera.clearFlags = CameraClearFlags.Color;
            cineCam.cachedCamera.backgroundColor = Color.clear;
           
            // Setup target

            // Save target data to be modified
            int targetLayer = target.layer;
            Transform targetParent = target.transform.parent;
            Vector3 targetPosition = target.transform.position;
            Quaternion targetRotation = target.transform.rotation;
            Vector3 targetScale = target.transform.localScale;

            // Position target in a safe place to take the snapshot at.
            target.transform.SetParent(null);
            target.transform.position = Vector3.zero;
            target.transform.rotation = Quaternion.identity;
            target.transform.localScale = Vector3.one;
            target.SetRootLayer(LayerMask.NameToLayer(layer));

            // Position camera
            cineCam.targets = new GameObject[] { target };
            cineCam.MoveTo(position, 0);
            cineCam.SetTargetOffset(offset, 0);
            cineCam.ZoomTo(fieldOfView, 0);
            cineCam.LateUpdate();

            // Render the snapshot
            cineCam.cachedCamera.Render();
            cineCam.cachedCamera.Render();
            cineCam.cachedCamera.Render();

            // Setup the output texture and grab the RenderTexture pixels in.
            Texture2D texture = new Texture2D(width, height);
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0, true);
            texture.Apply();

            // Cleanup
            RenderTexture.ReleaseTemporary(renderTexture);
            RenderTexture.active = activeRenderTexture;
            DestroyImmediate(cameraGO);

            target.transform.SetParent(targetParent);
            target.transform.position = targetPosition;
            target.transform.rotation = targetRotation;
            target.transform.localScale = targetScale;
            target.SetRootLayer(targetLayer);

            return texture;
        }
    
        #endregion

        #region Interface implementation
        #endregion

        #region Nested classes
        public struct ShotData
        {
            public GameObject[] targets;
            public Vector3 targetOffset;
            public PolarCoords position;
            public float fieldOfView;

            public ShotData(GameObject[] targets, Vector3 targetOffset, PolarCoords position, float fieldOfView)
            {
                this.targets = targets;
                this.targetOffset = targetOffset;
                this.position = position;
                this.fieldOfView = fieldOfView;
            }

            override public string ToString ()
            {
                return "offset:" + targetOffset.ToString() + " polar: "+ position.ToString();
            }
        }
        #endregion
    }
}
