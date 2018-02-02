using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawn : MonoBehaviour
{
	[HideInInspector]
	public List<Note> notes;

	public GameObject[] notePrefabs;

	private List<NoteData> noteDatas;
	private List<Note>[] notePools;
	private GameManager g;
	private NoteDataReader reader;



	private void Awake()
	{
		notes = new List<Note>();
		noteDatas = new List<NoteData>();
	}

    private void Start()
    {
        g = GameManager.instance;

        int maxType = 4;
        notePools = new List<Note>[maxType];
        for (int type = 0; type < maxType; ++type)
        {
            notePools[type] = new List<Note>();
			for (int i = 0; i < 5; ++i)
			{
				notePools[type].Add(Instantiate(notePrefabs[type]).GetComponent<Note>());
				if (type == Note.N_BATTER) // 연타 노트는 한 개만 생성.
					break;
            }
        }

		// StartCoroutine(DebugDragNote());
		TextAsset noteDataString = Resources.Load<TextAsset>(g.noteDataPath);
		reader = new NoteDataReader();
		reader.ReadData(noteDataString.text);

		noteDatas = reader.datas;
		g.bpm = reader.startBPM;
		g.noteSync = reader.noteSync;
    }

    private void Update()
    {
        for (int i = 0; i < noteDatas.Count; ++i)
        {
            NoteData data = noteDatas[i];
            if ((data.noteType == Note.N_BATTER && data.time > g.syncedTime) || 
				(data.time > g.noteMaxTime))
                continue;
            SpawnNote(data);
            noteDatas.RemoveAt(i--);
        }
    }

    //TEMP Normal & Long Note
    private IEnumerator DebugNormalAndLongNote()
    {
        int count = 0;
        int lineNum = 0;

        for (int i = 1; i <= 4; i++)
        {
            NoteData data = new NoteData();
            data.time = (i * 4f) * (120f / g.bpm);
            data.lineNum = lineNum;
            data.length = 2f * (120f / g.bpm);
            noteDatas.Add(data);

            lineNum = 1 - lineNum;
        }

        while (true)
        {
            if (noteDatas.Count < 8)
            {
                for (int i = 1; i <= 4; i++)
                {
                    NoteData data = new NoteData();
                    data.time = (count * 4f + i) * (120f / g.bpm);
                    data.lineNum = lineNum;
                    noteDatas.Add(data);
                }
                lineNum = 1 - lineNum;
                ++count;
            }



            yield return null;
        }
    }

    private IEnumerator DebugNormalNote()
    {
        int count = 0;
        int lineNum = 0;

        while (true)
        {
            if (noteDatas.Count < 8)
            {
                for (int i = 1; i <= 4; i++)
                {
                    NoteData data = new NoteData();
                    data.time = (count * 4f + i) * (120f / g.bpm);
                    data.lineNum = lineNum;
                    noteDatas.Add(data);
                }
                lineNum = 1 - lineNum;
                ++count;
            }

            yield return null;
        }
    }

    private IEnumerator DebugLongNote()
    {
        int lineNum = 0;

        for (int i = 1; i <= 19; i++)
        {
            NoteData data = new NoteData();
            data.time = (i * 4f) * (120f / g.bpm);
            data.lineNum = lineNum;
            data.length = 4f * (120f / g.bpm);
            noteDatas.Add(data);

            lineNum = 1 - lineNum;
        }
        yield return null;
    }

    private IEnumerator DebugDragNote()
    {
        int count = 0;
        int lineNum = 0;

        while (true)
        {
            if (noteDatas.Count < 8)
            {
                int drag = -1;
                for (int i = 1; i <= 8; i++)
                {
                    NoteData data = new NoteData();
                    data.time = (count * 4f + i * 0.5f) * (120f / g.bpm);
                    data.lineNum = lineNum;
					data.drag = drag;
                    // data.drag = Random.Range(0, 2) == 0 ? -1 : 1;
                    noteDatas.Add(data);

                    drag *= -1;
                }
                lineNum = 1 - lineNum;
                ++count;
            }

            yield return null;
        }
    }
	
	private IEnumerator DebugAllNote()
	{
		int count = 0;
		int lineNum = 0;

		while (true)
		{
			if (noteDatas.Count < 4)
			{
				switch (count % 4)
				{
					case Note.N_NORMAL:
						for (int i = 0; i < 4; i++)
						{
							NoteData data = new NoteData();
							data.time = (count * 4f + i) * (120f / g.bpm);
							data.lineNum = lineNum;
							noteDatas.Add(data);
						}
						break;
					case Note.N_LONG:
						for (int i = 0; i < 2; i++)
						{
							NoteData data = new NoteData();
							data.time = (count * 4f + i * 2f) * (120f / g.bpm);
							data.lineNum = lineNum;
							data.length = 1.75f * (120f / g.bpm);
							noteDatas.Add(data);
						}
						break;
					case Note.N_DRAG:
						for (int i = 0; i < 4; i++)
						{
							NoteData data = new NoteData();
							data.time = (count * 4f + i) * (120f / g.bpm);
							data.lineNum = lineNum;
							data.drag = Random.Range(0, 2) == 0 ? -1 : 1;
							noteDatas.Add(data);
						}
						break;
				}
				lineNum = 1 - lineNum;
				++count;
			}

			yield return null;
		}
	}

	private IEnumerator DebugBatterNote()
	{
		int count = 0;

		while (true)
		{
			if (noteDatas.Count < 2)
			{
				NoteData data = new NoteData();
				data.time = (count * 5f) * (120f / g.bpm);
				data.batterEndTime = (count * 5f + 3f) * (120f / g.bpm);
				data.batterHit = Random.Range(4, 17);
				data.lineNum = 0;
				noteDatas.Add(data);
				
				++count;
			}

			yield return null;
		}
	}

	private void SpawnNote(NoteData _data)
    {
        Note note = PopNote(_data.noteType);
        note.data = _data;
        note.time = _data.time;
        note.lineNum = _data.lineNum;
		note.transform.parent = g.lines[_data.lineNum];

		switch (_data.noteType)
        {
            case Note.N_LONG:
                LongNote ln = (LongNote)note;
				ln.UpdateTimeInterval();
				ln.length = _data.length;
                break;
            case Note.N_DRAG:
                DragNote dn = (DragNote)note;
                dn.drag = _data.drag;
                break;
			case Note.N_BATTER:
				BatterNote bn = (BatterNote)note;
				bn.maxHit = _data.batterHit;
				bn.endTime = _data.batterEndTime;
				note.transform.parent = g.lines[0].parent;
				break;
		}

		note.UpdatePosition();
        note.gameObject.SetActive(true);
        note.Start();

        notes.Add(note);
    }

    private void PushNote(Note _note)
    {
        _note.gameObject.SetActive(false);
        notePools[_note.data.noteType].Add(_note);
    }

    private Note PopNote(int _noteType)
    {
        Note note = null;
        if (notePools[_noteType].Count > 0)
        {
            note = notePools[_noteType][0];
            notePools[_noteType].RemoveAt(0);
        }
        else
            note = Instantiate(notePrefabs[_noteType]).GetComponent<Note>();
        return note;
    }

	public void RemoveNote(Note _note)
    {
        notes.Remove(_note);
        PushNote(_note);
    }
}
