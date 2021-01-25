using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using junkaki;

public class EnemyController : MonoBehaviour
{
    List<Enemy> enemy_List = new List<Enemy>();

    Transform playerPosition = null;

    const int nMaxAmount = 500;
    const int nMinCreateMonster = 5;
    const int nMaxCreateMonsterAmount = 30;
    const float fMonsterCreateTime = 5f;

    // 30초 마다 몹 생성 개수 + 1 (최대 30) 
    int nCreateMonsterAmount = 5;
    
    

    public void Init(Transform _playerPosition)
    {
        enemy_List.Clear();

        playerPosition = _playerPosition;
    }

    public void RespawnEnemy()
   {
        GameObject enemyObject = PoolManager.Get("Slime");

        Vector3 createEnemyPosition = new Vector3();

		createEnemyPosition.x = Random.Range(-1, 1);
		createEnemyPosition.y = Random.Range(-1, 1);

        enemyObject.transform.position = playerPosition.position + (createEnemyPosition * Random.Range(5, 10));

        // Enemy enemy = enemyObject.GetComponent<Enemy>();

        // enemy_List.Add(enemy);
   }
}
