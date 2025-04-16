using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float delayBeforeFall = 0.5f;
    public float respawnDelay = 3f;
    public float fallGravityScale = 3f;

    private Rigidbody2D rb;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isFalling = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFalling && collision.collider.CompareTag("Player"))
        {
            isFalling = true;
            Invoke(nameof(Fall), delayBeforeFall);
        }
    }

    void Fall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = fallGravityScale;
        Invoke(nameof(Respawn), respawnDelay);
    }

    void Respawn()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = startPosition;
        transform.rotation = startRotation;
        isFalling = false;
    }
}
