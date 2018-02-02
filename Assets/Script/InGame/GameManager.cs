using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
	public static float dragMinDistance = 200f;
	public static float dragMinDelta = 6f;

	public const float maxBeatInLine = 4f;
	public const float lineLength = 12.0f;
	public const float lineHeight = 6.5f;

	// [HideInInspector]
	public float bpm = 120.0f;
	// [HideInInspector]
	public float speed = 1f;
	[HideInInspector]
	public float musicSync = 0f;
	[HideInInspector]
	public float noteSync = 0f;
	// [HideInInspector]
	public float noteMinTime = 0f;
	// [HideInInspector]
	public float noteMaxTime = 0f;
	
	[HideInInspector]
	public NoteSpawn noteSpawn;
	[HideInInspector]
	public NoteJudgement noteJudgement;
	[HideInInspector]
	public int combo = 0;
	[HideInInspector]
	public string noteDataPath;
	[HideInInspector]
	public float time = -3f;

	public float syncedTime
	{
		get
		{
			return time + musicSync + noteSync;
		}
	}
	public float oneBeatToLine
	{
		get
		{
			return lineLength / maxBeatInLine;
		}
	}

	public Transform[] lines;
	public Text debugText;

	private MusicManager m;
	private GameUIManager u;



	private void Awake()
    {
        instance = this;

		noteSpawn = this.GetComponentInChildren<NoteSpawn>();
		noteJudgement = this.GetComponentInChildren<NoteJudgement>();

		float widthRatio = Screen.width / 1920f;
        dragMinDistance = 200f * widthRatio;
		dragMinDelta = 6f * widthRatio;
	}

	private void Start()
    {
		m = MusicManager.instance;
		u = GameUIManager.instance;

		DebugLoad();
		StartCoroutine(StartDelay());
    }

	private IEnumerator StartDelay()
	{
		time = -3f;
		m.Stop();
		while (time < 0f)
			yield return null;

		m.Play();
		yield return null;
		time = 0f;
		m.musicTime = 0f;
	}

	private void DebugLoad()
	{
		noteDataPath = "NoteDatas/DARK FORCE_Hard";
		AudioClip music = Resources.Load<AudioClip>("Musics/DARK FORCE");
		m.LoadMusic(music);
	}

    private void Update()
    {
		time += Time.deltaTime;
		// if (Mathf.Abs(time - m.musicTime) >= 0.075f)
		// 	time = m.musicTime;
		UpdateTimeLimits();

		debugText.text = 
			  "D : " + (time - m.musicTime).ToString("F3") + 
			", M : " + m.musicTimeS.ToString("F3") + 
			", S : " + syncedTime.ToString("F3") + 
			", E : " + Time.deltaTime;
    }

	public Vector3 ScreenToLinePosition(Vector2 _scrn, int _lineNum)
	{
		Vector3 world = Camera.main.ScreenToWorldPoint(_scrn);
		return lines[_lineNum].InverseTransformPoint(world);
	}

	private void UpdateTimeLimits()
	{
		float length = MusicManager.instance.musicLength;
		noteMinTime = Mathf.Clamp(syncedTime - NoteJudgement.judgePerfect, 0f, length);
		noteMaxTime = Mathf.Clamp(syncedTime + maxBeatInLine * (120f / bpm) / speed, noteMinTime, length);
	}

	public void SetDebugText(string data)
	{
		debugText.text = data;
	}
}
