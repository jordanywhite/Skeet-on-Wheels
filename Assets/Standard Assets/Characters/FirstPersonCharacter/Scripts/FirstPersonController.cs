using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
	[RequireComponent (typeof(CharacterController))]
	[RequireComponent (typeof(AudioSource))]
	public class FirstPersonController : MonoBehaviour
	{
		[SerializeField] private bool m_IsWalking;
		[SerializeField] private float m_WalkSpeed;
		[SerializeField] private float m_RunSpeed;
		[SerializeField] [Range (0f, 1f)] private float m_RunstepLenghten;
		[SerializeField] private float m_JumpSpeed;
		[SerializeField] private float m_StickToGroundForce;
		[SerializeField] private float m_GravityMultiplier;
		[SerializeField] private MouseLook m_MouseLook;
		[SerializeField] private bool m_UseFovKick;
		[SerializeField] private FOVKick m_FovKick = new FOVKick ();
		[SerializeField] private bool m_UseHeadBob;
		[SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob ();
		[SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob ();
		[SerializeField] private float m_StepInterval;
		[SerializeField] private AudioClip[] m_FootstepSounds;
		// an array of footstep sounds that will be randomly selected from.
		[SerializeField] private AudioClip m_JumpSound;
		// the sound played when character leaves the ground.
		[SerializeField] private AudioClip m_LandSound;
		// the sound played when character touches back on ground.

		private Camera m_Camera;
		private bool m_Jump;
		private float m_YRotation;
		private Vector2 m_Input;
		private Vector3 m_MoveDir = Vector3.zero;
		private CharacterController m_CharacterController;
		private CollisionFlags m_CollisionFlags;
		private bool m_PreviouslyGrounded;
		private Vector3 m_OriginalCameraPosition;
		private float m_StepCycle;
		private float m_NextStep;
		private bool m_Jumping;
		private AudioSource m_AudioSource;
		
		public GameObject car;
		public GameObject spawn;
		public GameObject gun;
		public Rigidbody projectile;
		public float bulletSpeed = 20;
		public int bulletCount = 5;
		public float bulletSpread = .01f;
		public float fireRate = .5f;

		public int MAX_AMMO = 4;
		public int currAmmo = 4;

		public bool canShoot = true;
		public bool reloading = false;

		private float waitTime = 0.0f;
		public float timeToReload = 3.0f;

		private float nextFire = 0.0f;

		public float startOffsetY = 0f;
		public float startOffsetZ = 0f;

		private Quaternion reloadPos;
		private Quaternion startPos;

		public AudioClip shootSound;
		public AudioClip reloadSound;

		// Use this for initialization
		private void Start ()
		{
			m_CharacterController = GetComponent<CharacterController> ();
			m_Camera = Camera.main;
			m_OriginalCameraPosition = m_Camera.transform.localPosition;
			m_FovKick.Setup (m_Camera);
			m_HeadBob.Setup (m_Camera, m_StepInterval);
			m_StepCycle = 0f;
			m_NextStep = m_StepCycle / 2f;
			m_Jumping = false;
			m_AudioSource = GetComponent<AudioSource> ();
			m_MouseLook.Init (transform, m_Camera.transform);


			float deltaY = transform.position.y - spawn.transform.position.y;
			float deltaZ = transform.position.z - spawn.transform.position.z;
			float angle = (float)Math.Atan (deltaY / deltaZ);
			startOffsetY = bulletSpeed * (float)Math.Sin (angle);
			startOffsetZ = bulletSpeed * (float)Math.Cos (angle);

			reloadPos = new Quaternion (gun.transform.localRotation.x, gun.transform.localRotation.y, 90f, gun.transform.localRotation.w);
			startPos = gun.transform.localRotation;
		}


		// Update is called once per frame
		private void Update ()
		{
			RotateView ();


			transform.position = new Vector3 (car.transform.position.x, car.transform.position.y + 3, car.transform.position.z);


			if (Input.GetMouseButtonDown (0)) {
				if (currAmmo == 0 || reloading) {
					loadGun ();
				} else if (canShoot) {
					checkShoot ();
				}
			}

		}

		private void checkShoot ()
		{
			if (Time.time < nextFire) {
				return;
			}


			for (int i = 0; i < bulletCount; i++) {
				GetComponent<AudioSource> ().PlayOneShot (shootSound);
				Quaternion bulletRot = transform.rotation;
				bulletRot.x = Random.Range (-bulletSpread, bulletSpread);
				bulletRot.y = Random.Range (-bulletSpread, bulletSpread);
				Rigidbody instantiatedProjectile = Instantiate (projectile, spawn.transform.position, bulletRot) as Rigidbody;

				float deltaX = spawn.transform.position.x - gun.transform.position.x;
				float deltaY = spawn.transform.position.y - gun.transform.position.y;
				float deltaZ = spawn.transform.position.z - gun.transform.position.z;

				print ("x: " + deltaX + "\ny: " + deltaY + "\nz: " + deltaZ);
				print("guny: " + gun.transform.position.y + "\ngunz: " + gun.transform.position.z);
				print("spawny: " + spawn.transform.position.y + "\nspawnz: " + spawn.transform.position.z);

				float tmp = (float) Math.Sqrt (deltaZ * deltaZ + deltaX * deltaX);
				float tmp2 = (float) Math.Sqrt (deltaY * deltaY + tmp * tmp);

				float scale = bulletSpeed / tmp2;

				instantiatedProjectile.GetComponent<Rigidbody>().AddForce(new Vector3 (deltaX+bulletRot.x, deltaY+bulletRot.y, deltaZ)*scale);
			}

			nextFire = Time.time + fireRate;
			//currAmmo--;
		}

		private double RadianToDegree(double angle)
		{
			return angle * (180.0 / Math.PI);
		}

		private void loadGun ()
		{
//			canShoot = false;
//
//			if (!reloading && !gun.transform.localPosition.Equals(reloadPos)) {
//				gun.transform.localRotation = Quaternion.RotateTowards(gun.transform.localRotation, reloadPos, .5f*Time.deltaTime);
//			} else if (waitTime == 0) {
//				waitTime = Time.time + timeToReload;
//				reloading = true;
//			}
//
//			if (reloading && Time.time > waitTime) {
//				if (gun.transform.localRotation.Equals(startPos)) {
//					currAmmo = MAX_AMMO;
//					canShoot = true;
//					reloading = false;
//					waitTime = 0;
//				} else {
//					gun.transform.localRotation = Quaternion.RotateTowards(gun.transform.localRotation, startPos, .5f*Time.deltaTime);
//				}
//			}
		}

		private void checkHit ()
		{

		}


		private void PlayLandingSound ()
		{
			m_AudioSource.clip = m_LandSound;
			m_AudioSource.Play ();
			m_NextStep = m_StepCycle + .5f;
		}


		private void FixedUpdate ()
		{
			float speed;
			GetInput (out speed);

			transform.position = new Vector3 (car.transform.position.x, car.transform.position.y + 3, car.transform.position.z);

			UpdateCameraPosition (speed);

			m_MouseLook.UpdateCursorLock ();
		}


		private void PlayJumpSound ()
		{
			m_AudioSource.clip = m_JumpSound;
			m_AudioSource.Play ();
		}


		private void ProgressStepCycle (float speed)
		{
			if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0)) {
				m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
				Time.fixedDeltaTime;
			}

			if (!(m_StepCycle > m_NextStep)) {
				return;
			}

			m_NextStep = m_StepCycle + m_StepInterval;

			PlayFootStepAudio ();
		}


		private void PlayFootStepAudio ()
		{
			if (!m_CharacterController.isGrounded) {
				return;
			}
			// pick & play a random footstep sound from the array,
			// excluding sound at index 0
			int n = Random.Range (1, m_FootstepSounds.Length);
			m_AudioSource.clip = m_FootstepSounds [n];
			m_AudioSource.PlayOneShot (m_AudioSource.clip);
			// move picked sound to index 0 so it's not picked next time
			m_FootstepSounds [n] = m_FootstepSounds [0];
			m_FootstepSounds [0] = m_AudioSource.clip;
		}


		private void UpdateCameraPosition (float speed)
		{
			Vector3 newCameraPosition;
			if (!m_UseHeadBob) {
				return;
			}
			if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded) {
				m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob (m_CharacterController.velocity.magnitude +
				(speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
				newCameraPosition = m_Camera.transform.localPosition;
				newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset ();
			} else {
				newCameraPosition = m_Camera.transform.localPosition;
				newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset ();
			}
			m_Camera.transform.localPosition = newCameraPosition;
		}


		private void GetInput (out float speed)
		{
			// Read input
			float horizontal = CrossPlatformInputManager.GetAxis ("Horizontal");
			float vertical = CrossPlatformInputManager.GetAxis ("Vertical");

			bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
			// On standalone builds, walk/run speed is modified by a key press.
			// keep track of whether or not the character is walking or running
			m_IsWalking = !Input.GetKey (KeyCode.LeftShift);
#endif
			// set the desired speed to be walking or running
			speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
			m_Input = new Vector2 (horizontal, vertical);

			// normalize input if it exceeds 1 in combined length:
			if (m_Input.sqrMagnitude > 1) {
				m_Input.Normalize ();
			}

			// handle speed change to give an fov kick
			// only if the player is going to a run, is running and the fovkick is to be used
			if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0) {
				StopAllCoroutines ();
				StartCoroutine (!m_IsWalking ? m_FovKick.FOVKickUp () : m_FovKick.FOVKickDown ());
			}
		}


		private void RotateView ()
		{
			m_MouseLook.LookRotation (transform, m_Camera.transform);
		}


		private void OnControllerColliderHit (ControllerColliderHit hit)
		{
			Rigidbody body = hit.collider.attachedRigidbody;
			//dont move the rigidbody if the character is on top of it
			if (m_CollisionFlags == CollisionFlags.Below) {
				return;
			}

			if (body == null || body.isKinematic) {
				return;
			}
			body.AddForceAtPosition (m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
		}
	}
}
