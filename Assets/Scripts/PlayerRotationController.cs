using UnityEngine;
using TMPro; // ✅ เพิ่มตัวนี้

public class PlayerRotationController : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;
    public float rotationAngle = 90f;
    
    [Header("Screen Edge Detection")]
    [Range(0.05f, 0.3f)]
    public float edgeThreshold = 0.1f;
    
    [Header("UI References")]
    public TextMeshProUGUI arrowLeft;   // ✅ เปลี่ยนจาก Image
    public TextMeshProUGUI arrowRight;  // ✅ เปลี่ยนจาก Image
    
    private bool isRotating = false;
    
    private Quaternion targetRotation;
    private Quaternion startRotation;
    
    private float rotationProgress = 0f;

    void Start()
    {
        // ซ่อนลูกศรตอนเริ่ม
        if (arrowLeft != null)
            arrowLeft.gameObject.SetActive(false);
            
        if (arrowRight != null)
            arrowRight.gameObject.SetActive(false);
        
        targetRotation = transform.rotation;
        startRotation = transform.rotation;
    }

    void Update()
    {
        HandleMousePosition();
        HandleRotationInput();
        PerformSmoothRotation();
    }

    void HandleMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        float screenWidth = Screen.width;
        
        float leftEdge = screenWidth * edgeThreshold;
        float rightEdge = screenWidth * (1f - edgeThreshold);
        
        bool isOnLeftEdge = mousePos.x < leftEdge;
        bool isOnRightEdge = mousePos.x > rightEdge;
        
        // แสดง/ซ่อนลูกศร
        if (arrowLeft != null)
            arrowLeft.gameObject.SetActive(isOnLeftEdge && !isRotating);
        
        if (arrowRight != null)
            arrowRight.gameObject.SetActive(isOnRightEdge && !isRotating);
    }

    void HandleRotationInput()
    {
        if (isRotating) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            float screenWidth = Screen.width;
            
            float leftEdge = screenWidth * edgeThreshold;
            float rightEdge = screenWidth * (1f - edgeThreshold);
            
            if (mousePos.x < leftEdge)
            {
                StartRotation(-rotationAngle);
            }
            else if (mousePos.x > rightEdge)
            {
                StartRotation(rotationAngle);
            }
        }
    }

    void StartRotation(float angle)
    {
        isRotating = true;
        rotationProgress = 0f;
        
        startRotation = transform.rotation;
        targetRotation = startRotation * Quaternion.Euler(0f, angle, 0f);
    }

    void PerformSmoothRotation()
    {
        if (!isRotating) return;
        
        rotationProgress += Time.deltaTime * rotationSpeed;
        rotationProgress = Mathf.Clamp01(rotationProgress);
        
        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, rotationProgress);
        
        if (rotationProgress >= 1f)
        {
            transform.rotation = targetRotation;
            isRotating = false;
            rotationProgress = 0f;
        }
    }
}