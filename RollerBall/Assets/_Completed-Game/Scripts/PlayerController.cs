using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;

public class PlayerController : MonoBehaviour {
	
	// Create public variables for player speed, and for the Text UI game objects
	public float speed;
	public Text countText;
	public Text winText;
    public float xNoiseMax = 10f;
    public float yNoiseMax = 10f;

    public Vector3 anglesArduino;

	// Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
	private Rigidbody rb;
	private int count;

	// At the start of the game..
	void Start ()
	{
        anglesArduino = new Vector3();
		// Assign the Rigidbody component to our private rb variable
		rb = GetComponent<Rigidbody>();

		// Set the count to zero 
		count = 0;

		// Run the SetCountText function to update the UI (see below)
		SetCountText ();

		// Set the text property of our Win Text UI to an empty string, making the 'You Win' (game over message) blank
		winText.text = "";
	}

	// Each physics step..
	void FixedUpdate ()
	{
        print(anglesArduino);
        float x = anglesArduino.x; //left and right
        float y = anglesArduino.y; //forward and backward

		Vector3 force = Vector3.zero;
		Vector3 angles = Vector3.zero;
		
		if (Mathf.Abs(x) > xNoiseMax){
			//es valido, este x nos sirve.
			float xForce = x > 0 ? 1 : -1;
			angles.Set(x, 0, angles.z);
			force.Set(xForce,0,force.z);
		}

		if (Mathf.Abs(y) > yNoiseMax){
			float zForce = y > 0 ? 1: -1;
			angles.Set(angles.x, 0, y);
			force.Set(force.x, 0, zForce);
		}

		Vector3 force2 = force.normalized;

		rb.AddForce(force2 * angles.magnitude / 300f, ForceMode.VelocityChange);
		//rb.AddForce(force2 * angles.magnitude / 10f, ForceMode.);
        //rb.AddForce(new Vector3(x, 0, y).normalized * 10f);
        //transform.Translate(new Vector3(x, 0, y).normalized / 10f);

	}

	// When this game object intersects a collider with 'is trigger' checked, 
	// store a reference to that collider in a variable named 'other'..
	void OnTriggerEnter(Collider other) 
	{
		// ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
		if (other.gameObject.CompareTag ("Pick Up"))
		{
			// Make the other game object (the pick up) inactive, to make it disappear
			other.gameObject.SetActive (false);

			// Add one to the score variable 'count'
			count = count + 1;

			// Run the 'SetCountText()' function (see below)
			SetCountText ();
		}
	}

	// Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
	void SetCountText()
	{
		// Update the text field of our 'countText' variable
		countText.text = "Count: " + count.ToString ();

		// Check if our 'count' is equal to or exceeded 12
		if (count >= 12) 
		{
			// Set the text value of our 'winText'
			winText.text = "You Win!";
		}
	}
}