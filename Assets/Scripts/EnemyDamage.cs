using UnityEngine;
using System.Collections;

public class EnemyDamage : MonoBehaviour
{
    [Header("Daño")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float damageInterval = 1f;

    private Coroutine damageCoroutine;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();

        if (health != null)
        {
            damageCoroutine = StartCoroutine(DamageOverTime(health));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    private IEnumerator DamageOverTime(PlayerHealth health)
    {
        while (true)
        {
            health.TakeDamage(damage);
            health.ApplyKnockback(transform.position);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}