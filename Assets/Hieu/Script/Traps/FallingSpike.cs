using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    [Header("Spike Behavior")]
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.1f;
    [SerializeField] private float gravityAfterFall = 3f;
    [SerializeField] private float destroyAfterSeconds = 3f;
    [SerializeField] private float damageAmount = 1f;

    private Rigidbody2D rb;
    private Vector3 originalPos;
    private bool hasFallen = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        originalPos = transform.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasFallen || !other.CompareTag("Player")) return;

        hasFallen = true;
        StartCoroutine(ShakeAndFall());
    }

    private IEnumerator ShakeAndFall()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            transform.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos; // reset position
        rb.gravityScale = gravityAfterFall;

        Destroy(gameObject, destroyAfterSeconds);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerStats stats = collision.collider.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.DecreaseHealth(damageAmount);
            }

            Destroy(gameObject); // destroy after hitting the player
        }
    }
}
