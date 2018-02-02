using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInfo
{
	public Vector2 position = Vector2.zero;
	public Vector2 lastPosition = Vector2.zero;
	public Vector2 startPosition = Vector2.zero;
	public Vector2 dragPosition = Vector2.zero;
	public float touchDelta = 0f;
	public TouchState state = TouchState.None;



	public void UpdateTouchDelta()
	{
		touchDelta = Mathf.Abs(Vector2.Distance(lastPosition, position));
	}

	public int CheckDrag()
	{
		if (Mathf.Abs(Vector2.Distance(dragPosition, position)) >= GameManager.dragMinDistance &&
			touchDelta >= GameManager.dragMinDelta)
        {
			if (dragPosition.x <= position.x)
				return 1;
			else
				return -1;
		}
		return 0;
	}

	public void ResetDrag()
	{
		dragPosition = position;
	}
}