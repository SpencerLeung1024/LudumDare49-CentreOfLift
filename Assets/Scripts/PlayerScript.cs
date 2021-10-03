using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public float maxSpeed;
    public float maxForce;
    public float jumpPower;
    //public GameObject platforms;
    public GameObject platformRoot;
    public GameObject ground;
    public GameObject pickUps;
    public GuiScript guiScript;
    public GameObject playerCamera; // the camera's direction is used to figure out movement.
   
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    //private Quaternion originalRotation;
    private bool canJump = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //originalRotation = transform.rotation;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnJump(InputValue movementValue)
    {
        if (canJump)
        {
            canJump = false;
            //rb.AddForce(transform.up * jumpPower);
            rb.AddForce(Vector3.up * jumpPower);
        }
    }

    // Update is called once per frame
    /*
    void Update()
    {
        
    }
    */

    void OnCollisionEnter(Collision otherCollision)
    {
        GameObject other = otherCollision.gameObject;
        if (other == ground)
        {
            transform.position = new Vector3(0.0f, 2.0f, 0.0f);
            transform.rotation = Quaternion.identity;
            rb.velocity = Vector3.zero;
        }
        else
        {
            GameObject otherParent = other.gameObject.transform.parent.gameObject;
            if (otherParent == platformRoot || other.CompareTag("Platform"))
            {
                canJump = true;
            }
            else if (otherParent == pickUps)
            {
                int value = 1;
                if (other.transform.localScale.x == 0.6f)
                {
                    value = 3;
                }
                else if (other.transform.localScale.x == 0.9f)
                {
                    value = 10;
                }
                if (other.CompareTag("Money"))
                {
                    guiScript.money = guiScript.money + value;
                }
                else if (other.CompareTag("Research"))
                {
                    guiScript.research = guiScript.research + value;
                }
                Destroy(other);
            }
        }
    }

    /*
    void OnCollisionExit(Collision other)
    {
        //canJump = false;
    }
    */

    // FixedUpdate is called once every physics step
    void FixedUpdate()
    {
        if (transform.position.y < -20.0f)
        {
            transform.position = new Vector3(0.0f, 2.0f, 0.0f);
            transform.rotation = Quaternion.identity;
            rb.velocity = Vector3.zero;
        }
        //Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        Vector3 rightVector = Vector3.Cross(Vector3.up, playerCamera.transform.forward.normalized).normalized;
        Vector3 forwardVector = Vector3.Cross(rightVector, Vector3.up).normalized;
        Vector3 movement = (rightVector * movementX) + (forwardVector * movementY);
        if (Vector3.Dot(rb.velocity, movement) < maxSpeed * maxSpeed)
        {
            //System.out.println(movement.ToString());
            rb.AddForce(movement * maxForce);
        }
        /*
        if (inputAction.PlayerControls.Jump.triggered)
        {
            if (canJump)
            {
                canJump = false;
                rb.AddForce(transform.up * jumpPower);
            }
        }
        */
        //transform.SetPositionAndRotation(transform.position, platforms.transform.rotation); // Always keep the platform's rotation
        //transform.SetPositionAndRotation(transform.position, originalRotation);
    }
}
