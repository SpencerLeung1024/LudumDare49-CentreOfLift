using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.ParticleSystemModule;

public class StormScript : MonoBehaviour
{
    public float stormHeight;
    public float maxStormPressure;
    public ParticleSystem wind;
    public ParticleSystem fog;
    public PlatformRootScript platformRoot;
    public GameObject player;
    public GameObject pickUps;
    public float stormTopAltitude;
    public float stormBottomAltitude;
    public float stormPressure;
    public float stormFactor; // 0 at the top, 1 at the bottom
    
    private ParticleSystem.MainModule windMain; // The modules of a particle system need to be referenced individually
    private ParticleSystem.EmissionModule windEmission;
    private ParticleSystem.MainModule fogMain;
    private ParticleSystem.EmissionModule fogEmission;

    // Start is called before the first frame update
    void Start()
    {
        windMain = wind.main;
        windEmission = wind.emission;
        fogMain = fog.main;
        fogEmission = fog.emission;
    }

    void PushObjects()
    {
        Rigidbody rb;
        rb = player.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(rb.mass * stormPressure, 0, 0));

        foreach (Transform pickUpTransform in pickUps.transform)
        {
            rb = pickUpTransform.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(new Vector3(rb.mass * Mathf.Max(0.0f, (10.0f - pickUpTransform.position.y) / 10.0f) * stormPressure, 0, 0)); // Only start pushing them when they're below 10 vertical units because otherwise they will fly over the platforms.
        }
    }

    // Update is called once per frame
    /*
    void Update()
    {
        
    }
    */

    // FixedUpdate is called once per physics step
    void FixedUpdate()
    {
        stormBottomAltitude = stormBottomAltitude + (Mathf.Max(5.0f, (platformRoot.altitude - stormBottomAltitude) / 300.0f) * Time.deltaTime); // The storm rises by 5 m/s or catches up to the platform in 5 minutes (10 m/s if 3 km above, 33.33 m/s if 10 km above), whichever is greater
        stormTopAltitude = stormBottomAltitude + stormHeight;
        stormFactor = Mathf.Max(0.0f, Mathf.Min((stormTopAltitude - platformRoot.altitude) / stormHeight, 1.0f));
        stormPressure = maxStormPressure * stormFactor;
        windEmission.rateOverTime = (int) (500.0f * stormFactor);
        float windStartSpeed = 250.0f * (0.2f + (0.8f * stormFactor));
        windMain.startSpeed = windStartSpeed;
        windMain.startLifetime = 500.0f / windStartSpeed;
        fogEmission.rateOverTime = (int) (100.0f * Mathf.Pow(stormFactor, 2)); // The fog is too dense at high altitudes.
        float fogStartSpeed = 50.0f * (0.2f + (0.8f * stormFactor));
        fogMain.startSpeed = fogStartSpeed;
        fogMain.startLifetime = 500.0f / fogStartSpeed;
        PushObjects();
    }

    // LateUpdate is called after other updates
    /*
    void LateUpdate()
    {

    }
    */
}
