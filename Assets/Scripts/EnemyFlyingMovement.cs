using UnityEngine;

public class EnemyFlyingMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    private EnemyDetection detection;
    private SpriteRenderer sprite;

    private void Awake()
    {
        detection = GetComponent<EnemyDetection>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!detection.HasTarget)
            return;

        Vector2 target = detection.Target.position;

        transform.position = Vector2.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime);

        sprite.flipX = target.x < transform.position.x;
    }
}
