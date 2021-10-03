using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRootScript : MonoBehaviour
{
    public float dragFactor; // Drag is proportional to number of platforms times speed squared times pressure
    public GameObject ground; // the platform stays fixed while the ground moves down
    public float altitude; // Metres
    public float speed; // Metres per second, positive is up
    public float acceleration; // Metres per second per second, positive is upwards
    public float mass; // Metric tons
    public float lift; // Kilonewtons, positive is upwards force
    public float drag; // Kilonewtons, positive is downwards force
    public float pressure; // Kilopascals
    public GuiScript gui; // Used for getting the current engine altitude. It goes EngineInfoScript -> GuiScript -> PlatformRootScript .
    public EngineInfoScript engineInfo; // Info on engines.

    private float pressureASL = 100.0f;
    private float scaleHeight = 8500.0f; // Metres. The atmosphere thins by a factor of e with every increase in altitude equal to this. Beyond the maximum altitude of engines, lift is reduced.

    //private int sideUnits = 2; // How many platforms away from the platform root to place the next engine.
    //private int enginesPerSide = 4; // Increases by 2 with every side length increase.
    //private int notEngines = 5; // How many platforms are not engines.
    //private int completedSquarePlatforms = 9; // How many platforms are in the completed square.
    // Too lazy to do the math.
    private List<Vector3> nextEngineOffsets = new List<Vector3>();
    private GameObject enginePrefab;

    private void AddAllDescendants(GameObject parent, List<GameObject> list) // Modified from code stolen from https://forum.unity.com/threads/finding-all-children-of-object.453466/ .
    {
        foreach (Transform childTransform in parent.transform)
        {
            list.Add(childTransform.gameObject);
            AddAllDescendants(childTransform.gameObject, list);
        }
    }

    /*
    private Vector2 GetPlatformOffsetFromIndex(int index)
    {
        int sideIndex = index - completedSquarePlatforms;
        int side = sideIndex % 4; // 1 = north = +Z, 2 = south = -Z, 3 = east = +X, 0 = west = -X
        int indexInSide = 
    }
    */

    public void AddEngine()
    {
        if (nextEngineOffsets.Count > 0)
        {
            Vector3 enginePosition = transform.TransformPoint(nextEngineOffsets[0]);
            nextEngineOffsets.RemoveAt(0);
            GameObject engine = Instantiate(enginePrefab, enginePosition, transform.rotation);
            engine.transform.parent = transform;
            engine.GetComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>(); // Connect the new engine with the platform root
            engine.name = "Engine" + gui.currentEngineCount.ToString(); // Give each engine a different index. The gui script already incremented the current engine count.
        }
        else
        {
            Debug.Log("No more engines can be added.");
        }
    }

    public void ChangeAllEngines()
    {
        if (engineInfo.enginePrefabArray[gui.currentEngineLevel])
        {
            enginePrefab = engineInfo.enginePrefabArray[gui.currentEngineLevel];
            GameObject engine;
            Vector3 enginePosition;
            string engineName;
            GameObject newEngine;
            // Has a tendency to crash the Unity editor.
            /*
            foreach (Transform engineTransform in transform)
            {
                engine = engineTransform.gameObject;
                Debug.Log(engine.name);
                if (engine.name.Contains("Engine")) // This is an engine.
                {
                    enginePosition = engineTransform.position;
                    engineName = engine.name;
                    Destroy(engine);
                    newEngine = Instantiate(enginePrefab, enginePosition, transform.rotation);
                    newEngine.transform.parent = transform;
                    newEngine.GetComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>(); // Connect the new engine with the platform root
                    newEngine.name = engineName;
                }
            }
            */
            Transform engineTransform;
            int engineIndex = 1;
            bool moreEngines = true;
            while (moreEngines)
            {
                engineName = "Engine" + engineIndex.ToString();
                engineTransform = transform.Find(engineName);
                if (engineTransform)
                {
                    engine = engineTransform.gameObject;
                    //Debug.Log(engine.name);
                    enginePosition = engineTransform.position;
                    Destroy(engine);
                    newEngine = Instantiate(enginePrefab, enginePosition, transform.rotation);
                    newEngine.transform.parent = transform;
                    newEngine.GetComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>(); // Connect the new engine with the platform root
                    newEngine.name = engineName;
                    engineIndex = engineIndex + 1;
                }
                else
                {
                    moreEngines = false;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        nextEngineOffsets.Add(new Vector3(0.0f, 0.0f, 10.0f));
        nextEngineOffsets.Add(new Vector3(0.0f, 0.0f, -10.0f));
        nextEngineOffsets.Add(new Vector3(10.0f, 0.0f, 0.0f));
        nextEngineOffsets.Add(new Vector3(-10.0f, 0.0f, 0.0f));

        nextEngineOffsets.Add(new Vector3(5.0f, 0.0f, 10.0f));
        nextEngineOffsets.Add(new Vector3(-5.0f, 0.0f, -10.0f));
        nextEngineOffsets.Add(new Vector3(10.0f, 0.0f, -5.0f));
        nextEngineOffsets.Add(new Vector3(-10.0f, 0.0f, 5.0f));

        nextEngineOffsets.Add(new Vector3(-5.0f, 0.0f, 10.0f));
        nextEngineOffsets.Add(new Vector3(5.0f, 0.0f, -10.0f));
        nextEngineOffsets.Add(new Vector3(10.0f, 0.0f, 5.0f));
        nextEngineOffsets.Add(new Vector3(-10.0f, 0.0f, -5.0f));

        nextEngineOffsets.Add(new Vector3(10.0f, 0.0f, 10.0f));
        nextEngineOffsets.Add(new Vector3(-10.0f, 0.0f, -10.0f));
        nextEngineOffsets.Add(new Vector3(10.0f, 0.0f, -10.0f));
        nextEngineOffsets.Add(new Vector3(-10.0f, 0.0f, 10.0f));

        enginePrefab = engineInfo.enginePrefabArray[gui.currentEngineLevel];
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
        mass = 0.0f;
        lift = 0.0f;
        List<GameObject> descendants = new List<GameObject>();
        AddAllDescendants(gameObject, descendants);
        Rigidbody descendantrb;
        ConstantForce descendantcf;
        foreach (GameObject descendant in descendants)
        {
            descendantrb = descendant.GetComponent<Rigidbody>();
            if (descendantrb != null)
            {
                mass = mass + descendantrb.mass;
            }
            descendantcf = descendant.GetComponent<ConstantForce>();
            if (descendantcf != null)
            {
                lift = lift + descendantcf.force.y;
            }
        }
        pressure = pressureASL * Mathf.Exp(-(altitude / scaleHeight));
        drag = (float) transform.childCount * speed * speed * dragFactor * pressure;
        if (speed < 0.0f) // If falling down, drag should slow the platform down.
        {
            drag = -drag;
        }
        if (altitude > gui.currentEngineAltitude)
        {
            lift = lift * Mathf.Exp(-((altitude - gui.currentEngineAltitude) / scaleHeight));
        }
        acceleration = (lift - (mass * -Physics.gravity.y) - drag) / mass;
        speed = speed + (acceleration * Time.deltaTime);
        altitude = altitude + (speed * Time.deltaTime);
        ground.transform.position = new Vector3(0.0f, -1.0f * Mathf.Max(altitude, 0.51f), 0.0f);
        //ground.transform.scale = new Vector3(Mathf.Max(10000.0f, altitude * 200.0f), 0.001f, Mathf.Max(10000.0f, altitude * 200.0f));
        ground.transform.localScale = new Vector3(Mathf.Max(10000.0f, altitude * 200.0f), 0.001f, Mathf.Max(10000.0f, altitude * 200.0f));
    }
}
