using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    // Higher values make the scenery appear farther away.
    [SerializeField, Range(0f, 1f)]
    private float followAmount = 0.8f;

    private Vector3 initialCameraPosition;
    private Vector3 initialBackgroundPosition;

    private void Start()
    {
        if (cameraTransform == null)
        {
            enabled = false;
            return;
        }

        initialCameraPosition = cameraTransform.position;
        initialBackgroundPosition = transform.position;
    }

    private void LateUpdate()
    {
        Vector3 movement =
            cameraTransform.position - initialCameraPosition;

        transform.position = initialBackgroundPosition
            + new Vector3(movement.x, movement.y, 0f) * followAmount;
    }
}