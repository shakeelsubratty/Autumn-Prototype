using UnityEngine;
using System.Collections;

/*TP Camera (ThirdPersonCamera) calulates the camera position*/

public class TP_Camera : MonoBehaviour
{
	public static TP_Camera Instance; //An instance of this class generated upon playing, allowing other classes to call on the functionality of TP Camera
	
	
	public Transform TargetLookAt; //Where the camera looks at, must be created in Unity and attached to character
	
	public float Distance = 5f; //Current Distance of camera from character
	public float DistanceMin = 3f; //The minimum distance the camera can be from the character
	public float DistanceMax = 10f; //The maximum distance the camera can be from the character
	public float DistanceSmooth = 0.05f; //The smoothing factor with camera dollying
	public float X_MouseSensitivity = 5f; //X - axis sensitvity from the mouse
	public float Y_MouseSensitivity = 5f; //Y - axis sensitvity from the mouse
	public float MouseWheelMouseSensitivity = 5f; //MouseWheel - axis sensitvity from the mouse
	public float X_Smooth = 0.05f; //Smoothing factor with camera rotation along X-axis
	public float Y_Smooth = 0.1f; //Smoothing factor with camera rotation along Y-axis
	public float Y_MinLimit = -40f; //Minimum value of Y-axis rotation...
	public float Y_MaxLimit = 80f; //Maximum value of Y-axis rotation...		...prevents camera from flipping upsidedown by going completely over/under the character
	public bool RightMouseHeldDown = false; //For development purposes, camera rotation can be used only when holding right mouse, allows for adjustments without the camera rotating wildly
	
	private float mouseX = 0f; //X value from mouse
	private float mouseY = 0f; //Y value from mouse
	private float velX = 0f;			//Velocity of change of//
	private float velY = 0f;			//X,Y,Z coordinateas of camera position//	
	private float velZ = 0f;			//as well as the distance of the camera//
	private float velDistance = 0f; 
	private float startDistance = 0f; //Starting distance of the camera
	private Vector3 position = Vector3.zero; //Current position of the camera
	private Vector3 desiredPosition = Vector3.zero; //Desired position of the camera 
	private float desiredDistance = 0f; //Desired distance of the camera
	
	
	void Awake()
	{
		Instance = this; //Creates an instance of the class
	}
	
	void Start() 
	{
		Distance = Mathf.Clamp(Distance,DistanceMin,DistanceMax); //Clamps the camera Distance between min and max values
		startDistance = Distance; //Current distance = Start distance
		Reset(); //Call Reset() function 
	}
	
	void LateUpdate() //Occurs sligtly after TP Controller update
	{
		if(TargetLookAt == null) //If no targetLookAt, return error.
			return;
		
		HandlePlayerInput();
		
		CalculateDesiredPosition();
		
		UpdatePosition();
		
	}
	
	void HandlePlayerInput()
	{
		var deadZone = 0.01f; //Axial input must be outside of the range -0.1 -- 0.1 in order to be registered, that way extremely slight movements do not affect the vectors
		
		if (RightMouseHeldDown) //If right mouse button must be held down...
		{	if(Input.GetMouseButton(1)) //If it is being held down...
			{
				mouseX += Input.GetAxis("Mouse X") * X_MouseSensitivity; //mouseX = axial input * sensitivity
				mouseY -= Input.GetAxis("Mouse Y") * Y_MouseSensitivity; //mouseY = axial input * sensitivity
			}
		}
		else //Otherwise, carry out same calculations without RightMouseHeldDown
		{
			mouseX += Input.GetAxis("Mouse X") * X_MouseSensitivity;
			mouseY -= Input.GetAxis("Mouse Y") * Y_MouseSensitivity;
		}
			
		
		
		mouseY = Helper.ClampAngle(mouseY, Y_MinLimit, Y_MaxLimit); // This is where we clamp Mouse Y rotation
		
		if (Input.GetAxis("Mouse ScrollWheel") < -deadZone || Input.GetAxis("Mouse ScrollWheel") > deadZone) //If axial input from scrollwheel is outside deadzone...
		{
			desiredDistance = Mathf.Clamp(Distance - Input.GetAxis("Mouse ScrollWheel") * MouseWheelMouseSensitivity, DistanceMin, DistanceMax); //Calculate the desired distance of the camera
			
		}
	}
	
	void CalculateDesiredPosition()
	{
		 
		Distance = Mathf.SmoothDamp(Distance, desiredDistance, ref velDistance, DistanceSmooth); //Evaluate distance
		
		
		desiredPosition = CalculatePosition(mouseY, mouseX, Distance); //Calculate desired position using CalculatePosition()
	}
	
	Vector3 CalculatePosition(float rotationX, float rotationY, float distance)
	{
		Vector3 direction = new Vector3(0,0,-distance);
		Quaternion rotation = Quaternion.Euler(rotationX,rotationY,0);
		return TargetLookAt.position + rotation * direction;   //Returns Vector3 position 
	}
	
	void UpdatePosition()
	{
		var posX = Mathf.SmoothDamp(position.x, desiredPosition.x,ref velX, X_Smooth); //Seperate SmoothDamp for X,Y and Z, taking in current position,  
		var posY = Mathf.SmoothDamp(position.y, desiredPosition.y,ref velY, Y_Smooth); //desired position, the rate at which it will change
		var posZ = Mathf.SmoothDamp(position.z, desiredPosition.z,ref velZ, X_Smooth); //and a smoothing factor.
		position = new Vector3(posX,posY,posZ); //Using the calculated X,Y,Z points, make a new Vector3 position.
		
		transform.position = position; //Make the camera's position = calculated position
		
		transform.LookAt(TargetLookAt); //Make camera look at TargetLookAt
	}
	
	public void Reset() //Resets position of camera on Awake()
	{
		mouseX = 0;
		mouseY = 10;
		Distance = startDistance;
		desiredDistance = Distance;
	}
	
	public static void UseExistingOrCreateNewMainCamera()
	{
		GameObject tempCamera; 
		GameObject targetLookAt;
		TP_Camera myCamera;
		
		if (Camera.main != null) //If main camera exists
		{
			tempCamera = Camera.main.gameObject; //Make tempCamera the main camera
		}
		
		else //If no main camera exists
		{
			tempCamera = new GameObject("Main Camera"); //Make a new camera called Main Camera
			tempCamera.AddComponent("Camera"); //Add Camera component
			tempCamera.tag = "MainCamera"; //Tag as "Main Camera"
		}
		
		tempCamera.AddComponent("TP_Camera"); //Add TP Camera script to camera
		myCamera = tempCamera.GetComponent("TP_Camera") as TP_Camera; //Assign myCamera to tempCamera 
		
		targetLookAt = GameObject.Find("targetLookAt") as GameObject; //Find targetLookAt
		
		if (targetLookAt == null) //If not found
		{
			targetLookAt = new GameObject("targetLookAt"); //Create new targetLookAt
			targetLookAt.transform.position = Vector3.zero; // Position at Vector3.zero
		}
		
		myCamera.TargetLookAt = targetLookAt.transform; //Make the myCamera.TargetLookAt = transform of the gameobject targetLookAt
	}
}
