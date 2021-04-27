using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;
using System;

public class SlingShot : MonoBehaviour
{

    //a vector that points in the middle between left and right parts of the slingshot
    private Vector3 SlingshotMiddleVector;
    public GameObject holder;

    [HideInInspector]
    public SlingshotState slingshotState;

    //the left and right parts of the slingshot
    public Transform LeftSlingshotOrigin, RightSlingshotOrigin;

    //two line renderers to simulate the "strings" of the slingshot
    public LineRenderer SlingshotLineRenderer1;
    public LineRenderer SlingshotLineRenderer2;
    
    //this linerenderer will draw the projected trajectory of the thrown bird
    public LineRenderer TrajectoryLineRenderer;
    
    [HideInInspector]
    //the bird to throw
    public GameObject BirdToThrow;

    //the position of the bird tied to the slingshot
    public Transform BirdWaitPosition;

    public float ThrowSpeed;
    public float force;
    [HideInInspector]
    public float TimeSinceThrown;
    //public GameObject trajectoryDot;
    //public GameObject[] trajectoryDots;
    public int dotsNumber;
    public GameObject dotPrefab;
    public GameObject dotsParent;
    public float dotSpacing;

    Transform[] dotsList;
    Vector3 location;
    float timeStamp;
    Vector3 maxPosition;
    Vector2 Pos;
    Vector2 direction;
    [SerializeField] [Range(0.5f, 1.5f)] float dotMinScale;
    [SerializeField] [Range(1.5f, 2f)] float dotMaxScale;

    // Use this for initialization
    void Start()
    {


        //Debug.Log(LevelCount);
        //DontDestroyOnLoad(LevelPassed);
        //PrepareDots();
        //trajectoryDots = new GameObject[number];

        //set the sorting layer name for the line renderers
        //for the slingshot renderers this did not work so I
        //set the z on the background sprites to 10
        //hope there's a better way around that!
        SlingshotLineRenderer1.sortingLayerName = "Foreground";
        SlingshotLineRenderer2.sortingLayerName = "Foreground";
        TrajectoryLineRenderer.sortingLayerName = "Foreground";

        slingshotState = SlingshotState.Idle;
        SlingshotLineRenderer1.SetPosition(0, LeftSlingshotOrigin.position);
        SlingshotLineRenderer2.SetPosition(0, RightSlingshotOrigin.position);

        //pointing at the middle position of the two vectors
        SlingshotMiddleVector = new Vector3((LeftSlingshotOrigin.position.x + RightSlingshotOrigin.position.x) / 2,
            (LeftSlingshotOrigin.position.y + RightSlingshotOrigin.position.y) / 2, 0);
    }

    //Dots of trajectory
   public void PrepareDots()
    {
  
        if (dotsParent.transform.childCount != 12)
        {
            dotsList = new Transform[dotsNumber];
            dotPrefab.transform.localScale = Vector3.one * dotMaxScale;

            float scale = dotMaxScale;
            float scaleFactor = scale / dotsNumber;

            for (int i = 0; i < dotsNumber; i++)
            {
                dotsList[i] = Instantiate(dotPrefab, null).transform;
                dotsList[i].parent = dotsParent.transform;
                dotsList[i].localScale = Vector3.one * scale;
                if (scale > dotMinScale)
                        scale -= scaleFactor;
            }

        }
    }

    //    void UpdateDots(float distance)
    //{
    //    SetTrajectoryLineRenderesActive(true);
    //    timeStamp =dotSpacing;
    //    Vector3 v2 = SlingshotMiddleVector - BirdToThrow.transform.position;
    //    Vector2 segVelocity = new Vector2(v2.x, v2.y) * ThrowSpeed * distance;
    //    dotList[0].position = BirdToThrow.transform.position;
    //    for (int i = 0; i < dotsnumber; i++)
    //    {
          
    //       // Vector2 currentposition = (Vector2)transform.position + direction.normalized * force * timeStamp + 0.5f * Physics2D.gravity * (timeStamp* timeStamp);
    //        dotList[i].position= (Vector2) BirdToThrow.transform.position + (Vector2)segVelocity * timeStamp + 0.5f * Physics2D.gravity * Mathf.Pow(timeStamp, 2);
          
    //        timeStamp += dotSpacing;
    //    }
    //    //TrajectoryLineRenderer.SetVertexCount(dotsnumber);
    //    //for (int i = 0; i < dotsnumber; i++)
    //    //{
    //    //    TrajectoryLineRenderer.SetPosition(i, dotList[i].position);
    //    //    //  Debug.Log(segments[i]);
    //    //}


    //}
    // Update is called once per frame
    void Update()
    {
        switch (slingshotState)
        {
            case SlingshotState.Idle:
                //fix bird's position
                InitializeBird();
                
                //display the slingshot "strings"
                DisplaySlingshotLineRenderers();
                if (Input.GetMouseButtonDown(0))
                {
                    //get the point on screen user has tapped
                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //if user has tapped onto the bird
                    if (BirdToThrow.GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(location))
                    {
                        slingshotState = SlingshotState.UserPulling;
                    }
                }
                //for(int i=0;i<number;i++)
                //{
                //    trajectoryDots[i] = Instantiate(trajectoryDot, gameObject.transform);
                //}
                break;
            case SlingshotState.UserPulling:
                DisplaySlingshotLineRenderers();

                if (Input.GetMouseButton(0))
                {
                    //get where user is tapping
                   location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                   location.z = 0;

                    PrepareDots();

                    //we will let the user pull the bird up to a maximum distance
                    if (Vector3.Distance(location, SlingshotMiddleVector) > 4f)
                    {
                        //basic vector maths :)
                        maxPosition = (location - SlingshotMiddleVector).normalized * 4f + SlingshotMiddleVector;
                        //Debug.Log(maxPosition);
                        BirdToThrow.transform.position = maxPosition;

                    }
                    else
                    {
                        BirdToThrow.transform.position = location;
                    }

                    //for(int i=0;i<number;i++)
                    //{
                    //    trajectoryDots[i].transform.position = calculatePosition(i * 0.1f);
                    //}
                   // direction = (location -).normalized;
                    float distance = Vector3.Distance(SlingshotMiddleVector, BirdToThrow.transform.position);
                    //display projected trajectory based on the distance
                     DisplayTrajectoryLineRenderer2(distance);
                 //  Vector3 Pforce = direction*distance * force;
                   // UpdateDots(distance);
                    
                }
                else//user has removed the tap 
                {
                    
                    SetTrajectoryLineRenderesActive(false);
                    //throw the bird!!!
                    TimeSinceThrown = Time.time;
                    float distance = Vector3.Distance(SlingshotMiddleVector, BirdToThrow.transform.position);
                    if (distance > 1)
                    {
                        SetSlingshotLineRenderersActive(false);
                        slingshotState = SlingshotState.BirdFlying;
                        ThrowBird(distance);
                    }
                    else//not pulled long enough, so reinitiate it
                    {
                        //distance/10 was found with trial and error :)
                        //animate the bird to the wait position
                        BirdToThrow.transform.positionTo(distance / 10, //duration
                            BirdWaitPosition.transform.position). //final position
                            setOnCompleteHandler((x) =>
                        {
                            x.complete();
                            x.destroy();
                           
                            InitializeBird();
                            //PrepareDots();
                         
                        });

                    }
                }
                break;

            case SlingshotState.BirdFlying:
                //Destroying Trajectory previous child dots
                //Debug.Log(dotsParent.transform.childCount);
                for (int i = 0; i <= dotsParent.transform.childCount; i++)
                Destroy(dotsParent.transform.GetChild(i).gameObject);
            
                break;
            default:
                break;
        }

    }
   
    private void ThrowBird(float distance)
    {
        //get velocity
        Vector3 velocity = SlingshotMiddleVector - BirdToThrow.transform.position;
        BirdToThrow.GetComponent<Bird>().OnThrow(); //make the bird aware of it
        //old and alternative way
        //BirdToThrow.GetComponent<Rigidbody2D>().AddForce
        //    (new Vector2(v2.x, v2.y) * ThrowSpeed * distance * 300 * Time.deltaTime);
        //set the velocity
        BirdToThrow.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y) * ThrowSpeed * distance;


        //notify interested parties that the bird was thrown
        if (BirdThrown != null)
            BirdThrown(this, EventArgs.Empty);
    }

   private event EventHandler BirdThrown;

    private void InitializeBird()
    {
        //initialization of the ready to be thrown bird
        BirdToThrow.transform.position = BirdWaitPosition.position;
        slingshotState = SlingshotState.Idle;
        SetSlingshotLineRenderersActive(true);
        
    }

    void DisplaySlingshotLineRenderers()
    {
        SlingshotLineRenderer1.SetPosition(1, BirdToThrow.transform.position);
        SlingshotLineRenderer2.SetPosition(1, BirdToThrow.transform.position);
    }

    void SetSlingshotLineRenderersActive(bool active)
    {
        SlingshotLineRenderer1.enabled = active;
        SlingshotLineRenderer2.enabled = active;
    }

    void SetTrajectoryLineRenderesActive(bool active)
    {
        TrajectoryLineRenderer.enabled = active;
    }


    /// <summary>
    /// Another solution (a great one) can be found here
    /// http://wiki.unity3d.com/index.php?title=Trajectory_Simulation
    /// </summary>
    /// <param name="distance"></param>
    void DisplayTrajectoryLineRenderer2(float distance)
    {
        // timeStamp = dotSpacing;
        // SetTrajectoryLineRenderesActive(true);
        // Vector3 v2 = SlingshotMiddleVector - BirdToThrow.transform.position;

        //  // Vector2[] segments = new Vector2[segmentCount];

        // // The first line point is wherever the player's cannon, etc is
        // dotList[0] = BirdToThrow.transform.position;
        // // Debug.Log(segments[0]);
        // // The initial velocity
        // Vector2 segVelocity = new Vector2(v2.x, v2.y) * ThrowSpeed * distance;
        //// Debug.Log("segvelocity" + segVelocity);
        // //float angle = Vector2.Angle(segVelocity, new Vector2(1, 0));
        // //float time = segmentScale / segVelocity.magnitude;
        // for (int i = 1; i < dotsnumber; i++)
        // {
        //     //x axis: spaceX = initialSpaceX + velocityX * time
        //     //y axis: spaceY = initialSpaceY + velocityY * time + 1/2 * accelerationY * time ^ 2
        //     //both (vector) space = initialSpace + velocity * time + 1/2 * acceleration * time ^ 2
        //     float time2 = i * Time.fixedDeltaTime * 5;
        //    // Debug.Log(time2);
        //     segments[i] = segments[0] + segVelocity * time2 + 0.5f * Physics2D.gravity * Mathf.Pow(time2, 2);
        //    // Debug.Log(segments[i]);
        // }

        // TrajectoryLineRenderer.SetVertexCount(dotsnumber);
        // for (int i = 0; i < dotsnumber; i++)
        // {
        //     TrajectoryLineRenderer.SetPosition(i, segme[i]);

        // }
        SetTrajectoryLineRenderesActive(true);
        timeStamp = dotSpacing;
        Vector3 v2 = SlingshotMiddleVector - BirdToThrow.transform.position;
        Vector2 segVelocity = new Vector2(v2.x, v2.y) * ThrowSpeed * distance;
        dotsList[0].position = BirdToThrow.transform.position;
        for (int i = 0; i < dotsNumber; i++)
        {

            // Vector2 currentposition = (Vector2)transform.position + direction.normalized * force * timeStamp + 0.5f * Physics2D.gravity * (timeStamp* timeStamp);
            dotsList[i].position = (Vector2)BirdToThrow.transform.position + (Vector2)segVelocity * timeStamp + 0.5f * Physics2D.gravity * Mathf.Pow(timeStamp, 2);

            timeStamp += dotSpacing;
        }
    }



    ///http://opengameart.org/content/forest-themed-sprites
    ///forest sprites found on opengameart.com
    ///© 2012-2013 Julien Jorge <julien.jorge@stuff-o-matic.com>

}
