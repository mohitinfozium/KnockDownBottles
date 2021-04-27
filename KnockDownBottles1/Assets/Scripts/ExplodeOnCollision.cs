using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explodable))]

public class ExplodeOnCollision : MonoBehaviour
{ 
    private Explodable explodable;


  
    void Start()
    {

        explodable = GetComponent<Explodable>();
        
    }

    void  OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "floor")
        {
            explodable.explode();
            //ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
            //ef.doExplosion(transform.position);

        }

    }
   
}
