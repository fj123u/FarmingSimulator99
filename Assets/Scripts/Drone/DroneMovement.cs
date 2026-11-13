using UnityEngine;
using UnityEngine.InputSystem;
public class DroneMovement : MonoBehaviour
{
    public InputActionReference moveAction;
    public float moveSpeed = 5f;
    public Transform cameraTransform;
    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;
    void Update()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        if (cameraTransform != null)
        {
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();
            Vector3 cameraRight = cameraTransform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();
            moveDirection = cameraRight * moveInput.x + cameraForward * moveInput.y;
        }
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        KeepInsideGarden();
    }
    private void KeepInsideGarden()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);
        transform.position = clampedPosition;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 center = new Vector3((minX + maxX) * 0.5f, transform.position.y, (minZ + maxZ) * 0.5f);
        Vector3 size = new Vector3(maxX - minX, 0.1f, maxZ - minZ);
        Gizmos.DrawWireCube(center, size);
    }
}