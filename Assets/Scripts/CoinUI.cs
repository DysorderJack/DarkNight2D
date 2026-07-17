using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;

    private void Start()
    {
        CoinManager.Instance.SetCoinText(coinText);
    }
}