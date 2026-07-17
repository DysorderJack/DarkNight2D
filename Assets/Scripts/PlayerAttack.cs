using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Ataque")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Vector2 attackSize = new Vector2(1.2f, 0.8f);

    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private int attack1Damage = 10;
    [SerializeField] private int attack2Damage = 20;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Attack1()
    {
        DoAttack(attack1Damage);
    }

    public void Attack2()
    {
        DoAttack(attack2Damage);
    }

    private void DoAttack(int damage)
    {
        Vector2 center = attackPoint.position;

        // Si el personaje mira a la izquierda, mover la hitbox al otro lado
        if (spriteRenderer.flipX)
        {
            center.x = transform.position.x - Mathf.Abs(attackPoint.localPosition.x);
        }
        else
        {
            center.x = transform.position.x + Mathf.Abs(attackPoint.localPosition.x);
        }

        Collider2D[] enemies =
            Physics2D.OverlapBoxAll(center, attackSize, 0f, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            EnemyHealth health = enemy.GetComponent<EnemyHealth>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(
            attackPoint.position,
            attackSize);
    }
}