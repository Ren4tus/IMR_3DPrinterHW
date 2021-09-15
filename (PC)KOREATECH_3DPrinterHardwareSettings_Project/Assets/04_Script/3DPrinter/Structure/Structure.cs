using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Structure : MonoBehaviour
{
    public Image image;
    public Sprite s;
    private void OnEnable()
    {
        image.sprite = s;
    }
    private void OnDisable()
    {
        
    }
}
