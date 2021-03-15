using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float cameraLeanRatio = 0.2f;

    Vector3 cameraPosition;

    void Update()
    {
        Vector3 mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerTransform.position;
        cameraPosition.x = playerTransform.position.x + mouseDir.x * cameraLeanRatio;
        cameraPosition.y = playerTransform.position.y + mouseDir.y * cameraLeanRatio;
        cameraPosition.z = transform.position.z;

        transform.position = cameraPosition;
    }
}
