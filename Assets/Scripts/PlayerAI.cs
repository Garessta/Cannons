using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class PlayerAI : MonoBehaviour//, IPointerDownHandler
{
    public FloatVar shieldCooldown;
    public const float rotationSpeed = 180;
    public const float shootingSpeed = 1;
    public const float shootingForce = 500;

    private float timeLeft = 0, timeLeftShield = 0, randDelay = 0;
    private const float damage = 5;
    private Vector3 targetRandom;
    [SerializeField] private GameObject cannonball;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject targetTower;
    [SerializeField] private GameObject targetShield;

    void Start()
    {
        randDelay = Random.value * (-3);
        targetRandom = Random.insideUnitSphere * 5;

        //wait a few seconds to start shooting or using shield after start
        timeLeft = 3;
        timeLeftShield = 3;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        timeLeftShield -= Time.deltaTime;

        //==========================================================================
        //block responsible for turning on shield (on cooldown + some random delay)

        if (timeLeftShield <= randDelay
            && !shield.activeSelf)
        {
            shield.SetActive(true);
            timeLeftShield = shieldCooldown.value;
            randDelay = Random.value * (-3);
        }

        //==========================================================================


        //==========================================================================
        //block responsible for turning the cannon towards the target

        Vector3 targetLocation;

        //check what is the current target
        if (targetShield.activeSelf)
        {
            targetLocation = targetShield.transform.position;
        }
        else
            targetLocation = targetTower.transform.position;

        //i decided to add some random movement just for fun
        targetLocation += targetRandom;

            targetLocation.z = transform.position.z; // ensure there is no 3D rotation by aligning Z position

            // vector from this object towards the target location
            Vector3 vectorToTarget = targetLocation - transform.position;
            // rotate that vector by 90 degrees around the Z axis
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, -90) * vectorToTarget;

            // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
            // (resulting in the X axis facing the target)
            Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

            if (transform.rotation != targetRotation)
            {
                Quaternion resultingRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                float angle = resultingRotation.eulerAngles.z;
                if (angle > 180f) angle -= 360f;

                //only apply resultingrotation if said result wont be out of turning bounds
                if ((angle < 45f) && (angle > -70f))
                { 
                    transform.rotation = resultingRotation; 
                }
            }

            //block responsible for turning the cannon towards the touch
            //==========================================================================

            //==========================================================================
            //block responsible for shooting

            if (timeLeft <= 0)
            {
                timeLeft = shootingSpeed;

                GameObject newBall = Instantiate(cannonball);

                //set ball position at the end of the cannon, but under it
                Vector3 ballPosition = transform.position + transform.rotation * new Vector3(-0.7f, 0, 0);
                ballPosition.z = transform.position.z + 1;
                newBall.transform.position = ballPosition;

                //set that this ball belongs to the current player's cannon
                newBall.tag = tag;

                //give the cannonball damage
                newBall.GetComponent<Cannonball>().damage = damage;

                //have to set active since the original is not
                newBall.SetActive(true);

                //shoot (at cannon's direction)
                newBall.GetComponent<Rigidbody2D>().AddForce(transform.rotation * Vector2.left * shootingForce);

                //making ourselves a new random value
                targetRandom = Random.insideUnitSphere * 5;
            }

            //block responsible for shooting
            //==========================================================================

    }
}
