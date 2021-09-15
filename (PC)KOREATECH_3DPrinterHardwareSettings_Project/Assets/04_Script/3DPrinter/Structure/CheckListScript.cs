using UnityEngine.EventSystems;
using UnityEngine;

public class CheckListScript : MonoBehaviour, IPointerClickHandler
{
    private Animator ani;
    private bool isOpen = true;

    void Start()
    {
        ani = this.GetComponent<Animator>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isOpen)
        {
            isOpen = false;
            ani.SetTrigger("CheckPanelClose");
        }
        else
        {
            isOpen = true;
            ani.SetTrigger("CheckPanelOpen");
        }
    }
}
