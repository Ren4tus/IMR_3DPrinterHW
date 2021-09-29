/*
 * 열기, 닫기 등의 기본 동작
 * 상속받아 사용
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CommonEquipActionAS : MonoBehaviour
{
    private Animator _animator;
    private bool isEquiped = true;

    public Animator animator => _animator;
    public bool IsEquiped => isEquiped;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Equip()
    {
        isEquiped = true;
        _animator.SetTrigger("Equip");
    }

    public void Remove()
    {
        isEquiped = false;
        _animator.SetTrigger("Remove");
    }

    public virtual void PlayEquipSound()
    {
        //...
    }

    public virtual void PlayRemoveSound()
    {
        //...
    }
}
