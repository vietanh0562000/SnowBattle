using UnityEngine;
using System.Collections;


namespace Framework.AIModels
{
	/// <summary>
	/// StateBaseGeneric.cs
	/// 
	/// Generic version class for states. By having the owner T variable, we can have 
	/// easy access to owners's public members from within states.
	/// 
	/// By Jorge L. Chávez Herrera.
	/// </summary>

	public class StateBaseGeneric<T> : StateBase
	{
	    #region Class members
	    protected T owner;    // This state's owner
	    #endregion

	    #region Class implementation
	    public StateBaseGeneric(T inOwner)
	    {
	        owner = inOwner;
	    }
	    #endregion
	}
}
