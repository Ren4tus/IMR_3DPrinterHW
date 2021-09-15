using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnSpriteSwapNoNextSeq : MonoBehaviour
{
    public Sprite HoverSprite;
    public Sprite ClickSprite;
    public Sprite NormalSprite;

    public SpriteRenderer btn;

    private void OnMouseEnter()
    {
        btn.sprite = HoverSprite;
    }

    private void OnMouseExit()
    {
        btn.sprite = NormalSprite;
    }

    private void OnMouseDown()
    {
        btn.sprite = ClickSprite;
    }
}
