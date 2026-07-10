using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int maxHealth = 30;

    private int currentHealth;
    private bool isDead = false;

    private Animator animator;

    private void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        Debug.Log($"{gameObject.name} recibió {damage} de daño. Vida restante: {currentHealth}");

        // Animación de recibir daño (si existe)
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        Debug.Log(gameObject.name + " murió");

        if (animator != null)
        {
            animator.SetTrigger("Die");

            // Espera un segundo antes de destruir el enemigo
            Destroy(gameObject, 1f);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}