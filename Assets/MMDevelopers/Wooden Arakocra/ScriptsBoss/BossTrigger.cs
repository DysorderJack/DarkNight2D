using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private BossHealthUI bossHealthUI;
    [SerializeField] private BossIA bossIA;

    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
{
    if (activated)
        return;

    if (!other.CompareTag("Player"))
        return;

    activated = true;

    bossHealthUI.ShowHealthBar();

    bossIA.ActivateBoss();

    Debug.Log("¡Comenzó el combate!");
}
}