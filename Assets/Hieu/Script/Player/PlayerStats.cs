using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject deathChunkParticle, deathBloodParticle;

    private float currentHealth;
    private int coinCount;

    private GameManager GM;

    public static System.Action<int> OnCoinsChanged;
    public static System.Action<float> OnHealthChanged;

    public static PlayerStats Instance { get; private set; }

    private void Awake()
    {
        Instance = this; // Always update instance for the current player
    }

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth); // Ensure UI updates at start
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public float GetMaxHealth() => maxHealth;
    public float GetCurrentHealth() => currentHealth;

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        GM.Respawn();
        Destroy(gameObject);
    }

    public int GetCoins() => coinCount;

    public void AddCoins(int amount)
    {
        coinCount += amount;
        OnCoinsChanged?.Invoke(coinCount);
    }

    public void SpendCoins(int amount)
    {
        coinCount = Mathf.Max(0, coinCount - amount);
        OnCoinsChanged?.Invoke(coinCount);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }
}
