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
    [SerializeField] Transform m_StartSlime;

    Vector2Int minVecotr = new Vector2Int();
    Vector2Int maxVector = new Vector2Int();
    
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

    public void GetTilemapGrid()
    {
        if(m_StartSlime == null)
            return;

        Vector3Int vecPlayer = Convert(m_Target.position);


        Debug.LogWarning (m_Tilemap.GetTile(vecPlayer).name);

        int Slime_x = Mathf.RoundToInt(m_StartSlime.position.x);
        int Slime_y = Mathf.RoundToInt(m_StartSlime.position.y);
        
        int Target_x = Mathf.RoundToInt(m_Target.position.x);
        int Target_y = Mathf.RoundToInt(m_Target.position.y);

        int nMinX = Slime_x < Target_x ? Slime_x : Target_x;
        int nMinY = Slime_y < Target_y ? Slime_y : Target_y;

        int nMaxX = Slime_x < Target_x ? Target_x : Slime_x;
        int nMaxY = Slime_y < Target_y ? Target_y : Slime_y;
        
        minVecotr.x = nMinX;
        minVecotr.y = nMinY;

        maxVector.x = nMaxX;
        maxVector.y = nMaxY;

        Debug.LogWarning(minVecotr);
        Debug.LogWarning(maxVector);
    }

    public Vector3Int Convert(Vector3 _data)
    {
        return new Vector3Int(Mathf.RoundToInt(_data.x), Mathf.RoundToInt(_data.y),0);
    }

    // draw the grid :) 
	void OnDrawGizmos ()
	{
        if(m_StartSlime == null)
            return;
       Gizmos.matrix = transform.localToWorldMatrix;
 
		// set colours
		Color dimColor = new Color(0,0,0,255); 
		Color brightColor = new Color(0,0,0,255); 
 
		// draw the horizontal lines
		for (int x = minVecotr.x; x < maxVector.x+1; x++)
		{
			// find major lines
			Gizmos.color = dimColor;
			if (x == 0)
				Gizmos.color = brightColor;
 
			Vector3 pos1 = new Vector3(x, minVecotr.y, 0) * 1;  
			Vector3 pos2 = new Vector3(x, maxVector.y, 0) * 1;  
 
			// convert to topdown/overhead units if necessary
			if (false)
			{
				pos1 = new Vector3(pos1.x, 0, pos1.y); 
				pos2 = new Vector3(pos2.x, 0, pos2.y); 
			}
 
			Gizmos.DrawLine ((Vector3.zero + pos1), (Vector3.zero + pos2)); 
		}
 
		// draw the vertical lines
		for (int y = minVecotr.y; y < maxVector.y + 1; y++)
		{
			// find major lines
			Gizmos.color = dimColor;
			if (y == 0)
				Gizmos.color = brightColor;
 
			Vector3 pos1 = new Vector3(minVecotr.x, y, 0) * 1;  
			Vector3 pos2 = new Vector3(maxVector.x, y, 0) * 1;  
 
			// convert to topdown/overhead units if necessary
			if (false)
			{
				pos1 = new Vector3(pos1.x, 0, pos1.y); 
				pos2 = new Vector3(pos2.x, 0, pos2.y); 
			}
 
			Gizmos.DrawLine ((Vector3.zero + pos1), (Vector3.zero + pos2)); 
		}
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
