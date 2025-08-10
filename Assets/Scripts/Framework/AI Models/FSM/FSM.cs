using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Framework.AIModels
{
    /// <summary>
    /// FSM.cs
    /// 
    /// This class defines a Finite State Machine component with functions for handling state updates & state changes.
    /// In practice, using state machines for AI requires a lot of branching when handling collisions & triggers,
    /// By having collision & trigger handler functions specific for each estate, we can reduce the complexity on the desicion tree. 
    /// 
    /// By Jorge L. Chávez Herrera.
    /// </summary>
    public class FSM : MonoBehaviour
    {
        #region Class members
        private Dictionary<string, StateBase> statesDict = new Dictionary<string, StateBase>();

        [System.NonSerialized]
        public StateBase previousState, currentState;
        #endregion

        #region CLass accessors
        /// <summary>
        /// Returns a dictionary with all states for this machine
        /// </summary>
        /// <value>
        /// The states.
        /// </value>
        public Dictionary<string, StateBase> states
        {
            get { return statesDict; }
        }
        #endregion

        #region MonoBehaviour Overrides
        /// <summary>
        /// Executes the current state update function
        /// </summary>
        private void Update()
        {
            if (currentState != null)
            {
                currentState.time += Time.deltaTime;
                currentState.OnUpdate();
            }
        }

        /// <summary>
        /// Executes the current state fixed update function
        /// </summary>
        private void FixedUpdate()
        {
            if (currentState != null)
            {
                currentState.OnFixedUpdate();
            }
        }

        /// <summary>
        /// Executes the current state late update function
        /// </summary>
        private void LateUpdate()
        {
            if (currentState != null)
            {
                currentState.OnLateUpdate();
            }
        }

        /// <summary>
        /// Raises the OnCollisionEnter event for the current state.
        /// </summary>
        /// <param name="collision">Collision.</param>
        private void OnCollisionEnter(Collision collision)
        {
            if (currentState != null)
                currentState.OnCollisionEnter(collision);
        }

        /// <summary>
        /// Raises the OnCollisionStay event for the current state.
        /// </summary>
        /// <param name="collision">Collision.</param>
        private void OnCollisionStay(Collision collision)
        {
            if (currentState != null)
                currentState.OnCollisionStay(collision);
        }

        /// <summary>
        /// Raises the OnCollisionExit event for the current state.
        /// </summary>
        /// <param name="collision">Collision.</param>
        private void OnCollisionExit(Collision collision)
        {
            if (currentState != null)
                currentState.OnCollisionExit(collision);
        }

        /// <summary>
        /// Raises the OnTriggerEnter event for the current state.
        /// </summary>
        /// <param name="collider">Collider.</param>
        private void OnTriggerEnter(Collider collider)
        {
            if (currentState != null)
                currentState.OnTriggerEnter(collider);
        }

        /// <summary>
        /// Raises the OnTriggerStay event for the current state.
        /// </summary>
        /// <param name="collider">Collider.</param>
        private void OnTriggerStay(Collider collider)
        {
            if (currentState != null)
                currentState.OnTriggerStay(collider);
        }

        /// <summary>
        /// Raises the OnTriggerExit event for the current state.
        /// </summary>
        /// <param name="collider">Collider.</param>
        private void OnTriggerExit(Collider collider)
        {
            if (currentState != null)
                currentState.OnTriggerExit(collider);
        }

        /// <summary>
        /// Raises the OnCollisionEnter2D event for the current state.
        /// </summary>
        /// <param name="collision2D">Collision2 d.</param>
        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (currentState != null)
                currentState.OnCollisionEnter2D(collision2D);
        }

        /// <summary>
        /// Raises the OnCollisionStay2D event for the current state.
        /// </summary>
        /// <param name="collision2D">Collision2 d.</param>
        private void OnCollisionStay2D(Collision2D collision2D)
        {
            if (currentState != null)
                currentState.OnCollisionStay2D(collision2D);
        }

        /// <summary>
        /// Raises the OnCollisionExit2D event for the current state.
        /// </summary>
        /// <param name="collision2D">Collision2 d.</param>
        private void OnCollisionExit2D(Collision2D collision2D)
        {
            if (currentState != null)
                currentState.OnCollisionExit2D(collision2D);
        }

        /// <summary>
        /// Raises the OnTriggerEnter2D event for the current state.
        /// </summary>
        /// <param name="collider2D">Collider2 d.</param>
        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (currentState != null)
                currentState.OnTriggerEnter2D(collider2D);
        }

        /// <summary>
        /// Raises thw OnTriggerStay2D event for the current state.
        /// </summary>
        /// <param name="collider2D">Collider2 d.</param>
        private void OnTriggerStay2D(Collider2D collider2D)
        {
            if (currentState != null)
                currentState.OnTriggerStay2D(collider2D);
        }

        /// <summary>
        /// Raises the OnTriggerExit2D event for the current state.
        /// </summary>
        /// <param name="collider2D">Collider2 d.</param>
        private void OnTriggerExit2D(Collider2D collider2D)
        {
            if (currentState != null)
                currentState.OnTriggerExit2D(collider2D);
        }
        #endregion

        #region Class functions
        /// <summary>
        /// Adds a new state to this state machine.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="state">State.</param>
        public void AddState(string name, StateBase state)
        {
            state.name = name;
            statesDict.Add(state.name, state);
        }

        /// <summary>
        /// Replaces a previously created state with the specified instance.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="state">State.</param>
        public void ReplaceState(string name, StateBase state)
        {
            if (state != null)
                state.name = name;

            statesDict[name] = state;
        }

        /// <summary>
        /// Gets a state.
        /// </summary>
        /// <returns>The state.</returns>
        /// <param name="name">Name.</param>
        public StateBase GetState(string name)
        {
            return statesDict[name];
        }

        /// <summary>
        /// Clears all states instance.
        /// </summary>
        public void Clear()
        {
            statesDict.Clear();
        }

        /// <summary>
        /// Sets the current state.
        /// </summary>
        /// <param name="stateName">State name.</param>
        public void SetState(string stateName)
        {
            // Get next state or null
            StateBase nextState = null;

            if (string.IsNullOrEmpty(stateName) == false)
            {
                nextState = statesDict[stateName];
            }

            // Exit current state
            if (currentState != null)
            {
                StopAllCoroutines(); // ????? is this really needed ?
                currentState.Exit();
            }

            // Enter new state
            previousState = currentState;
            currentState = nextState;

            if (currentState != null)
            {
                currentState.Enter();
            }
        }

        /// <summary>
        /// Sets the state after a specified delay, cancells all previous calls to the coroutine
        /// used by this function.
        /// </summary>
        /// <param name="stateName">State name.</param>
        /// <param name="delay">Delay.</param>
        public void SetStateWithDelay(string stateName, float delay)
        {
            StartCoroutine(SetStateWithDelayCoroutine(stateName, delay));
        }

        /// <summary>
        /// Sets a state with a delay coroutine.
        /// </summary>
        /// <returns>The state with delay coroutine.</returns>
        /// <param name="stateName">State name.</param>
        /// <param name="delay">Delay.</param>
        private IEnumerator SetStateWithDelayCoroutine(string stateName, float delay)
        {
            yield return new WaitForSeconds(delay);
            SetState(stateName);
        }

        /// <summary>
        /// Returns wheter the current state is the specified one.
        /// </summary>
        /// <returns><c>true</c>, if state is was currented, <c>false</c> otherwise.</returns>
        /// <param name="stateName">State name.</param>
        public bool CurrentStateIs(string stateName)
        {
            return currentState != null && currentState.name == stateName;
        }
        #endregion
    }
}