using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoHandler : MonoBehaviour
{
    public RawImage mScreen = null;
    public VideoPlayer mVideoPlayer = null;
    public Text TitleText;

    private void Start()
    {
        TitleText.text = "프린팅과정(1배속)";

        if (mVideoPlayer != null)
        {
            string filename = null;
            switch (SceneManager.GetActiveScene().name)
            {
                case "KnowledgeCH1":
                    filename = "재료분사프린터_프린팅과정";
                    break;
                case "KnowledgeCH2":
                    filename = "광중합프린터_프린팅과정";
                    break;
                case "KnowledgeCH3":
                    filename = "분말적층용융프린터_프린팅과정";
                    break;
            }
            mVideoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, filename + ".mp4");
        }
    }

    void OnEnable()
    {
        if (mScreen != null && mVideoPlayer != null)
        {
            // 비디오 준비 코루틴 호출
            StartCoroutine(PrepareVideo());
        }
    }

    protected IEnumerator PrepareVideo()
    {
        // 비디오 준비
        mVideoPlayer.Prepare();

        // 비디오가 준비되는 것을 기다림
        while (!mVideoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(0.5f);
        }

        // VideoPlayer의 출력 texture를 RawImage의 texture로 설정한다
        mScreen.texture = mVideoPlayer.texture;

        PlayVideo();
    }

    public void PlayVideo()
    {
        if (mVideoPlayer != null && mVideoPlayer.isPrepared)
        {
            // 비디오 재생
            mVideoPlayer.Play();
        }
    }

    public void StopVideo()
    {
        if (mVideoPlayer != null && mVideoPlayer.isPrepared)
        {
            // 비디오 멈춤
            mVideoPlayer.Stop();
        }
    }

    public void FasterVideo()
    {
        if (mVideoPlayer.playbackSpeed >= 3.0f)
            return;
        
        mVideoPlayer.playbackSpeed += 1.0f;

        TitleText.text = SetTitleText((int)mVideoPlayer.playbackSpeed);
    }

    public void SlowerVideo()
    {
        if (mVideoPlayer.playbackSpeed <= 1.0f)
            return;

        mVideoPlayer.playbackSpeed -= 1.0f;

        TitleText.text = SetTitleText((int)mVideoPlayer.playbackSpeed);
    }

    public void ResetSpeed()
    {
        mVideoPlayer.playbackSpeed = 1f;

        TitleText.text = "프린팅과정(1배속)";
    }

    public string SetTitleText(int speed)
    {
        string returnValue = "프린팅과정";
        switch (speed)
        {
            case 1:
                returnValue = "프린팅과정(1배속)";
                break;
            case 2:
                returnValue = "프린팅과정(2배속)";
                break;
            case 3:
                returnValue = "프린팅과정(3배속)";
                break;
            default:                
                break;
        }

        return returnValue;
    }
}