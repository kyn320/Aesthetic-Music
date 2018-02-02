using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeEffect : MonoBehaviour
{
	private const float duration = 1f;
	private const float moveDuration = 0.3f;
	private const float moveHeight = 0.5f;

	/// <summary>
	/// 0 : Miss, 1 : Perfect, 2 : Nice, 3 : Good
	/// </summary>
	public Sprite[] judgeSprite;

	private float timer = 0f;
	private bool show = false;
	private SpriteRenderer spriteRenderer;
	private Animator animator;



	private void Awake()
	{
		spriteRenderer = this.GetComponent<SpriteRenderer>();
		animator = this.GetComponent<Animator>();
    }

	private void Start()
	{
		Hide();
	}

	private void Update()
	{
		if (show)
		{
			timer += Time.deltaTime;
			if (timer >= duration)
				Hide();
		}
	}

	public void Show(Judge _judge)
	{
		show = true;
		timer = 0f;

		int judgeNum = 0;
		switch (_judge)
		{
			case Judge.Perfect:
				judgeNum = 1;
				break;
			case Judge.Nice:
				judgeNum = 2;
				break;
			case Judge.Good:
				judgeNum = 3;
				break;
			case Judge.PreMiss:
			case Judge.Miss:
				judgeNum = 0;
				break;
		}
		spriteRenderer.sprite = judgeSprite[judgeNum];
		spriteRenderer.enabled = true;
		animator.Play("Show", -1, 0);
    }

	public void Hide()
	{
		show = false;
		spriteRenderer.enabled = false;
	}
}
