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
        //print(anglesArduino);
        float x = anglesArduino.x; //left and right, a partir de lo que le hemos pasado.
        float y = anglesArduino.y; //forward and backward, a partir de lo que le hemos pasado.

		Vector3 force = Vector3.zero;//creamos un vector
		Vector3 angles = Vector3.zero;//creamos otro vector
		

        //hacemos la comprobación. Fijamos un umbral mínimo (xNoiseMax y yNoiseMax) para obviar tods los valores que nos lleguen menores q esos.
		if (Mathf.Abs(x) > xNoiseMax){
			//es valido, este x nos sirve.
			float xForce = x > 0 ? 1 : -1;
            //modificamos los vectores
			angles.Set(x, 0, angles.z);
			force.Set(xForce,0,force.z);
		}

		if (Mathf.Abs(y) > yNoiseMax){
            //es valido, este y nos sirve.
			float zForce = y > 0 ? 1: -1;
            //modificamos los vectores
			angles.Set(angles.x, 0, y);
			force.Set(force.x, 0, zForce);
		}

		Vector3 force2 = force.normalized; //normalizamos el vector force, por lo que este vector nos aporta la direccion.

		rb.AddForce(force2 * angles.magnitude / 300f, ForceMode.VelocityChange); //añadimos una fuerza con la direccion de force y la magnitud de angles.
        //le aplicamos un reductivo de 300 para que no vaya tan rapido y el tipo de fuerza cambio de velocidad para menospreciar la inercia.

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