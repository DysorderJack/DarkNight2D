using UnityEngine;
using System.Collections;


public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int maxHealth = 100;

    [Header("UI")]
    [SerializeField] private HeartUI heartUI;

    // Lo dejo serializado para que puedas ver la vida en el Inspector mientras pruebas
    [SerializeField] private int currentHealth;

    [Header("Invulnerabilidad")]
[SerializeField] private float invulnerabilityTime = 1f;

private bool isInvulnerable = false;

[Header("Knockback")]
[SerializeField] private float knockbackForce = 8f;

[SerializeField] private GameOverUI gameOverUI;

private SpriteRenderer sprite;

private Rigidbody2D rb;

private Animator animator;
private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (heartUI != null)
        {
            heartUI.UpdateHearts(currentHealth);
        }
        else
        {
            Debug.LogWarning("No se ha asignado el HeartUI en PlayerHealth.");
        }
    }

    public void TakeDamage(int damage)
{
    if (isInvulnerable)
        return;

    currentHealth -= damage;
    currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

    if (heartUI != null)
        heartUI.UpdateHearts(currentHealth);


    if (currentHealth <= 0)
    {
        Die();
    }

    StartCoroutine(Invulnerability());
}

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (heartUI != null)
        {
            heartUI.UpdateHearts(currentHealth);
        }

        Debug.Log("Vida actual: " + currentHealth);
    }

    public void Die()
{
    Invoke(nameof(ShowGameOver),1f);
    if (isDead)
        return;

    isDead = true;


    animator.SetTrigger("Die");

    // Desactivar movimiento
    GetComponent<PlayerController>().enabled = false;

    // Opcional: desactivar el collider
    GetComponent<Collider2D>().enabled = false;

    // Opcional: detener la velocidad
    GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

    Rigidbody2D rb = GetComponent<Rigidbody2D>();

    rb.linearVelocity = Vector2.zero;
    rb.gravityScale = 0;
    rb.constraints = RigidbodyConstraints2D.FreezeAll;

    
    
    

}

 private IEnumerator Invulnerability()
{
    isInvulnerable = true;

    float timer = 0f;

    while (timer < invulnerabilityTime)
    {
        sprite.enabled = !sprite.enabled;

        yield return new WaitForSeconds(0.1f);

        timer += 0.1f;
    }

    sprite.enabled = true;
    isInvulnerable = false;
}

public void ApplyKnockback(Vector2 enemyPosition)
{
    float direction = transform.position.x > enemyPosition.x ? 1f : -1f;

    rb.linearVelocity = Vector2.zero;

    rb.AddForce(new Vector2(direction * knockbackForce, 2f), ForceMode2D.Impulse);
}

private void ShowGameOver()
{
    
    if(gameOverUI != null)
        gameOverUI.ShowGameOver();
}

}