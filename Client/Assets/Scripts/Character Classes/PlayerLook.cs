using UnityEngine;
using System.Collections;

public class PlayerLook: MonoBehaviour {
	// Position and Deltas
	public Vector3 screenMousePosition;
	public Vector3 worldMousePosition;
	public Vector3 mouseWorldDelta;
	public Vector3 mouseScreenDelta;
	public Vector2 mouseVector;
	public float speed = 4.0f;
	public Vector3 deltaCast;
	public Vector3 forward;
	public Vector3 right;
	public float forwardDot;
	public float rightDot;
	RaycastHit myhit = new RaycastHit();
    public Ray myray = new Ray();

	
	// External Game Objects
	public Transform playerTransform;
	public Camera cam;
	
	void Start() {
		
	}
	
	void Update (){
		
		// Grab the screen position of Mouse Cursor
		screenMousePosition = Input.mousePosition;
		
		// Calculate the MouseScreenDelta
		mouseScreenDelta.x = (screenMousePosition.x - ((float)Screen.width / 2)) / ((float) Screen.width / 2);
		mouseScreenDelta.y = (screenMousePosition.y - ((float) Screen.height /2)) / ((float) Screen.height / 2);
		
		//MouseScreenDelta = MouseScreenDelta.x + MouseScreenDelta.x;
		
		// Feed Camera v (Height) into ScreenMousePosition
		screenMousePosition.z = cam.transform.position.y;
		
		// Convert Screen Coords to World Coords
		worldMousePosition = cam.ScreenToWorldPoint(screenMousePosition);
		
		// Calculate delta offset based on Player Position
		mouseWorldDelta.x = worldMousePosition.x + playerTransform.position.x;
		mouseWorldDelta.z = worldMousePosition.z - playerTransform.position.z;
		
		//playerTransform.transform.Rotate(Vector3.right, Time.deltaTime); 
		
		//playerTransform.transform.LookAt(Camera.main.ScreenToWorldPoint(Vector3(mousePos.x, mousePos.y, zDistance)));
	
		 myray = Camera.main.ScreenPointToRay(Input.mousePosition);
       	 if (Physics.Raycast(myray,out myhit, 1000.0f))
		{ 
			//print(myhit.collider.transform.position);
		deltaCast = (myhit.point - playerTransform.position);
			deltaCast.Normalize();
		forward = playerTransform.transform.TransformDirection(Vector3.down);
		forwardDot = Vector3.Dot(deltaCast,forward);
			//print(DeltaCast);
		right = playerTransform.transform.TransformDirection(Vector3.right);
  		rightDot = Vector3.Dot(deltaCast,right);
				  playerTransform.transform.Rotate(Vector3.forward * rightDot * 20.0f);
		/*do 
			{playerTransform.transform.Translate((deltaCast.x), deltaCast.y , 0);
			}
		while(Input.GetMouseButtonUp(0));
		*/	
		}

}
}
