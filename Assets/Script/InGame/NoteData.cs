using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteData
{
	public float time = 0f;
	public int lineNum = 0;

	public float length = 0f;

	public int drag = 0;

	public float batterEndTime = 0f;
	public int batterHit = 0;

	public int noteType
	{
		get
		{
			if (batterHit > 0)
				return Note.N_BATTER;
			if (drag != 0)
				return Note.N_DRAG;
			if (length > 0f)
				return Note.N_LONG;
			return Note.N_NORMAL;
		}
	}
}