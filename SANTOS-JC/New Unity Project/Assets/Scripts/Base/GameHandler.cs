using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;

public class GameHandler : MonoBehaviour
{


    public EventHandler<SwipeEventArgs> OnSwipe;
    public EventHandler<SpreadEventArgs> OnSpread;
    public EventHandler<RotateEventArgs> OnRotate;

    public SwipeProperty _swipeProperty;
    public SpreadProperty _spreadProperty;
    public RotateProperty _rotateProperty;
    private Touch trackedFinger1;
    private Touch trackedFinger2;
    private Vector2 startPoint = Vector2.zero;
    private Vector2 endPoint = Vector2.zero;
    private float gestureTime1 = 0;

    private bool firstTime = false;
    private float bufferTimer = 0;


    public TimerUI timerUi;


    Vector3 acceleration;
    [SerializeField] GameObject MainMenu;
  
    [Header("Game Vars")]
    public int CurrentLife = 3;
    public int MaxLife = 3;
    public int CurrentScore = 0;
    public LifeUI lifeui;

    /// <summary>
    /// Target Sequence of notes to be done by the player
    /// </summary>
    [Header("Sequences")]
    public Notes[] TargetSequence;
    /// <summary>
    /// Current Sequence of notes done by the player
    /// </summary>
    public List<Notes> HistorySequence = new List<Notes>();

    /// <summary>
    /// Current Time in the sequence
    /// </summary>
    [Header("Timers")]
    public float CurrentTime = 0;
    /// <summary>
    /// Time Limit to input the sequence
    /// </summary>
    public float MaxTime = 10;

    /// <summary>
    /// Note prefab to spawn in either the target sequence or the history sequence
    /// </summary>
    [Header("Notes")]
    public GameObject NotePrefab;
    /// <summary>
    /// Transform to hold the Target Sequence notes
    /// </summary>
    public Transform Sequence_Holder;
    /// <summary>
    /// Transform to hold the history of notes done by the player
    /// </summary>
    public Transform Sequence_History;

    /// <summary>
    /// Game character- reacts if you pressed the correct note in the sequence or not
    /// </summary>
    public Animator Noel;

    /// <summary>
    /// Current prefabs in the Sequence holder
    /// </summary>
    private List<GameObject> currentNotes_Holder = new List<GameObject>();

    /// <summary>
    /// Current prefabs in the History holder
    /// </summary>
    private List<GameObject> currentNotes_History = new List<GameObject>();


    [HideInInspector] public int incrementLimit = 1;


   


    private void Start()
    {
        
        MainMenu.SetActive(false);
        GetRandomSequence(1);
    }

    private void Update()
    {
      
        acceleration = Input.acceleration;

        if (acceleration.sqrMagnitude >= 2.0f && firstTime == false)
        {
            firstTime = true;
            ClearHistoryNotes();
        }

        else if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1)
            {
                //Move the previous code for single finger checks here
                CheckSingleFingerGestures();
            }
            else if (Input.touchCount > 1 )
            {
                //Assign the finger being tracked
                trackedFinger1 = Input.GetTouch(0);
                trackedFinger2 = Input.GetTouch(1);
                if (trackedFinger1.phase == TouchPhase.Moved || trackedFinger2.phase == TouchPhase.Moved) 
                {
                    //Get the positions of the finger in the previous frame
                    Vector2 prevPoint1 = GetPreviousPOint(trackedFinger1);
                    Vector2 prevPoint2 = GetPreviousPOint(trackedFinger2);

                    //Get the vectors of the fingers and their respective prev. position
                    Vector2 diffVector = trackedFinger1.position - trackedFinger2.position;
                    Vector2 prevDiffVector = prevPoint1 - prevPoint2;
                    //Get the distances
                    float currDistance = Vector2.Distance(trackedFinger1.position, trackedFinger2.position);
                    float prevDistance = Vector2.Distance(prevPoint1, prevPoint2);

                    float angle = Vector2.Angle(prevDiffVector, diffVector);

                    if (Vector2.Distance(trackedFinger1.position, trackedFinger2.position) >= (Screen.dpi * _rotateProperty.MinDistance) && (angle >= _rotateProperty.minAngleChange))
                    {
                        Vector3 cross = Vector3.Cross(prevDiffVector, diffVector);
                        FireRotateEvent(angle, cross);
                    }
                    
                        //Get the positions of the finger in the previous frame
                       
                    else if (Mathf.Abs(currDistance - prevDistance) >= (_spreadProperty.MinDistanceChange * Screen.dpi))
                    {
                        FingerSpreadEvent(currDistance - prevDistance);
                    }
                            
                       
                        

                    

                    
                }
               

              
                


            }
        }
    }

    private void FixedUpdate()
    {
        if (CurrentLife > 0)
        {
            //Count down
            CurrentTime += Time.fixedDeltaTime;
            if (CurrentTime >= MaxTime)
            {
                HurtNoel();
                CurrentTime = 0;
                CurrentLife -= 1;
                GetRandomSequence(incrementLimit);

            }
        }
    }

    /// <summary>
    /// Generates a new sequence for the player to follow
    /// Also clears the current notes in the history
    /// </summary>
    /// <param name="limit">Max number of notes to generate</param>
    public void GetRandomSequence(int limit = 7)
    {
        limit = Mathf.Max(limit, 1);
        CurrentTime = 0;

        ClearHistoryNotes();

        TargetSequence = new Notes[limit];

        for (int i = 0; i < limit; i++)
        {
            TargetSequence[i] = (Notes)Random.Range(0, 8);
        }

        SpawnTargetSequence();
    }

    /// <summary>
    /// Spawns a note in the history holder and adds them to the history sequence
    /// Also checks if the last note spawned matches the position in the sequence
    /// </summary>
    /// <param name="note">Note to spawn</param>
    public void AddHistoryNote(Notes note)
    {
        HistorySequence.Add(note);

        GameObject spawn = NoteScript.SpawnNote(NotePrefab, Sequence_History, note);
        currentNotes_History.Add(spawn);

        CheckLastNoteMatch();
    }

    /// <summary>
    /// Clears all the notes in the history as well as in the array
    /// </summary>
    public void ClearHistoryNotes()
    {
        HistorySequence.Clear();
        ClearNotes(currentNotes_History);
    }

    /// <summary>
    /// Checks if the last note matches each other
    /// Mainly for animation purposes
    /// </summary>
    public void CheckLastNoteMatch()
    {
        if (TargetSequence.Length >= HistorySequence.Count)
        {
            if (TargetSequence[HistorySequence.Count - 1] == HistorySequence[HistorySequence.Count - 1])
            {
                Noel.SetTrigger("Attack");
            }
          
            else
            {

                Noel.SetTrigger("Hurt");
            }
        }
    }

    /// <summary>
    /// Plays the dead animation for the character
    /// </summary>
    public void KillNoel()
    {
        Noel.SetTrigger("Dead");
       
    }
    public void HurtNoel()
    {
        Noel.SetTrigger("Hurt");
    }

    /// <summary>
    /// Plays the revive animation for the character
    /// Only usable when dead was played earlier
    /// </summary>
    public void ReviveNoel()
    {
        Noel.SetTrigger("Revive");
    }

    /// <summary>
    /// Spawns the prefabs in the target sequence
    /// </summary>
    private void SpawnTargetSequence()
    {
        ClearNotes(currentNotes_Holder);
        for (int i = 0; i < TargetSequence.Length; i++)
        {
            SpawnTargetNote(TargetSequence[i]);
        }
    }

    private void SpawnTargetNote(Notes note)
    {
        GameObject spawn = NoteScript.SpawnNote(NotePrefab, Sequence_Holder, note);
        currentNotes_Holder.Add(spawn);
    }

    public void ClearNotes(List<GameObject> note_holder)
    {
        for (int i = 0; i < note_holder.Count; i++)
        {
            Destroy(note_holder[i]);
        }

        note_holder.Clear();
    }

  

    public void ExitGame()
    {
        Debug.Log("Exit...");
        Application.Quit();
    }
    public void ReturntoMain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ReloadScene()
    {
        float bufferTimeSpawn = 0;
        ReviveNoel();
        bufferTimeSpawn = +Time.deltaTime;
        if(bufferTimeSpawn == 0.3f)
        {
            for (int i = 0; i < currentNotes_Holder.Count; i++)
            {
                currentNotes_Holder[i].SetActive(true);
            }
            for (int i = 0; i < currentNotes_History.Count; i++)
            {
                currentNotes_History[i].SetActive(true);
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
       
    }

    public void OnTapGame()
    {
        firstTime = false;
        if (TargetSequence.Length >= HistorySequence.Count)
        {
            if (TargetSequence[HistorySequence.Count - 1] == HistorySequence[HistorySequence.Count - 1])
            {
                if (incrementLimit < 7)
                {
                    incrementLimit += 1;

                }
                GetRandomSequence(incrementLimit);
                CurrentScore += 1;
            }
            else
            {
               CurrentLife -= 1;
                GetRandomSequence(incrementLimit);
            }
        }
        else
        {
            CurrentLife -= 1;
            GetRandomSequence(incrementLimit);
        }
        if (CurrentLife == 0)
        {
            KillNoel();
            MainMenu.SetActive(true);
            for (int i = 0; i < currentNotes_Holder.Count; i++)
            {
                currentNotes_Holder[i].SetActive(false);
            }
            for (int i = 0; i < currentNotes_History.Count; i++)
            {
                currentNotes_History[i].SetActive(false);
            }


        }

       
    }

    private void FireRotateEvent(float angle, Vector3 cross)
    {
        bool one = false;
       
        if (HistorySequence.Count < TargetSequence.Length && one == false)
        {
            one = true;
            if (cross.z > 0)
            {
                Debug.Log($"Rotate CCW {angle}");
                AddHistoryNote(Notes.ROT_CCW);
               
            }
            else
            {
                Debug.Log($"Rotate CW {angle}");
               AddHistoryNote(Notes.ROT_CW);
                
            }
        }
    }

    private void FingerSpreadEvent(float dist)
    {
        bool one = false;
        if (HistorySequence.Count < TargetSequence.Length && one == false)
        {
            one = true;
            //Pinch if otherwise
            if (dist > 0)
            {
                Debug.Log("spread");

                AddHistoryNote(Notes.SPREAD);
                
            }
            else
            {
                Debug.Log("pinch");
                AddHistoryNote(Notes.PINCH);
                
            }
        }
    }

    private Vector2 GetPreviousPOint(Touch finger)
    {
        //Subtract with delta to get the prev. position
        return finger.position - finger.deltaPosition;
    }

    private void CheckSingleFingerGestures()
    {
        trackedFinger1 = Input.GetTouch(0);

        if (trackedFinger1.phase == TouchPhase.Began)
        {
            gestureTime1 = 0;
            startPoint = trackedFinger1.position;
        }
        else if (trackedFinger1.phase == TouchPhase.Ended)
        {
            endPoint = trackedFinger1.position;

            // Swipe Events
            if (gestureTime1 <= _swipeProperty.swipeTime &&
                Vector2.Distance(startPoint, endPoint) >= (Screen.dpi * _swipeProperty.minSwipeDistance))
            {
                SwipeEvent();
            }
        }

    }
    //swipe event
    private void SwipeEvent()
    {
        Debug.Log("Swiped");
        Vector2 dir = endPoint - startPoint;

        Ray r = Camera.main.ScreenPointToRay(startPoint);
        RaycastHit hit = new RaycastHit();
        GameObject hitObj = null;

        if (Physics.Raycast(r, out hit, Mathf.Infinity))
        {
            hitObj = hit.collider.gameObject;
        }

        if (HistorySequence.Count < TargetSequence.Length)
        {

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) // Horizontal Swipes
            {
                if (dir.x > 0)
                {
                    Debug.Log("Right");
                    AddHistoryNote(Notes.SWIPE_RIGHT);
                }
                else
                {
                    Debug.Log("Left");
                    AddHistoryNote(Notes.SWIPE_LEFT);
                }
            }
            else // Vertical Swipes
            {
                if (dir.y > 0)
                {
                    Debug.Log("Up");
                    AddHistoryNote(Notes.SWIPE_UP);
                }
                else
                {
                    Debug.Log("Down");
                    AddHistoryNote(Notes.SWIPE_DOWN);
                }
            }
        }
    }
}
