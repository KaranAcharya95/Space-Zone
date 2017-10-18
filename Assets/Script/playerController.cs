using UnityEngine;
using System.Collections;


public class playerController : MonoBehaviour {
	[SerializeField]
	private float speed = 0.05f;
	private Transform _transform;
	private Vector2 _currentPos;
	public GameObject playerBullet;
	public bool playerIsImmortal = false; // Here you can cheat ;)
	public int playerLives = 3; // read by GUI script
	public bool isGameOver = false; // read by GUI script

	// Tuning

	private float pushUpForce = 6.0f; // force applied when fly button is tapped
	private float playerBulletXOffset = 0.5f;
	private float playerBulletYOffset = 0f;
	private float timeBetweenShots = 0.2f;  // 0.2 = 5 shots per second
	private float timestamp;


	[SerializeField]
	private float leftX = 0;
	[SerializeField]
	private float rightX = 0;
	[SerializeField]
	private float leftY = 0;
	[SerializeField]
	private float rightY = 0;

	void Start(){
		_transform = gameObject.GetComponent<Transform>();
		_currentPos = _transform.position;
	}

	void FixedUpdate () {

		if (Input.GetKeyDown("space"))
		{
			Fire2Pressed ();
		}
	}

	void Fire1Pressed () {
		//Fly up
		GetComponent<Rigidbody2D>().AddForce(new Vector2(0, pushUpForce));

	}

	void Fire2Pressed () {

		if (Time.time >= timestamp) {
			// Shoot bullet
			Instantiate(playerBullet, transform.position+new Vector3 (playerBulletXOffset,playerBulletYOffset,0), Quaternion.identity);
			timestamp = Time.time + timeBetweenShots;
		}
	}


	// NB. Player and its bullet go in two dedicated layers where collisions must be disabled (Edit->project settings->physics 2D)
	// Collision with Trigger is for the bullets and enemies
	void OnTriggerEnter2D(Collider2D thisObject)
	{
		playerDidCollide ();
	}
		

	void playerDidCollide () {

		if (playerLives > 0 && !playerIsImmortal) {
			playerLives=playerLives-1; // lose 1 life
			//Debug.Log("Collision!"+" "+playerLives+" Lives");

		} else if (playerLives <= 0 && !playerIsImmortal) { // no lives remaining
			
			gameOver();
		}
	}


	void gameOver () {

		//Debug.Log("GAME OVER");
		isGameOver = true; // This is picked by the Update function in GUI.cs
		Time.timeScale = 0.0F; // Stop the game

	}

	// NB. Update is not affected by Time.timeScale (i.e. it also works during Game Over)
	void Update () {

		// NB Input.GetMouseButtonDown(0) not only triggers on the left mouse click, but also iOS & Android touch events 
		if (isGameOver && Input.GetButtonDown("Fire1") || isGameOver && Input.GetMouseButtonDown(0)) {
			//Debug.Log("GAME RESTART");
			Time.timeScale = 1.0F; // Restart the time
			//Application.LoadLevel(Application.loadedLevel); // Reload this level
		}

		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		if (moveHorizontal < 0)
		{
			//if left arrow is pressed move left = A
			_currentPos -= new Vector2(speed, 0);
		}

		if (moveHorizontal > 0)
		{
			//if right arrow is pressed move right = D
			_currentPos += new Vector2(speed, 0);
		}

		if (moveVertical < 0)
		{
			//if up arrow is pressed move up = W
			_currentPos -= new Vector2(0, speed);
		}

		if (moveVertical > 0)
		{
			//if down arrow is pressed move down = S
			_currentPos += new Vector2(0, speed);
		}
		CheckBounds ();
		_transform.position = _currentPos;
	}

	//Validating boundaries for player. Player muyst stay within the game boundaries
	private void CheckBounds()
	{
		//Checks X axis
		if (_currentPos.x < leftX)
		{
			_currentPos.x = leftX;
		}

		if (_currentPos.x > rightX)
		{
			_currentPos.x = rightX;
		}

		//Checks Y axis
		if (_currentPos.y < leftY)
		{
			_currentPos.y = leftY;
		}

		if (_currentPos.y > rightY)
		{
			_currentPos.y = rightY;
		}
	}

}