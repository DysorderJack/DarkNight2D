using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int maxHealth = 300;

    private int currentHealth;

    [Header("UI")]
    [SerializeField] private Slider healthSlider;

    [Header("Componentes")]
    [SerializeField] private BossIA bossIA;
    [SerializeField] private Animator animator;

    private bool dead = false;

 private void Start()
{
    currentHealth = maxHealth;

    healthSlider.maxValue = maxHealth;
    healthSlider.value = maxHealth;

    StartCoroutine(TestRoutine());
}

private IEnumerator TestRoutine()
{
    yield return new WaitForSeconds(2f);

    Debug.Log("Quitando vida");

    TakeDamage(50);
}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            healthSlider.value -= 10;

            Debug.Log("Slider manual: " + healthSlider.value);
        }
    }

    public void TakeDamage(int damage)
{
    

    if (dead)
        return;

    currentHealth -= damage;

    currentHealth = Mathf.Max(currentHealth, 0);

    healthSlider.SetValueWithoutNotify(currentHealth);

    Debug.Log("Slider.value = " + healthSlider.value);

    

    if (currentHealth <= 0)
        Die();
}

    private void Die()
    {
        dead = true;

        bossIA.enabled = false;

        animator.SetTrigger("Die");

        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(2f);

        // Aquí mostraremos Game Complete
    }

    [ContextMenu("Quitar 20 HP")]
    private void TestDamage()
    {
        
        TakeDamage(20);
    }

    private void Awake()
    {
        Debug.Log("BossHealth Awake");
    }

    private void OnEnable()
    {
        Debug.Log("BossHealth OnEnable");
    }
}
