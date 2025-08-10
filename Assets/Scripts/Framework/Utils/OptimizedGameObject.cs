using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Pooling;
using Framework.AIModels;

namespace Framework.Utils 
{
	/// <summary>
	/// OtimizedGameObject.
	/// Defines accessors for cached versions of the most commonly used components
	/// 
	/// By: Jorge L. Chávez Herrera.
	/// </summary>
	public class OptimizedGameObject : MonoBehaviour 
	{
		#region Class accessors

		#region Transforms
		/// <summary>
		/// Accessor to the cached Transform component.
		/// </summary>
		/// <value>The cached transform.</value>
		protected Transform _cachedTransform;
		virtual public Transform cachedTransform
		{
			get 
			{
				if (_cachedTransform == null)
					_cachedTransform = GetComponent<Transform> ();

				return _cachedTransform;
			}
		}

		/// <summary>
		/// Accessor to the cached RectTransform component.
		/// </summary>
		/// <value>The cached rect transform.</value>
		protected RectTransform _cachedRectTransform;
		virtual public RectTransform cachedRectTransform
		{
			get 
			{
				if (_cachedRectTransform == null)
					_cachedRectTransform = GetComponent<RectTransform> ();
				
				return _cachedRectTransform;
			}
		}

		/// <summary>
		/// Accessor to the cached parent Transform component.
		/// </summary>
		/// <value>The cached rect transform.</value>
		private Transform _parentTransform = null;
		public Transform parentTransform 
		{
			get 
			{
				if (_parentTransform == null && transform.parent != null)
					_parentTransform = transform.parent.GetComponent<Transform>();

				return parentTransform;
			}
		}
		#endregion

		#region Renderers
		protected SpriteRenderer _cachedSpriteRenderer;
		virtual public SpriteRenderer cachedSpriteRenderer
		{
			get 
			{
				if (_cachedSpriteRenderer == null)
					_cachedSpriteRenderer = GetComponentInChildren<SpriteRenderer> ();

				return _cachedSpriteRenderer;
			}
		}
		#endregion

		#region Animation
		protected Animation _cachedAnimation;
		virtual public Animation cachedAnimation
		{
			get 
			{
				if (_cachedAnimation == null)
					_cachedAnimation = GetComponentInChildren<Animation> ();

				return _cachedAnimation;
			}
		}

		protected Animator _cachedAnimator;
		virtual public Animator cachedAnimator
		{
			get 
			{
				if (_cachedAnimator == null)
					_cachedAnimator = GetComponentInChildren<Animator> ();

				return _cachedAnimator;
			}
		}
		#endregion

		#region Phisycs
		/// <summary>
		/// Gets the cached collider component.
		/// </summary>
		/// <value>The cached transform.</value>
		protected Collider _cachedCollider;
		virtual public Collider cachedCollider
		{
			get 
			{
				// Store this component's reference for the first time
				if (_cachedCollider == null)
					_cachedCollider = GetComponentInChildren<Collider>();

				return _cachedCollider;
			}
		}
			
		/// <summary>
		/// Gets the cached rigidbody component.
		/// </summary>
		/// <value>The cached rigidbody.</value>
		protected Rigidbody _cachedRigidbody;
		virtual public Rigidbody cachedRigidbody
		{
			get 
			{
				// Store this component's reference for the first time
				if (_cachedRigidbody == null)
					_cachedRigidbody = GetComponentInChildren<Rigidbody>();

				return _cachedRigidbody;
			}
		}

		/// <summary>
		/// Gets the cached 2D rigidbody component.
		/// </summary>
		/// <value>The cached 2D rigidbody.</value>
		protected Rigidbody2D _cachedRigidbody2D;
		virtual public Rigidbody2D cachedRigidbody2D
		{
			get 
			{
				// Store this component's reference for the first time
				if (_cachedRigidbody2D == null)
					_cachedRigidbody2D = GetComponent<Rigidbody2D>();

				return _cachedRigidbody2D;
			}
		}

        /// <summary>
        /// Gets the cached character controller component.
        /// </summary>
        /// <value>The cached 2D rigidbody.</value>
        protected CharacterController _cachedCharacterController;
        virtual public CharacterController cachedCharacterController
        {
            get
            {
                // Store this component's reference for the first time
                if (_cachedCharacterController == null)
                    _cachedCharacterController = GetComponent<CharacterController>();

                return _cachedCharacterController;
            }
        }
		#endregion

		#region Audio
		/// <summary>
		/// Gets the cached audio source.
		/// </summary>
		/// <value>The cached audio source.</value>
		protected AudioSource _cachedAudioSource;
		virtual public AudioSource cachedAudioSource
		{
			get 
			{
				if (_cachedAudioSource == null)
					_cachedAudioSource = GetComponent<AudioSource> ();

				if (_cachedAudioSource == null)
					_cachedAudioSource = gameObject.AddComponent<AudioSource> ();

				return _cachedAudioSource;
			}
		}
		#endregion

		#region Camera
		/// <summary>
		/// Gets the cached camera component.
		/// </summary>
		/// <value>The cached camera.</value>
		protected Camera _cachedCamera;
		virtual public Camera cachedCamera 
		{
			get 
			{
				// Store this component's reference for the first time
				if (_cachedCamera == null)
					_cachedCamera = GetComponentInChildren<Camera>();

				return _cachedCamera;
			}
		}
		#endregion

		#region Custom classes
		/// <summary>
		/// Gets the cached PoolManaged component.
		/// </summary>
		/// <value>The PoolManaged component.</value>
		protected PoolManaged _cachedPoolManaged;
		virtual public PoolManaged cachedPoolManaged 
		{
			get 
			{
				// Store this component's reference for the first time
				if (_cachedPoolManaged == null)
					_cachedPoolManaged = GetComponent<PoolManaged> ();

				return _cachedPoolManaged;
			}
		}

		/// <summary>
		/// Gets the cached camera component.
		/// </summary>
		/// <value>The cached camera.</value>
		protected FSM _cahcedFSM;
		virtual public FSM cachedFSM 
		{
			get 
			{
				// Store this component's reference for the first time
				if (_cahcedFSM == null)
					_cahcedFSM = GetComponentInChildren<FSM>();

				return _cahcedFSM;
			}
		}
		#endregion 

		#endregion
	}
}
