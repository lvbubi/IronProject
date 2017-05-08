using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {

    private const float Y_ANGLE_MIN = 30.0f;
    private const float Y_ANGLE_MAX = 75.0f;

    public Transform lookAt;
    public Transform camTransform;
    private float distance = 20.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;

    private void Start()
    {
        camTransform = transform;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            currentX += 1.5f*Input.GetAxis("Mouse X");
            currentY += -1*Input.GetAxis("Mouse Y");
        }
        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN,Y_ANGLE_MAX);
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            distance--;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            distance++;
        }

    }

}
