using UnityEngine;

/* Helper is a small class that holds somewhat miscellaneous functions and methods for use by other classes*/
public static class Helper
{
	public static float ClampAngle(float angle, float min, float max) //Clamps the angle of the camera between a min/max point
	{
		do
		{
			if (angle < -360)
				angle += 360;
			if (angle > 360)
				angle -= 360;
		} while (angle < -360 || angle > 360);
		
		return Mathf.Clamp(angle, min, max);
	}
}


