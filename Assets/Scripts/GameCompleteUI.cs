using UnityEngine;

public class GameCompleteUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private void Start()
    {
        panel.SetActive(false);
    }

    public void ShowGameComplete()
    {
        panel.SetActive(true);

        Time.timeScale = 0f;
    }
}