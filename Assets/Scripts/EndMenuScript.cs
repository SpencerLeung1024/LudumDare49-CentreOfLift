using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenuScript : MonoBehaviour
{
    public void ReturnToStart()
    {
        SceneManager.LoadScene("StartScene");
    }

    // Start is called before the first frame update
    /*
    void Start()
    {
        
    }
    */

    // Update is called once per frame
    /*
    void Update()
    {
        
    }
    */
}
