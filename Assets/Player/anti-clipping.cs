using UnityEngine;

public class CameraClip : MonoBehaviour
{
    public float sphereRadius = 0.1f;
    public LayerMask wallLayers;

    private Vector3 originalLocalPos;

    void Start()
    {
        originalLocalPos = transform.localPosition;
    }

    void LateUpdate()
    {
        // Si hay pared entre el player y la camara, acerca la camara
        RaycastHit hit;
        Vector3 worldPos = transform.parent.TransformPoint(originalLocalPos);

        if (Physics.SphereCast(transform.parent.position, sphereRadius,
            (worldPos - transform.parent.position).normalized,
            out hit, originalLocalPos.magnitude, wallLayers))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.localPosition = originalLocalPos;
        }
    }
}