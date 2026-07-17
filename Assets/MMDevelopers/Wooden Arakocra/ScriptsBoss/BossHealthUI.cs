using UnityEngine;



public class BossHealthUI : MonoBehaviour
{
    [SerializeField] private GameObject bossHealthPanel;

    private void Start()
    {
        bossHealthPanel.SetActive(false);
    }

    public void ShowHealthBar()
    {
        bossHealthPanel.SetActive(true);
    }

    public void HideHealthBar()
    {
        bossHealthPanel.SetActive(false);
    }

 
}
