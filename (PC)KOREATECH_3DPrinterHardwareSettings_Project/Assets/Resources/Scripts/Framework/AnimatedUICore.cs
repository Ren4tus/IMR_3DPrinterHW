using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimatedUICore
{
    public class AnimatedUICoreStruct
    {
        public enum PivotType
        {
            UpperLeft = 0,
            UpperCenter,
            UpperRight,
            MiddleLeft,
            MiddleCenter,
            MiddleRight,
            LowerLeft,
            LowerCenter,
            LowerRight
        }

        public enum AnimationType
        {
            None = 0,
            SlideFromTop,
            SlideFromBottom,
            SlideFromLeft,
            SlideFromRight,
            FadeIn,
            FadeOut
        }
    }
}
