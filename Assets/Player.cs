using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Animator ani;
    AudioSource audioPlayer;
    RaycastHit2D hit;

    public int dir = 1;

	// Use this for initialization
	void Start () {
        ani = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
	}


	void Update () {

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            CheckNote(-1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            CheckNote(1);
        }
	}

    void CheckNote(int _dir)
    {
        Note targetNote = null;
        GameManager g = GameManager.instance;
        for (int i = 0; i < g.noteList.Count; ++i)
        {
            Note n = g.noteList[i];
            if (n.dir != _dir)
                continue;
            if (!g.JudgeCheck(n))
                continue;
               
            if (targetNote == null)
                targetNote = n;
            else if (n.noteTime < targetNote.noteTime)
                targetNote = n;
        }

        if (targetNote == null)
            return;

        g.noteList.Remove(targetNote);
        Destroy(targetNote.gameObject);
        g.SetDebugText("Success");
        ShotAudio();
        PlayAnimation(_dir);
    }

    void PlayAnimation(int _dir) {
        if (dir != _dir)
        {
            
            ani.SetTrigger("Ani_R");
        }
        else {
            ani.SetTrigger("Ani_2");
        }
    }

    public void SetDir(int _dir) {
        dir = dir * _dir;
        transform.parent.localScale = new Vector2(dir,1);
    }

    public void ShotAudio() {
        audioPlayer.PlayOneShot(audioPlayer.clip,0.5f);
    }

}
