using UnityEngine;
using System.Collections;


namespace Framework.AIModels
{
	/// <summary>
	/// StateBase.cs
	/// 
	/// Base class for all states, defines functions for state changes, collision & trigger events.
	/// 
	/// Main functions:
	/// OnEnter  - called when entering the state, put all initialization code here.
	/// OnUpdate - called on every update while the state is current, put animation & input code here.
	/// OnFixedUpdate - called by physics engine (this happens several times between each frame so use it only for physics accuracy)
	/// OnLateUpdate - called after all updates in the scene, used for animating cameras & other update dependant effects.
	/// OnExit   - called before entering a new state, put all cleanup code here.
	/// 
	/// All Collider, trigger, collider 2D & trigger 2D functions are implemented as well.
	/// 
	/// By Jorge L. Chávez Herrera.
	/// </summary>
	public abstract class StateBase
	{
	    #region Class members
		[System.NonSerialized] public string name; // Holds the state's name
		[System.NonSerialized] public float time;  // Holds the state's internal time
	    #endregion

	    #region Class accessors
	    private float _lastEnterTime;
	    public float lastEnterTime // Returns the last time this state was entered
	    {
	        get { return _lastEnterTime; }
	    }
	    private float _lastExitTime;
	    public float lastExitTime // Returns the last time this state was exited
	    {
	        get { return _lastExitTime; }
	    }
	    #endregion

	    #region Class implementation
		/// <summary>
		/// Enter this state.
		/// </summary>
	    public void Enter ()
	    {
	        time = 0;
	        _lastEnterTime = Time.time;
	        OnEnter();
	    }

		/// <summary>
		/// Exit this state.
		/// </summary>
	    public void Exit ()
	    {
	        _lastExitTime = Time.time;
	        OnExit();
	    }

		// State switch functions
	    virtual public void OnEnter () { }
		virtual public void OnExit () { }

		// Updte functions
	    virtual public void OnUpdate () { }
	    virtual public void OnFixedUpdate () { }
	    virtual public void OnLateUpdate () { }
	   
	    // Collision & trigger handling functions
	    virtual public void OnCollisionEnter (Collision collision) { }
	    virtual public void OnCollisionStay  (Collision collision) { }
	    virtual public void OnCollisionExit  (Collision collision) { }

	    virtual public void OnTriggerEnter (Collider collider) { }
	    virtual public void OnTriggerStay  (Collider collider) { }
	    virtual public void OnTriggerExit  (Collider collider) { }

		virtual public void OnCollisionEnter2D (Collision2D collision2D) { }
		virtual public void OnCollisionStay2D  (Collision2D collision2D) { }
		virtual public void OnCollisionExit2D  (Collision2D collision2D) { }

		virtual public void OnTriggerEnter2D (Collider2D collider2D) { }
		virtual public void OnTriggerStay2D  (Collider2D collider2D) { }
		virtual public void OnTriggerExit2D  (Collider2D collider2D) { }
	    #endregion
	}
}
