using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public Vector2 moveVector;
    public float moveSpeed;
    public float damping;
    public Rigidbody2D rig;
    public CircleCollider2D collider;
    public float timer;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        collider.isTrigger = true;
        timer = 0.5f;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            collider.isTrigger = false;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        rig.velocity = moveVector * moveSpeed;
        moveVector = Vector2.Lerp(moveVector, Vector2.zero, damping);
    }
}
