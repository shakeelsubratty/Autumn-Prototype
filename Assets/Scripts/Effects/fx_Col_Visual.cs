using UnityEngine;
using System.Collections;

public class fx_Col_Visual : MonoBehaviour {

    public float rotatingSpeed = 40.0f; // rotation speed degrees per second
    public float maxUpAndDown = 1;       // amount of meters going up and down
    public float speed = 200;      // up and down speed
    protected float angle = 0;       // angle to determin the height by using the sinus
    protected float toDegrees  = Mathf.PI/180;    // radians to degrees

    public Vector3 currentPos;

	void Start () {
        currentPos = this.transform.position;

	}
	void Update () {
        ProcessRotation();
        ProcessFloating();
	}
    void ProcessRotation()
    {
        transform.Rotate(0, rotatingSpeed * Time.deltaTime, 0, Space.World);
    }
    void ProcessFloating()
    {
        angle += speed * Time.deltaTime;
        if (angle > 360) { angle -= 360; }
        transform.position.Set(currentPos.x, maxUpAndDown * Mathf.Sin(angle * toDegrees), currentPos.z);
    }
}
