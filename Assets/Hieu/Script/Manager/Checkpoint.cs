using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject activeIndicator;
    [SerializeField] private GameObject promptUI; // This is the world-space canvas

    private static Checkpoint activeCheckpoint;
    private bool playerInRange = false;

    private void Start()
    {
        activeIndicator.SetActive(false);
        promptUI.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ActivateCheckpoint();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            promptUI.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            promptUI.SetActive(false);
            playerInRange = false;
        }
    }

    private void ActivateCheckpoint()
    {
        if (activeCheckpoint != null)
            activeCheckpoint.DeactivateCheckpoint();

        activeCheckpoint = this;

        activeIndicator.SetActive(true);
        promptUI.SetActive(false);

        GameObject.Find("GameManager").GetComponent<GameManager>().SetRespawnPoint(transform);
    }

    private void DeactivateCheckpoint()
    {
        activeIndicator.SetActive(false);
    }
}
