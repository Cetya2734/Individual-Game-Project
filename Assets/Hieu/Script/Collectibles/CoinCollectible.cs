using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectible : MonoBehaviour
{
    public int coinValue = 1;
    public AudioClip collectSound; // Assign this in the Inspector
    public float volume = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Play sound at the coin's position
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position, volume);
            }

            PlayerStats.Instance.AddCoins(coinValue);
            Destroy(gameObject);
        }
    }
}
