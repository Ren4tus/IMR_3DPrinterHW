using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LUMenuScript : MonoBehaviour
{
    public Animator m_Animator;
    public bool bIsMenuOpen;

    // Start is called before the first frame update
    void Start()
    {
        bIsMenuOpen = m_Animator.GetBool("IsMenuOpen");
    }

    public void ToggleLUMenu()
    {
        if (bIsMenuOpen)
        {
            bIsMenuOpen = false;
        }
        else
        {
            bIsMenuOpen = true;
        }
        
        m_Animator.SetBool("IsMenuOpen", bIsMenuOpen);
    }
}
