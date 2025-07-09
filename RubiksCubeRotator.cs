using UnityEngine;

public class RubiksCubeRotator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float dragSpeed = 5f;
    [SerializeField] private float inertiaDamping = 0.95f;

    private Vector3 rotationVelocity;
    private bool isDragging;

    private void Update()
    {
        HandleInput();
        ApplyRotation();
    }

    private void HandleInput()
    {
        // Правый клик или Shift+ЛКМ для вращения
        bool rotationInput = Input.GetMouseButton(1) || (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButton(0));

        if (Input.GetMouseButtonDown(1)) StartDragging();
        if (!rotationInput && isDragging) StopDragging();

        if (isDragging)
        {
            rotationVelocity.x += Input.GetAxis("Mouse Y") * dragSpeed;
            rotationVelocity.y -= Input.GetAxis("Mouse X") * dragSpeed;
        }
    }

    private void StartDragging()
    {
        isDragging = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void StopDragging()
    {
        isDragging = false;
        Cursor.lockState = CursorLockMode.None;
    }

    private void ApplyRotation()
    {
        transform.Rotate(rotationVelocity * Time.deltaTime, Space.World);
        rotationVelocity *= inertiaDamping;
    }

    // Сброс по R
    private void OnGUI()
    {
        if (Event.current.keyCode == KeyCode.R && Event.current.type == EventType.KeyDown)
        {
            transform.rotation = Quaternion.identity;
            rotationVelocity = Vector3.zero;
        }
    }
}