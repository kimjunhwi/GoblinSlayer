using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using junkaki;
using UnityEngine.Tilemaps;
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

    [SerializeField] Tilemap m_tileMap;
    [SerializeField] TilemapRenderer m_tileRender;

    public Dictionary<Vector3Int,bool> colliderList = new Dictionary<Vector3Int,bool>();


    public void Start()
    {
        enemyController.Init(player.transform);

        enemyController.RespawnEnemy();
        // enemyController.RespawnEnemy();
        

        // foreach (Vector3Int item in m_tileMap.cellBounds.)
        // {
        //     TileBase tile = m_tileMap.GetTile(item);

        //     if(tile != null)
        //         colliderList.Add(item,true);
        // }

        //     var testVector1 = new Vector3Int(-4,-2,0);
        //     var testVector2 = new Vector3Int(-10,-2,0);

        // Debug.LogWarning(colliderList.ContainsKey(testVector1));
        // Debug.LogWarning(colliderList.ContainsKey(testVector2));
    }

    public bool isTileCollider(Vector3Int vecPosition)
    {
        return colliderList.ContainsKey(vecPosition);
    }
}
