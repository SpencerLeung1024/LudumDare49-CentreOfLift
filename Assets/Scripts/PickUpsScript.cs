using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpsScript : MonoBehaviour
{
    public float spawnHeight;
    public GameObject moneyTemplate;
    public GameObject researchTemplate;
    public GameObject platformRoot;

    private Vector3 GetSpawnPosition()
    {
        GameObject platform = platformRoot.transform.GetChild(Random.Range(0, platformRoot.transform.childCount)).gameObject;
        Vector3 vectorToRoot = (platformRoot.transform.position - platform.transform.position).normalized;
        Vector3 result = platform.transform.position + (vectorToRoot * 2.5f); // Move towards the inside so it doesn't roll off immediately.
        result = result + Random.insideUnitSphere;
        result = result + (Vector3.up * spawnHeight);
        return result;
    }

    private void ResizePickUp(GameObject pickUp)
    {
        float value = Random.Range(0.0f, 1.0f);
        if (value < 0.1)
        {
            pickUp.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f); // Will have a value of 10.
            pickUp.GetComponent<Rigidbody>().mass = 0.4f;
            pickUp.transform.Find("Point Light").GetComponent<Light>().range = 12f;
        }
        else if (value < 0.4)
        {
            pickUp.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f); // Will have a value of 3.
            pickUp.GetComponent<Rigidbody>().mass = 0.2f;
            pickUp.transform.Find("Point Light").GetComponent<Light>().range = 8f;
        }
    }

    private void SpawnMoney()
    {
        if (Random.Range(0.0f, 1.0f) < 0.5) // Each time this runs, it will only instantiate a money object 50% of the time, for randomness.
        {
            GameObject moneyObject = Instantiate(moneyTemplate, GetSpawnPosition(), Quaternion.identity);
            moneyObject.transform.parent = transform;
            ResizePickUp(moneyObject);
        }
    }

    private void SpawnResearch()
    {
        if (Random.Range(0.0f, 1.0f) < 0.5)
        {
            GameObject researchObject = Instantiate(researchTemplate, GetSpawnPosition(), Quaternion.identity);
            researchObject.transform.parent = transform;
            ResizePickUp(researchObject);
        }
    }

    private void CleanUp()
    {
        foreach (Transform childTransform in transform)
        {
            if (childTransform.position.y < -20.0f)
            {
                Destroy(childTransform.gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnMoney", 1.0f, 1.0f);
        InvokeRepeating("SpawnResearch", 1.5f, 1.0f);
        InvokeRepeating("CleanUp", 1.0f, 1.0f);
    }

    // Update is called once per frame
    /*
    void Update()
    {
        
    }
    */

    // FixedUpdate is called once every physics step
    /*
    void FixedUpdate()
    {
        
    }
    */
}
