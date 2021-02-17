using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    bool isNextScene = false;
    void Start()
    {
        GameManager.Instance._DataManager.Load();
    }

    void Update()
    {
        // 추후 수정
        if(GameManager.Instance._DataManager.GetDownloadEnd() && isNextScene == false)
        {
            isNextScene = true;
            GameManager.Instance.Load("GameScene");
        }
    }
}
