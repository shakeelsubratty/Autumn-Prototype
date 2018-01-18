using UnityEngine;
using System.Collections;

/*TP Motor (ThirdPerson Motor) carries out all the calculations for motion*/

public class TP_Motor : MonoBehaviour
{
	public static TP_Motor Instance; //An instance of this class generated upon playing, allowing other classes to call on the functionality of TP Motor
	
	public float DefaultForwardSpeed = 5F; //The default ForwardSpeed, to be assigned when character is no longer sprinting
	public float ForwardSpeed = 5F; //The current ForwardSpeed
	public float RunningSpeed = 10f; //The speed of the character while Sprinting 
	public float BackwardSpeed = 2F; //The speed of the character while walking backwards
	public float StrafingSpeed = 5F; //The speed of the character moving left and right
	public float JumpSpeed = 6f; //The height at which the character Jumps
	public float FloatSpeed = 12f; //The height at which the character Floats
	public float FloatFallSpeed = 3f; //The speed at which the character falls while floating (Floating Terminal Velocity)
 	public float TerminalVelocity = 20f; //The current speed at which the character is falling
	public float DefaultTerminalVelocity = 20f; //The default speed of falling for Jumping 
	public float Gravity = 21f; //Force of Gravity acting on character
	
	
	public Vector3 MoveVector { get; set; } 
	public float VerticalVelocity {get; set;}
	
	private bool floating; //Are we floating?
	
	void Awake() 
	{
		Instance = this; //Creates instance of this class
	}
	
	public void UpdateMotor() //Called by TP controller every frame to carry out calculations
	{
		SnapAlignCharacterWithCamera(); //Make character rotation = rotation of camera
		ProcessMotion(); //Calculate motion
	}
	
	void ProcessMotion()
	{
		MoveVector = transform.TransformDirection(MoveVector); //Make MoveVector = current player direction
		
		if(MoveVector.magnitude > 1)
			MoveVector = Vector3.Normalize(MoveVector); //If magnitude > 1, normalize vector to acquire smoother motion
		
		MoveVector *= MoveSpeed(); //Mutliply Vector by the moveSpeed value returned by MoveSpeed()
		
		MoveVector = new Vector3(MoveVector.x,VerticalVelocity,MoveVector.z);//Reapply Vertical Velociry to movevector.y to generate upwards and downwards motino
		
		ApplyGravity(); 
		
		TP_Controller.CharacterController.Move(MoveVector * Time.deltaTime); //Tell TP Controller to move the character using MoveVector
		
		if(TP_Controller.CharacterController.isGrounded)
			TerminalVelocity = DefaultTerminalVelocity; //If character is grounded, make current Terminal Velocity = Default
	}
	
	void ApplyGravity()
	{
			if (MoveVector.y > -TerminalVelocity)
				MoveVector = new Vector3(MoveVector.x,MoveVector.y - Gravity * Time.deltaTime,MoveVector.z); //If character is above -TerminalVelocity, pull him down by the value of gravity
			if (TP_Controller.CharacterController.isGrounded && MoveVector.y < -1)
				MoveVector = new Vector3(MoveVector.x,-1,MoveVector.z); 
	}
	
	public void Jump()
	{
		if(TP_Controller.CharacterController.isGrounded)//Only if the player is grounded
		{
			VerticalVelocity = JumpSpeed; //Make the Vertical Velocity = JumpSpeed so he rises to that value on the y - axis
		}
	}
	
	public void Float()
	{
		VerticalVelocity = FloatSpeed; //Make the Vertical Velocity = FloatSpeed so he rises to that value on the y - axis
		TerminalVelocity = FloatFallSpeed; //Decrease TerminalVelocity to FloatFallSpeed, allowing a slower fall, as if floating.
		TP_Controller.Instance.lastFloatTime = Time.time; //Set TP Controller's lastFloatTime = the current time
	}
	
	public void Running()
	{
		ForwardSpeed = RunningSpeed; //Set ForwardSpeed = RunningSpeed
	}
	
	void SnapAlignCharacterWithCamera() 
	{
		if(MoveVector.x != 0 || MoveVector.z != 0) //If not standing still...
		{
			transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y,transform.eulerAngles.z); //Let the rotation of the player = the rotation of the camera
		}
	}
	
	float MoveSpeed() //Calculates current moveSpeed
	{
		var moveSpeed = 0f;
		
		switch (TP_Animator.Instance.MoveDirection) //Switch for the MoveDirection calculated by TP_Animator
		{
			case TP_Animator.Direction.Stationary:
				moveSpeed = 0;
				break;
			case TP_Animator.Direction.Forward:
				moveSpeed = ForwardSpeed;
				break;
			
			case TP_Animator.Direction.Backward:
				moveSpeed = BackwardSpeed;
				break;
			
			case TP_Animator.Direction.Left:
				moveSpeed = StrafingSpeed;
				break;
			case TP_Animator.Direction.Right:
				moveSpeed = StrafingSpeed;
				break;
			case TP_Animator.Direction.LeftForward:  //45 degree movements use forward and backward speeds
				moveSpeed = ForwardSpeed;
				break;
			case TP_Animator.Direction.RightForward:
				moveSpeed = ForwardSpeed;
				break;
			case TP_Animator.Direction.LeftBackward:
				moveSpeed = BackwardSpeed;
				break;
			case TP_Animator.Direction.RightBackward:
				moveSpeed = BackwardSpeed;
				break;
			
			
		}
		return moveSpeed; //Return the current moveSpeed
	} 
}
