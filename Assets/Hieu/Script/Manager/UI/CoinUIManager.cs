using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    private void Start()
    {
        UpdateCoinUI(PlayerStats.Instance.GetCoins());
        PlayerStats.OnCoinsChanged += UpdateCoinUI;
    }

    private void OnDestroy()
    {
        PlayerStats.OnCoinsChanged -= UpdateCoinUI;
    }

    private void UpdateCoinUI(int coins)
    {
        coinText.text = $"Coins: {coins}";
    }
}
