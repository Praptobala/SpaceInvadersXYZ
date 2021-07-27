using UnityEngine;

/// <summary>
/// Handles Missile/Laser hits
/// </summary>
public class ShelterBrick : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		//Destroy the Missile/Laser when hits the brick
		//NOT the right way to access the gameobejct - TO BE CHANGED
		Destroy(other.transform.parent.gameObject);

		//Set the shelter brick to false since its hit
		gameObject.SetActive(false);	
	}
}
