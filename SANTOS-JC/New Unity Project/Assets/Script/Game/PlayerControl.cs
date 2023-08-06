using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class PlayerControl : MonoBehaviour
{
	public GameObject GameManagerGO;
	public GameObject PlayerBulletGo;
	public GameObject bulletPosition1;
	public GameObject bulletPosition2;
	public GameObject ExplosionGO;

	public float speed = 7.0f;

	public Text GoldUIText;
	public Text LivesUIText;
	public int MaxLives = 3;
	int lives;

	float accelStartY;

	private Vector2 startPoint;
	private Vector2 endPoint;
	private Touch trackedFinger1;
	private Touch trackedFinger2;
	private float gestureTime;
	public SpreadProperty _spreadProperty;
	public SwipeProperty _swipeProperty;
	public void IncreaseHp()
    {
		MaxLives++;
    }
	public void Init()
    {
		lives = MaxLives;
		LivesUIText.text = lives.ToString();

		transform.position = new Vector2(0, 0);
		gameObject.SetActive(true);
    }
    // Use this for initialization
    void Start()
	{
		accelStartY = Input.acceleration.y;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.touchCount > 0)
		{
			
			if (Input.touchCount == 1)
			{
				trackedFinger1 = Input.GetTouch(0);
				if (trackedFinger1.phase == TouchPhase.Began)
				{
					gestureTime = 0;
					startPoint = trackedFinger1.position;
				}
				else if (trackedFinger1.phase == TouchPhase.Ended)
				{
					endPoint = trackedFinger1.position;
					if (gestureTime <= _swipeProperty.swipeTime && Vector2.Distance(startPoint, endPoint) >= (Screen.dpi * _swipeProperty.minSwipeDistance))
					{
						ShootAction();
					}

				}
				else
				{
					gestureTime += Time.deltaTime;

				}
			}
            else
            {
				trackedFinger1 = Input.GetTouch(0);
				trackedFinger2 = Input.GetTouch(1);
				if (trackedFinger1.phase == TouchPhase.Moved || trackedFinger2.phase == TouchPhase.Moved)
			   {
				   Vector2 prevPoint1 = GetPrevPoint(trackedFinger1);
				   Vector2 prevPoint2 = GetPrevPoint(trackedFinger2);

				   float prevDistance = Vector2.Distance(prevPoint1, prevPoint2);
				   float currDistance = Vector2.Distance(trackedFinger1.position, trackedFinger2.position);

				   if(Mathf.Abs(currDistance - prevDistance) >=  (_spreadProperty.MinDistanceChange * Screen.dpi))
				   {
						ShootAction();
					}
			   }
			}

		}
		float x = Input.acceleration.x;
		float y = Input.acceleration.y - accelStartY;

		//now based on the input we compute a direction vector, and we normalize it to get a unit vector
		Vector2 direction = new Vector2(x, y);

		if(direction.sqrMagnitude > 1)
        {
			direction.Normalize();
        }

		//noe we call the function that computes and sets the player's position
		Move(direction);
	}
	private Vector2 GetPrevPoint(Touch finger)
	{
		return finger.position - finger.deltaPosition;
	}

	void Move(Vector2 direction)
	{
		//find the screen limits to the player's movement (left, right, top and bottom edges of the screen)
		Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)); //this is the bottom-left point (corner) of the screen
		Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)); //this is the top-right point (corner) of the screen

		max.x = max.x - 0.225f; //subtract the player sprite half width
		min.x = min.x + 0.225f; //add the player sprite half width

		max.y = max.y - 0.285f; //subtract the player sprite half height
		min.y = min.y + 0.285f; //add the player sprite half height

		//Get the player's current position
		Vector2 pos = transform.position;

		//Calculate the new position
		pos += direction * speed * Time.deltaTime;

		//Make sure the new position is outside the screen
		pos.x = Mathf.Clamp(pos.x, min.x, max.x);
		pos.y = Mathf.Clamp(pos.y, min.y, max.y);

		//Update the player's position
		transform.position = pos;
	}

	void OnTriggerEnter2D(Collider2D col)
    {
		if(col.tag == "EnemyShipTag" || col.tag == "EnemyBulletTag")
        {
			PlayExplosion();
			lives--;
			LivesUIText.text = lives.ToString();

			if(lives == 0)
            {
				
				GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);
				gameObject.SetActive(false);
			}
			
        }
    }

	void PlayExplosion()
    {
		GameObject explosion = (GameObject)Instantiate(ExplosionGO);

		explosion.transform.position = transform.position;
    }

	public void ShootAction()
    {
		
			GetComponent<AudioSource>().Play();
			GameObject bullet01 = (GameObject)Instantiate(PlayerBulletGo);
			bullet01.transform.position = bulletPosition1.transform.position;

			GameObject bullet02 = (GameObject)Instantiate(PlayerBulletGo);
			bullet02.transform.position = bulletPosition2.transform.position;
		
	}
}