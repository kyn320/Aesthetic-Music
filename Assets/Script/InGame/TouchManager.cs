using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
	public static TouchManager instance;

	public List<TouchInfo> infos;

	private GameManager g;



	private void Awake()
	{
		instance = this;
		Input.multiTouchEnabled = true;
		infos = new List<TouchInfo>();
	}

	private void Start()
	{
		g = GameManager.instance;
	}

	private void Update()
	{
		// 터치 기기의 경우. (Android, iOS 등)
		if (Input.touchSupported)
		{
			while (infos.Count != Input.touchCount)
			{
				if (infos.Count > Input.touchCount)
					infos.RemoveAt(infos.Count - 1);
				else
					infos.Add(new TouchInfo());
			}

			for (int i = 0; i < Input.touchCount; ++i)
			{
				Touch t = Input.GetTouch(i);
				TouchInfo info = infos[i];
				info.lastPosition = info.position;
				info.position = t.position;

				if (t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Stationary)
					info.state = TouchState.Stay;
				else if (t.phase == TouchPhase.Began)
				{
					info.state = TouchState.Press;
					info.startPosition = t.position;
					info.dragPosition = t.position;
				}
				else if (t.phase == TouchPhase.Ended)
					info.state = TouchState.Release;

				info.UpdateTouchDelta();
			}
		}
		// 그 외의 경우. (에디터, 윈도우 등)
		else
		{
			if (Input.GetMouseButton(0))
			{
				TouchInfo info = null;
				if (Input.GetMouseButtonDown(0))
				{
					info = new TouchInfo();
					infos.Add(info);
					info.state = TouchState.Press;
					info.startPosition = Input.mousePosition;
					info.dragPosition = Input.mousePosition;
				}
				else
				{
					info = infos[0];
					info.state = TouchState.Stay;
				}

				info.lastPosition = info.position;
				info.position = Input.mousePosition;
				info.UpdateTouchDelta();
			}
			else if (Input.GetMouseButtonUp(0))
			{
				TouchInfo info = infos[0];
				info.lastPosition = info.position;
				info.position = Input.mousePosition;
				info.state = TouchState.Release;
				info.UpdateTouchDelta();
			}
			else if (infos.Count > 0)
				infos.RemoveAt(0);
		}
	}
}

public enum TouchState
{
	None = 0,
	Press = 1,
	Stay = 2,
	Release = 3
}
