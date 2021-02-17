using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using System.Linq;

[System.Serializable]
public class Node
{
    public Node(bool _isWall, int _x, int _y)
    {
        isWall = _isWall;
        x = _x; 
        y = _y;
    }

    public bool isWall;
    public Node ParentNode;

    public int x, y;
    
    // G : Start時点から移動した距離
    public int G;
    
    // H : |Width| + |Height| 障害物を無視して目標までの距離 
    public int H;
    
    //F : G + H
    public int F { get { return G + H; } }
}

public class Enemy : Controller
{
    const int WallLayer = 23;
     [SerializeField]
    private Vector2 movePosition;

    public Vector2Int vecBottomLeft;
    public Vector2Int vecTopRight;
    public Vector2Int vecStartPos;
    public Vector2Int vecTargetPos;
    public List<Node> finalNodeList = new List<Node>();
    
    public List<Vector2> enemyMoveList = new List<Vector2>();

    int sizeX, sizeY;
    public Node[,] NodeArray;
    Node startNode, targetNode, curNode;
    
    List<Node> OpenList = new List<Node>();
    List<Node> ClosedList = new List<Node>();

    public bool isMove = false;

    ///
    [SerializeField] Animator m_Animator;
    [SerializeField] Rigidbody2D m_Rigidbody;
    [SerializeField] SpriteRenderer m_SpriteRenderer;
    [SerializeField] float m_Speed = 2.0f;
    [SerializeField] Transform m_Target;
    [SerializeField] float m_StopDistance = 0.1f;
    [SerializeField] float m_Hp = 10;
    [SerializeField] bool isStun;

    Vector3Int vecCheckTile = Vector3Int.zero;

    public void MoveUpdate()
    {
        if(!isStun)
        {
            if (finalNodeList.Count <= 1) 
                return;

            Vector2 FinalNodePos = new Vector2(finalNodeList[1].x, finalNodeList[1].y);
                transform.position = Vector2.MoveTowards(transform.position, FinalNodePos, m_Speed * Time.deltaTime);

            if ((Vector2)transform.position == FinalNodePos)
                finalNodeList.RemoveAt(0);
        }
    }

    public void InitPosition(Transform playerPosition, Vector2Int _vecStartPosition , Vector2Int _vecTargetPosition)
    {
        m_Target = playerPosition;
        
        vecStartPos = _vecStartPosition;
        movePosition = transform.position;
        vecTargetPos = _vecTargetPosition;

        vecBottomLeft = new Vector2Int(Mathf.Min(_vecStartPosition.x, _vecTargetPosition.x) - 10, Mathf.Min(_vecStartPosition.y, _vecTargetPosition.y) - 10);
        vecTopRight = new Vector2Int(Mathf.Max(_vecStartPosition.x, _vecTargetPosition.x) + 10, Mathf.Max(_vecStartPosition.y, _vecTargetPosition.y) + 10);
    }

    public List<Vector2> GetEnemyMoveList()
    {
        return enemyMoveList;
    }
    
    public void PathFinding()
    {
        // NodeArrayのサーズを決めて, isWall, x, y 入れる
        sizeX = vecTopRight.x - vecBottomLeft.x + 1;
        sizeY = vecTopRight.y - vecBottomLeft.y + 1;
        NodeArray = new Node[sizeX, sizeY];

        var colliders = Physics2D.OverlapBoxAll(new Vector2(vecBottomLeft.x + (sizeX * 0.5f),vecBottomLeft.y + (sizeY * 0.5f)),
                                                         new Vector2(sizeX + 10, sizeY + 10),
                                                         0,
                                                         23 << WallLayer
                                                        ).ToList();
                                                        
        for (int nWidth = 0; nWidth < sizeX; nWidth++)
        {
            for (int nHeight = 0; nHeight < sizeY; nHeight++)
            {
                var isWall = colliders.Find((x) => x.OverlapPoint(new Vector2(nWidth + vecBottomLeft.x, nHeight + vecBottomLeft.y))) != null;
                NodeArray[nWidth, nHeight] = new Node(isWall, nWidth + vecBottomLeft.x, nHeight + vecBottomLeft.y);
            }
        }

        // StartNode, TargetNode, CloseNode,FinalNodeList Init
        startNode = NodeArray[vecStartPos.x - vecBottomLeft.x, vecStartPos.y - vecBottomLeft.y];
        targetNode = NodeArray[vecTargetPos.x - vecBottomLeft.x, vecTargetPos.y - vecBottomLeft.y];

        OpenList = new List<Node>() { startNode };
        ClosedList.Clear();
        enemyMoveList.Clear();
        finalNodeList.Clear();

        while (OpenList.Count > 0)
        {
            // OpenListの中で一番Fが小さいし、もしFが同じだったらHが小さいものを現在ノードにしてOpenListからCloseListに移動
            curNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= curNode.F && OpenList[i].H < curNode.H) curNode = OpenList[i];

            OpenList.Remove(curNode);
            ClosedList.Add(curNode);
            
            // 最後
            if (curNode == targetNode)
            {
                Node TargetCurNode = targetNode;
                
                while (TargetCurNode != startNode)
                {
                    finalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }

                finalNodeList.Add(startNode);
                finalNodeList.Reverse();

                return;
            }

             // ↗↖↙↘
            OpenListAdd(curNode.x + 1, curNode.y + 1);
            OpenListAdd(curNode.x - 1, curNode.y + 1);
            OpenListAdd(curNode.x - 1, curNode.y - 1);
            OpenListAdd(curNode.x + 1, curNode.y - 1);
            
            // ↑ → ↓ ←
            // 後でランダムで方向を変わるように修正
            // 今の状況だったら始める方向がほぼ同じ
            OpenListAdd(curNode.x, curNode.y + 1);
            OpenListAdd(curNode.x + 1, curNode.y);
            OpenListAdd(curNode.x, curNode.y - 1);
            OpenListAdd(curNode.x - 1, curNode.y);
        }
    }

    void OpenListAdd(int checkX, int checkY)
    {
        // 上下左右の範囲を出さず、壁ではなく、CloseListにいなければ、
        if (checkX >= vecBottomLeft.x && checkX < vecTopRight.x + 1 && 
            checkY >= vecBottomLeft.y && checkY < vecTopRight.y + 1 && 
            !NodeArray[checkX - vecBottomLeft.x, checkY - vecBottomLeft.y].isWall && !ClosedList.Contains(NodeArray[checkX - vecBottomLeft.x, checkY - vecBottomLeft.y]))
        {
            // if (NodeArray[curNode.x - vecBottomLeft.x, checkY - vecBottomLeft.y].isWall && NodeArray[checkX - vecBottomLeft.x, curNode.y - vecBottomLeft.y].isWall) 
            //     return;

            // 隣ノードに入れて、　直線は 10, 対角線は　14
            Node NeighborNode = NodeArray[checkX - vecBottomLeft.x, checkY - vecBottomLeft.y];
            int MoveCost = curNode.G + (curNode.x - checkX == 0 || curNode.y - checkY == 0 ? 10 : 14);
            
            //　移動Costが隣ノードGより小さいか、OpenListに隣ノードがないなら G, H, ParentNodeを設定した後にOpenListに追加 
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - targetNode.x) + Mathf.Abs(NeighborNode.y - targetNode.y)) * 10;
                NeighborNode.ParentNode = curNode;

                OpenList.Add(NeighborNode);
            }
        }
    }

    ///

    void Update()
    {
        if(isStun)
        {
            return;
        }
    }
    void TryMove()
    {
        Vector3 dir = m_Target.position - transform.position;
        // Debug.Log(string.Format("{0:F4} {1:F4}", dir.sqrMagnitude, m_StopDistance * m_StopDistance));
        if(dir.sqrMagnitude > m_StopDistance * m_StopDistance)
        {
            m_Rigidbody.velocity = dir.normalized * m_Speed;
        }else
        {
            m_Rigidbody.velocity = Vector3.zero;
        }
    }
    void TryDie()
    {
        if(m_Hp <= 0)
        {
            m_Animator.SetBool("Die", true);
            m_Rigidbody.velocity = Vector3.zero;
            this.enabled = false;
            Destroy(gameObject, 1f);
        }
    }

    public void _Stun(float sec)
    {
        if(!isStun)
        {
            StartCoroutine(Stun(sec));
        }
    }

    IEnumerator Stun(float sec)
    {
        isStun = true;
        m_Rigidbody.velocity = Vector3.zero;
        yield return new WaitForSeconds(sec);
        m_Rigidbody.velocity = Vector3.zero;
        isStun = false;
    }

    public void BeAttacked(int Damage, Vector2 Direaction, float KnockbackForce = 1f, float StunTime = 1f)
    {
        if(!isStun)
        {
            StartCoroutine(Stun(StunTime));
            // m_Animator.SetTrigger("Hit");
            m_Rigidbody.AddForce(Direaction * KnockbackForce, ForceMode2D.Impulse);
        }
    }
}

public struct AstarPathFind : IJobParallelForTransform
{

    public void Execute(int index, TransformAccess transform)
    {

    }
}
