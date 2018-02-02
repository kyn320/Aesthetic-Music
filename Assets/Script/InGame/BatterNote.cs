using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterNote : Note
{
	public int maxHit = 3;
	public float endTime = 0f;
	public Transform progressCircle;

	public bool batterEnabled = false;
	public int currentHit = 0;
	public float endTimeInterval = 0f;
	public float progress = 0f;
	public float progressScale = 0.3f;
	public Vector3 circleScale = new Vector3(0.3f, 0.3f, 0.3f);
	public Animator animator;



	protected override void Awake()
	{
		g = GameManager.instance;
		animator = this.GetComponent<Animator>();
		gameObject.SetActive(false);
	}

	public override void Start()
	{
		currentHit = 0;
		progress = 0f;
		progressScale = 0.3f;
		circleScale = new Vector3(0.3f, 0.3f, 0.3f);
		progressCircle.localScale = circleScale;
		batterEnabled = false;

		g.noteJudgement.currentBatter = this;
		animator.Play("Appear", -1, 0f);
	}

	protected override void Update()
	{
		timeInterval = time - g.syncedTime;
		endTimeInterval = endTime - g.syncedTime;
		UpdateBatter();
	}

	private void UpdateBatter()
	{
		if (endTimeInterval < 0f && batterEnabled)
		{
			batterEnabled = false;
			animator.Play("Disappear", -1, 0f);
		}
		
	}

	public override void UpdatePosition()
	{
		this.transform.localPosition = Vector3.zero;
	}

	public void Hit()
	{
		if (!batterEnabled)
			return;

		progress = ++currentHit / (float)maxHit;
		if (currentHit == maxHit)
		{
			Clear();
			return;
		}

		animator.Play("Beat", -1, 0f);
		progressScale = 0.3f + 0.7f * progress;
		circleScale.x = progressScale;
		circleScale.y = progressScale;
		progressCircle.localScale = circleScale;
	}

	private void Clear()
	{
		batterEnabled = false;
		animator.Play("Clear", -1, 0f);
	}

	public void OnBatterStart()
	{
		batterEnabled = true;
    }

	public void OnBatterRemove()
	{
		batterEnabled = false;
		g.noteJudgement.currentBatter = null;
		g.noteSpawn.RemoveNote(this);
	}
}
