using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private float respawnTime;

    private float respawnTimeStart;
    private bool respawn;

    private int savedCoins = 0;

    public static System.Action<PlayerStats> OnPlayerRespawned;

    private CinemachineVirtualCamera CVC;

    private void Start()
    {
        CVC = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        CheckRespawn();
    }

    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
    }

    private void CheckRespawn()
    {
        if (Time.time >= respawnTimeStart + respawnTime && respawn)
        {
            var playerTemp = Instantiate(player, respawnPoint.position, Quaternion.identity);
            CVC.m_Follow = playerTemp.transform;
            respawn = false;

            var playerStats = playerTemp.GetComponent<PlayerStats>();
            playerStats.SetCoins(savedCoins); // Restore coins
            OnPlayerRespawned?.Invoke(playerStats);
        }
    }

    public void SetRespawnPoint(Transform newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
    }

    // Save coin count before the player dies
    public void SaveCoins(int amount)
    {
        savedCoins = amount;
    }

    // Optionally expose this if other systems need to read it
    public int LoadCoins()
    {
        return savedCoins;
    }
}
