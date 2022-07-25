using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    //[SerializeField] public GameObject player; //the player the cannonball belongs to
    public float damage;
    void OnTriggerEnter2D(Collider2D collision)
    {
        //deletes on entering collisionable objects of not current player
        if (!CompareTag(collision.tag))
        {
            //ignores other cannonballs tho
            if (!collision.name.Equals(name))
                Destroy(gameObject);
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
