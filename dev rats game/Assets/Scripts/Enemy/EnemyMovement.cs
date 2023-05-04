using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    [SerializeField] private float waitTimer = 3; 
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D.GetComponent<Rigidbody2D>();
        StartCoroutine(MoveEnemy());
    }

    public IEnumerator MoveEnemy()
    {
       yield return new WaitForSeconds(waitTimer);
       Vector2 currentPosition = _rigidbody2D.position;
       Vector2 newPosition = new Vector2(Random.Range(0, 5), Random.Range(0, 5)); // Placeholder, to be revised. 
       _rigidbody2D.MovePosition(currentPosition + newPosition);
    }
}
