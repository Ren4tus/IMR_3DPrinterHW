using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CommonDoorActionAS : MonoBehaviour
{
    protected Animator _animator;
    protected bool isOpen = false; // 초기 문들은 닫혀있다고 가정
    
    public bool IsOpen => isOpen;

    protected void Awake()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();
    }

    public void Open()
    {
        if (!IsOpen)
        {
            isOpen = true;
            _animator.Play("Open");
            _animator.SetTrigger("Open");
        }
    }

    public void Close()
    {
        if (IsOpen)
        {
            isOpen = false;
            _animator.Play("Close");
            _animator.SetTrigger("Close");
        }
    }

    public void DoorToggle()
    {
        if (IsOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public virtual void PlayOpenSound()
    {
        //...
    }

    public virtual void PlayCloseSound()
    {
        //...
    }
}
