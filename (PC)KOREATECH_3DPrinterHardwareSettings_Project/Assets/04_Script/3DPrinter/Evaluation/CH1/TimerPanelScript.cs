using UnityEngine;
using UnityEngine.EventSystems;

public class TimerPanelScript : MonoBehaviour, IPointerClickHandler
{
    private Animator ani;
    private bool isOpen = false;

    public Animator GuidePanelAni;

    void Start()
    {
        ani = this.GetComponent<Animator>();
        GuidePanelAni = GuidePanelAni.GetComponent<Animator>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isOpen)
        {
            isOpen = false;
            ani.SetTrigger("TimerPanelClose");
            GuidePanelAni.SetTrigger("GuidePanelOpen");
        }
        else
        {
            isOpen = true;
            ani.SetTrigger("TimerPanelOpen");
            GuidePanelAni.SetTrigger("GuidePanelClose");
        }
    }
}
