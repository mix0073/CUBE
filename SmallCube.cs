using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SmallCube : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 270f;
    [SerializeField] private float clickCooldown = 0.3f;
    [SerializeField] private AudioClip rotateSound;

    private bool isRotating = false;
    private float cooldownTimer = 0f;
    private Quaternion targetRotation;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<BoxCollider>().size = Vector3.one * 0.95f; // Подгонка коллайдера под отступы
    }

    private void Update()
    {
        if (isRotating)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
            }
        }

        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
    }

    private void OnMouseDown()
    {
        if (CanRotate()) StartRotation(GetHitAxis());
    }

    private bool CanRotate()
    {
        return !isRotating && cooldownTimer <= 0 && !Input.GetKey(KeyCode.LeftShift);
    }

    private Vector3 GetHitAxis()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Cubes")))
        {
            // Возвращаем нормаль удара (ось вращения)
            return hit.normal;
        }
        return Vector3.up; // Fallback
    }

    private void StartRotation(Vector3 axis)
    {
        isRotating = true;
        cooldownTimer = clickCooldown;
        targetRotation = transform.rotation * Quaternion.AngleAxis(90, axis);

        if (rotateSound) audioSource.PlayOneShot(rotateSound);
    }

    // Визуальная обратная связь
    private void OnMouseEnter() => HighlightFace(true);
    private void OnMouseExit() => HighlightFace(false);

    private void HighlightFace(bool enable)
    {
        if (isRotating) return;
        GetComponent<Renderer>().material.SetColor("_EmissionColor", enable ? Color.cyan * 0.3f : Color.black);
    }
}