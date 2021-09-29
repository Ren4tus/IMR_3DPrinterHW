using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControlByMouse : MonoBehaviour
{
    [Serializable]
    public struct DesignatedLocation
    {
        public Vector3 position;
        public Vector3 rotation;
    }

    public float ZoomSpeed = 2f;
    public float RotationSpeed = 2f;

    private float lookSensitivity = -1;
    private float cameraRotationLimit = 75f; //최대 카메라 회전각
    private float currentCameraRotationX = 0;
    private float currentCameraRotationY = 0;
    private IEnumerator _cameraMoveCoroutine = null;
    private IEnumerator _mouseCoroutine = null;

    private Vector3 tempPosition;
    private Quaternion tempRotation;

    public Camera _camera;

    [Header("미리 지정된 카메라 이동 위치들")]
    [SerializeField]
    public DesignatedLocation[] DesignatedPosition;

    [Header("이동 최소/최대")]
    public float MaxX;
    public float MinX;
    public float MaxY;
    public float MinY;
    public float MaxZ;
    public float MinZ;

    private Vector3 MovingArea;
    
    void Start()
    {
        //마우스 커서 숨기기
        /*
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        */
        CaptureCurrentPosition();

        currentCameraRotationX = _camera.transform.localEulerAngles.x;
        currentCameraRotationY = _camera.transform.localEulerAngles.y;
    }

    public void CameraControllOn()
    {
        StopAllCoroutines();
        _mouseCoroutine = MouseMove_Co();
        StartCoroutine(_mouseCoroutine);
    }
    public void CameraControllOff()
    {
        if (_mouseCoroutine != null)
            StopCoroutine(_mouseCoroutine);
    }

    private IEnumerator MouseMove_Co()
    {
        //LoadCapturedPosition();

        while (true)
        {
            CameraZoom();
            CameraRotation();
            yield return null;
        }
    }

    public void SetCurrentPosition()
    {
        currentCameraRotationX = _camera.transform.rotation.eulerAngles.x;
        currentCameraRotationY = _camera.transform.rotation.eulerAngles.y;
    }
    public void CaptureCurrentPosition()
    {
        tempPosition = transform.position;
        tempRotation = transform.rotation;
    }
    public void LoadCapturedPosition()
    {
        transform.position = tempPosition;
        transform.rotation = tempRotation;
    }

    public void CameraZoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel");        

        // if max min 머시기 해서 return;
        

        if (distance != 0)
        {
            // 카메라가 바라보고 있는 방향을 기준으로 y축은 고정한 채 x, z축만 움직임
            Vector3 movePos = _camera.transform.forward;
            //movePos.y = 0;

            transform.Translate(movePos * distance * ZoomSpeed);

            MovingArea.x = Mathf.Clamp(transform.localPosition.x, MinX, MaxX);
            MovingArea.y = Mathf.Clamp(transform.localPosition.y, MinY, MaxY);
            MovingArea.z = Mathf.Clamp(transform.localPosition.z, MinZ, MaxZ);

            transform.localPosition = MovingArea;
        }
    }

    public void CameraRotation()
    {
        if (Input.GetMouseButton(0))
        {
            if (IsPointerOverUIObject()) // UI 레이어 클릭 시 움직이지 않도록 함
                return;

            currentCameraRotationY += Input.GetAxisRaw("Mouse X") * lookSensitivity * RotationSpeed;

            currentCameraRotationX += Input.GetAxisRaw("Mouse Y") * lookSensitivity * RotationSpeed;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            _camera.transform.rotation = Quaternion.Euler(new Vector3(currentCameraRotationX, -currentCameraRotationY, 0f));
        }
    }

    public void CameraMoveStart(Vector3 targetPosition, Vector3 TargetRotation, float moveSpeed = 1.0f, float rotateSpeed = 1.0f)
    {
        _cameraMoveCoroutine = CameraTransform_Co(targetPosition, TargetRotation, moveSpeed, rotateSpeed);
        StartCoroutine(_cameraMoveCoroutine);
    }
    public void CameraMoveStop(Vector3 targetPosition, Vector3 TargetRotation, float moveSpeed = 1.0f, float rotateSpeed = 1.0f)
    {
        if (_cameraMoveCoroutine != null)
            StopCoroutine(_cameraMoveCoroutine);
    }

    public bool CameraMove(Vector3 targetPosition, float moveSpeed = 1.0f)
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

        if (Vector3.Distance(targetPosition, transform.position) <= 0.001f)
            return true;

        return false;
    }
    public bool CameraRotate(Vector3 TargetRotation, float rotateSpeed = 1.0f)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(TargetRotation), Time.deltaTime * rotateSpeed);

        if (Quaternion.Angle(Quaternion.Euler(TargetRotation), transform.rotation) <= 0.01f)
            return true;

        return false;
    }

    IEnumerator CameraTransform_Co(DesignatedLocation targetlocation, float moveSpeed = 1.0f, float rotateSpeed = 1.0f)
    {
        CameraControllOff();

        while (true)
        {
            yield return new WaitForFixedUpdate();
            bool bMoveComplete = CameraMove(targetlocation.position, moveSpeed);
            bool bRotateComplete = CameraRotate(targetlocation.rotation, rotateSpeed);

            if (bMoveComplete && bRotateComplete)
                break;
        }

        CameraControllOn();
    }
    IEnumerator CameraTransform_Co(Vector3 targetPosition, Vector3 targetRotation, float moveSpeed = 1.0f, float rotateSpeed = 1.0f)
    {
        CameraControllOff();

        while (true)
        {
            yield return new WaitForFixedUpdate();
            bool bMoveComplete = CameraMove(targetPosition, moveSpeed);
            bool bRotateComplete = CameraRotate(targetRotation, rotateSpeed);

            if (bMoveComplete && bRotateComplete)
                break;
        }

        CameraControllOn();
    }

    // 오브젝트를 클릭할때 UI 레이어를 함께 클릭했는지 검사하는 함수
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.layer.Equals(LayerMask.NameToLayer("UI")))
                return true;
        }

        return false;
    }
}
