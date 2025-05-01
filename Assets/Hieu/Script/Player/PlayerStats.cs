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

    [SerializeField] private AudioClip deathSound;
    [SerializeField] private float deathSoundVolume = 1.0f;

    [SerializeField] private AudioClip damageSound;
    [SerializeField] private float damageSoundVolume = 0.5f;

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this; // Always update instance for the current player
    }

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth); // Ensure UI updates at start
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        audioSource = GetComponent<AudioSource>();
    }

    public float GetMaxHealth() => maxHealth;
    public float GetCurrentHealth() => currentHealth;

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth > 0f && damageSound && audioSource)
        {
            audioSource.PlayOneShot(damageSound, damageSoundVolume);
        }

        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // Save coins before dying
        GM.SaveCoins(coinCount);

        Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);

        if (deathSound)
        {
            GameObject soundObject = new GameObject("DeathSoundPlayer");
            soundObject.transform.position = transform.position; // Play sound at player's death position

            SoundPlayer soundPlayer = soundObject.AddComponent<SoundPlayer>();
            soundPlayer.PlaySound(deathSound, deathSoundVolume);
        }

        GM.Respawn();
        Destroy(gameObject); // No delay needed!
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

    // NEW: Called from GameManager when a player is respawned
    public void SetCoins(int amount)
    {
        coinCount = amount;
        OnCoinsChanged?.Invoke(coinCount);
    }
}
