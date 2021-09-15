using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;
using Enums;

public class MaterialController : MonoBehaviour
{
    public MainController mainController;
    public SequenceGroupData sequenceGroupData;

    [Space]
    public Highlighter[] AllHighlights;
    //public MaterialData[] materialDataBySeq;

    public void firstSceneInit()
    {
        for (int i = 0; i < AllHighlights.Length; i++)
        {
            AllHighlights[i].tween = false;
        }
    }

    public void setOnOffBySeq(int prevSeq, int nextSeq)
    {
        int i = 0;

        if (sequenceGroupData.AllDataBySeq[prevSeq].GetComponent<MaterialData>().HighlightedObj == null || 
            sequenceGroupData.AllDataBySeq[nextSeq].GetComponent<MaterialData>().HighlightedObj == null)
            return;

        GameObject[] prevObjects = sequenceGroupData.AllDataBySeq[prevSeq].GetComponent<MaterialData>().HighlightedObj;
        GameObject[] nextObjects = sequenceGroupData.AllDataBySeq[nextSeq].GetComponent<MaterialData>().HighlightedObj;

        for (i = 0; i < prevObjects.Length; i++)
        {
            if (prevObjects[i].GetComponent<Highlighter>().constant)
            {
                prevObjects[i].GetComponent<Highlighter>().constant = false;
            }
        }

        for (i = 0; i < nextObjects.Length; i++)
        {
            if (!nextObjects[i].GetComponent<Highlighter>().constant)
            { 
                nextObjects[i].GetComponent<Highlighter>().constant = true;
            }
        }

        mainController.finishedByCont(Controllers.Material);
    }

    public void showComponent(int currentSeq)
    {
        /*
        GameObject[] CurrentObjects = sequenceGroupData.AllDataBySeq[currentSeq].GetComponent<MaterialData>().HighlightedObj;
        MeshRenderer[] meshRenderers;
        for (int i = 0; i < CurrentObjects.Length; i++)
        {
            meshRenderers = CurrentObjects[i].GetComponents<MeshRenderer>();
            foreach (var x in meshRenderers)
            {
                for (int j = 0; j < x.materials.Length; j++)
                {
                    if (x.materials[j].name == "Transparent")
                        continue;

                    x.materials[j].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    x.materials[j].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    x.materials[j].SetInt("_ZWrite", 1);
                    x.materials[j].DisableKeyword("_ALPHATEST_ON");
                    x.materials[j].DisableKeyword("_ALPHABLEND_ON");
                    x.materials[j].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    x.materials[j].renderQueue = -1;
                    Color setColor = x.materials[j].color;
                    setColor.a = 1.0f;
                    x.materials[j].color = setColor;
                }
            }
        }
        */
    }

    public void hideComponent(int currentSeq)
    {
        /*
        GameObject[] CurrentObjects = sequenceGroupData.AllDataBySeq[currentSeq].GetComponent<MaterialData>().HighlightedObj;
        MeshRenderer[] meshRenderers;
        for (int i = 0; i < CurrentObjects.Length; i++)
        {
            meshRenderers = CurrentObjects[i].GetComponents<MeshRenderer>();
            foreach (var x in meshRenderers)
            {
                for (int j = 0; j < x.materials.Length; j++)
                {
                    if (x.materials[j].name == "Transparent")
                        continue;

                    x.materials[j].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    x.materials[j].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    x.materials[j].SetInt("_ZWrite", 0);
                    x.materials[j].DisableKeyword("_ALPHATEST_ON");
                    x.materials[j].EnableKeyword("_ALPHABLEND_ON");
                    x.materials[j].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    x.materials[j].renderQueue = 3000;
                    Color setColor = x.materials[j].color;
                    setColor.a = 0.0f;
                    x.materials[j].color = setColor;

                }
            }
        }
        */
    }
}
