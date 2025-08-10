using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class CanvasEstensions 
{
	public static Vector2 SizeToParent (this RawImage image, float padding = 0) 
	{
		float w = 0, h = 0;
		RectTransform parent = image.GetComponentInParent<RectTransform>();
		RectTransform imageTransform = image.GetComponent<RectTransform>();

		// Check if there is something to do
		if (image.texture != null) 
		{
			
			//if we don't have a parent, just return our current width;
			if (parent == null) 
			{ 
				return imageTransform.sizeDelta; 
			}


			padding = 1 - padding;
			float ratio = image.texture.width / (float)image.texture.height;
			Rect bounds = new Rect(0, 0, parent.rect.width, parent.rect.height);

			if (Mathf.RoundToInt (imageTransform.eulerAngles.z) % 180 == 90) 
			{
				// Invert the bounds if the image is rotated
				bounds.size = new Vector2(bounds.height, bounds.width);
			}
			//Size by height first
			h = bounds.height * padding;
			w = h * ratio;

			// If it doesn't fit, fallback to width;
			if (w > bounds.width * padding) 
			{ 
				w = bounds.width * padding;
				h = w / ratio;
			}
		}
		imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
		imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
		return imageTransform.sizeDelta;
	}
}
