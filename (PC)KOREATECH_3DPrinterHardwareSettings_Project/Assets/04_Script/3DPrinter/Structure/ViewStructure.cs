using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ViewStructure : MonoBehaviour
{
    public GameObject[] gameObjects;
    public MainController MainController;
    public GameObject cam;
    Vector3 mousepos;
    public Vector3[] Centers;
    // Update is called once per frame
    Vector3 x;
    void Update()
    {
        if(MainController.GetCurrentSeq() + 1 == MainController.NumberOfSeq)
        {

            if (Input.GetMouseButton(0)){
                x += (Input.mousePosition - mousepos) * 0.1f;
                Quaternion Q = new Quaternion();
                Q.eulerAngles = new Vector3(-1 * x.y, x.x);
                
                cam.transform.rotation = Q;
            }
        }
        mousepos = Input.mousePosition;
    }
    public void ChangeStructure(int a)
    {
        if (MainController.GetCurrentSeq() + 1 == MainController.NumberOfSeq)
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i].SetActive(false);
            }
            gameObjects[a].SetActive(true);
            transform.position = Centers[a];
        }
    }
}