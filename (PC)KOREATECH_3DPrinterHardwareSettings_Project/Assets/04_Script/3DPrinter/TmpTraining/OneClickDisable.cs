using UnityEngine;
using UnityEngine.UI;

public class OneClickDisable : MonoBehaviour
{
    public Image ButtonHighlight;

    private void OnMouseUp()
    {
        this.gameObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        
    }

    private void OnMouseExit()
    {
        
    }
}
