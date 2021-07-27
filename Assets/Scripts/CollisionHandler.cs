using System;
using UnityEngine;

/// <summary>
/// Handles collisions and triggers
/// Send callbacks to parent object
/// </summary>
public class CollisionHandler : MonoBehaviour
{
	#region PUBLIC_VARS
	/// <summary>
	/// Callback to handle when Trigger entered
	/// </summary>
	public Action<Collider> TriggerEntered;

	/// <summary>
	/// Callback to handle when Collision entered
	/// </summary>
	public Action<Collision> CollisionEntered;
	#endregion

	#region METHODS
	private void OnTriggerEnter(Collider other)
	{
		TriggerEntered?.Invoke(other);
	}

	private void OnCollisionEnter(Collision collision)
	{
		CollisionEntered?.Invoke(collision);
	}
	#endregion
}
