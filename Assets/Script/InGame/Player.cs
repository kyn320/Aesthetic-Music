using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player instance;

    Animator ani;
    AudioSource audioPlayer;

    [SerializeField]
    bool isRun = false;

    public int dir = 1;
    public float moveSpeed = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ani = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        SetAniSpeed(1f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            isRun = !isRun;
            ani.SetBool("Run", isRun);
            if (isRun)
                moveSpeed = 0.3f;
            else
                moveSpeed = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            ani.SetTrigger("Jump");

        if (Input.GetKeyDown(KeyCode.Z))
            SetDir(-dir);

        BackgroundScroller.instance.scrollSpeed = dir * moveSpeed;
    }

    /// <summary>
    /// 일반 타격 액션을 취합니다.
    /// </summary>
    /// <param name="_lineNum">판정 방향</param>
    public void PlayAction(int _lineNum)
    {
        if (ConvertDir(_lineNum) == -1)
        {
            ani.SetTrigger("LeftArm");
            ani.SetFloat("LeftNormal", Random.Range(1, 1));
        }
        else if (ConvertDir(_lineNum) == 1)
        {
            ani.SetTrigger("RightArm");
            ani.SetFloat("RightNormal", Random.Range(1, 1));
        }
    }

    /// <summary>
    /// 지속 타격 액션을 취합니다.
    /// </summary>
    /// <param name="_lineNum">판정 방향</param>
    /// <param name="_during">진행 상황</param>
    public void PlayAction(int _lineNum, float _during)
    {

        if (Mathf.Abs(_during) == 1)
        {
            if (ConvertDir(_lineNum) == -1)
            {
                ani.SetFloat("LeftMaintain", -1);
            }
            else if (ConvertDir(_lineNum) == 1)
            {
                ani.SetFloat("RightMaintain", -1);
            }
            return;
        }

        if (ConvertDir(_lineNum) == -1)
        {
            ani.SetTrigger("LeftArm");
            ani.SetFloat("LeftNormal", -1);
            ani.SetFloat("LeftMaintain", _during);
        }
        else if (ConvertDir(_lineNum) == 1)
        {
            ani.SetTrigger("RightArm");
            ani.SetFloat    ("RightNormal", -1);
            ani.SetFloat("RightMaintain", _during);
        }

    }

    /// <summary>
    /// 방향을 지정합니다.
    /// </summary>
    /// <param name="_dir">-1 = Left , 1 = Right</param>
    public void SetDir(int _dir)
    {
        dir = _dir;

        if (_dir == -1)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        else
            transform.localRotation = Quaternion.Euler(0, 0, 0);

    }

    private int ConvertDir(int _lineNum)
    {
        return dir * (_lineNum == 0 ? -1 : 1);
    }

    public void ShotAudio()
    {
        audioPlayer.PlayOneShot(audioPlayer.clip, 0.5f);
    }

    /// <summary>
    /// 애니메이션의 스피드를 제어합니다.
    /// </summary>
    /// <param name="_speed"></param>
    public void SetAniSpeed(float _speed)
    {
        ani.speed = _speed;
    }

}
