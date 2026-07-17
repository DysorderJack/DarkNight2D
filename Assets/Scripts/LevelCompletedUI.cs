using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletedUI : MonoBehaviour
{
    [SerializeField] private GameObject levelCompletedPanel;

    private void Start()
    {
        levelCompletedPanel.SetActive(false);
    }

    public void ShowLevelCompleted()
    {
        levelCompletedPanel.SetActive(true);

        // Pausar el juego
        Time.timeScale = 0f;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        if (nextScene < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
    }
}