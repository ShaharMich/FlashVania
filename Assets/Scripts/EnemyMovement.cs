using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D majinRigidbody;
    void Start()
    {
        majinRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        majinRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2 (-(Mathf.Sign(majinRigidbody.velocity.x)),1f);
    }
}
