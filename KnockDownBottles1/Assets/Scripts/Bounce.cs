using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    private float fireballXValue;
    public float fireballSpeed;

    void Start()
    {
        // getting the initial position where prefab is created
        fireballXValue = gameObject.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        // adding speed value to the X axis position
        // value
        fireballXValue += fireballSpeed;
        // setting new X value to position
        gameObject.transform.position = new Vector2(fireballXValue, gameObject.transform.position.y);
    }
}


