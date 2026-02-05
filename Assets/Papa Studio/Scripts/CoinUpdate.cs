using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePolygon;
using UnityEngine.UI;

public class CoinUpdate : MonoBehaviour
{
    public Text TotalCoins;

    private void OnEnable()
    {
        // Giá trị ban đầu
        TotalCoins.text = CoinManager.Instance.Coins.ToString();
        // Chỉ update khi số coin thực sự thay đổi
        CoinManager.CoinsUpdated += OnCoinsUpdated;
    }

    private void OnDisable()
    {
        CoinManager.CoinsUpdated -= OnCoinsUpdated;
    }

    private void OnCoinsUpdated(int newCoinsValue)
    {
        TotalCoins.text = newCoinsValue.ToString();
    }
}
