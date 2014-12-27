using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	public static bool ePress;
    static GameObject attachedTo;
    private CharacterMotor motor;
    
	// Use this for initialization
	void Start () {
	
	}

    void Awake()
    {
        attachedTo = this.gameObject;
        ePress = false;
        motor = this.GetComponent<CharacterMotor>();
    }
	
	// Update is called once per frame
	void Update () 
    {
        
            Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (directionVector != Vector3.zero)
            {
                // Get the length of the directon vector and then normalize it
                // Dividing by the length is cheaper than normalizing when we already have the length anyway
                float directionLength = directionVector.magnitude;
                directionVector = directionVector / directionLength;

                // Make sure the length is no bigger than 1
                directionLength = Mathf.Min(1, directionLength);

                // Make the input vector more sensitive towards the extremes and less sensitive in the middle
                // This makes it easier to control slow speeds when using analog sticks
                directionLength = directionLength * directionLength;

                // Multiply the normalized direction vector by the modified length
                directionVector = directionVector * directionLength;
            }

            // Apply the direction to the CharacterMotor
            motor.inputMoveDirection = transform.rotation * directionVector;
            motor.inputJump = Input.GetButton("Jump");

            if (Input.GetKeyDown(KeyCode.E) && ePress == false)
            {
                DisableMotor();
            }
            if (Input.GetKeyDown(KeyCode.Escape) && ePress == true)
            {
                EnableMotor();
            }
        
	}

	public static void DisableMotor()
	{
	    attachedTo.GetComponent<CharacterMotor>().movement.maxGroundAcceleration = 0.0f;
	    attachedTo.GetComponent<CharacterMotor>().movement.maxAirAcceleration = 0.0f;
	    attachedTo.GetComponent<CharacterMotor>().jumping.enabled = false;
		ePress = true;
        ConsoleScreen.pressedE = true;
	}
	public static void EnableMotor() {
        attachedTo.GetComponent<CharacterMotor>().movement.maxGroundAcceleration = 30.0f;
        attachedTo.GetComponent<CharacterMotor>().movement.maxAirAcceleration = 20.0f;
	    attachedTo.GetComponent<CharacterMotor>().jumping.enabled = true;
		ePress = false;
        ConsoleScreen.pressedE = false;
	}

}

