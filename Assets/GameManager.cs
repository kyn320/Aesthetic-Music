using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public float bpm;
    public float currentTime;

    public float speed;
    public float sync;

    public Transform[] judgeLine;

    public GameObject note;


    //어떻게 만들죠?
    //음 노트가 어떻게 해야 움직이게 할까요.
    // noteX = (판정선의 X좌표) + (노트 시간 - (현재 시간 + 싱크) ) * 속도  * (곡의 BPM / 기준 BPM(60) )
    //  움직임은 Position = new Vector(x,y,z); 

    private void Awake()
    {
        instance = this;
    }


    // Use this for initialization
    void Start () {
        int dir = 1;
        for (int i = 1; i <= 20; i++) {
            GameObject g = Instantiate(note);
            g.GetComponent<Note>().noteTime = i * 0.8f;
            g.GetComponent<Note>().dir = dir * -1;
            dir *= -1; 
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
    }




}
