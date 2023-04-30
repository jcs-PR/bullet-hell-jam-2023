
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Aim : MonoBehaviour
{
    [SerializeField] private Shoot shoot;
    [SerializeField] private Transform player;
    [SerializeField] private float playerRadius;
    [Tooltip("frames skiped while predicting position")]
    [Range(1f, 20f)]
    [FormerlySerializedAs("_frames")] [SerializeField] private int frames = 10;
    [Range(1f, 100f)]
    [FormerlySerializedAs("_accuracy")][SerializeField] private float accuracy = 100f;

    [HideInInspector] public Vector2 targetPosition;

    //Private varibles
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
            //predict target's position
            targetPos += playerRb.velocity * Time.fixedDeltaTime * frames;
            predictedPlayerPos.Add(targetPos);
            //predict bullet's position
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
                    if (accuracy != 100) targetPosition = CheckAccuracy(target);
                    else targetPosition = target;

                    finished = true;
                    break;
                }
            }
            
        }
    }

    private Vector2 CheckAccuracy(Vector2 target)
    {
        float randomness = 100 - accuracy;
        Vector2 targetDir = target - new Vector2(player.position.x, player.position.y);
        randomness = Random.Range(-randomness, randomness);
        Vector2 targetPos = (targetDir.normalized * randomness) + target;
        return targetPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(targetPosition, 1f);
        /*
        foreach (var position in predictedBulletPos)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(position, 1f);
        }
        */
    }
}
