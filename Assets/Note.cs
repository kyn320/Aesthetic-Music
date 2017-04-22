using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{

    public static GameManager gameManager;

    public float noteTime;
    

    public int dir = -1;
    private Vector2 notePos;
    private Transform tr;

    private void Awake()
    {
        gameManager = GameManager.instance;
        tr = GetComponent<Transform>();

    }


    private void FixedUpdate()
    {
        if (dir < 0)
            notePos.x = gameManager.judgeLine[0].position.x + (dir * (noteTime - (gameManager.currentTime + gameManager.sync)) * gameManager.speed * (gameManager.bpm / 60));
        else
            notePos.x = gameManager.judgeLine[1].position.x + (dir * (noteTime - (gameManager.currentTime + gameManager.sync)) * gameManager.speed * (gameManager.bpm / 60));
        tr.position = notePos;

        if (noteTime < gameManager.currentTime - 0.5f)
        {
            gameManager.SetDebugText("Fail");
            gameManager.noteList.Remove(this);
            Destroy(this.gameObject);
        }

    }


    
}
