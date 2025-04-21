using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDamageOverTime : MonoBehaviour
{
    [SerializeField] private float damageAmount = 1f;
    [SerializeField] private float damageInterval = 1f;

    private bool playerInLava = false;
    private float damageTimer;
    private PlayerStats playerStats;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInLava = true;
            playerStats = other.GetComponent<PlayerStats>();
            damageTimer = damageInterval; // damage instantly on enter
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInLava = false;
            playerStats = null;
        }
    }

    private void Update()
    {
        if (playerInLava && playerStats != null)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0f)
            {
                playerStats.DecreaseHealth(damageAmount);
                damageTimer = damageInterval;
            }
        }
    }
}
