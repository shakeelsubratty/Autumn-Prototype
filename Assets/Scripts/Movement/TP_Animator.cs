using UnityEngine;
using System.Collections;

/*TP_Animator (ThirdPerson Animator) currently just calculates the movement states the character is in, but will eventually be used to assign animation to these seperate states*/

public class TP_Animator : MonoBehaviour {

	public enum Direction //Enumerator holding the different directions the player could be moving in.
	{
		Stationary, Forward, Backward, Left, Right,
		LeftForward, RightForward, LeftBackward, RightBackward
	}
	
	public static TP_Animator Instance; //An instance of this class generated upon playing, allowing other classes to call on the functionality of TP Animator
	
	public Direction MoveDirection {get; set;} //The MoveDirection itself
	
	void Awake () 
	{
		Instance = this; //Creates an instance of this class
	}
	
	void Update () 
	{
	
	}
	
	public void DetermineCurrentMoveDirection()
	{
		var forward = false;
		var backward = false;
		var left = false;
		var right = false; //Default to false
		
		if (TP_Motor.Instance.MoveVector.z > 0) //If character is moving forward...
			forward = true;
		if (TP_Motor.Instance.MoveVector.z < 0) //If character is moving backward...
			backward = true;
		if (TP_Motor.Instance.MoveVector.x > 0) //etc...
			right = true;	
		if (TP_Motor.Instance.MoveVector.x < 0)	//etc...
			left = true;
		
		if (forward) //If he is moving forward...
		{
			if (left) //Is he moving forward and left?
				MoveDirection = Direction.LeftForward;
			else if (right) //Is he moving forward and right?
				MoveDirection = Direction.RightForward;
			else //If not, he is just moving forward
				MoveDirection = Direction.Forward;
		}
		else if (backward) //Same^^
		{
			if (left)
				MoveDirection = Direction.LeftBackward;
			else if (right)
				MoveDirection = Direction.RightBackward;
			else
				MoveDirection = Direction.Backward;
		}
		else if (left) //If he is moving left...
		{
			MoveDirection = Direction.Left;
		}
		else if (right) //If he is moving right...
		{
			MoveDirection = Direction.Right;
		}
		else //If all above is false, he is stationary...
		{
			MoveDirection = Direction.Stationary;
		}
		
	}  
		
		
}
