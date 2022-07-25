using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class Cannon : MonoBehaviour//, IPointerDownHandler
{
    public const float rotationSpeed = 180;
    public const float shootingSpeed = 1;
    public const float shootingForce = 500;

    public bool isShooting = false;
    private float timeLeft = 0;
    private const float damage = 5;
    [SerializeField] private GameObject cannonball;

    void Start()
    {

    }
    public void OnScreenClick(PointerDownEvent evt)
    {
        isShooting = true;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;

        if (Input.GetMouseButton(0) && isShooting)
        {

            //==========================================================================
            //block responsible for turning the cannon towards the touch

            Vector3 targetLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            targetLocation.z = transform.position.z; // ensure there is no 3D rotation by aligning Z position

            // vector from this object towards the target location
            Vector3 vectorToTarget = targetLocation - transform.position;
            // rotate that vector by 90 degrees around the Z axis
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * vectorToTarget;

            // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
            // (resulting in the X axis facing the target)
            Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

            if (transform.rotation != targetRotation)
            {
                Quaternion resultingRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                float angle = resultingRotation.eulerAngles.z;
                if (angle > 180f) angle -= 360f;

                //only apply resultingrotation if said result wont be out of turning bounds
                if ((angle > -45f) && (angle < 70f)) transform.rotation = resultingRotation;
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
                Vector3 ballPosition = transform.position + transform.rotation * new Vector3(0.7f, 0, 0);
                ballPosition.z = transform.position.z + 1;
                newBall.transform.position = ballPosition;

                //set that this ball belongs to the current player's cannon
                newBall.tag = tag;

                //give the cannonball damage
                newBall.GetComponent<Cannonball>().damage = damage;

                //have to set active since the original is not
                newBall.SetActive(true);

                //shoot (at cannon's direction)
                newBall.GetComponent<Rigidbody2D>().AddForce(transform.rotation * Vector2.right * shootingForce);

            }

            //block responsible for shooting
            //==========================================================================

            //returns before we could turn off shooting
            return;
        }
        //turns off shooting when user stops holding down finger/mouse
        isShooting = false;
    }
}
