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
	
	// Update is called once per frame
	void FixedUpdate () {
        //판정을 어떻게 해야 할까
        // 좌우 입력을 따로 처리 할까?
        // 눌렀을때 시간 체크?
        // 오차 시각 = Mathf.Abs(노트 시각 - 현재 시각);
        // 판정 Square = 0.016f;
        // if (오차 시각 <= Square) 네 판정 = Square;

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            CheckNote(-1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            CheckNote(1);
        }
	}

    void CheckNote(int _dir) {
        hit = Physics2D.Raycast(transform.position, Vector2.right * _dir, 5f);
        Debug.DrawRay(transform.position, Vector2.right * _dir * 5f);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.GetComponent<Note>().dir == _dir)
            {
                if (GameManager.instance.JudgeCheck(hit.collider.gameObject.GetComponent<Note>()))
                {
                    ShotAudio();
                    PlayAnimation(_dir);
                }
            }
        }
    }

    void PlayAnimation(int _dir) {
        if (dir != _dir)
        {
            
            ani.SetTrigger("Ani_2");
        }
        else {
            ani.SetTrigger("Ani_" + Random.Range(0,2));
        }
    }

    public void SetDir(int _dir) {
        dir = dir * _dir;
        transform.localScale = new Vector2(dir,1);
    }

    public void ShotAudio() {
        audioPlayer.PlayOneShot(audioPlayer.clip,0.5f);
    }

}
