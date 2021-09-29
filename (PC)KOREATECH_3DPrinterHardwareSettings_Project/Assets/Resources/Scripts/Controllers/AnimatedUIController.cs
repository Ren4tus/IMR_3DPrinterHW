using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimatedUICore;

public class AnimatedUIController : AnimatedUI
{
    public AnimatedUICoreStruct.PivotType m_PivotType;
    public AnimatedUICoreStruct.AnimationType m_ShowAnimationType;
    public bool IsAnimationWhenStart = true;

    private bool isShow = true;

    private void Start()
    {
        if (IsAnimationWhenStart)
            Show();
    }

    public void Show()
    {
        switch (m_ShowAnimationType)
        {
            case AnimatedUICoreStruct.AnimationType.None:
                break;
            case AnimatedUICoreStruct.AnimationType.FadeIn:
                FadeIn(m_duration);
                break;
            case AnimatedUICoreStruct.AnimationType.SlideFromTop:
                SlideFromTop();
                break;
            case AnimatedUICoreStruct.AnimationType.SlideFromBottom:
                break;
            case AnimatedUICoreStruct.AnimationType.SlideFromLeft:
                SlideFromLeft();
                break;
            case AnimatedUICoreStruct.AnimationType.SlideFromRight:
                break;
        }
    }

    public void SlideFromTop()
    {
        switch (m_PivotType)
        {
            case AnimatedUICoreStruct.PivotType.UpperLeft:
                break;
            case AnimatedUICoreStruct.PivotType.UpperCenter:
                Move(new Vector2(0f, 0f + originSizeDelta.y / 2), new Vector2(0, -originSizeDelta.y/2), m_duration);
                break;
            case AnimatedUICoreStruct.PivotType.UpperRight:
                break;

            default:
                break;
        }
    }

    public void SlideFromLeft()
    {
        switch (m_PivotType)
        {
            case AnimatedUICoreStruct.PivotType.UpperLeft:
                break;
            case AnimatedUICoreStruct.PivotType.MiddleLeft:
                Move(new Vector2(-originSizeDelta.x / 2, originPosition.y), new Vector2(originSizeDelta.x / 2, originPosition.y), m_duration);
                break;
            case AnimatedUICoreStruct.PivotType.LowerLeft:
                break;

            default:
                break;
        }
    }

    public void Hide()
    {
        switch (m_PivotType)
        {
            case AnimatedUICoreStruct.PivotType.UpperLeft:
                break;
            case AnimatedUICoreStruct.PivotType.UpperCenter:
                Move(new Vector2(originPosition.x, originPosition.y - originSizeDelta.y / 2), new Vector2(originPosition.x, originPosition.y + originSizeDelta.y / 2), m_duration);
                break;
            case AnimatedUICoreStruct.PivotType.UpperRight:
                break;
            case AnimatedUICoreStruct.PivotType.MiddleLeft:
                break;
            case AnimatedUICoreStruct.PivotType.MiddleCenter:
                break;
            case AnimatedUICoreStruct.PivotType.MiddleRight:
                break;
            case AnimatedUICoreStruct.PivotType.LowerLeft:
                break;
            case AnimatedUICoreStruct.PivotType.LowerCenter:
                break;
            case AnimatedUICoreStruct.PivotType.LowerRight:
                break;

            default:
                break;
        }
    }

    public void Toggle()
    {
        switch (m_ShowAnimationType)
        {
            case AnimatedUICoreStruct.AnimationType.None:
                break;
            case AnimatedUICoreStruct.AnimationType.FadeIn:
                FadeOut(m_duration);
                break;
            case AnimatedUICoreStruct.AnimationType.SlideFromTop:
                if (isShow)
                {
                    isShow = false;
                    Move(new Vector2(0, -originSizeDelta.y / 2), new Vector2(0f, 0f + originSizeDelta.y / 2), m_duration);
                }
                else
                {
                    isShow = true;
                    Move(new Vector2(0f, 0f + originSizeDelta.y / 2), new Vector2(0, -originSizeDelta.y / 2), m_duration);
                }
                break;
            case AnimatedUICoreStruct.AnimationType.SlideFromBottom:
                break;
            case AnimatedUICoreStruct.AnimationType.SlideFromLeft:
                if (isShow)
                {
                    isShow = false;
                    Move(new Vector2(originSizeDelta.x / 2, originPosition.y), new Vector2(-originSizeDelta.x / 2, originPosition.y), m_duration);
                }
                else
                {
                    isShow = true;
                    Move(new Vector2(-originSizeDelta.x / 2, originPosition.y), new Vector2(originSizeDelta.x / 2, originPosition.y), m_duration);
                }
                break;
            case AnimatedUICoreStruct.AnimationType.SlideFromRight:
                break;
        }
    }
}
