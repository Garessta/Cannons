using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHandler : MonoBehaviour
{
    public FloatVar HPStart;
    private float HPCurrent;
    // Start is called before the first frame update
    void Start()
    {
        HPCurrent = HPStart.value;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!CompareTag(collider.tag))
        {
            if (collider.GetComponent<Cannonball>() != null)
            {
                HPCurrent -= collider.GetComponent<Cannonball>().damage;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf
            && HPCurrent <= 0)
        {
            HPCurrent = HPStart.value;
            gameObject.SetActive(false);
        }
        
    }
}
