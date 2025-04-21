using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingPlatform : MonoBehaviour
{
    public float bounceForce = 15f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player landed on top of the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Optional: make sure the collision came from above
                if (collision.contacts[0].normal.y < -0.5f)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0f); // Reset vertical velocity
                    rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}
