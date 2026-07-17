using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyGroundMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    private EnemyDetection detection;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private void Awake()
    {
        detection = GetComponent<EnemyDetection>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (!detection.HasTarget)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        float direction = Mathf.Sign(
            detection.Target.position.x - transform.position.x);

        rb.linearVelocity = new Vector2(
            direction * speed,
            rb.linearVelocity.y);

        sprite.flipX = direction < 0;
    }
}