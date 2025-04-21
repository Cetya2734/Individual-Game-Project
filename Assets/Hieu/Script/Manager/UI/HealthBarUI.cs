using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    private void OnEnable()
    {
        GameManager.OnPlayerRespawned += HandlePlayerRespawn;

        // Subscribe to health change event right away in case of initial health update
        PlayerStats.OnHealthChanged += UpdateHealthUI;

        // Initialize the health bar immediately if PlayerStats is already set
        if (PlayerStats.Instance != null)
        {
            healthSlider.maxValue = PlayerStats.Instance.GetMaxHealth();
            healthSlider.value = PlayerStats.Instance.GetCurrentHealth();
        }
    }

    private void OnDisable()
    {
        GameManager.OnPlayerRespawned -= HandlePlayerRespawn;
        PlayerStats.OnHealthChanged -= UpdateHealthUI;
    }

    private void HandlePlayerRespawn(PlayerStats newStats)
    {
        PlayerStats.OnHealthChanged -= UpdateHealthUI; // Clean up just in case
        InitializeHealthUI(newStats);
    }

    private void InitializeHealthUI(PlayerStats stats)
    {
        healthSlider.maxValue = stats.GetMaxHealth();
        healthSlider.value = stats.GetCurrentHealth();

        PlayerStats.OnHealthChanged += UpdateHealthUI;
    }

    private void UpdateHealthUI(float currentHealth)
    {
        healthSlider.value = currentHealth;
    }
}
