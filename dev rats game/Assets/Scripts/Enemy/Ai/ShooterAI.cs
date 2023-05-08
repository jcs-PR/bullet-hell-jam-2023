using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterAI : MonoBehaviour
{
    [SerializeField] private TwoD_Grid grid;
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D enemyRb;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float followRange;
    [SerializeField] private float safeRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float maxTension = 100;
    [SerializeField] private float tensionRate;
    [HideInInspector] public TwoD_Node targetNode;

    //Behaviours
    enum Behaviour {Search, Follow, Attack, Run, Investigate};
    Behaviour currentBehaviour;
    Behaviour previousBehaviour;

    //private variables
    List<TwoD_Node> path = new List<TwoD_Node>();
    TwoD_Node playerNode;
    Vector2 curVelocity;
    Vector2 moveDir;
    int pathIndex;
    float checkDelay = 0.1f;
    bool gotPath;
    bool inFollowRange;
    float size;
    bool usingPathfinder = false;
    bool hasSeenPlayer = false;
    bool move;
    float tension;
    Shoot shoot;

    private void Awake()
    {
        grid = GameObject.Find("A*").GetComponent<TwoD_Grid>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        size = GetComponent<CircleCollider2D>().radius;
        enemyRb = GetComponent<Rigidbody2D>();
        shoot = GetComponent<Shoot>();
        GetComponent<Aim>().maxRange = attackRange;
    }
    // Start is called before the first frame update
    void Start()
    {
        MoveRandomly();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(RefreshAI());
        StartCoroutine(CheckPath());

        if(currentBehaviour == Behaviour.Attack || currentBehaviour == Behaviour.Run)
            shoot.canAttack = true;
        else
            shoot.canAttack = false;
    }

    private void FixedUpdate()
    {
        if (move)
            MoveTowardsTarget();
        else
            enemyRb.velocity = Vector2.zero;
    }

    private void GetPathToTarget()
    {
        move = usingPathfinder = gotPath = false;
        pathIndex = 0;
        path.Clear();
        Vector2 vector = targetNode.nodePosition - transform.position;

        if (Physics2D.CircleCast(transform.position, size, vector.normalized, vector.magnitude, obstacleMask))
        {
            usingPathfinder = true;
            PathManager.WantPath.AI.Add(this);
        }

        else
        {
            moveDir = targetNode.nodePosition - transform.position;
            shoot.RotateTowardsTarget(targetNode.nodePosition);
            List<TwoD_Node> node = new List<TwoD_Node>();
            node.Add(targetNode);
            GetVariables(true, node);
        }
    }

    private void MoveTowardsTarget()
    {
        if (usingPathfinder)
        {
            Vector3 node = path[pathIndex].nodePosition;
            moveDir = node - transform.position;
            shoot.RotateTowardsTarget(node);
        }

        enemyRb.velocity = moveDir.normalized * moveSpeed * Time.deltaTime;
    }

    public void GetVariables(bool _gotPath, List<TwoD_Node> _path)
    {
        move = gotPath = _gotPath;
        path = _path;
    }

    private IEnumerator CheckPath()
    {
        yield return new WaitForSeconds(checkDelay);
        if (!gotPath)
            yield return null;

        //Stop at target
        if (grid.NodeFromWorldPosition(transform.position) == targetNode)
        {
            if (currentBehaviour == Behaviour.Search)
                MoveRandomly();
            else if (currentBehaviour == Behaviour.Attack)
            {
                move = false;
                Attack();
            }
        }
        else if (usingPathfinder && grid.NodeFromWorldPosition(transform.position) == path[pathIndex])
        {
            pathIndex++;
        }

    }

    private IEnumerator RefreshAI()
    {
        yield return new WaitForSeconds(checkDelay);

        CheckPlayerPos();

        if (currentBehaviour == Behaviour.Follow && playerNode !=
            grid.NodeFromWorldPosition(player.position))
        {
            MoveInAttackRange();
        }

        if (previousBehaviour != currentBehaviour)
        {
            if(currentBehaviour == Behaviour.Attack || currentBehaviour == Behaviour.Run)
            {
                Attack();

                if (currentBehaviour == Behaviour.Run)
                    RunAwayFromPlayer();
            }

            else if (currentBehaviour == Behaviour.Investigate)
            {
                ToLastPostion();
            }
        }
            previousBehaviour = currentBehaviour;

    }

    private void CheckPlayerPos()
    {
        Vector2 vector = player.position - transform.position;
        if (Physics2D.OverlapCircle(transform.position, followRange, playerMask) &&
            !Physics2D.CircleCast(transform.position, size, vector.normalized,
            vector.magnitude, obstacleMask))
        {
            hasSeenPlayer = true;
            //If player in vision(followRange)
            currentBehaviour = Behaviour.Follow;
            if (Physics2D.OverlapCircle(transform.position, attackRange, playerMask))
            {
                //If player in Attack range
                currentBehaviour = Behaviour.Attack;

                if (Physics2D.OverlapCircle(transform.position, safeRange, playerMask))
                {
                    //If player in safe range(too close)
                    currentBehaviour = Behaviour.Run;
                }
            }
        }
        else if (hasSeenPlayer)
        {
            hasSeenPlayer = false;
            currentBehaviour = Behaviour.Investigate;
        }

        else
            currentBehaviour = Behaviour.Search;
    }

    private void Attack()
    {
        StartCoroutine(IncreaseTensionOvetime());
        if(tension >= maxTension)
        {
            tension = 0f;
            ChangePosition();
        }
    }

    private void MoveInAttackRange()
    {
        Vector3 vector = player.position - transform.position;
        Vector3 targetPos = transform.position + (vector.normalized *
            (vector.magnitude - attackRange));
        targetPos = GetRandomPos(targetPos, vector, followRange - attackRange);

        if (IsValidTarget(targetPos))
            GetPathToTarget();
        else
            MoveInAttackRange();
        playerNode = grid.NodeFromWorldPosition(player.position);
    }

    private void MoveRandomly()
    {
        Vector2 targetPos = transform.position + Vector3.ClampMagnitude(new Vector3(Random.Range(-followRange, followRange),
            Random.Range(-followRange, followRange)), followRange);

        if (IsValidTarget(targetPos))
            GetPathToTarget();
        else
            MoveRandomly();
    }

    private void RunAwayFromPlayer()
    {
        Vector3 vector = transform.position - player.position;
        Vector3 targetPos = transform.position + (vector.normalized *
            ((safeRange + 1) - vector.magnitude));
        targetPos = GetRandomPos(targetPos, vector, attackRange - safeRange);

        if (IsValidTarget(targetPos))
            GetPathToTarget();
        else
            RunAwayFromPlayer();
    }

    private void ToLastPostion()
    {
        IsValidTarget(player.position);
        GetPathToTarget();
    }
    private Vector2 GetRandomPos(Vector2 target, Vector2 vector, float rand)
    {
        Vector2 randPos = new Vector2(Random.Range(0, rand), Random.Range(0, rand)) * vector.normalized;
        target += randPos;

        return target;
    }

    private bool IsValidTarget(Vector2 targetPos)
    {
        targetNode = grid.NodeFromWorldPosition(targetPos);
        if (!targetNode.walkable || grid.NodeFromWorldPosition(transform.position) == targetNode)
            return false;
        else
            return true;
    }

    private void ChangePosition()
    {
        Vector2 randVector = new Vector2(Random.Range(safeRange, attackRange),
            Random.Range(safeRange, attackRange));

        Vector2 targetPos = (player.position + new Vector3(randVector.x, randVector.y));

        if (IsValidTarget(targetPos))
        {
            GetPathToTarget();
        }
        else
            ChangePosition();
    }

    public void IncreaseTension(float _tension) => tension += _tension;

    private IEnumerator IncreaseTensionOvetime()
    {
        if (currentBehaviour == Behaviour.Attack)
        {
            tension++;
            yield return new WaitForSeconds(Mathf.Abs(tensionRate - checkDelay));
        }
        else
            yield return null;

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
            }
            if (path.Count > 0 && grid.showStartToTarget)
            {
                Gizmos.color = green;
                Gizmos.DrawCube(path[0].nodePosition, Vector3.one * (grid.nodeDiameter + 0.1f));
                Gizmos.color = red;
                Gizmos.DrawCube(path[path.Count-1].nodePosition, Vector3.one * (grid.nodeDiameter + 0.1f));
            }
        }

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, followRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, safeRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
