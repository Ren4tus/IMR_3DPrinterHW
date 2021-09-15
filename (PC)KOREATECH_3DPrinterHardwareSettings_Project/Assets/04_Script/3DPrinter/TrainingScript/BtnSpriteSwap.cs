using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnSpriteSwap : MonoBehaviour
{
    public Sprite HoverSprite;
    public Sprite ClickSprite;
    public Sprite NormalSprite;

    public SpriteRenderer btn;
    
    /*
    private void OnMouseOver()
    {
        btn.sprite = HoverSprite;
    }
    */

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

    private void OnMouseUp()
    {
            MainController.instance.isRightClick = true;
            MainController.instance.goNextSeq();
            MainController.instance.isRightClick = false;
    }
}