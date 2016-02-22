using UnityEngine;
using System.Collections;

public class moveonPath : MonoBehaviour {

	public birdPath PathToFollow;

	public int CurrentWayPointID = 0;
	public float speed; //speed to follow the path
	private float reachDistance = 1.0f;
	public float rotationSpeed = 5.0f; //Speed we are rotating around the curve
	public string pathName;
	
	Vector3 last_position; //where we have been
	Vector3 current_position; //where we want to go

	void Start () 
	{

			//PathToFollow = GameObject.Find(pathName).GetComponent<birdPath> ();
			last_position = transform.position;
	}
	
	
	void Update () 
	{
		float distance = Vector3.Distance(PathToFollow.path_objs[CurrentWayPointID].position, transform.position);
		transform.position = Vector3.MoveTowards(transform.position, PathToFollow.path_objs[CurrentWayPointID].position, Time.deltaTime * speed);

		if(distance <= reachDistance)
		{
			CurrentWayPointID++;
		}

		if(CurrentWayPointID>= PathToFollow.path_objs.Count)
		{
			CurrentWayPointID = 0;
		}
	}
}
