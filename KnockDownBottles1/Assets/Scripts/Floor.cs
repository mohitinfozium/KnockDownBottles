using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    
    void OnCollisionEnter2D(Collision2D col)
    {
        
        if (col.gameObject.name == "Bottle piece")
        {
        
            Destroy(col.gameObject,0.5f);
            
        }

        if (col.gameObject.tag == "Bird")
        {
            Destroy(col.gameObject, 5f);
        }
        
    }
    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.name == "Bottle piece")
        {

            Destroy(col.gameObject, 0.5f);

        }
        if (col.gameObject.tag == "Bird")
        {
            Destroy(col.gameObject, 5f);
        }

    }
}
