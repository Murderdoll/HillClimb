using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CarScript : MonoBehaviour {

	WheelJoint2D[] wheelJoints;
	JointMotor2D frontWheel;
	JointMotor2D backWheel;

	public float maxSpeed = -1000f;
	private float maxBackSpeed = 1500f;
	private float acceleration = 800f;
	private float deacceleration = -100f;
	public float brakeForce = 3000f;
	private float gravity = 9.8f;
	private float angleCar = 0;
	public bool grounded = false;
	public LayerMask MAP;
	public Transform Bwheel;
	private int coinsInt = 0;
	public Text coinsText;
	public float fuelSize;
	public float fuelUsage;
	private float currentFuel;
	public GameObject fuelProgressBar; 
	public GameObject centerMass;
	public float fuelAdd = 3;
	private AudioSource carSound;
	public AudioSource coinSound;

	public ClickScript[] ControlCar;

	// Use this for initialization
	void Start () {

		GetComponent<Rigidbody2D> ().centerOfMass = centerMass.transform.localPosition;
		wheelJoints = gameObject.GetComponents<WheelJoint2D>();
		backWheel = wheelJoints[1].motor;
		frontWheel = wheelJoints[0].motor;
		currentFuel = fuelSize;
		carSound = GetComponent<AudioSource>();
	}
	void Update()
	{
		coinsText.text = coinsInt.ToString ();
		grounded = Physics2D.OverlapCircle (Bwheel.transform.position, 0.30f, MAP);
	}
	void FixedUpdate() {

		if (currentFuel <= 0) {
			print ("Закончилось топливо");
			return;
		}
		frontWheel.motorSpeed = backWheel.motorSpeed;

		angleCar = transform.localEulerAngles.z;

		if (angleCar >= 180)
		{
			angleCar = angleCar - 360;
		}


		if (grounded == true)
		{
			if (ControlCar[0].clickedIs == true)
			{
				backWheel.motorSpeed = Mathf.Clamp(backWheel.motorSpeed - (acceleration - gravity * Mathf.PI * (angleCar / 2)) *  Time.deltaTime, maxSpeed, maxBackSpeed) ;
				currentFuel -= (fuelUsage /0.5f) * Time.deltaTime;


			}
			else if ((backWheel.motorSpeed < 0) || (ControlCar[0].clickedIs == false && backWheel.motorSpeed == 0 && angleCar < 0))
			{
				backWheel.motorSpeed = Mathf.Clamp(backWheel.motorSpeed - (deacceleration - gravity * Mathf.PI * (angleCar / 2)) * Time.deltaTime, maxSpeed, 0);
				currentFuel -= (fuelUsage / 0.5f) * Time.deltaTime;
			}
			if ((ControlCar[0].clickedIs == false && backWheel.motorSpeed > 0) || (ControlCar[0].clickedIs == false && backWheel.motorSpeed == 0 && angleCar > 0))
			{
				backWheel.motorSpeed = Mathf.Clamp(backWheel.motorSpeed - (-deacceleration - gravity * Mathf.PI * (angleCar / 2)) *  Time.deltaTime, maxSpeed, 0);
				currentFuel -= (fuelUsage /0.5f) * Time.deltaTime;
			}
		}
		else if (ControlCar[0].clickedIs == false && backWheel.motorSpeed < 0)
		{
			backWheel.motorSpeed = Mathf.Clamp(backWheel.motorSpeed - deacceleration * Time.deltaTime, maxSpeed, 0);
			currentFuel -= (fuelUsage /0.5f) * Time.deltaTime;
		}
		else if (ControlCar[0].clickedIs == false && backWheel.motorSpeed > 0)
		{
			backWheel.motorSpeed = Mathf.Clamp(backWheel.motorSpeed + deacceleration * Time.deltaTime, 0, maxBackSpeed);
			currentFuel -= (fuelUsage /0.5f) * Time.deltaTime;
		}
		if (ControlCar [0].clickedIs == true && grounded == false)
		{
			backWheel.motorSpeed = Mathf.Clamp(backWheel.motorSpeed - (acceleration - gravity * Mathf.PI * (angleCar / 2)) * Time.deltaTime, maxSpeed, maxBackSpeed);
			currentFuel -= (fuelUsage /0.5f) * Time.deltaTime;
		}
		if (ControlCar [0].clickedIs == false && grounded == false)
		{
			currentFuel -= (fuelUsage /0.5f) * Time.deltaTime;
		}

		if (ControlCar[1].clickedIs == true && backWheel.motorSpeed > 0)
		{
			backWheel.motorSpeed = Mathf.Clamp(backWheel.motorSpeed - brakeForce * Time.deltaTime, 0, maxBackSpeed);
		}
		else if (ControlCar[1].clickedIs == true && backWheel.motorSpeed < 0)
		{
			backWheel.motorSpeed = Mathf.Clamp(backWheel.motorSpeed + brakeForce * Time.deltaTime, maxSpeed, 0);
		}

		wheelJoints[1].motor = backWheel;
		wheelJoints[0].motor = frontWheel;

		carSound.pitch = Mathf.Clamp (-backWheel.motorSpeed / 1000, 0.3f, 3);
		fuelProgressBar.transform.localScale = new Vector2 (currentFuel / fuelSize,1);
	}
	void OnTriggerEnter2D(Collider2D trigger)
	{
		if (trigger.gameObject.tag == "coins") {
			coinsInt++;
			coinSound.Play ();
			Destroy (trigger.gameObject);
		} else if (trigger.gameObject.tag == "Finish") {
			SceneManager.LoadScene (0);
		} else if (trigger.gameObject.tag == "Fuel") {
			currentFuel += fuelAdd;
			Destroy (trigger.gameObject);
		}
	}
}