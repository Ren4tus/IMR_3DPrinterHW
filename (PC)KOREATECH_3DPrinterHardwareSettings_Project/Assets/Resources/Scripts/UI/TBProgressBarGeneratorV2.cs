/*
 * Topbar에 맞게 프로그래스 바를 생성합니다.
 * 사용법 : Topbar 오브젝트 차일드에 빈 오브젝트를 생성하고 이 스크립트를 붙여주면 됩니다. (pivot이 center여야 합니다)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TBProgressBarGeneratorV2 : MonoBehaviour
{
    [Header("단계 정보가 표시될 오브젝트")]
    public GameObject stepInfoObject;

    [Header("프로그레스바 프리팹")]
    public GameObject progressBarPrefab;

    [Header("총 단계 수")]
    public int sequenceCount = 9;

    [Header("레이아웃")]
    public float padding = 10.0f;
    public float spacing = 5.0f;
    public float height = 6.0f;
    public float fixedPosY = -24.0f;
    public Sprite stepOn;

    private float topbarWidth;

    [HideInInspector]
    public GameObject[] progressSteps;
    public string[] stepNames;

    public void Awake()
    {
        GenerateProgressBar();

        for (int i=0; i<sequenceCount; i++)
        {
            SetSequenceName(i, stepNames[i]);
        }

        EventSetting();
    }

    public void EventSetting()
    {
        for (int i = 0; i < sequenceCount; i++)
        {
            int tempIndex = i;
            EventTrigger.Entry entryClick = new EventTrigger.Entry();
            entryClick = new EventTrigger.Entry();
            entryClick.eventID = EventTriggerType.PointerDown;
            entryClick.callback.AddListener((eventData) => MainController.instance.GoJumpStep(tempIndex));

            progressSteps[i].GetComponent<EventTrigger>().triggers.Add(entryClick);
        }
    }

    public void SetSequenceName(int sequence, string name)
    {
        progressSteps[sequence].GetComponent<ProgressBarContainer>().SetString(name);
    }
    public void GenerateProgressBar()
    {
        if (sequenceCount > 0)
        {
            float topbarWidth = transform.GetComponent<RectTransform>().rect.width - padding * 2 - (spacing * sequenceCount);
            float stepbarWidth = topbarWidth / sequenceCount;
            float pos = 0.0f;
            RectTransform rt;

            progressSteps = new GameObject[sequenceCount];

            for (int i = 0; i < sequenceCount; i++)
            {
                progressSteps[i] = (GameObject)Instantiate(progressBarPrefab);

                // 좌표 초기화
                rt = progressSteps[i].GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(stepbarWidth, height);

                progressSteps[i].transform.SetParent(this.transform);
                progressSteps[i].transform.localScale = Vector3.one;

                if (sequenceCount % 2 == 0)
                {
                    pos = (-1 * (sequenceCount / 2 - i) * (stepbarWidth + spacing)) + (stepbarWidth + spacing) / 2;
                }
                else
                {
                    pos = (-1 * (sequenceCount / 2 - i) * (stepbarWidth + spacing));
                }

                progressSteps[i].transform.localPosition = new Vector3(pos, fixedPosY, 0);
            }
        }
        else
        {
            return;
        }

        progressSteps[0].GetComponent<Image>().sprite = stepOn;
    }
}
