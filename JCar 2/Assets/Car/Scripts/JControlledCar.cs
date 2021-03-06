using UnityEngine;
using System.Collections;
using Pose = Thalmic.Myo.Pose;

public class JControlledCar : JCar {
	public GameObject myo = null;
	public GameObject Joint = null;
	// automatic, if true car shifts automatically up/down
	public bool automatic = true;
	public float multiply=1.0f;
	public float shiftDownRPM = 1500.0f; // rpm script will shift gear down
	public float shiftUpRPM = 2700.0f; // rpm script will shift gear up
	
	void Update() {
		ThalmicHub hub = ThalmicHub.instance;
		ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();
		if (Input.GetKeyDown("page up")) {
			ShiftUp();
		}
		if (Input.GetKeyDown("page down")) {
			ShiftDown();
		}
		if (Input.GetKeyDown("g")) {
			automatic = !automatic;
		}
		if (Input.GetKeyDown("-")) {
			multiply=-multiply;
		}
		if (Input.GetKeyDown("t")) {
			switch (wheelDrive) {
			case JWheelType.Front : wheelDrive = JWheelType.All; break;
			case JWheelType.Back : wheelDrive = JWheelType.Front; break;
			case JWheelType.All : wheelDrive = JWheelType.Back; break;
			}
			foreach (WheelData w in wheels) {
				WheelCollider col = w.col;
				col.motorTorque = 0f;
				col.brakeTorque = 0f;
			}
		}
	}
	
	
	// handle the physics of the engine
	void FixedUpdate () {
		
		float steer = 0; // steering -1.0 .. 1.0
		float accel = 0; // accelerating -1.0 .. 1.0
		bool brake = false; // braking (true is brake)
		if ((checkForActive == null) || checkForActive.active) {
			// we only look at input when the object we monitor is
			// active (or we aren't monitoring an object).
			steer = -multiply*1.3f*Joint.transform.rotation.z; //0~180 = left , ~15 max. 180-360 = right, ~345 max
			accel = (float) (multiply *(2.4*(Joint.transform.rotation.x))); //0~180 = forwards, with 90 max. 180~360 =backwards, with 270 minimum
			steer+=Input.GetAxis ("Horizontal");
			accel+=Input.GetAxis ("Vertical");
			brake = Input.GetButton("Jump");
		}
		
		// handle automatic shifting
		if (automatic && (CurrentGear == 1) && (accel < 0.0f)) {
			ShiftDown(); // reverse
		}
		else if (automatic && (CurrentGear == 0) && (accel > 0.0f)) {
			ShiftUp(); // go from reverse to first gear
		}
		else if (automatic && (MotorRPM > shiftUpRPM) && (accel > 0.0f)) {
			ShiftUp(); // shift up
		}
		else if (automatic && (MotorRPM < shiftDownRPM) && (CurrentGear > 1)) {
			ShiftDown(); // shift down
		}
		if (automatic && (CurrentGear == 0)) {
			accel = - accel; // in automatic mode we need to hold arrow down for reverse
		}
		if (brake) {
			accel = -1f;
		}
		
		HandleMotor(steer, accel);
	}
}