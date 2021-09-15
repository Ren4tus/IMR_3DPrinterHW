using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuickBar : MonoBehaviour
{
    public GameObject[] gameObjects = new GameObject[2];
    public GameObject parent;
    //현재 SeqNum의 인덱스
    Image buttonimage;
    public Sprite image;
    public Sprite defaultimage;
    private void Awake()
    {
        buttonimage = this.GetComponent<Image>();
        defaultimage = buttonimage.sprite;
    }
    private void Update()
    {
        if (gameObjects[0].activeSelf || (gameObjects[1].activeSelf&&parent.activeSelf))
        {
            buttonimage.sprite = image;
        }
        else
        {
            buttonimage.sprite = defaultimage;
        }
    }
}