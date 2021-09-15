using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeController : MonoBehaviour
{
    
    // Update is called once per frame
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            SceneManager.LoadScene("Test");
        }
    }
}
