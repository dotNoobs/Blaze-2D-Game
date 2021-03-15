using UnityEngine;

public class RotationResetter : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
