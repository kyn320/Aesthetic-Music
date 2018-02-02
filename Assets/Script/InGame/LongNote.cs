using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNote : Note
{
	public const float longTickBeat = 1f / 4f;

	private const float bodyWidth = 0.48f;

	public float length = 0f;
	public bool touched = false;
	public bool firstTouched = false;
	public Transform body;
	public Transform end;
	public float endTime = 0f;

	private float endTimeInterval = 0f;
	private float noTouchTick = 0f;
	private float longNextTick = 0f;
	private bool longEnabled = false;
	private Vector3 endNotePos = Vector3.zero;
	private Vector3 bodyScale = Vector3.one;
	private Color bodyColor = new Color(0.75f, 0.75f, 0.75f, 1f);
	private Color endColor = Color.white;
	private SpriteRenderer bodyRenderer;
	private SpriteRenderer endRenderer;

	

	protected override void Awake()
	{
		base.Awake();

		bodyRenderer = body.GetComponent<SpriteRenderer>();
		endRenderer = end.GetComponent<SpriteRenderer>();
    }

	public override void Start()
	{
		base.Start();
		touched = false;
		noTouchTick = 0f;
		longEnabled = false;
		firstTouched = false;
		endTime = time + length;
		longNextTick = longTickBeat * (120f / g.bpm);

		bodyColor = new Color(0.75f, 0.75f, 0.75f, 1f);
		endColor = Color.white;
		bodyRenderer.color = bodyColor;
		endRenderer.color = endColor;

		bodyRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
		endRenderer.sortingOrder = spriteRenderer.sortingOrder;
	}

	protected override void Update()
	{
		UpdateTimeInterval();
		UpdateTouch();
		UpdateLong();
		base.Update();

		spriteRenderer.color = noteColor;
		bodyRenderer.color = bodyColor;
		endRenderer.color = endColor;
	}

	public void UpdateTimeInterval()
	{
		timeInterval = time - g.syncedTime;
		endTimeInterval = endTime - g.syncedTime;
	}

	private void UpdateTouch()
	{
		if (firstTouched)
		{
			if (-timeInterval >= longNextTick)
			{
				longNextTick += longTickBeat * (120f / g.bpm);
				g.noteJudgement.JudgementNote((longEnabled) ? Judge.Perfect : Judge.Miss, this);
			}
			if (!touched)
			{
				noTouchTick += Time.deltaTime;
				if (noTouchTick >= longTickBeat * (120f / g.bpm))
					longEnabled = false;
			}
			else
			{
				longEnabled = true;
				noTouchTick = 0f;
			}

			if (endTimeInterval <= 0f)
			{
				if (longEnabled)
					g.noteSpawn.RemoveNote(this);
				else
					firstTouched = false;
			}

			if (!longEnabled)
			{
				noteColor.a = 0.5f;
				bodyColor.a = 0.5f;
				endColor.a = 0.5f;
			}
			else
			{
				noteColor.a = 1f;
				bodyColor.a = 1f;
				endColor.a = 1f;
			}
		}
		else if (timeInterval < -NoteJudgement.judgePerfect && endTimeInterval > 0f)
		{
			firstTouched = true;
			longEnabled = false;
			g.noteJudgement.JudgementNote(Judge.Miss, this);
		}

		touched = false;
	}

	private void UpdateLong()
	{
		if (!firstTouched && timeInterval > 0f)
			endNotePos.x = length * (g.bpm / 120.0f) * g.speed * -g.oneBeatToLine * 0.25f;
		else if (endTimeInterval >= 0f)
			endNotePos.x = endTimeInterval * (g.bpm / 120.0f) * g.speed * -g.oneBeatToLine * 0.25f;
		else
			endNotePos.x = 0f;
		bodyScale.x = -endNotePos.x / bodyWidth;

		end.localPosition = endNotePos;
		body.localScale = bodyScale;
	}

	public override void UpdatePosition()
	{
		if (!firstTouched && timeInterval > 0f)
			notePos.x = timeInterval * (g.bpm / 120.0f) * g.speed * -g.oneBeatToLine;
		else if (endTimeInterval >= 0f)
			notePos.x = 0f;
		else
			notePos.x = disappearDistance * -endTimeInterval / disappearDuration;
		notePos.y = 0f;
		this.transform.localPosition = notePos;
	}

	protected override void UpdateDisappear()
	{
		if (endTimeInterval < 0f)
		{
			firstTouched = false;
			if (endTimeInterval < -disappearDuration)
				g.noteSpawn.RemoveNote(this);
			else
			{
				float percent = 0.5f + endTimeInterval / disappearDuration * 0.5f;
				noteColor.a = percent;
				bodyColor.a = percent;
				endColor.a = percent;
			}
		}
	}

	public float GetLongProgress()
	{
		if (!longEnabled)
			return -1f;
		return 1f - Mathf.Clamp01(endTimeInterval / length);
	}
}
