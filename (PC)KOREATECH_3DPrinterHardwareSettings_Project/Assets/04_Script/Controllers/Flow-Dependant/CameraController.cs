using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public MainController mainController;
    public SequenceGroupData sequenceGroupData;

    public static CameraController instance;

    [Space]
    public Transform cam;
    public CameraControll cameraControll;
    public Transform dummyForMove;
    public ViewStructure_new2 viewStructure;
    //public Transform[] camPositionBySeq; // 각 시퀀스에서 카메라가 있어야 할 위치.
    private IEnumerator move;
    private bool IsCamMove;

    float MoveDistForward = 0.0f;
    float MoveDistSide = 0.0f;
    bool CanMoveCam = false;

    // 카메라 이동 끝난 후 camMoveDelay동안 대기 후 애니메이션 재생
    public bool CamMoveComplete;
    private float camMoveDelay = 1f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Start()
    {
        IsCamMove = false;
        CamMoveComplete = false;
    }

    public void setCameraByTransform(Transform prev, Transform next)
    {
        // 이전 위치에서 다음 위치로 카메라를 이동시킨다.
        if (IsCamMove)
            StopCoroutine(move);

        move = MoveToTarget(prev, next);
        StartCoroutine(move);
    }

    public void setCameraBySeq(int prev, int next)
    {
        // 이전 시퀀스 카메라 위치에서 다음 시퀀스 카메라 위치로 카메라를 이동시킨다.
        // 메인 컨트롤러 임시 구현을 위한 임시 메소드
        if (IsCamMove)
            StopCoroutine(move);

        move = MoveToTarget(sequenceGroupData.AllDataBySeq[prev].transform, sequenceGroupData.AllDataBySeq[next].transform);
        StartCoroutine(move);
    }

    public void setCameraByObj(Transform prev, Transform obj, Vector3 pos)
    {
        // 위치에서 오브젝트를 바라보도록 카메라 위치를 변경
        if (IsCamMove)
            StopCoroutine(move);

        Transform next = dummyForMove.transform; // 추후 수정
        next.position = obj.position + pos;
        next.LookAt(obj);
        move = MoveToTarget(prev, next);
        StartCoroutine(move);
    }

    public void setCameraByObj(Transform prev, Transform obj, Vector3 pos, Vector3 rot)
    {
        if (IsCamMove)
            StopCoroutine(move);

        Transform next = dummyForMove.transform; // 추후 수정
        next.position = obj.position + pos;
        Vector3 tempRot = next.rotation.eulerAngles + rot;
        next.rotation = Quaternion.Euler(tempRot);
        move = MoveToTarget(prev, next);
        StartCoroutine(move);
    }

    public void skipCamMoveBySeq(int next)
    {
        if (IsCamMove)
            StopCoroutine(move);
        
        if (viewStructure != null)
            viewStructure.InitializeTransformBySeqButton();

        cam.position = sequenceGroupData.AllDataBySeq[next].transform.position;
        cam.rotation = sequenceGroupData.AllDataBySeq[next].transform.rotation;

        IsCamMove = false;
    }

    public IEnumerator MoveToTarget(Transform prev, Transform next)
    {
        float startTime = Time.time;
        float normalizsedTime = 0f;

        Vector3 startPos = prev.position;
        Quaternion startRot = prev.rotation;

        float sinVal = 0f;
        float halfPI = Mathf.PI * 0.6f;

        float time = 2f;
        //Vector3.Distance(cam.position, camTransforms[idx].position) * 0.2f + 0.1f;

        IsCamMove = true;
        CamMoveComplete = true;
        while (normalizsedTime < 0.8f)
        {
            normalizsedTime = (Time.time - startTime) / time;

            sinVal = Mathf.Sin(normalizsedTime * halfPI);

            cam.position = Vector3.Lerp(startPos, next.position, sinVal);
            cam.rotation = Quaternion.Lerp(startRot, next.rotation, sinVal);

            yield return null;
        }

        cam.position = next.position;
        cam.rotation = next.rotation;

        IsCamMove = false;

        float delay = 0f;
        while(delay >= camMoveDelay)
        {
            delay += Time.deltaTime;
        }
        
        if (viewStructure != null)
            viewStructure.InitializeTransformBySeqButton();

        CamMoveComplete = false;
    }

    public IEnumerator MoveToTargetWithCamRot(Transform prev, Transform next, int index)
    {
        float startTime = Time.time;
        float normalizsedTime = 0f;

        Vector3 startPos = prev.position;
        Quaternion startRot = prev.rotation;

        float sinVal = 0f;
        float halfPI = Mathf.PI * 0.6f;

        float time = 2f;
        //Vector3.Distance(cam.position, camTransforms[idx].position) * 0.2f + 0.1f;

        IsCamMove = true;
        while (normalizsedTime < 0.8f)
        {
            normalizsedTime = (Time.time - startTime) / time;

            sinVal = Mathf.Sin(normalizsedTime * halfPI);

            cam.position = Vector3.Lerp(startPos, next.position, sinVal);
            cam.rotation = Quaternion.Lerp(startRot, next.rotation, sinVal);

            yield return null;
        }

        if (cameraControll != null)
            cameraControll.SetRotation(index);

        cam.position = next.position;
        cam.rotation = next.rotation;
        
        if (viewStructure != null)
            viewStructure.InitializeTransformBySeqButton();
        
        IsCamMove = false;
    }

    public void setCameraOnlyByNext(int next)
    {
        // 현재 위치에서 다음 시퀀스의 지정 위치로 이동하기 위한 카메라 이동 메소드
        if (mainController.popupController != null)
            mainController.popupController.popupPanel.SetActive(false);

        if (IsCamMove)
            StopCoroutine(move);        

        // 시퀀스가 변경되므로 카메라 이동 거리를 초기화
        MoveDistForward = 0.0f;
        MoveDistSide = 0.0f;
        CanMoveCam = false;

        // Camera Controll에 누적된 Rotation 정보 초기화
        
        if (cameraControll != null)
            move = MoveToTargetWithCamRot(cam.transform, sequenceGroupData.AllDataBySeq[next].transform, next);
        else
            move = MoveToTarget(cam.transform, sequenceGroupData.AllDataBySeq[next].transform);
        StartCoroutine(move);
    }

    public bool getIsCamMove()
    {
        return IsCamMove;
    }
}
