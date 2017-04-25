using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public static readonly float JUDGEMENT_TIME = 0.16f; // 40 (80) frames;

    public float bpm;
    public float currentTime;

    public float speed;
    public float sync;

    public Transform[] judgeLine;

    public GameObject note;

    public List<Note> noteList;


    public Text debugText;

    //어떻게 만들죠?
    //음 노트가 어떻게 해야 움직이게 할까요.
    // noteX = (판정선의 X좌표) + (노트 시간 - (현재 시간 + 싱크) ) * 속도  * (곡의 BPM / 기준 BPM(60) )
    // 움직임은 Position = new Vector(x,y,z); 

    private void Awake()
    {
        instance = this;
    }


    // Use this for initialization
    void Start()
    {
        DebugNote();
        
    }


    void DebugNote() {
        int dir = 1;
        int cnt = 1;
        for (int j = 1; j <= 7; j++) {
            dir *= -1;
            for (int i = 1; i <= 10; i++)
            {
                
                GameObject g = Instantiate(note);
                g.transform.parent = transform;
                noteList.Add(g.GetComponent<Note>());
                g.GetComponent<Note>().noteTime = cnt;
                g.GetComponent<Note>().dir = dir;
                cnt++;
            }
        }
        
    }


    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;

    }

    public bool JudgeCheck(Note note) {
        float over = Mathf.Abs(note.noteTime - currentTime);

        if (over <= JUDGEMENT_TIME)
        {
            return true;
        }
        else
            return false;
    }

    public void SetDebugText(string data)
    {
        debugText.text = data;
    }


}
