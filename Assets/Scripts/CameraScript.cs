using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    public float sensitivity;
    public float distance;
    //private Vector3 offset;


    private float movementX;
    private float movementY;
    private Vector3 originalCameraPosition;
    private Vector3 oldPlayerPosition;

    // Start is called before the first frame update
    void Start()
    {
        //offset = transform.position - player.transform.position;
        originalCameraPosition = transform.position - player.transform.position;
    }

    /*
    void OnLook(InputValue movementValue)
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();
            movementX = movementVector.x;
            movementY = movementVector.y;
            //Vector3 rotateValue = new Vector3(-movementY, movementX, 0);
            //transform.eulerAngles = transform.eulerAngles + (rotateValue * sensitivity); // Modified from code stolen from https://answers.unity.com/questions/1179680/how-to-rotate-my-camera.html .
            transform.RotateAround(player.transform.position, Vector3.up, movementX * sensitivity); // yaw
            transform.RotateAround(player.transform.position, transform.right, -movementY * sensitivity); // pitch
        }
    }
    */

    // Update is called once per frame
    /*
    void Update()
    {
        
    }
    */

    // LateUpdate runs after all other updates are done
    void LateUpdate()
    {
        if (player.transform.position.y - oldPlayerPosition.y > 20.0f) // The only explanation for jumping 20 units in a single frame is teleportation.
        {
            transform.position = originalCameraPosition;
        }
        oldPlayerPosition = player.transform.position;

        //Vector3 rotateValue = new Vector3(-movementY, movementX, 0);
        //transform.eulerAngles = transform.eulerAngles + (rotateValue * sensitivity); // Modified from code stolen from https://answers.unity.com/questions/1179680/how-to-rotate-my-camera.html .
        //transform.position = player.transform.position + offset;
        transform.LookAt(player.transform);
        transform.position = player.transform.position + (transform.forward * -distance);

        //if (Mouse.current.rightButton.wasPressedThisFrame)
        //if (Mouse.current.leftButton.wasPressedThisFrame)
        //if (Keyboard.current.leftAltKey.wasPressedThisFrame)
        //if (Keyboard.current.leftCtrlKey.wasPressedThisFrame)
        if (Mouse.current.rightButton.isPressed)
        {
            Vector2 delta = Mouse.current.delta.ReadValue();
            movementX = delta.x;
            movementY = delta.y;
            transform.RotateAround(player.transform.position, Vector3.up, movementX * sensitivity); // yaw
            transform.RotateAround(player.transform.position, transform.right, -movementY * sensitivity); // pitch
        }
    }
}
