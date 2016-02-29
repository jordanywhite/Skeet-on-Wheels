using UnityEngine;
using System.Collections;

public class audioScript : MonoBehaviour {
	private GameObject player;
	private AudioSource audio;
	private bool hasPlayed;
	public int triggerDistance = 30;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
		audio = this.gameObject.GetComponent <AudioSource> ();
		audio.playOnAwake = false;
		hasPlayed = false;
	}



	// Update is called once per frame
	void Update () {
		if (Vector3.Distance (player.transform.position, this.gameObject.transform.position) <= triggerDistance && hasPlayed == false)
		{
			audio.Play ();
			hasPlayed = true;
		}
	}
}
