using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineInfoScript : MonoBehaviour
{
    public string[] engineNameArray =
    {
        "Party Balloon",
        "Hot Air Balloon",
        "Rotor",
        "Turboprop",
        "Turbofan",
        "Turbojet",
        "Weather Balloon",
        "Ramjet",
        "Scramjet",
        "Rocket"
    };

    public int[] engineResearchArray =
    {
        0,
        20,
        30,
        30,
        50,
        60,
        60,
        70,
        80,
        100
    };

    public float[] engineAltitudeArray =
    {
        4000.0f,
        6000.0f,
        8000.0f,
        12000.0f,
        20000.0f,
        30000.0f,
        40000.0f,
        50000.0f,
        75000.0f,
        100000.0f
    };

    public GameObject[] enginePrefabArray = new GameObject[10]; // This needs to be set manually in the editor.

    // Start is called before the first frame update
    /*
    void Start()
    {
        string[] newEngineNameArray = // The actual info needs to be initialized at start because the Unity inspector always overwrites it.
        {
            "Party Balloon",
            "Hot Air Balloon",
            "Rotor",
            "Turboprop",
            "Turbofan",
            "Turbojet",
            "Weather Balloon",
            "Ramjet",
            "Scramjet",
            "Rocket"
        };
        engineNameArray = newEngineNameArray;

        int[] newEngineResearchArray =
        {
            0,
            20,
            30,
            30,
            50,
            60,
            60,
            70,
            80,
            100
        };
        engineResearchArray = newEngineResearchArray;

        int[] newEngineAltitudeArray =
        {
            4000,
            6000,
            8000,
            12000,
            20000,
            30000,
            40000,
            50000,
            75000,
            100000
        };
        engineAltitudeArray = newEngineAltitudeArray;
    }
    */

    // Update is called once per frame
    /*
    void Update()
    {
        
    }
    */
}
