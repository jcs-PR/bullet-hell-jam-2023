using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    [SerializeField] private TwoD_Grid grid;
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D enemyRb;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] public bool follow = false;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float followRange;
    [SerializeField] private float safeRange;
    [SerializeField] private float attackRange;
    public TwoD_Node targetNode;

    List<TwoD_Node> path = new List<TwoD_Node>();
    int pathIndex = 0;
    float checkDelay = 0.2f;
    Vector2 curVelocity;
    bool gotPath;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        MoveRandomly();
    }

    // Update is called once per frame
    void Update()
    {
        if (gotPath)
            StartCoroutine(CheckForPath());

        GetTargetPos();

    }

    private void FixedUpdate()
    {
        if (follow)
        {
            FollowPlayer();
        }
        else
            enemyRb.velocity = Vector2.zero;
    }

    private void GetPathToTarget()
    {
        follow = gotPath = false;
        pathIndex = 0;
        path.Clear();
        PathManager.WantPath.moveTowards.Add(this);
    }

    public void GetVariables(bool _gotPath, List<TwoD_Node> _path)
    {
        follow = gotPath = _gotPath;

        path = _path;
    }

    private void FollowPlayer()
    {
        Vector2 moveDir = path[pathIndex].nodePosition - transform.position;
        enemyRb.velocity = moveDir.normalized * moveSpeed * Time.deltaTime;
    }

    private IEnumerator CheckForPath()
    {
        yield return new WaitForSeconds(checkDelay);

        //Stop at target
        if (grid.NodeFromWorldPosition(transform.position) == targetNode)
        {
            MoveRandomly();
        }
        else if (grid.NodeFromWorldPosition(transform.position) == path[pathIndex])
            pathIndex++;

        
    }

    private void GetTargetPos()
    {
        if(Physics2D.OverlapCircle(transform.position, followRange, playerMask))
        {
            Vector3 dir = player.position - transform.position;
            Vector3 targetPos = transform.position + (dir.normalized * (followRange - safeRange));

        }
    }

    private void MoveRandomly()
    {
        Vector2 targetPos = transform.position + Vector3.ClampMagnitude(new Vector3(Random.Range(-followRange, followRange),
            Random.Range(-followRange, followRange)), followRange);
        targetNode = grid.NodeFromWorldPosition(targetPos);
        if (!targetNode.walkable || grid.NodeFromWorldPosition(transform.position) == targetNode)
        {
            MoveRandomly();
            return;
        }

        GetPathToTarget();
        
    }

    private void OnDrawGizmos()
    {
        Color blue = Color.blue;
        blue.a = 0.5f;
        Color black = Color.black;
        black.a = 1f;
        Color red = Color.red;
        red.a = 0.8f;
        Color green = Color.green;
        green.a = 0.8f;

        if (grid != null)
        {
            foreach (TwoD_Node n in path)
            {
                if (grid.showPath && path != null)
                {
                    if (path.Contains(n))
                    {
                        if (n != path[path.Count-1])
                        {
                            Gizmos.color = blue;
                            Gizmos.DrawCube(n.nodePosition, Vector3.one * (grid.nodeDiameter - 0.01f));
                        }
                    }
                }
                if (!n.walkable)
                {
                    Gizmos.color = black;
                    Gizmos.DrawCube(n.nodePosition, Vector3.one * (grid.nodeDiameter - 0.1f));

                }
            }
            if (path.Count > 0 && grid.showStartToTarget)
            {
                Gizmos.color = green;
                Gizmos.DrawCube(path[0].nodePosition, Vector3.one * (grid.nodeDiameter + 0.1f));
                Gizmos.color = red;
                Gizmos.DrawCube(path[path.Count-1].nodePosition, Vector3.one * (grid.nodeDiameter + 0.1f));
            }
        }

        Gizmos.DrawWireSphere(transform.position, followRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, safeRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
