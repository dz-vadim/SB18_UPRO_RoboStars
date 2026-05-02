using UnityEngine;

public class LookToCamera : MonoBehaviour
{
    private Camera _camera;
    void Awake()
    {
        _camera = Camera.main;
    }

    void LateUpdate()
    {
        transform.LookAt(_camera.transform.position);
        transform.Rotate(Vector3.up * 180);
    }
}
