/*
 * Topbar에 맞게 프로그래스 바를 생성합니다.
 * 사용법 : Topbar 오브젝트 차일드에 빈 오브젝트를 생성하고 이 스크립트를 붙여주면 됩니다. (pivot이 center여야 합니다)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TBProgressBarGenerator : MonoBehaviour
{
    [Header("씬 시작 시 자동으로 프로그레스바 생성 여부")]
    public bool isAutoGenerateTopBar = false;

    [Header("단계 정보가 표시될 오브젝트")]
    public GameObject stepInfoObject;

    [Header("이미지가 설정된 프로그레스바 프리팹")]
    public GameObject progressBarPrefab;

    public int sequenceCount = 9;

    [Header("레이아웃")]
    public float padding = 10.0f;
    public float spacing = 5.0f;
    public float height = 6.0f;
    public float fixedPosY = -24.0f;

    private float topbarWidth;

    [HideInInspector]
    public GameObject[] progressSteps;
    public string[] stepNames;

    private void Start()
    {
        if (isAutoGenerateTopBar)
            GenerateProgressBar(sequenceCount);
    }

    public void SetSequenceCount(int count)
    {
        sequenceCount = count;
    }

    public void GenerateProgressBar(int progressCount, string[] titles)
    {
        if (progressCount > 0)
        {
            float topbarWidth = this.transform.parent.GetComponent<RectTransform>().rect.width - padding * 2 - (spacing * progressCount);
            float stepbarWidth = topbarWidth / progressCount;
            float pos = 0.0f;
            RectTransform rt;

            progressSteps = new GameObject[progressCount];

            for (int i = 0; i < progressCount; i++)
            {
                progressSteps[i] = (GameObject)Instantiate(progressBarPrefab);
                progressSteps[i].transform.SetParent(this.transform);
                progressSteps[i].transform.localScale = Vector3.one; // 추가

                // 좌표 초기화
                rt = progressSteps[i].GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(stepbarWidth, height);

                if (sequenceCount % 2 == 0)
                    pos = (-1 * (sequenceCount / 2 - i) * (stepbarWidth + spacing)) + (stepbarWidth + spacing) / 2;
                else
                    pos = (-1 * (sequenceCount / 2 - i) * (stepbarWidth + spacing));

                progressSteps[i].transform.localPosition = new Vector3(pos, fixedPosY, 0);
                progressSteps[i].GetComponent<ProgressBarContainer>().SetString(titles[i+1]);
            }
        }
    }

    public void GenerateProgressBar(int progressCount)
    {
        if (progressCount > 0)
        {
            float topbarWidth = this.transform.parent.GetComponent<RectTransform>().rect.width - padding * 2 - (spacing * progressCount);
            float stepbarWidth = topbarWidth / progressCount;
            float pos = 0.0f;
            RectTransform rt;

            progressSteps = new GameObject[progressCount];

            for (int i = 0; i < progressCount; i++)
            {
                progressSteps[i] = (GameObject)Instantiate(progressBarPrefab);
                progressSteps[i].transform.SetParent(this.transform);
                progressSteps[i].transform.localScale = new Vector3(1f, 1f, 1f); // 추가

                // 좌표 초기화
                rt = progressSteps[i].GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(stepbarWidth, height);

                if (sequenceCount % 2 == 0)
                    pos = (-1 * (sequenceCount / 2 - i) * (stepbarWidth + spacing)) + (stepbarWidth + spacing) / 2;
                else
                    pos = (-1 * (sequenceCount / 2 - i) * (stepbarWidth + spacing));

                progressSteps[i].transform.localPosition = new Vector3(pos, fixedPosY, 0);
                progressSteps[i].GetComponent<ProgressBarContainer>().SetString(stepNames[i]);
            }
        }
    }
}
