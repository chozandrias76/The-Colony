using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform playerTransform;
	public Camera cam;
	public Vector3 toTarget;
	public Vector3 toAngle;
	public Vector3 angle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*toTarget = (playerTransform.position - transform.position);
		toTarget.Normalize();
		transform.Translate(toTarget);
		toAngle = (angle - transform.position);
		transform.Rotate(toAngle);
	*/
	}
}
