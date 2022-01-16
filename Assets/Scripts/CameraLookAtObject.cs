using UnityEngine;

public class CameraLookAtObject : MonoBehaviour
{
    [SerializeField] private GameObject _objectToLookAt;

    private void LateUpdate()
    {
        transform.LookAt(_objectToLookAt.transform);
    }
}
