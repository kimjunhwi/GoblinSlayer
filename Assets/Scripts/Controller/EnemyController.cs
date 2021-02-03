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
    
    

    Vector2Int beforVector = new Vector2Int();

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

        Enemy enemy = enemyObject.GetComponent<Enemy>();

        enemy_List.Add(enemy);
   }

   public void Update()
   {
       if(enemy_List.Count != 0)
        {
            var player = new Vector2Int(Mathf.RoundToInt(playerPosition.position.x),Mathf.RoundToInt(playerPosition.position.y));
            
            enemy_List[0].MoveUpdate();
            
            if(beforVector == player)
                return;

            beforVector = player;

            Debug.LogWarning("EnemyPosition : " + new Vector2Int(Mathf.RoundToInt(enemy_List[0].transform.position.x),Mathf.RoundToInt( enemy_List[0].transform.position.y)));
            Debug.LogWarning("PlayerPosition : " + beforVector);

            enemy_List[0].InitPosition(playerPosition, new Vector2Int(Mathf.RoundToInt(enemy_List[0].transform.position.x),Mathf.RoundToInt( enemy_List[0].transform.position.y)),
                                        beforVector);
            enemy_List[0].PathFinding();
            
        }
   }

   public void EnemyMove ()
   {
       
   }
}
