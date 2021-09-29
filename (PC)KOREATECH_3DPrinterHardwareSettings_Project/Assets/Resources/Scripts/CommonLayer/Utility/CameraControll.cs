using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControll : MonoBehaviour
{
    public Quaternion TargetRotation;  // 최종적으로 축적된 Gap이 이 변수에 저장됨.
    public Transform CameraVector;

    public float MoveSpeed = 100.0f;
    public float RotationSpeed;        // 회전 스피드.
    public Vector3 TranslationSpeed = Vector3.one;        // 이동 스피드.
    public float ZoomSpeed;            // 줌 스피드.
    public float Distance;             // 카메라와의 거리.

    public Vector3[] seqInitRots;

    private Vector3 AxisVec;           // 축의 벡터.
    private Vector3 GapRot;           // 회전 축적 값.
    private Vector3 GapTrans;           // 회전 축적 값.
    private Vector3 capturedPosition;
    private Quaternion capturedRotation;

    private Transform MainCamera;      // 카메라 컴포넌트.
    private bool IsMoving = false;
    private Camera thisCamera;

    void Start()
    {
        MainCamera = Camera.main.transform;
        TargetRotation = MainCamera.rotation;
        GapRot = new Vector3(20.0f, 0.0f, 0.0f);
        /*
        capturedPosition = MainCamera.position;
        capturedRotation = MainCamera.rotation;
        */
        thisCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if(!IsOverUI())
            moveObjectFunc();
        /*
        PositionCapture();
        if (!IsMoving)
        {
            moveObjectFunc();
        }
        else
        {
            if (transform.position != capturedPosition)
            {
                transform.position = Vector3.Slerp(transform.position, capturedPosition, Time.deltaTime);
            }
            if (transform.rotation != capturedRotation)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, capturedRotation, Time.deltaTime);
            }
            // 현재 위치에서 저장된 위치로 이동
        }*/
    }
    
    void moveObjectFunc()
    {
        ZoomT();
        CameraRotation();
        /*
        float keyH = Input.GetAxis("Horizontal");
        float keyV = Input.GetAxis("Vertical");
        keyH = keyH * MoveSpeed * Time.deltaTime;
        keyV = keyV * MoveSpeed * Time.deltaTime;
        transform.Translate(Vector3.right * keyH);
        transform.Translate(Vector3.forward * keyV);
        */
    }

    // 카메라 줌.
    void Zoom()
    {
        Distance += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * -1;
        Distance = Mathf.Clamp(Distance, 5f, 20f);

        AxisVec = transform.forward * -1;
        AxisVec *= Distance;
        MainCamera.position = transform.position + AxisVec;
    }

    void ZoomT()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        /*
        if(thisCamera.fieldOfView <= 0f && scroll < 0)
        {
            thisCamera.fieldOfView = 0f;
        }
        else if(thisCamera.fieldOfView >= 100f && scroll > 0)
        {
            thisCamera.fieldOfView = 100f;
        }
        else
        {
            thisCamera.fieldOfView += scroll;
        }
        */
        thisCamera.fieldOfView += -scroll;
    }

    // 현재 상태 저장
    void PositionCapture()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.KnowledgeTabButtonClick);

            // 회전값과 위치를 저장
            capturedPosition = transform.position;
            capturedRotation = transform.rotation;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            IsMoving = true;
            CL_CommonFunctionManager.Instance.PlayUXSound(CL_SoundManager.UXSound.TypingComplete);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            IsMoving = false;
        }
    }

    // 카메라 회전.
    void CameraRotation()
    {
        // if (transform.rotation != TargetRotation)
            // transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, RotationSpeed * Time.deltaTime);

        if (Input.GetMouseButton(0))
        {
            // 값을 축적.            
            GapRot.x -= Input.GetAxis("Mouse Y") * RotationSpeed;
            GapRot.y += Input.GetAxis("Mouse X") * RotationSpeed;

            // 카메라 회전범위 제한.
            //Gap.x = Mathf.Clamp(Gap.x, -85f, 85f);
            // 회전 값을 변수에 저장.
            // TargetRotation = Quaternion.Euler(GapRot);

            // 카메라벡터 객체에 Axis객체의 x,z회전 값을 제외한 y값만을 넘긴다.
            // Quaternion q = TargetRotation;
            //q.x = q.z = 0;
            // CameraVector.transform.rotation = q;            

            CameraVector.transform.eulerAngles = GapRot;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 multiplierVector = TranslationSpeed * CameraVector.transform.position.z;
            
            // 값을 축적.
            GapTrans.x = Input.GetAxis("Mouse X") * multiplierVector.x;
            GapTrans.y = Input.GetAxis("Mouse Y") * multiplierVector.y;
            
            // 카메라벡터 객체에 Axis객체의 x,z회전 값을 제외한 y값만을 넘긴다.
            //q.x = q.z = 0;
            CameraVector.transform.position -= GapTrans;
        }
    }

    public void InitializeRotation()
    {
        TargetRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

        GapRot = new Vector3(0.0f, 0.0f, 0.0f);
        GapTrans = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void SetRotation(int seqNum)
    {
        GapRot = seqInitRots[seqNum - 1];
        CameraVector.transform.eulerAngles = GapRot;
    }

    public bool IsOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}