using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] [Range(0,100)] private int dropChance = 40;

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
            //animator.SetTrigger("Hit");
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
            //animator.SetTrigger("Die");
             DropLoot();
            // Espera un segundo antes de destruir el enemigo
            Destroy(gameObject, 1f);

        }
        else
        {
             DropLoot();
            Destroy(gameObject);
        }
    }

  private void DropLoot()
{
    int random = Random.Range(0, 25);

    if (random < dropChance)
    {
        GameObject heart = Instantiate(
            heartPrefab,
            transform.position + Vector3.up * 0.5f,
            Quaternion.identity);

        Rigidbody2D rb = heart.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.AddForce(
                 new Vector2(Random.Range(-0.5f, 0.5f), 1.5f),
                ForceMode2D.Impulse);
        }
    }
}
}