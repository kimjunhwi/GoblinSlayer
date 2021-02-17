using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    

    Vector2Int beforVector = new Vector2Int();

    public void Init(Transform _playerPosition)
    {
        enemy_List.Clear();

        playerPosition = _playerPosition;

        RespawnEnemy();
        RespawnEnemy();
    }

    public void RespawnEnemy()
   {
        GameObject enemyObject = PoolManager.Get("Slime");

        Vector3 createEnemyPosition = new Vector3();

		createEnemyPosition.x = Random.Range(-1, 1);
		createEnemyPosition.y = Random.Range(-1, 1);

        createEnemyPosition.x *= Random.Range(10,20);
        createEnemyPosition.y *= Random.Range(10,20);

        enemyObject.transform.position = playerPosition.position + (createEnemyPosition);

        Enemy enemy = enemyObject.GetComponent<Enemy>();

        enemy_List.Add(enemy);
   }

   public void Update()
   {
       var player = new Vector2Int(Mathf.RoundToInt(playerPosition.position.x),Mathf.RoundToInt(playerPosition.position.y));

       foreach (var enemy in enemy_List)
       {     
            beforVector = player;

            enemy.InitPosition(playerPosition, new Vector2Int(Mathf.RoundToInt(enemy.transform.position.x),Mathf.RoundToInt(enemy.transform.position.y)),
                                        beforVector);
            enemy.PathFinding();

            enemy.MoveUpdate();
       }
   }

   public void EnemyMove ()
   {
       
   }
}
