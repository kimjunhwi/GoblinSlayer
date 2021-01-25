using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map_Generator : MonoBehaviour
{
    [SerializeField] Grid m_Grid;
    [SerializeField] Tilemap m_Tilemap;
    [SerializeField] TileBase[] m_TileBackground;
    [SerializeField] Vector2Int m_CardSize = new Vector2Int(32, 32);
    [SerializeField] Vector2Int m_Offset = new Vector2Int(-16, -16);
    [SerializeField] Transform m_Target;
    
    public void Generate_Tilemap()
    {
        Vector3Int[] positions = new Vector3Int[m_CardSize.x * m_CardSize.y];
        TileBase[] tileArray = new TileBase[positions.Length];

        for (int index = 0; index < positions.Length; index++)
        {
            positions[index] = new Vector3Int(index % m_CardSize.x + m_Offset.x, index / m_CardSize.x + m_Offset.y, 0);
            tileArray[index] = m_TileBackground[Random.Range(0, m_TileBackground.Length)];
        }
        m_Tilemap.SetTiles(positions, tileArray);
    }

    public void Generate_Tilemap(Vector3Int m_Offset)
    {
        Vector3Int[] positions = new Vector3Int[m_CardSize.x * m_CardSize.y];
        TileBase[] tileArray = new TileBase[positions.Length];

        for (int index = 0; index < positions.Length; index++)
        {
            positions[index] = new Vector3Int(index % m_CardSize.x + m_Offset.x, index / m_CardSize.x + m_Offset.y, 0);
            tileArray[index] = m_TileBackground[(Random.Range(0,100) > 15)?0:Random.Range(1, m_TileBackground.Length)];
        }
        m_Tilemap.SetTiles(positions, tileArray);
    }

    public Vector3Int GetGridPosition(Vector3 worldPos)
    {
        return new Vector3Int(Mathf.RoundToInt(worldPos.x / m_Tilemap.cellSize.x), Mathf.RoundToInt(worldPos.y / m_Tilemap.cellSize.y), 0);
    }
    public Vector3Int GetCardPosition(Vector3 worldPos)
    {
        Vector3Int gridPos = GetGridPosition(worldPos);
        return new Vector3Int(Mathf.RoundToInt(gridPos.x / m_CardSize.x), Mathf.RoundToInt(gridPos.y / m_CardSize.y), 0);
    }

    void Update()
    {
        TryGenerateTileMap();
    }

    void TryGenerateTileMap()
    {
        Vector3Int now = GetCardPosition(m_Target.position);
        // 9 grid
        int[,] D = new int[9, 2] { { 0, 0 }, { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 }, { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };
        for (int d = 0; d < 9; ++d)
        {
            Vector3Int there = new Vector3Int(
                (now.x + D[d, 0]) * m_CardSize.x + m_Offset.x,
                (now.y + D[d, 1]) * m_CardSize.y + m_Offset.y,
                0);
            // Debug.Log(string.Format("there : {0}", there));
            if (ReferenceEquals(m_Tilemap.GetTile(there), null))
            {
                Generate_Tilemap(there);
            }
        }
    }

    public void ClearAllTiles()
    {
        m_Tilemap.ClearAllTiles();
    }
}
