using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// template for a MonoBehaviour
/// </summary>

public class StoredPlayerPrefs : MonoBehaviour
{
    public string[] gender = new string[3];
    public string[] age = new string[3];
    public string[] name = new string[3];
    public string[] facialHair = new string[3];
    public string[] hair = new string[3];
    public Color[] FacialHairColor  = new Color[3];
    public Color[] HairColor = new Color[3];
    public string[] underwear = new string[3];
    public string[,] jobPrefrences = new string[3,3];
    public int playerChoice = 0;

    public void GetPlayerPrefs()
    {
        
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

    }

    /// <summary>
    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// </summary>
    void LateUpdate()
    {

    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {

    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

    }

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Reset to default values.
    /// </summary>
    void Reset()
    {

    }


    /// <summary>
    /// OnMouseEnter is called when the mouse entered the GUIElement or Collider.
    /// </summary>
    void OnMouseEnter()
    {

    }

    /// <summary>
    /// OnMouseOver is called every frame while the mouse is over the GUIElement or Collider.
    /// </summary>
    void OnMouseOver()
    {

    }

    /// <summary>
    /// OnMouseExit is called when the mouse is not any longer over the GUIElement or Collider.
    /// </summary>
    void OnMouseExit()
    {

    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {

    }

    /// <summary>
    /// OnMouseUp is called when the user has released the mouse button.
    /// </summary>
    void OnMouseUp()
    {

    }

    /// <summary>
    /// OnMouseDrag is called when the user has clicked on a GUIElement or Collider and is still holding down the mouse.
    /// </summary>
    void OnMouseDrag()
    {

    }


    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    void OnTriggerEnter()
    {

    }

    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
    /// </summary>
    void OnTriggerExit()
    {

    }

    /// <summary>
    /// OnTriggerStay is called once per frame for every Collider other that is touching the trigger.
    /// </summary>
    void OnTriggerStay()
    {

    }


    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.
    /// </summary>
    void OnCollisionEnter()
    {

    }

    /// <summary>
    /// OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider.
    /// </summary>
    void OnCollisionExit()
    {

    }

    /// <summary>
    /// OnCollisionStay is called once per frame for every collider/rigidbody that is touching rigidbody/collider.
    /// </summary>
    void OnCollisionStay()
    {

    }

    /// <summary>
    /// OnControllerColliderHit is called when the controller hits a collider while performing a Move.
    /// </summary>
    void OnControllerColliderHit()
    {

    }

    /// <summary>
    /// Called when a joint attached to the same game object broke.
    /// </summary>
    void OnJointBreak()
    {

    }

    /// <summary>
    /// OnParticleCollision is called when a particle hits a collider.
    /// </summary>
    void OnParticleCollision()
    {

    }




    /// <summary>
    /// OnBecameVisible is called when the renderer became visible by any camera.
    /// </summary>
    void OnBecameVisible()
    {

    }

    /// <summary>
    /// OnBecameInvisible is called when the renderer is no longer visible by any camera.
    /// </summary>
    void OnBecameInvisible()
    {

    }




    /// <summary>
    /// This function is called after a new level was loaded.
    /// </summary>
    void OnLevelWasLoaded()
    {

    }




    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {

    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled () or inactive.
    /// </summary>
    void OnDisable()
    {

    }




    /// <summary>
    /// OnPreCull is called before a camera culls the scene.
    /// </summary>
    void OnPreCull()
    {

    }

    /// <summary>
    /// OnPreRender is called before a camera starts rendering the scene.
    /// </summary>
    void OnPreRender()
    {

    }

    /// <summary>
    /// OnPostRender is called after a camera finished rendering the scene.
    /// </summary>
    void OnPostRender()
    {

    }

    /// <summary>
    /// OnRenderObject is used to render your own objects using Graphics.DrawMesh or other functions.
    /// </summary>
    void OnRenderObject()
    {

    }

    /// <summary>
    /// OnWillRenderObject is called once for each camera if the object is visible.
    /// </summary>
    void OnWillRenderObject()
    {

    }

    /// <summary>
    /// OnRenderImage is called after all rendering is complete to render image
    /// </summary>
    void OnRenderImage()
    {

    }




    /// <summary>
    /// OnGUI is called for rendering and handling GUI events.
    /// </summary>
    void OnGUI()
    {

    }




    /// <summary>
    /// Implement this OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {

    }

    /// <summary>
    /// Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected.
    /// </summary>
    void OnDrawGizmosSelected()
    {

    }




    /// <summary>
    /// Sent to all game objects when the player pauses.
    /// </summary>
    void OnApplicationPause()
    {

    }

    /// <summary>
    /// Sent to all game objects before the application is quit.
    /// </summary>
    void OnApplicationQuit()
    {

    }





    /// <summary>
    /// Called on the server whenever a new player has successfully connected.
    /// </summary>
    void OnPlayerConnected()
    {

    }

    /// <summary>
    /// Called on the server whenever a Network.InitializeServer was invoked and has completed.
    /// </summary>
    void OnServerInitialized()
    {

    }

    /// <summary>
    /// Called on the client when you have successfully connected to a server
    /// </summary>
    void OnConnectedToServer()
    {

    }

    /// <summary>
    /// Called on the server whenever a player disconnected from the server.
    /// </summary>
    void OnPlayerDisconnected()
    {

    }

    /// <summary>
    /// Called on the client when the connection was lost or you disconnected from the server.
    /// </summary>
    void OnDisconnectedFromServer()
    {

    }

    /// <summary>
    /// Called on the client when a connection attempt fails for some reason.
    /// </summary>
    void OnFailedToConnect()
    {

    }

    /// <summary>
    /// Called on clients or servers when there is a problem connecting to the master server.
    /// </summary>
    void OnFailedToConnectToMasterServer()
    {

    }

    /// <summary>
    /// Called on objects which have been network instantiated with Network.Instantiate
    /// </summary>
    void OnNetworkInstantiate()
    {

    }

    /// <summary>
    /// Used to customize synchronization of variables in a script watched by a network view.
    /// </summary>
    void OnSerializeNetworkView()
    {

    }

}
