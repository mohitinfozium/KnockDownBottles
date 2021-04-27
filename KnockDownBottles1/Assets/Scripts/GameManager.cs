using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isGamePaused = false;

    public Button NextLevelButton;


    public static GameManager instance;


    public List<GameObject> Prefab;

    public int LevelCount = 0;

    int BirdsThrown=0;

    public GameObject pauseMenu;
    public GameObject pauseButton;
    public GameObject GameOverWon1, GameOverWon2, GameOverWon3;
    public GameObject GameOverLost;
    //public CameraFollow cameraFollow;
    int currentBirdIndex;
    public SlingShot slingshot;
    StartMenu WonLostBackBtn = new StartMenu();

    //[HideInInspector]
    public GameState CurrentGameState = GameState.Start;
    public List<GameObject> Bricks;
    public List<GameObject> Birds;
    public List<GameObject> Pigs;

    // functions defined

    public void WonLostBackButtton()
    {
        SceneManager.LoadScene(0);

        WonLostBackBtn.SandLevelsPanel.SetActive(true);
    }
    public void Wait()
    {
        if (CurrentGameState == GameState.Won)
        {
 
            if(BirdsThrown == Birds.Count)
            {
                GameOverWon1.SetActive(true);
                GameOverLost.SetActive(false);
            }
            if (BirdsThrown < Birds.Count)
            {
                GameOverWon2.SetActive(true);
                GameOverLost.SetActive(false);
            }
            if(BirdsThrown < Birds.Count - 1)
            {
                GameOverWon3.SetActive(true);
                GameOverWon2.SetActive(false);
                GameOverLost.SetActive(false);
            }
          
        }
        else
        {
            GameOverLost.SetActive(true);
            GameOverWon1.SetActive(false);
            GameOverWon2.SetActive(false);
            GameOverWon3.SetActive(false);
        }
        Debug.Log("Waiting for 2 seconds");
        Time.timeScale = 0;
    }

    public void LoadNextLevel()
    {
        //Destroy(Prefab[LevelCount]);
        LevelCount++;
        PlayerPrefs.SetInt("Levels", LevelCount);
        Debug.Log("current nextlevel: " + PlayerPrefs.GetInt("Levels"));
        SceneManager.LoadScene(1);

    }
    public void Restart()
    {
        //Destroy(Prefab[LevelCount]);
        //PlayerPrefs.SetInt("Levels", LevelCount);
        Debug.Log("current restart level: " + PlayerPrefs.GetInt("Levels"));
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isGamePaused = false;
    }

    public void PauseGame()
    {
        GameOverLost.SetActive(false);
        GameOverWon1.SetActive(false);
        GameOverWon2.SetActive(false);
        GameOverWon3.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isGamePaused = true;
    }


    // Use this for initialization
    void Start()
    {
        pauseButton.SetActive(true);


    Time.timeScale = 1;
        if (PlayerPrefs.HasKey("Levels"))
        {
            LevelCount = PlayerPrefs.GetInt("Levels");
            Debug.Log("The key " + LevelCount + " exists");

        }
        else
        {
            PlayerPrefs.SetInt("Levels", 0);
            LevelCount = PlayerPrefs.GetInt("Levels");

            Debug.Log("The key" + LevelCount + "does not exist");
        }

        Debug.Log("Before Instantiate" + LevelCount);
        Debug.Log("Check LevelPrefab = "+ Prefab.ToString());
        Instantiate(Prefab[LevelCount],new Vector3(0,0,0),Quaternion.identity);
        Debug.Log("After Instantiate" + LevelCount);
    
        //levelManager.Levelcount();
        //find all relevant game objects
        Bricks = new List<GameObject>(GameObject.FindGameObjectsWithTag("Brick"));
        Birds = new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird"));
        Pigs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Pig"));
        slingshot = GameObject.FindGameObjectWithTag("Slingshot").GetComponent<SlingShot>();
        //unsubscribe and resubscribe from the event
        //this ensures that we subscribe only once
        //slingshot.BirdThrown -= Slingshot_BirdThrown; slingshot.BirdThrown += Slingshot_BirdThrown;

        //GameObject GameOverParent = GameObject.Find("Canvas");
        //GameOverPanel1 = GameObject.Find("GameOverPanel1");
        //GameOverPanel2 = GameObject.Find("GameOverPanel2");
        //GameOverPanel1.SetActive(false);
        //GameOverPanel2.SetActive(false);


        CurrentGameState = GameState.Start;
        slingshot.enabled = false;
        pauseMenu.SetActive(false);

        Debug.Log("Start Code Ended");

    }


    // Update is called once per frame
    void Update()
    {
        //panel giving fouroptions in all four corners
        //OptionsPanel = GameObject.Find("Optionspanel");
        //OptionsPanel.SetActive(true);

        //Debug.Log("State of game= "+CurrentGameState);
        switch (CurrentGameState)
        {
            case GameState.Start:
                //if player taps, begin animating the bird 
                //to the slingshot
                Debug.Log("Start state of Switch");
                if (Input.GetMouseButtonUp(0))
                {
                    AnimateBirdToSlingshot();
                    if (AllPigsDestroyed())
                    {
                   
                        CurrentGameState = GameState.Won;
                       
                        break;
                    }
                }
                break;
            case GameState.BirdMovingToSlingshot:
                Debug.Log("BirdMovingToSlingShot state of Switch");
                if (currentBirdIndex > Birds.Count-1)
                {
                    if (AllPigsDestroyed())
                    {
                      
                        //All bottles destroyed
                        CurrentGameState = GameState.Won;
                    
                        break;
                    }
                    else
                    {
                        CurrentGameState = GameState.Lost;
                        break;
                    }
                }
                    //do nothing
                    break;
            case GameState.Playing:
                Debug.Log("Playing state of Switch");
                //if we have thrown a bird
                //and either everything has stopped moving
                //or there has been 5 seconds since we threw the bird
                //animate the camera to the start position
                if (AllPigsDestroyed())
                {
                    //All bottles destroyed
                
                    CurrentGameState = GameState.Won;
                   
                    break;

                }
               
                else if (slingshot.slingshotState == SlingshotState.BirdFlying &&
                    (BricksBirdsPigsStoppedMoving() || Time.time - slingshot.TimeSinceThrown > 3f))
                {
                    slingshot.enabled = false;
                    CurrentGameState = GameState.BirdMovingToSlingshot;
                    AnimateCameraToStartPosition();
             
                }

                break;
            //if we have won or lost, we will restart the level
            //in a normal game, we would show the "Won" screen 
            //and on tap the user would go to the next level
            case GameState.Won:
                if (AllPigsDestroyed())
                {
                
                    CurrentGameState = GameState.Won;
                    
                    break;
                }
                break;
            case GameState.Lost:
                if (AllPigsDestroyed())
                {
                  
                    CurrentGameState = GameState.Won;
                    
                    break;
                }
                //SceneManager.LoadScene(0);
                break;
            default:
                break;
        }

        Debug.Log("Hello you are in update code");
    }

    //Wait to Display GameOverPanels
    

    /// <summary>
    /// A check whether all Pigs are null
    /// i.e. they have been destroyed
    /// </summary>
    /// <returns></returns>
    private bool AllPigsDestroyed()
    {
      
        return Pigs.All(x => x == null);
    }

    /// <summary>
    /// Animates the camera to the original location
    /// When it finishes, it checks if we have lost, won or we have other birds
    /// available to throw
    /// </summary>
    private void AnimateCameraToStartPosition()
    {
        //float duration = Vector2.Distance(Camera.main.transform.position, cameraFollow.StartingPosition) / 10f;
        //if (duration == 0.0f) duration = 0.1f;
        //animate the camera to start
        //Camera.main.transform.positionTo
        //    (duration,
        //    cameraFollow.StartingPosition). //end position
        //    setOnCompleteHandler((x) =>
        //                {
        //                    cameraFollow.IsFollowing = false;

        //                });
        BirdsThrown++;

        if (AllPigsDestroyed())
        {
            for(int i = 0; i < Birds.Count; i++)
            {
                Birds.Remove(Birds[i]);
            }
            //All bottles destroyed
         
            CurrentGameState = GameState.Won;
          

            //animate the next bird, if available
            if (currentBirdIndex == Birds.Count - 1)
            {
           
                Debug.Log(Birds.Count);
                //no more birds, go to finished
                CurrentGameState = GameState.Lost;
             
                if (AllPigsDestroyed())
                {
                    BirdsThrown = Birds.Count;

                    //All bottles destroyed
                    CurrentGameState = GameState.Won;
                  
                }

            }

        }

        else
        {
            slingshot.slingshotState = SlingshotState.Idle;
            //bird to throw is the next on the list
            currentBirdIndex++;
            if (CurrentGameState != GameState.Won && CurrentGameState != GameState.Lost)
            {
                AnimateBirdToSlingshot();
            }

        }
    }

  
    /// <summary>
    /// Animates the bird from the waiting position to the slingshot
    /// </summary>
    void AnimateBirdToSlingshot()
    {
        //CurrentGameState = GameState.BirdMovingToSlingshot;
        Birds[currentBirdIndex].transform.positionTo
            (Vector2.Distance(Birds[currentBirdIndex].transform.position/20,
            slingshot.BirdWaitPosition.transform.position)/20, //duration
            slingshot.BirdWaitPosition.transform.position). //final position
                setOnCompleteHandler((x) =>
                        {
                            x.complete();
                            x.destroy(); //destroy the animation
                            CurrentGameState = GameState.Playing;
                            slingshot.enabled = true; //enable slingshot
                            //current bird is the current in the list
                            slingshot.BirdToThrow = Birds[currentBirdIndex];
                            
                        });
       
        Debug.Log("taking bird to slingshot");
    }

    /// <summary>
    /// Event handler, when the bird is thrown, camera starts following it
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Slingshot_BirdThrown(object sender, System.EventArgs e)
    {
        //cameraFollow.BirdToFollow = Birds[currentBirdIndex].transform;
        //cameraFollow.IsFollowing = true;
    }

    /// <summary>
    /// Check if all birds, pigs and bricks have stopped moving
    /// </summary>
    /// <returns></returns>
    bool BricksBirdsPigsStoppedMoving()
    {
        foreach (var item in Bricks.Union(Birds).Union(Pigs))
        {
            if (item != null && item.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > Constants.MinVelocity)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Found here
    /// http://www.bensilvis.com/?p=500
    /// </summary>
    /// <param name="screenWidth"></param>
    /// <param name="screenHeight"></param>
    public static void AutoResize(int screenWidth, int screenHeight)
    {
        Vector2 resizeRatio = new Vector2((float)Screen.width / screenWidth, (float)Screen.height / screenHeight);
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x, resizeRatio.y, 1.0f));
    }

    /// <summary>
    /// Shows relevant GUI depending on the current game state
    /// </summary>
    public void OnGUI()
    {
        GUI.contentColor = Color.black;
        AutoResize(800, 480);
        switch (CurrentGameState)
        {
            case GameState.Start:
                //GUI.Label(new Rect(0, 150, 200, 100), "Tap the screen to start");
                break;
            case GameState.Won:
                Invoke(nameof(Wait), 2.5f);
                Debug.Log("On GUI won");

               // GUI.Label(new Rect(0, 150, 200, 100), "You won! Tap the screen to restart");
                break;
            case GameState.Lost:
                Invoke("Wait", 2.5f);
                Debug.Log("On GUI Lost");

                //GUI.Label(new Rect(0, 150, 200, 100), "You lost! Tap the screen to restart");
                break;
            default:
                break;
        }
    }

}
