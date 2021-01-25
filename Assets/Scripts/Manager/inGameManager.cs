using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using junkaki;

public class inGameManager : GenericMonoSingleton<inGameManager>
{
    public enum GameState
    {
        GamePlay,
        GamePause,
        GameOver,
    }

    [SerializeField]
    EnemyController enemyController;
    
    [SerializeField]
    Player player;


    public void Start()
    {
        enemyController.Init(player.transform);

        enemyController.RespawnEnemy();
        enemyController.RespawnEnemy();
    }
}
