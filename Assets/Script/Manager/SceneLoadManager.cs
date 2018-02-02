using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{

    public static SceneLoadManager instance;

    UnityEvent sceneAction = null;

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);


        instance = this;
    }

    /// <summary>
    /// 등록된 씬 로드 함수를 실행합니다.
    /// </summary>
    public void LoadScene()
    {
        sceneAction.Invoke();
    }

    public void LoadMain()
    {
        sceneAction.AddListener(delegate { SceneManager.LoadScene("Main"); });
    }

    public void LoadMatch()
    {
        sceneAction.AddListener(delegate { SceneManager.LoadScene("Match"); });
    }

    public void LoadInGame()
    {
        sceneAction.AddListener(delegate { SceneManager.LoadScene("GameScene"); });
    }

    public void LoadOption()
    {
        sceneAction.AddListener(delegate { SceneManager.LoadScene("Option"); });
    }


}
