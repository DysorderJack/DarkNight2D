using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private float delay = 1f;
    [SerializeField] private LevelCompletedUI levelCompletedUI;
    [SerializeField] private LevelTimer levelTimer;

    private bool levelCompleted = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (levelCompleted)
            return;

        if (!other.CompareTag("Player"))
            return;

        levelCompleted = true;

        

        // Desactivar movimiento del jugador
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
            player.enabled = false;

        Invoke(nameof(ShowPanel), delay);
    
        levelTimer.StopTimer();
    }

    private void ShowPanel()
    {
        
        levelCompletedUI.ShowLevelCompleted();
    }
}