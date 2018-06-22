using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyCameraController : MonoBehaviour {

	[SerializeField] private float smoothing = 7f;
	[SerializeField] private Vector3 offset = new Vector3 (0f, 1.5f, 0f);
	[SerializeField] private Transform playerPosition;
	[SerializeField] private Transform tavernEdge;
	[SerializeField] private Transform ruinEdge;
	[SerializeField] private bool rotateCamera = true;
	[SerializeField] private bool moveCamera = true;
	[SerializeField] private bool changeXAxis = false;
	[SerializeField] private bool changeYAxis = false;
	[SerializeField] private bool changeZAxis = false;
	[SerializeField] private float moveCameraDistance = 0f;

	private Vector3 newPosition;
	private float step;

	private IEnumerator Start () {
		if (!moveCamera && !rotateCamera)
			yield break;

		yield return null;

		if (!moveCamera) {
			newPosition = transform.position;
			if (changeZAxis) {
				if (playerPosition.position.z < tavernEdge.position.z) {
					newPosition.z = tavernEdge.position.z + moveCameraDistance;
				} else if (playerPosition.position.z > ruinEdge.position.z) {
					newPosition.z = ruinEdge.position.z+ moveCameraDistance;
				} else {
					newPosition.z = playerPosition.position.z + moveCameraDistance;
				}
			}

			if (changeYAxis) {
				if (playerPosition.position.y < tavernEdge.position.y) {
					newPosition.y = tavernEdge.position.y + moveCameraDistance;
				} else if (playerPosition.position.y > ruinEdge.position.y) {
					newPosition.y = ruinEdge.position.y + moveCameraDistance;
				} else {
					newPosition.y = playerPosition.position.y + moveCameraDistance;
				}
			}

			if (changeXAxis) {
				if (playerPosition.position.x < tavernEdge.position.x) {
					newPosition.x = tavernEdge.position.x + moveCameraDistance;
				} else if (playerPosition.position.x > ruinEdge.position.x) {
					newPosition.x = ruinEdge.position.x + moveCameraDistance;
				} else {
					newPosition.x = playerPosition.position.x + moveCameraDistance;
				}
			}
				
			transform.position = newPosition;
		}

		if (!rotateCamera)
			transform.rotation = Quaternion.LookRotation (playerPosition.position - transform.position + offset);
	}

	private void LateUpdate () {
		if (!moveCamera && !rotateCamera)
			return;
		
		if (moveCamera) {
			step = 30f * Time.deltaTime;
			newPosition = transform.position;

			if (changeZAxis) {
				if (playerPosition.position.z < tavernEdge.position.z) {
					newPosition.z = tavernEdge.position.z + moveCameraDistance;
				} else if (playerPosition.position.z > ruinEdge.position.z) {
					newPosition.z = ruinEdge.position.z + moveCameraDistance;
				} else {
					newPosition.z = playerPosition.position.z + moveCameraDistance;
				}
			}

			if (changeYAxis) {
				if (playerPosition.position.y < tavernEdge.position.y) {
					newPosition.y = tavernEdge.position.y + moveCameraDistance;
				} else if (playerPosition.position.y > ruinEdge.position.y) {
					newPosition.y = ruinEdge.position.y + moveCameraDistance;
				} else {
					newPosition.y = playerPosition.position.y + moveCameraDistance;
				}
			}

			if (changeXAxis) {
				if (playerPosition.position.x < tavernEdge.position.x) {
					newPosition.x = tavernEdge.position.x + moveCameraDistance;
				} else if (playerPosition.position.x > ruinEdge.position.x) {
					newPosition.x = ruinEdge.position.x + moveCameraDistance;
				} else {
					newPosition.x = playerPosition.position.x + moveCameraDistance;
				}
			}

			transform.position = Vector3.MoveTowards (transform.position, newPosition, step);
		}

		
		if (rotateCamera) {
			Quaternion newRotation = Quaternion.LookRotation (playerPosition.position - transform.position + offset);
			transform.rotation = Quaternion.Slerp (transform.rotation, newRotation, Time.deltaTime * smoothing);
		}



	}
}
