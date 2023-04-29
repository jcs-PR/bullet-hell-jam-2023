using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    [SerializeField] private Shoot shoot;
    [SerializeField] private Transform player;
    [SerializeField] private float playerRadius;
    [Tooltip("frames skiped while predicting position")]
    [SerializeField] private int frames = 10;
    [SerializeField] public Vector2 targetPosition;

    Rigidbody2D playerRb;
    List<Vector2> predictedPlayerPos = new List<Vector2>();
    List<Vector2> predictedBulletPos = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        shoot = this.GetComponent<Shoot>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(targetPosition, transform.position, Color.black);
        Debug.DrawLine(player.position, playerRb.velocity.normalized * float.MaxValue, Color.yellow);
        PredictTargetPosition();
    }

    private void PredictTargetPosition()
    {
        Vector2 targetPos = playerRb.position;
        Vector2 bulletPos = transform.position;
        predictedPlayerPos.Clear();
        predictedBulletPos.Clear();
        for(int i = 1; i < 20; i++)
        {
            //predict targets position
            targetPos += playerRb.velocity * Time.fixedDeltaTime * frames;
            predictedPlayerPos.Add(targetPos);

            Vector2 shootDir = targetPos - new Vector2(transform.position.x, transform.position.y);
            bulletPos += shootDir.normalized * shoot.bulletSpeed * Time.fixedDeltaTime * frames;
            predictedBulletPos.Add(bulletPos);    
        }
        bool finished = false;
        foreach (var target in predictedPlayerPos)
        {
            if (finished)
                break;
            foreach (var position in predictedBulletPos)
            {
                if (Vector2.Distance(target, position) <= playerRadius)
                {
                    //Shoot
                    targetPosition = target;
                    finished = true;
                    break;
                }
            }
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(targetPosition, 1f);

    }
}
