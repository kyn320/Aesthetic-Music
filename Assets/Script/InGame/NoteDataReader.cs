using System.Collections.Generic;
using UnityEngine;

public class NoteDataReader
{
	public bool dataReaded = false;
	public List<NoteData> datas;
	public float startBPM = 120f;
	public float noteSync = 0f;

	private int dataVersion = 1;

	public void ReadData(string _ntd)
	{
		GameManager g = GameManager.instance;

		string[] lines = _ntd.Split('\n');
		datas = new List<NoteData>();
		NoteData data = null;
		foreach (string line in lines)
		{
			if (string.IsNullOrEmpty(line))
				break;

			string[] words = line.Split(':');
			switch (words[0])
			{
				case "NoteDataVersion":
					dataVersion = int.Parse(words[1]);
					break;
				case "StartBPM":
					startBPM = float.Parse(words[1]);
					break;
				case "NoteSync":
					noteSync = int.Parse(words[1]) / 1000f;
					break;
				case "NormalNote":
					data = new NoteData();
					data.lineNum = int.Parse(words[1]);
					data.time = int.Parse(words[2]) / 1000f;
					break;
				case "LongNote":
					data = new NoteData();
					data.lineNum = int.Parse(words[1]);
					data.time = int.Parse(words[2]) / 1000f;
					data.length = (int.Parse(words[3]) / 1000f) - data.time;
					break;
				case "DragNote":
					data = new NoteData();
					data.lineNum = int.Parse(words[1]);
					data.time = int.Parse(words[2]) / 1000f;
					data.drag = int.Parse(words[3]);
					break;
				case "BatterNote":
					data = new NoteData();
					data.batterHit = int.Parse(words[1]);
					data.time = int.Parse(words[2]) / 1000f;
					data.batterEndTime = int.Parse(words[3]) / 1000f;
					break;
				default:
					Debug.LogError("NoteDataReader::Type Unknown : " + words[0]);
					break;
			}

			if (data != null)
			{
				datas.Add(data);
				data = null;
			}
		}

		dataReaded = true;
	}
}
