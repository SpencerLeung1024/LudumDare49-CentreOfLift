using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsScript : MonoBehaviour
{
    public float altitude;
    public float torqueMultiplier;
    //public float azimuth = 0.0f;
    //public float pitch = 0.0f;
    public GameObject root; // An empty transform used to join all the platforms together so floating point errors don't accumulate and cause gaps.
    public Vector3 offset; // Offset from the root. This "platforms" game object handles all movement and everything related to the theme (unstable).
    public GameObject player;
    public Vector3 CoM; // Centre of mass
    public Vector3 CoL; // Centre of lift. Actually thrust but whatever

    private Rigidbody rb;

    private void CalculateBalance()
    {
        rb.mass = 0.0f;
        Rigidbody childrb;
        foreach (Transform childTransform in transform) // What
        {
            childrb = childTransform.gameObject.GetComponent<Rigidbody>();
            if (childrb != null)
            {
                if (rb.mass == 0.0f)
                {
                    CoM = childTransform.position;
                }
                else
                {
                    CoM = Vector3.Lerp(CoM, childTransform.position, childrb.mass / rb.mass);
                    rb.mass = rb.mass + childrb.mass;
                }
            }
        }
        float playerHeight = transform.InverseTransformPoint(player.transform.position).y;
        if (playerHeight > 0.9 && playerHeight < 1.1) // Wacky method for figuring out if the player is on the platforms
        {
            childrb = player.GetComponent<Rigidbody>();
            CoM = Vector3.Lerp(CoM, player.transform.position, childrb.mass / rb.mass);
            rb.mass = rb.mass + childrb.mass;
        }
        CoL = root.transform.position; // Placeholder
    }

    private void ApplyRotation()
    {
        Vector3 vectorToCoM = CoM - CoL;
        //float newAzimuth = 0;
        if (vectorToCoM != Vector3.zero)
        {
            //newAzimuth = atan2(vectorToCoM.x, vectorToCoM.z); // 0 = north = +Z, pi / 4 = east = +X, pi / 2 = south = -Z, 3 * pi / 2 = west = -X
            // TODO: Torque calculations.
            //pitch = pitch + 
            //Vector3 rotationAxis = Vector3.Cross(vectorToCoM.normalized, Vector3.up);
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, vectorToCoM.normalized);
            float torqueMagnitude = vectorToCoM.magnitude * torqueMultiplier;
            transform.RotateAround(transform.position, rotationAxis, torqueMagnitude * Time.deltaTime);
        }
        
        //transform.RotateAround(transform.position, Vector3.up, angularVelocity * Time.deltaTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        root = transform.Find("Root").gameObject;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    /*
    void Update()
    {
        
    }
    */

    void FixedUpdate()
    {
        CalculateBalance();
        ApplyRotation();
    }
}
