using UnityEngine;
using System.Collections;
 
[AddComponentMenu("Camera-Control/Mouse drag Orbit with zoom")]
public class DragMouseOrbit : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
 
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
 
    public float distanceMin = .5f;
    public float distanceMax = 15f;
 
    public float smoothTime = 2f;
 
    float rotationYAxis = 0.0f;
    float rotationXAxis = 0.0f;
 
    float velocityX = 0.0f;
    float velocityY = 0.0f;
	float onLoadRotation;
	
	
 
    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        rotationYAxis = angles.y;
        rotationXAxis = angles.x;
		//onLoadRotation = target.rotation.y - transform.rotation.y;
 		//transform.rotation = new Quaternion (onLoadRotation, onLoadRotation, onLoadRotation, target.rotation.w);
		Quaternion.Slerp(transform.rotation,target.rotation, 1.0f);
        // Make the rigid body not change rotation
        if (rigidbody)
        {
            rigidbody.freezeRotation = true;
        }
    }
 
    void LateUpdate()
    {
        if (target)
        {
            if (Input.GetMouseButton(2))
            {
                velocityX += xSpeed * Input.GetAxis("Mouse X") * distance * 0.01f;
                //velocityY += ySpeed * Input.GetAxis("Mouse Y") * 0.02f;
				velocityY = 0.0f;
            }
 
            rotationYAxis += velocityX;
            rotationXAxis -= velocityY;
 
            rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);
 
           // Quaternion fromRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
            Quaternion rotation = toRotation;
 
            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
 
            RaycastHit hit;
            if (Physics.Linecast(target.position, transform.position, out hit))
            {
                //distance -= hit.distance;
				distance = 3.2f;
            }
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;
			position = position + new Vector3(0,2,0);
 
            transform.rotation = rotation;
            transform.position = position;
 
            velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * smoothTime);
            velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * smoothTime);
        }
		
	}
 
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}