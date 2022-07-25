using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHit : MonoBehaviour
{
    public FloatVar hp;
    [SerializeField] public GameObject shield;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!CompareTag(collider.tag))
        {
            //check in case collided something that's not a cannonball
            if (collider.GetComponent<Cannonball>() != null)
            {
                //check that the shield isnt active (in case player overshoots it, for example)
                if (!shield.activeSelf)
                {
                    hp.value -= collider.GetComponent<Cannonball>().damage;
                    HPBarsHandler.instance.UpdateHPBars();
                }
            }
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
