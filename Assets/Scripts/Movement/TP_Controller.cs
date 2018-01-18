using UnityEngine;
using System.Collections;

/*TP_Controller (ThirdPerson Controller) is the brain of this movement system, it takes in all input and orders TP_Motor and Camera to function.*/

public class TP_Controller : MonoBehaviour 
{
	public static CharacterController CharacterController; 
	public static TP_Controller Instance; //An instance of this class generated upon playing, allowing other classes to call on the functionality of TP Controller
	
	public float DoubleJumpPressTime = 0.5f; //For jumping and floating, the time between each jump press needed for the character to float.
	public float FloatWaitTime = 5f; //The time the player must wait in order to float again
	
	
	public float lastJumpButtonTime; //The last time the Jump button was pressedf#
	public float lastFloatTime = -20; //The last time the character Floated. Begins as a negative number to allow player to float from the beginning of the game, is set after the first jump to TP_Controller.Instance.lastFloatTime = Time.time in Motor script.
	
	void Awake() 
	{
		CharacterController = GetComponent("CharacterController") as CharacterController; //Get the character controller component
		Instance = this; //Create the instance of this class
		
		TP_Camera.UseExistingOrCreateNewMainCamera(); //Tell camera script to use existing main camera or create one if none is there
	}
	
	void Update() 
	{
		if(Camera.main == null) //If there is no main camera, return an error
			return;
		
		GetLocomotionInput();
		HandleActionInput();
		
		TP_Motor.Instance.UpdateMotor(); //Tell TP Motor to carry out update method, which calculates alignment with camera and movement.
	}
	
	void GetLocomotionInput()
	{
		var deadZone = 0.1F; //Axial input must be outside of the range -0.1 -- 0.1 in order to be registered, that way extremely slight movements do not affect the vectors
		
		TP_Motor.Instance.VerticalVelocity = TP_Motor.Instance.MoveVector.y;
		TP_Motor.Instance.MoveVector = Vector3.zero;
		
		
		if (Input.GetAxis("Vertical") > deadZone || Input.GetAxis("Vertical") < deadZone)      //Takes in axial input and adds it to the Movement vector of the character
			TP_Motor.Instance.MoveVector += new Vector3(0,0,Input.GetAxis("Vertical"));
		if (Input.GetAxis("Horizontal") > deadZone || Input.GetAxis("Horizontal") < deadZone)
			TP_Motor.Instance.MoveVector += new Vector3(Input.GetAxis("Horizontal"),0,0);
		TP_Animator.Instance.DetermineCurrentMoveDirection(); // Determines what direction the character is moving
		isFloatPossible(); //boolean function to determine with Floating is possible
	}
	
	bool isFloatPossible()
	{
		if(Time.time - lastFloatTime < FloatWaitTime) //Is the time since the last Float less than the amount of time needed to wait?
			return false; //If true, return false
		else 
			return true; //If false, return true
	}
	
	void HandleActionInput()
	{
		if(Input.GetButtonDown("Jump")) //Has the Jump button been pressed?
		{	
			if(isFloatPossible()) //Is floating possible?
			{	if (Time.time - lastJumpButtonTime < DoubleJumpPressTime) //Is the time since the last Jump button press less than the time needed to double press Jump
				{
					Float(); //Call Float() function
					lastJumpButtonTime = Time.time; //Set the last time the Jump button was pressed to be the current time
				}
				else 
				{
					Jump();
					lastJumpButtonTime = Time.time;
				}
			}
			else  										//Else, call Jump() and set the last time the Jump button was pressed to be the current time
			{
				Jump();
				lastJumpButtonTime = Time.time;
			}
		}
		if(Input.GetButton("Sprint") && CharacterController.isGrounded) //If the player is holding Sprint and the character is on the ground
		{
			Running(); //Call Running()
		}
		if(Input.GetButtonUp("Sprint")) //If the Sprint button is let go...
		{
			TP_Motor.Instance.ForwardSpeed = TP_Motor.Instance.DefaultForwardSpeed; //...Make the current Forward speed the default Forward speed
		}
	}
	
	void Jump()
	{
		TP_Motor.Instance.Jump(); //Call TP Motor's Jump()
	}
	
	void Float()
	{
		TP_Motor.Instance.Float(); //Call TP Motor's Float()
	}
	
	void Running()
	{
		TP_Motor.Instance.Running(); //Call TP Motor's Running()
	}

}
