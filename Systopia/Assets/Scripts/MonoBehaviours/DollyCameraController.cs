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

	private Vector3 newPosition;
	private float step;

	private IEnumerator Start () {
		if (!moveCamera && !rotateCamera)
			yield break;

		yield return null;

		if (!moveCamera) {
			newPosition = transform.position;
			if (playerPosition.position.z < tavernEdge.position.z) {
				newPosition.z = tavernEdge.position.z;
			} else if (playerPosition.position.z > ruinEdge.position.z) {
				newPosition.z = ruinEdge.position.z;
			} else {
				newPosition.z = playerPosition.position.z;
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
			if (playerPosition.position.z < tavernEdge.position.z) {
				newPosition.z = tavernEdge.position.z;
			} else if (playerPosition.position.z > ruinEdge.position.z) {
				newPosition.z = ruinEdge.position.z;
			} else {
				newPosition.z = playerPosition.position.z;
			}
			transform.position = Vector3.MoveTowards (transform.position, newPosition, step);
		}

		
		if (rotateCamera) {
			Quaternion newRotation = Quaternion.LookRotation (playerPosition.position - transform.position + offset);
			transform.rotation = Quaternion.Slerp (transform.rotation, newRotation, Time.deltaTime * smoothing);
		}



	}
}
