using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNote : Note
{
	public int drag = 1;



	protected override void Awake()
	{
		base.Awake();
	}

	public override void Start()
	{
		base.Start();
		spriteRenderer.flipX = (drag < 0) ? true : false;
	}

	protected override void Update()
	{
		base.Update();
	}
}
