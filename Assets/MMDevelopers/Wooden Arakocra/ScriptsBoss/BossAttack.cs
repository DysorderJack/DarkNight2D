using UnityEngine;
using System.Collections;

public class BossAttack : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform slashPoint;
    [SerializeField] private Transform peckPoint;

    [Header("Configuración")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Ataque 1 - Zarpazo")]
    [SerializeField] private float slashRadius = 1f;
    [SerializeField] private int slashDamage = 20;

    [Header("Ataque 2 - Picotazo")]
    [SerializeField] private float peckRadius = 1f;
    [SerializeField] private int peckDamage = 30;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashDuration = 0.35f;

private Rigidbody2D rb;

    public void DoSlashAttack()
    {
    
        StartCoroutine(DashAttack());
        

        
    }

    public void DoPeckAttack()
    {
        Attack(peckPoint, peckRadius, peckDamage);
    }

    private void Attack(Transform attackPoint, float radius, int damage)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            radius);

            

        foreach (Collider2D hit in hits)
        {
            PlayerHealth player = hit.GetComponent<PlayerHealth>();
            Debug.Log("Encontrado: " + hit.name + " | Layer: " + LayerMask.LayerToName(hit.gameObject.layer));

            if (player != null)
            {
                 

                player.TakeDamage(damage);
                player.ApplyKnockback(transform.position);
            }
        }
    }

    private IEnumerator DashAttack()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        Vector2 direction = (player.position - transform.position).normalized;

        float timer = 0f;

        while (timer < dashDuration)
        {
            rb.linearVelocity = direction * dashSpeed;

            timer += Time.deltaTime;

            yield return null;
        }

        rb.linearVelocity = Vector2.zero;

        // Ahora que el Boss ya llegó al destino,
        // comprobamos si el jugador está dentro del ataque.
        Attack(slashPoint, slashRadius, slashDamage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (slashPoint != null)
            Gizmos.DrawWireSphere(slashPoint.position, slashRadius);

        Gizmos.color = Color.yellow;

        if (peckPoint != null)
            Gizmos.DrawWireSphere(peckPoint.position, peckRadius);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}