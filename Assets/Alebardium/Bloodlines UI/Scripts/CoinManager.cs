using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [SerializeField] private TMP_Text coinText;

    private int coins = 0;

   private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin(int amount)
    {
        coins += amount;

        coinText.text = coins.ToString();
    }

    public int GetCoins()
    {
        return coins;
    }

    public void SetCoinText(TMP_Text newText)
    {
        coinText = newText;
        coinText.text = coins.ToString();
    }
}
