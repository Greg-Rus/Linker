using UnityEngine;
using System.Collections;

public static class Utilities {

	public static void alignGroupCenterToOrigin(Transform groupTransform, GameObject element, int xCount, int yCount)
	{
		float elementWidth = element.transform.localScale.x;
		float elementHeight = element.transform.localScale.y;
		Vector3 newPosition = new Vector3 ((elementWidth * xCount) * -0.5f + elementWidth * 0.5f, 
		                                   (elementHeight * yCount) * -0.5f + elementHeight * 0.5f,
		                                   0f);
		groupTransform.position = newPosition;
	}
	public static bool vectorsInRange(Vector2 vector1, Vector2 vector2, int range)
	{
		Vector2 neighbourDistance = vector1 - vector2;
		if (Mathf.Abs (neighbourDistance.x) > range || Mathf.Abs (neighbourDistance.y) > range) 
		{
			return false;
		} 
		else 
		{
			return true;
		}
	}
}
