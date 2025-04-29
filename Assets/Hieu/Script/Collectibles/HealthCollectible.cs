using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float healthAmount = 1f;
    [SerializeField] private AudioClip collectSound; //sound to play
    [SerializeField] private float soundVolume = 0.5f; //volume control

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.Heal(healthAmount);

                // Play the sound at the collectible's position
                if (collectSound != null)
                {
                    AudioSource.PlayClipAtPoint(collectSound, transform.position, soundVolume);
                }

                Destroy(gameObject);
            }
        }
    }
}
