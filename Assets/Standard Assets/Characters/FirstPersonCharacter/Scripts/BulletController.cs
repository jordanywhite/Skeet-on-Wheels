using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets.Characters.FirstPerson
{
	public class BulletController : MonoBehaviour
	{

		public float timeToLive = 5;
		private float timeAlive;
		private Text text;

		FirstPersonController player;

		// Use this for initialization
		private void Start ()
		{
			timeAlive = Time.time;
			text = GameObject.FindGameObjectWithTag ("Score").GetComponent<Text> () as Text;
			player = GameObject.FindGameObjectWithTag ("Player").GetComponent<FirstPersonController> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (Time.time > timeAlive + timeToLive) {
				Destroy (gameObject);
			}
		}

		void OnTriggerEnter (Collider col)
		{
			//all projectile colliding game objects should be tagged "Enemy" or whatever in inspector but that tag must be reflected in the below if conditional
			if (col.gameObject.tag == "Animal") {
				player.score += 5;
				text.text = ("Score: " + player.score);
				Destroy (col.gameObject);
				//add an explosion or something
				//destroy the projectile that just caused the trigger collision
				Destroy (gameObject);
			}
		}
	}
}