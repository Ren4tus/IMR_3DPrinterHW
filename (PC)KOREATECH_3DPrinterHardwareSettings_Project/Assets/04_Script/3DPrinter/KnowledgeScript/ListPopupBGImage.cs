using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListPopupBGImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform BGImage;

    private void Start()
    {
        BGImage = BGImage.GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BGImage.gameObject.SetActive(true);

        BGImage.position = new Vector3(Screen.width/2, this.gameObject.transform.position.y, 0);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BGImage.gameObject.SetActive(false);
    }
}