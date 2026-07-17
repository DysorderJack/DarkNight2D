using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTimer : MonoBehaviour
{
    [Header("Tiempo")]
    [SerializeField] private float levelTime = 180f; // 3 minutos

    [Header("UI")]
    [SerializeField] private TMP_Text timerText;

    [Header("Jugador")]
    [SerializeField] private PlayerHealth playerHealth;

    private float currentTime;
    private bool finished = false;

    private void Start()
    {
         currentTime = levelTime;
        finished = false;

        UpdateUI();
    }

    private void Update()
    {
        if (finished)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime < 0)
            currentTime = 0;

        UpdateUI();

        if (currentTime <= 0)
        {
            finished = true;

            playerHealth.Die();
            
        }
    }

    private void UpdateUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (currentTime <= 10)
        {
            timerText.color = Color.red;
        }
        else if (currentTime <= 30)
        {
            timerText.color = new Color(1f, 0.5f, 0f); // Naranja
        }
        else
        {
            timerText.color = Color.white;
        }
    }

    public void StopTimer()
    {
        finished = true;
    }

    public void ResetTimer()
    {
        currentTime = levelTime;
        finished = false;
        UpdateUI();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetTimer();
    }
}
