using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour {

	public Animator animator;
	public NavMeshAgent agent;
	public PlayerLocation playerLocation;
	public float turnSmoothing = 15f;
	public float speedDampTime = 0.1f;
	public float slowingSpeed = 0.175f;
	public float turnSpeedThreshold = 0.5f; // speed beyond which the player can move and turn normally
	public float inputHoldDelay = 0.5f;

	private Interactable currentInteractable;
	private Vector3 destinationPosition;
	private bool handleInput = true;
	private WaitForSeconds inputHoldWait;

	public const string startingPositionKey = "starting position";
	public const string currentSceneKey = "current scene";

	private readonly int hashSpeedPara = Animator.StringToHash ("Speed");
	private const float stopDistanceProportion = 0.1f; 
	private const float navMeshSampleDistance = 4f; // maximum distance a click can be accepted

	private void Start () {
		agent.updateRotation = false; // player will be rotated by this script so the nav mesh agent should not rotate it
		inputHoldWait = new WaitForSeconds (inputHoldDelay);
		string startingPositionName = playerLocation.startingPositionName;
		Transform startingPosition = StartingPosition.FindStartingPosition (startingPositionName);
		if (startingPosition == null)
			startingPosition = this.transform;
		if (playerLocation.currentPositionSet) {
			startingPosition.position = playerLocation.currentPosition;
			startingPosition.rotation = playerLocation.currentRotation;
			playerLocation.startingPositionName = "";
		}

		transform.position = startingPosition.position;
		transform.rotation = startingPosition.rotation;

		destinationPosition = transform.position;
	}

	private void OnAnimatorMove () {
		if (!GameStateManager.isPaused)
			agent.velocity = animator.deltaPosition / ((Time.deltaTime  == 0) ? 1f : Time.deltaTime ) ; // set velocity of nav mesh agent to speed of the animator
	}

	private void Update () {
		if (agent.pathPending)
			return;
		float speed = agent.desiredVelocity.magnitude;
		if (agent.remainingDistance <= agent.stoppingDistance * stopDistanceProportion)
			Stopping (out speed);
		else if (agent.remainingDistance <= agent.stoppingDistance)
			Slowing (out speed, agent.remainingDistance);
		else if (speed > turnSpeedThreshold)
			Moving ();

		animator.SetFloat (hashSpeedPara, speed, speedDampTime, Time.deltaTime);
	}

	private void Stopping (out float speed) {
		agent.isStopped = true;
		transform.position = destinationPosition;
		speed = 0f;

		if (currentInteractable) {
			transform.rotation = currentInteractable.interactionLocation.rotation;
			currentInteractable.Interact ();
			currentInteractable = null;

			StartCoroutine (WaitForInteraction ());
		}
	}

	private void Slowing (out float speed, float distanceToDestination) {
		agent.isStopped = true; // moving will be controlled manually
		float proportionalDistance = 1f - distanceToDestination / agent.stoppingDistance;
		Quaternion targetRotation = currentInteractable ? currentInteractable.interactionLocation.rotation : transform.rotation;
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, proportionalDistance);
		transform.position = Vector3.MoveTowards (transform.position, destinationPosition, slowingSpeed * Time.deltaTime);
		speed = Mathf.Lerp (slowingSpeed, 0f, proportionalDistance);
	}

	private void Moving () {
		Quaternion targetRotation = Quaternion.LookRotation (agent.desiredVelocity);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
	}

	public void OnGroundClick (BaseEventData data) {
		Debug.Log ("on ground click");
		if (!handleInput)
			return;

		currentInteractable = null;
		PointerEventData pData = (PointerEventData)data;

		NavMeshHit hit;
		if (NavMesh.SamplePosition (pData.pointerCurrentRaycast.worldPosition, out hit, navMeshSampleDistance, NavMesh.AllAreas))
			destinationPosition = hit.position;
		else
			destinationPosition = pData.pointerCurrentRaycast.worldPosition;

		agent.SetDestination (destinationPosition);
		agent.isStopped = false;
	}

	public void OnInteractableClick (Interactable interactable) {
		Debug.Log ("on interactable click");
		if (!handleInput)
			return;

		currentInteractable = interactable;
		destinationPosition = currentInteractable.interactionLocation.position;

		agent.SetDestination (destinationPosition);
		agent.isStopped = false;
	}

	private IEnumerator WaitForInteraction () {
		handleInput = false;
		yield return inputHoldWait;
		handleInput = true;
	}
}
