/*
 * 테마 컬러를 담고있는 클래스
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPresetData
{
    // 테마 컬러
    private static Color32 themeColor = new Color32(247, 150, 71, 255);
    private static Color32 themeColorHighlight = new Color32(255, 255, 255, 255);

    // 텍스트 컬러
    private static Color32 textColorInActive = new Color32(187, 187, 187, 255);

    public static Color32 ThemeColor => themeColor;
    public static Color32 ThemeColorHighlight => themeColor;
    public static Color32 TextColorInActive => textColorInActive;
}
