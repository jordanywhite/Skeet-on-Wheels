using UnityEngine;
using System.Collections;

public class Recoil : MonoBehaviour {

	public float recoilSpeed = 0.01f;    // Speed to move camera

	Quaternion startingPos;
	Quaternion recoilPos;
	private void Start () {
		startingPos = transform.localRotation;
		recoilPos = transform.localRotation;
	}

	private void Update () {

		if (Input.GetMouseButtonDown(0)){
			recoilBack();
		}

		if (Input.GetMouseButtonDown(0)){
			recoilForward();
		}

	}

	// Move current weapon to zoomed in position smoothly over time
	private IEnumerator MoveToPosition(Quaternion newPosition, float time){
		float elapsedTime = 0f;
		var startingPos = transform.localRotation;

		while (elapsedTime < time){
			transform.rotation = Quaternion.RotateTowards(startingPos, recoilPos, (elapsedTime / time));
			elapsedTime += Time.deltaTime;
			yield return new WaitForSeconds(.05f);
		}
	}

	private void recoilBack(){

		// Start coroutine to move the camera up smoothly over time
		//Vector3 zoomOutOffset = new Vector3(0f, 0f, 0.5f);
		//Vector3 zoomOutWorldPosition = transform.TransformDirection( zoomOutOffset );
		Quaternion newPos = new Quaternion(0f, 0f, 90f, 0f);
		// Move the camera smoothly 
		StartCoroutine(MoveToPosition(recoilPos, recoilSpeed));             
	}

	private void recoilForward(){

		// Start coroutine to move the camera down smoothly over time
		//Vector3 zoomInOffset = new Vector3(0f, 0f, -0.5f);
		//Vector3 zoomInWorldPosition = transform.TransformDirection( zoomInOffset );
		// Move the camera smoothly 
		Quaternion newPos = new Quaternion(0f, 0f, 0f, 0f);
		StartCoroutine(MoveToPosition(startingPos, recoilSpeed));
	}
}
