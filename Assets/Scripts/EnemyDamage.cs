using UnityEngine;
using System.Collections;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float damageInterval = 1f;

    private bool canDamage = true;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!canDamage)
            return;

        if (!other.CompareTag("Player"))
            return;

        PlayerHealth player = other.GetComponent<PlayerHealth>();

        if (player == null)
            return;

        player.TakeDamage(damage);
        player.ApplyKnockback(transform.position);

        StartCoroutine(DamageCooldown());
    }

    private IEnumerator DamageCooldown()
    {
        canDamage = false;

        yield return new WaitForSeconds(damageInterval);

        canDamage = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
{
    Debug.Log("Entró al trigger: " + other.name);
}
}