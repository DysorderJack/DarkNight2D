using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    [Header("Corazones")]
    [SerializeField] private Image[] hearts;

    [Header("Sprites")]
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private Sprite quarterHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite threeQuarterHeart;
    [SerializeField] private Sprite fullHeart;

    public void UpdateHearts(int currentHealth)
    {
        Debug.Log("Actualizando corazones. Vida: " + currentHealth);
        int hp = currentHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hp >= 20)
            {
                hearts[i].sprite = fullHeart;
            }
            else if (hp >= 15)
            {
                hearts[i].sprite = threeQuarterHeart;
            }
            else if (hp >= 10)
            {
                hearts[i].sprite = halfHeart;
            }
            else if (hp >= 5)
            {
                hearts[i].sprite = quarterHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            hp -= 20;
        }
    }
}