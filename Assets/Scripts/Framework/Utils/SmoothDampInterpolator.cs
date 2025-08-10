using UnityEngine;

namespace Framework.Utils
{
    /// <summary>
    /// Smooth Damp Interpolator
    /// Defines Float, Angle, Vector2, Vector3 and Polar coordinate value interpolators 
    /// encapsulatiing all other variables needed for the computation.
    /// 
    /// Create by Jorge Luis Chavez Herrera.
    /// </summary>
	#region SDInterpolatorBase
	/// <summary>
	/// SD Interpolator Base.
	/// Base class for Smooth Damp interpolators
	/// </summary>
	public class SDInterpolatorBase
	{
		public float smoothTime;
	}
	#endregion
	
	#region FloatSDInterpolator
	/// <summary>
	/// Float Smooth Damp interpolator.
	///  Interpolates a float value unsing smooth damping.
	/// </summary>
	public class FloatSDInterpolator : SDInterpolatorBase
	{
		public float targetValue;
		
		private float _value;
		private float velocity;
		
		public float InstantValue
		{
			set 
			{ _value = targetValue = value; }
		}
		
		public float Value 
		{
			get 
			{
                _value = Mathf.SmoothDamp (_value, targetValue, ref velocity, smoothTime, Mathf.Infinity, Time.deltaTime);
				return _value;
			}
		}

        public void ResetAndTarget(float value, float target, float smoothTime = -1)
        {
            _value = value;
            targetValue = target;
            velocity = 0;

            if (smoothTime >= 0)
                this.smoothTime = smoothTime;
        }
	
		public FloatSDInterpolator (float smoothTime)
		{
			this.smoothTime = smoothTime;
		}
	}
	#endregion
	
	#region AngleSDInterpolator
	/// <summary>
	/// Angle Smooth Damp interpolator.
	/// Interpolates a float angle value unsing smooth damping.
	/// </summary>
	public class AngleSDInterpolator : SDInterpolatorBase
	{
		public float targetValue;
		
		private float _value;
		private float velocity;
		
        public float InstantValue
		{
			set 
			{   _value = targetValue = value;
                velocity = 0;
            }
		}
		
        public float Value 
		{
			get 
			{
                _value = Mathf.SmoothDampAngle(_value, targetValue, ref velocity, smoothTime, Mathf.Infinity, Time.deltaTime);
				return _value;
			}
		}

        public void ResetAndTarget(float value, float target, float smoothTime = -1)
        {
            _value = value;
            targetValue = target;
            velocity = 0;

            if (smoothTime >= 0)
                this.smoothTime = smoothTime;
        }
	
		public AngleSDInterpolator (float smoothTime)
		{
			this.smoothTime = smoothTime;
		}
	}
	#endregion

	#region EulerAnglesInterpolator
    /// <summary>
    /// Euler Angle Smooth Damp interpolator.
    /// Interpolates a Vector3 euler angle value unsing smooth damping.
    /// </summary>
	public class EulerAnglesSDInterpolator : SDInterpolatorBase
	{
		public Vector3 targetValue;
		
		private Vector3 _value;
		private Vector3 velocity;
		
        public Vector3 InstantValue
		{
			set 
			{ 
                _value = targetValue = value; 
                velocity = Vector3.zero;
            }
		}
		
        public Vector3 Value 
		{
			get 
			{
				// Update value only when needed
                _value = new Vector3 (Mathf.SmoothDampAngle (_value.x, targetValue.x, ref velocity.x, smoothTime, Mathf.Infinity, Time.deltaTime),
                                      Mathf.SmoothDampAngle (_value.y, targetValue.y, ref velocity.y, smoothTime, Mathf.Infinity, Time.deltaTime),
                                      Mathf.SmoothDampAngle (_value.z, targetValue.z, ref velocity.z, smoothTime, Mathf.Infinity, Time.deltaTime));
				return _value;
			}
		}

        public void ResetAndTarget(Vector3 value, Vector3 target, float smoothTime = -1)
        {
            _value = value;
            targetValue = target;
            velocity = Vector3.zero;

            if (smoothTime >= 0)
                this.smoothTime = smoothTime;
        }
		
		public EulerAnglesSDInterpolator (float smoothTime)
		{
			this.smoothTime = smoothTime;
		}
	}
	#endregion
	
	#region Vector2SDInterpolator
	/// <summary>
	/// Vector2 Smooth Damp interpolator.
	/// Interpolates a Vector2 value unsing smooth damping.
	/// </summary>
	public class Vector2SDInterpolator : SDInterpolatorBase
	{
		public Vector2 targetValue;
		
		private Vector2 _value;
		private Vector2 velocity;
		
        public Vector2 InstantValue
		{
			set 
			{
                _value = targetValue = value; 
                velocity = Vector2.zero;
            }
		}
		
        public Vector2 Value 
		{
			get 
			{
				_value = Vector2.SmoothDamp (_value, targetValue, ref velocity, smoothTime, float.MaxValue, Time.deltaTime);
				
				return _value;
			}
		}

        public void ResetAndTarget(Vector2 value, Vector2 target, float smoothTime = -1)
        {
            _value = value;
            targetValue = target;
            velocity = Vector2.zero;

            if (smoothTime >= 0)
                this.smoothTime = smoothTime;
        }
		
		public Vector2SDInterpolator (float smoothTime)
		{
			this.smoothTime = smoothTime;
		}
	}
	#endregion
	
	#region Vector3SDInterpolator
	/// <summary>
	/// Vector3 Smooth Damp interpolator.
	/// Interpolates a Vector3 value unsing smooth damping.
	/// </summary>
	public class Vector3SDInterpolator : SDInterpolatorBase
	{
		public Vector3 targetValue;
		
		private Vector3 _value;
		private Vector3 velocity;
		
		public Vector3 InstantValue
		{
			set 
			{ 
                _value = targetValue = value; 
                velocity = Vector3.zero;
            }
		}
		
		public Vector3 Value 
		{
			get 
			{	
                _value = Vector3.SmoothDamp (_value, targetValue, ref velocity, smoothTime, Mathf.Infinity, Time.deltaTime);
				return _value;
			}
		}

        public void ResetAndTarget(Vector3 value, Vector3 target, float smoothTime = -1)
        {
            _value = value;
            targetValue = target;
            velocity = Vector3.zero;

            if (smoothTime >= 0)
                this.smoothTime = smoothTime;
        }
	
		public Vector3SDInterpolator (float smoothTime)
		{
			this.smoothTime = smoothTime;
		}
	}
	#endregion

    #region PolarSDInterpolator
    /// <summary>
    /// Polar Coordinates Smooth Damp interpolator.
    /// Interpolates a Polar value unsing smooth damping.
    /// </summary>
    public class PolarSDInterpolator : SDInterpolatorBase
    {
        public PolarCoords targetValue;
        private PolarCoords _value;
        private PolarCoords velocity;

        public PolarCoords InstantValue
        {
            set
            { 
                _value = targetValue = value;
                velocity = new PolarCoords();
            }
        }

        public PolarCoords Value
        {
            get
            {
                _value = PolarCoords.SmoothDamp(_value, targetValue, ref velocity, smoothTime, Mathf.Infinity, Time.deltaTime);
                return _value;
            }
        }

        public void ResetAndTarget(PolarCoords value, PolarCoords target, float smoothTime = -1)
        {
            _value = value;
            targetValue = target;
            velocity = new PolarCoords();

            if (smoothTime >= 0)
                this.smoothTime = smoothTime;
        }
    

        public PolarSDInterpolator(float smoothTime)
        {
            this.smoothTime = smoothTime;
        }
    }
    #endregion
}