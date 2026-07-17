using UnityEngine;
using System.Collections;

public class BossIA : MonoBehaviour
{
    [Header("Objetivo")]
    [SerializeField] private Transform player;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 3f;
    

    [Header("Vuelo")]
    [SerializeField] private float floatAmplitude = 0.2f;
    [SerializeField] private float floatSpeed = 2f;

    [Header("Altura")]
    [SerializeField] private float verticalOffset = 1.5f;
    [SerializeField] private float randomHeight = 0.5f;
    [SerializeField] private float changeHeightEvery = 2.5f;

    [Header("Límites del Boss")]
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;
    [SerializeField] private Transform topLimit;
    [SerializeField] private Transform bottomLimit;

    [Header("Distancias")]
    [SerializeField] private float minDistance = 2.5f;
    [SerializeField] private float maxDistance = 5f;

    private bool isAwake = false;

    private float currentHeightOffset;

    // TRUE = Boss permanece a la derecha del jugador
    // FALSE = Boss permanece a la izquierda
    private bool stayRightSide = true;

    [Header("Ataques")]
    [SerializeField] private float attackCooldown = 3f;

    private bool isAttacking = false;

    private void Start()
    {
        currentHeightOffset = verticalOffset;

        StartCoroutine(ChangeHeightRoutine());
    }

    private void Update()
    {
        if (!isAwake)
            return;

        switch (currentState)
        {
        case BossState.Idle:
            FollowPlayer();
            LookAtPlayer();
            break;

        case BossState.Attacking:
            // No hace nada.
            break;

        case BossState.Recovering:
            // Tampoco se mueve.
            break;
        }

       

    }

    

    private void FollowPlayer()
    {
        if (player == null)
            return;

        Vector3 targetPosition = transform.position;

        float distance = Mathf.Abs(player.position.x - transform.position.x);

        // Sólo moverse si está demasiado lejos
        if (distance > maxDistance)
        {
            if (player.position.x > transform.position.x)
                targetPosition.x = player.position.x - minDistance;
            else
                targetPosition.x = player.position.x + minDistance;
        }

        // Si está muy cerca, retrocede un poco
        else if (distance < minDistance)
        {
            if (player.position.x > transform.position.x)
                targetPosition.x -= moveSpeed * Time.deltaTime;
            else
                targetPosition.x += moveSpeed * Time.deltaTime;
        }

        // Movimiento vertical

        targetPosition.y =
            player.position.y +
            currentHeightOffset +
            Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        targetPosition.x = Mathf.Clamp(
            targetPosition.x,
            leftLimit.position.x,
            rightLimit.position.x);

        targetPosition.y = Mathf.Clamp(
            targetPosition.y,
            bottomLimit.position.y,
            topLimit.position.y);

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime);
    }

    private void LookAtPlayer()
    {
        if (player == null)
            return;

        Vector3 scale = transform.localScale;

        if (player.position.x > transform.position.x)
            scale.x = Mathf.Abs(scale.x);
        else
            scale.x = -Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

    private IEnumerator ChangeHeightRoutine()
    {
        while (true)
        {
            currentHeightOffset =
                verticalOffset +
                Random.Range(-randomHeight, randomHeight);

            yield return new WaitForSeconds(changeHeightEvery);
        }
    }

    // Lo usaremos después del Dash y del Picotazo
    public void ChangeSide()
    {
        stayRightSide = !stayRightSide;
    }

    // Muy útil para depurar la zona de vuelo
    private void OnDrawGizmosSelected()
{
    if (leftLimit == null || rightLimit == null ||
        topLimit == null || bottomLimit == null)
        return;

    Gizmos.color = Color.red;

    Vector3 center = new Vector3(
        (leftLimit.position.x + rightLimit.position.x) / 2f,
        (topLimit.position.y + bottomLimit.position.y) / 2f,
        0f);

    Vector3 size = new Vector3(
        rightLimit.position.x - leftLimit.position.x,
        topLimit.position.y - bottomLimit.position.y,
        0f);

    Gizmos.DrawWireCube(center, size);
}

[ContextMenu("Probar Attack 1")]
private void TestAttack1()
{
    animator.SetTrigger("Attack1");
}

[ContextMenu("Probar Attack 2")]
private void TestAttack2()
{
    animator.SetTrigger("Attack2");
}

private Animator animator;

private void Awake()
{
    animator = GetComponent<Animator>();
}

private IEnumerator AttackRoutine()
{
    while (isAwake)
    {
        // Esperar antes del siguiente ataque
        yield return new WaitForSeconds(attackCooldown);

        if (isAttacking)
            continue;

        isAttacking = true;
        currentState = BossState.Attacking;

        int randomAttack = Random.Range(0, 100);

        if (randomAttack < 70)
            animator.SetTrigger("Attack1");
        else
            animator.SetTrigger("Attack2");

        // Esperar hasta que la animación indique que terminó
        while (isAttacking)
            yield return null;
    }
}

public void FinishAttack()
{
    StartCoroutine(RecoveryRoutine());
}


public void ActivateBoss()
{
    isAwake = true;

    StartCoroutine(AttackRoutine());
}

private enum BossState
{
    Idle,
    Attacking,
    Recovering
}

private BossState currentState = BossState.Idle;

private IEnumerator RecoveryRoutine()
{
    currentState = BossState.Recovering;

    yield return new WaitForSeconds(0.6f);

    currentState = BossState.Idle;

    isAttacking = false;
}


}