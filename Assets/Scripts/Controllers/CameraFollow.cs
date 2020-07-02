using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public float xSmooth = 8f; 
	public Vector2 maxXAndY; 
	public Vector2 minXAndY; 

	private Transform Player;
	private GameController Gamecontroller;

	private void Awake()
	{
		Gamecontroller = FindObjectOfType<GameController>() as GameController;
		Player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	private void Update()
	{
		if(Gamecontroller.Started)
		{
			TrackPlayer();
		}
		else
			UnTrackPlayer();
	}		
	private void TrackPlayer()
	{
		float targetX;
		float targetY = transform.position.y;

		targetX = Mathf.Lerp(transform.position.x, Player.position.x, xSmooth * Time.deltaTime);
		
		targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
		
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}
	private void UnTrackPlayer()
	{
		transform.position = transform.position;
	}
}

