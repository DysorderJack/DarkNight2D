using UnityEngine;

public class DiamondPickup : MonoBehaviour
{
    [SerializeField] private int value = 1;

    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        CoinManager.Instance.AddCoin(value);

        AudioSource.PlayClipAtPoint(
            pickupSound,
            transform.position);

        Destroy(gameObject);
    }
}
