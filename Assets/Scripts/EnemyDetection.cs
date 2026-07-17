using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [Header("Detección")]
    [SerializeField] private float detectionRadius = 8f;
    [SerializeField] private LayerMask playerLayer;

    public Transform Target { get; private set; }

    public bool HasTarget => Target != null;

    private void Update()
    {
        Collider2D player = Physics2D.OverlapCircle(
            transform.position,
            detectionRadius,
            playerLayer);

        Target = player != null ? player.transform : null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}