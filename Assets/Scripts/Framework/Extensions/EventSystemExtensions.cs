using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Event system extensions.
/// 
/// By Jorge L. Chávez Herrera.
/// 
/// Extensions methods for EventSystem class.
/// </summary>
public static class EventSystemExtensions 
{
	/// <summary>
	/// Work around for using Input.mouse on Android & still repoting if user has clicked over an UI widget.
	/// </summary>
	/// <returns><c>true</c> if is pointer over user interface object the specified eventSystem; otherwise, <c>false</c>.</returns>
	/// <param name="eventSystem">Event system.</param>
	static public bool IsPointerOverUIObject (this EventSystem eventSystem) 
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData (eventSystem);
		eventDataCurrentPosition.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll (eventDataCurrentPosition, results);

		return results.Count > 0;
	}
}
