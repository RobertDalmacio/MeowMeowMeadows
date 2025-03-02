using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    Vector3 camOffset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camOffset = transform.position - target.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position = target.position + camOffset;
    }
}
