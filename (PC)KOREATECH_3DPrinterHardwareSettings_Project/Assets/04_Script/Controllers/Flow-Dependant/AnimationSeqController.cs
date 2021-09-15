using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationSeqController : MonoBehaviour
{
    public MainController mainController;

    [Serializable]
    public struct AnimSeq
    {
        public string name;
        public int index;
        public Animation animation;
        public AnimationClip clip;
    };

    public AnimSeq[] animationSeqs;

    bool animPlaying = false;

    public void ClearAnimPlaying()
    {
        animPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        int seq = mainController.GetCurrentSeq();

        if (!animPlaying)
        {
            for (int i = 0; i < animationSeqs.Length; i++)
            {
                Animation partSeqAnim = animationSeqs[i].animation;
                AnimationClip partSeqClip = animationSeqs[i].clip;

                if (seq < animationSeqs[i].index) // 작을 경우
                {
                    partSeqAnim[partSeqClip.name].normalizedTime = 0.0f;
                    partSeqAnim[partSeqClip.name].speed = 0;
                    partSeqAnim.Play(partSeqClip.name);
                }
                else if (seq == animationSeqs[i].index) // 동일할 경우
                {
                    partSeqAnim[partSeqClip.name].normalizedTime = 0.0f;
                    partSeqAnim[partSeqClip.name].speed = 1;
                    partSeqAnim.Play(partSeqClip.name);
                }
                else /* (seq > animationSeqs[i]) */ // 클 경우
                {
                    partSeqAnim[partSeqClip.name].normalizedTime = 1.0f;
                    partSeqAnim[partSeqClip.name].speed = 0;
                    partSeqAnim.Play(partSeqClip.name);
                }
            }

            animPlaying = true;
        }
    }
}
