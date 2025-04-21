using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject activeIndicator;
    [SerializeField] private GameObject promptUI;
    [SerializeField] private int coinCost = 5;

    private static Checkpoint activeCheckpoint;
    private bool playerInRange = false;
    private TextMeshProUGUI promptText;

    private void Start()
    {
        activeIndicator.SetActive(false);

        if (promptUI != null)
        {
            promptUI.SetActive(false);
            promptText = promptUI.GetComponentInChildren<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogWarning("Checkpoint is missing a promptUI reference!");
        }
    }

    private void Update()
    {
        if (!playerInRange) return;

        // Update prompt dynamically based on coin count
        if (PlayerStats.Instance != null && promptText != null)
        {
            int coins = PlayerStats.Instance.GetCoins();
            if (coins >= coinCost && activeCheckpoint != this) // If the player has enough coins and this checkpoint is not already active
            {
                promptText.text = $"Press [E] to activate checkpoint (Cost: {coinCost})";
            }
            else if (activeCheckpoint == this) // If this checkpoint is already active
            {
                promptText.text = "Checkpoint Activated";
            }
            else
            {
                promptText.text = $"Need {coinCost} coins to activate!";
            }
        }

        // Activation input
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryActivateCheckpoint();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (promptUI != null)
        {
            promptUI.SetActive(true);
        }

        Debug.Log("Player entered checkpoint trigger.");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        if (promptUI != null)
        {
            promptUI.SetActive(false);
        }

        Debug.Log("Player exited checkpoint trigger.");
    }

    private void TryActivateCheckpoint()
    {
        if (PlayerStats.Instance == null || promptText == null) return;

        int playerCoins = PlayerStats.Instance.GetCoins();
        Debug.Log($"Trying to activate checkpoint. Player Coins: {playerCoins}, Coin Cost: {coinCost}");

        if (playerCoins >= coinCost && activeCheckpoint != this)
        {
            PlayerStats.Instance.SpendCoins(coinCost);
            Debug.Log("Checkpoint activated.");
            ActivateCheckpoint();
        }
        else if (activeCheckpoint == this)
        {
            Debug.Log("This checkpoint is already activated.");
        }
        else
        {
            Debug.Log("Not enough coins.");
            promptText.text = $"Not enough coins! ({coinCost} required)";
        }
    }

    private void ActivateCheckpoint()
    {
        if (activeCheckpoint != null)
        {
            activeCheckpoint.DeactivateCheckpoint();
        }

        activeCheckpoint = this;

        activeIndicator?.SetActive(true);
        promptUI?.SetActive(false);

        GameManager gm = GameObject.Find("GameManager")?.GetComponent<GameManager>();
        if (gm != null)
        {
            gm.SetRespawnPoint(transform);
        }
        else
        {
            Debug.LogWarning("GameManager not found in scene.");
        }

        Debug.Log("Checkpoint activated and respawn point set.");
    }

    private void DeactivateCheckpoint()
    {
        activeIndicator?.SetActive(false);
        Debug.Log("Checkpoint deactivated.");
    }
}
