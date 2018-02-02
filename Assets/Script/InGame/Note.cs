using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
	public const int N_NORMAL = 0;
	public const int N_LONG = 1;
	public const int N_DRAG = 2;
	public const int N_BATTER = 3;

	protected const float disappearDuration = 0.5f;
	protected const float disappearDistance = 3f;

	public float time = 0f;
	public int lineNum = 0;
	public NoteData data = null;

	protected float timeInterval = 0f;
    protected Vector3 notePos;
	protected Color noteColor = Color.white;
	protected SpriteRenderer spriteRenderer;
	protected bool missed = false;
	protected GameManager g;



    protected virtual void Awake()
    {
        g = GameManager.instance;
		spriteRenderer = this.GetComponent<SpriteRenderer>();
		gameObject.SetActive(false);
    }

	public virtual void Start()
	{
		missed = false;
		noteColor = Color.white;
		spriteRenderer.color = noteColor;
		this.transform.localScale = new Vector3(4f, 4f, 4f);
	}

	protected virtual void Update()
    {
		UpdatePosition();
		UpdateDisappear();
	}

    public virtual void UpdatePosition()
	{
		timeInterval = time - g.syncedTime;
		if (timeInterval > 0f)
			notePos.x = timeInterval * (g.bpm / 120.0f) * g.speed * -g.oneBeatToLine;
		else if (timeInterval > -NoteJudgement.judgePerfect)
			notePos.x = 0f;
		else
			notePos.x = disappearDistance * (-timeInterval - NoteJudgement.judgePerfect) / disappearDuration;
		notePos.z = timeInterval;
		this.transform.localPosition = notePos;
	}

	protected virtual void UpdateDisappear()
	{
		if (timeInterval < -NoteJudgement.judgePerfect)
		{
			if (!missed)
				MissNote();
			if (timeInterval < -NoteJudgement.judgePerfect - disappearDuration)
				g.noteSpawn.RemoveNote(this);
			else
			{
				float percent = 1f - (-timeInterval - NoteJudgement.judgePerfect) / disappearDuration;
				noteColor = new Color(1f, 1f, 1f, percent * 0.75f);
				spriteRenderer.color = noteColor;
			}
		}
	}

	protected void MissNote()
	{
		missed = true;
        g.noteJudgement.JudgementNote(Judge.Miss, this);
	}
}
