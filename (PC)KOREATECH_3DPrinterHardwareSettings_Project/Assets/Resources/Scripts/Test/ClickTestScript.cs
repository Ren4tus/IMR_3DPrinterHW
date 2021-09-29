using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTestScript : MonoBehaviour
{
    private GameObject target;

    public GameObject printerCover;
    public GameObject printerBlock;
    private Animator animator;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            target = GetClickedObject();

            if (target == printerCover) 
            {
                if (animator.GetBool("IsCoverOpen") == true)
                {
                }
                else
                {
                    animator.SetTrigger("CoverOpen");
                }
            }
        }

    }
    
    private GameObject GetClickedObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            target = hit.transform.gameObject;
            return target;
        }
        return null;
    }

    public void CoverOpen()
    {
        animator.SetBool("IsCoverOpen", true);
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.MJP_CoverOpen);
    }
    public void CoverClose()
    {
        animator.SetBool("IsCoverOpen", false);
        CL_CommonFunctionManager.Instance.PlayEquipSound(CL_SoundManager.EquipSound.MJP_CoverClose);
    }
}
