#if NCITE_LUA
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Framework.Lua
{	
	/// <summary>
	/// Lua pointer handlers.
	/// 
	/// Implements PointerHandler interfaces to call Lua script functions.
	/// 
	/// By Jorge L. Chávez Herrera.
	/// </summary>
	public class LuaPointerHandlers : OptimizedLuaGameObject, IPointerClickHandler /*, IPointerDownHandler, IPointerUpHandler, 
	IPointerEnterHandler, IPointerExitHandler*/
	{
		#region Class members
		public string pointerClickHandler = "PointerClickHandler";
		public string pointerDownHandler = "PointerDownHandler";
		public string pointerUpHandler = "PointerUpHandler";
		public string pointerEnterHandler = "PointerEnterHandle";
		public string pointerExitandler = "PointerExitandler";
		#endregion

		#region IPointerClickHandler implementation

		public void OnPointerClick (PointerEventData eventData)
		{
			if (string.IsNullOrEmpty (pointerClickHandler) == false) 
			{
				if (cachedLuaMonoBinder != null) 
				{
					cachedLuaMonoBinder.CallFunction (pointerClickHandler, eventData);
				}
			}
		}
		
		#endregion

		#region IPointerDownHandler implementation

		public void OnPointerDown (PointerEventData eventData)
		{
			if (string.IsNullOrEmpty (pointerDownHandler) == false) 
			{
				if (cachedLuaMonoBinder != null)
					cachedLuaMonoBinder.CallFunction (pointerDownHandler, eventData);
			}
		}
		#endregion

		#region IPointerUpHandler implementation

		public void OnPointerUp (PointerEventData eventData)
		{
			if (string.IsNullOrEmpty (pointerUpHandler) == false) 
			{
				if (cachedLuaMonoBinder != null)
					cachedLuaMonoBinder.CallFunction (pointerUpHandler, eventData);
			}
		}

		#endregion

		#region IPointerEnterHandler implementation

		public void OnPointerEnter (PointerEventData eventData)
		{
			if (string.IsNullOrEmpty (pointerEnterHandler) == false) 
			{
				if (cachedLuaMonoBinder != null)
					cachedLuaMonoBinder.CallFunction (pointerEnterHandler, eventData);
			}
		}

		#endregion

		#region IPointerExitHandler implementation

		public void OnPointerExit (PointerEventData eventData)
		{
			if (string.IsNullOrEmpty (pointerExitandler) == false) 
			{
				if (cachedLuaMonoBinder != null)
					cachedLuaMonoBinder.CallFunction (pointerExitandler, eventData);
			}
		}

		#endregion
	}
}
#endif
