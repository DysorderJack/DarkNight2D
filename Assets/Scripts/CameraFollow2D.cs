using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string playerGameObjectName = "Jugador";

    [Header("Follow Settings")]
    [SerializeField] private float smoothSpeed = 12f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, -10f);

    [Header("Boundary Limits (Optional)")]
    [SerializeField] private bool useLimits = false;
    [SerializeField] private float minX = -50f;
    [SerializeField] private float maxX = 50f;

    private void Start()
    {
        // Automatically find target if not assigned
        if (target == null)
        {
            GameObject playerObj = GameObject.Find(playerGameObjectName);

            if (playerObj == null)
            {
                playerObj = GameObject.FindWithTag(playerTag);
            }

            if (playerObj != null)
            {
                target = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("CameraFollow2D: No target assigned and could not find player.");
            }
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Solo se mueve en X
        float desiredX = target.position.x + offset.x;

        // Aplicar límites horizontales si están habilitados
        if (useLimits)
        {
            desiredX = Mathf.Clamp(desiredX, minX, maxX);
        }

        // Mantener Y y Z actuales
        Vector3 desiredPosition = new Vector3(
            desiredX,
            transform.position.y,
            transform.position.z
        );

        // Movimiento suave
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}