using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] private int healAmount = 20;

    private void OnTriggerEnter2D(Collider2D other)
{
    if (!other.CompareTag("Player"))
        return;

    PlayerHealth health = other.GetComponent<PlayerHealth>();

    if (health != null)
    {
        health.Heal(healAmount);
        Destroy(transform.parent.gameObject); // Destruye todo el corazón
    }
}
}