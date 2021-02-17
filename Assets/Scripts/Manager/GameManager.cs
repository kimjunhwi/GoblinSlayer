using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using junkaki;

public class GameManager : GenericMonoSingleton<GameManager>
{
    [Tooltip("Managers")]
    
    [SerializeField]
    DataManager dataManager;


    public DataManager _DataManager { get; set; }

    void Awake()
    {
        DontDestroyOnLoad(this);
        
        _DataManager = dataManager;
    }

    public void Load(string strSceneName)
    {
        SceneManager.LoadScene(strSceneName, LoadSceneMode.Single);
    }
}
