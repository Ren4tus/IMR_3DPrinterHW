/*
 * 테마 컬러 및 기타 리소스 정보를 담고있는 클래스
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CL_UIPresetData
{
    public static Color32 ThemeColor { get; } = new Color32(105, 156, 176, 255); // 699CB0
    public static Color32 ThemeColorHighlight { get; } = new Color32(255, 255, 255, 255);
    public static Color32 TextColorInActive { get; } = new Color32(187, 187, 187, 255);

    public static Color32 ProgressBarProcessing { get; } = new Color32(47, 107, 130, 255);
    public static Color32 ProgressBarComplete { get; } = new Color32(44, 80, 98, 255);
    public static Color32 ProgressBarInactive { get; } = new Color32(82, 82, 82, 255);
    
    public static string RgbToHex(byte r, byte g, byte b)
    {
        byte[] colors = { r, g, b };
        string hex = BitConverter.ToString(colors);

        return "#" + hex.Replace("-", "");
    }
    public static string RgbToHex(Color32 color)
    {
        byte[] colors = { color.r, color.g, color.b };
        string hex = BitConverter.ToString(colors);

        return "#" + hex.Replace("-", "");
    }
}
