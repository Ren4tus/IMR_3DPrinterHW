using UnityEngine;
using UnityEngine.EventSystems;

public class ViewStructure_new2 : MonoBehaviour
{
    public GameObject[] targetObjects;
    public Transform[] seqTransforms;
    public int[] seq;
    public MainController MainController;
    public GameObject cam;
    Vector3 mousepos;
    public Vector3[] Centers;
    public Vector3[] initialTranslations;
    // Update is called once per frame
    Vector3 x;

    public Transform m_TargetCamera;
    [SerializeField]
    private Transform m_GhostCameraLocator;
    [SerializeField]
    private Transform m_ShootLocation;

    [Header("[회전 처리]")]
    public Vector3 m_RotationAngle;
    public Vector3 m_InitialRotationAngle;
    public Vector3 m_RotationAngleSpeed = new Vector3(5, 5, 1);
    public Vector3 m_MinRotationAngle = new Vector3(-85, -180, 0);
    public Vector3 m_MaxRotationAngle = new Vector3(85, 180, 0);

    [Header("[이동/줌]")]
    public Vector3 m_Translation;
    // public Vector3 m_InitialTranslation;
    public Vector3 m_TranslationSpeed = Vector3.one;
    public Vector3 m_TranslationSpeedFreeMode = Vector3.one;
    public Vector3 m_MinTranslation = new Vector3(-5, -5, 0.2f);
    public Vector3 m_MaxTranslation = new Vector3(5, 5, 5);

    [Header("[키조합]")]
    [Range(0, 10)]
    public float m_ShiftMultiplier = 0.2f;
    [Range(0, 10)]
    public float m_CtrlMultipplier = 5;

    [Header("[잠금]")]
    public bool m_LockAngleX;
    public bool m_LockAngleY;
    public bool m_LockTranslationX;
    public bool m_LockTranslationY;
    public bool m_LockZoom;

    [Header("[자연스럽게]- 0은 OFF")]
    [Range(0, 5)]
    public float m_TransitionTime = 0.5f;
    [Range(0, 3)]
    public float m_SmoothFollowTimeSpeed = 0.5f;
    
    private int currentObjIdx;

    [Header("ETC")]
    public GameObject[] Arrow;
    public int ArrowSeqNum;

    bool _PauseInteration = false;
    public bool PauseInteraction
    {
        get { return _PauseInteration; }
        set { _PauseInteration = value; }
    }
    Vector3 m_AlwaysRotate = Vector3.zero;
    public Vector3 AlwaysRotate
    {
        get { return m_AlwaysRotate; }
        set { m_AlwaysRotate = value; }
    }

    public bool IsAlwaysRotate
    {
        get { return m_AlwaysRotate == Vector3.zero; }
        set { if (!value) { m_AlwaysRotate = Vector3.zero; } }
    }

    public void StopAlwaysRotate() { m_AlwaysRotate = Vector3.zero; }

    void ApplyAlwaysRotate()
    {
        if (!IsAlwaysRotate) { return; }
        // apply always rotate
        m_RotationAngle += m_AlwaysRotate * Time.deltaTime;
    }

    // EDITOR COMMAND
    public bool ApplyTargetTransform()
    {
        if (!m_TargetCamera || !m_GhostCameraLocator)
        {
            Debug.LogError("카메라 정보가 올바르게 설치되지 않았습니다.");
            return true;
        }

        ClampRotationAndTranslation();
        bool r = ApplyTransformTo(m_GhostCameraLocator, m_ShootLocation.position, m_RotationAngle, m_Translation);

        DoSmoothGhostCameraFollow();
        return true;
    }

    void OnEnable()
    {
        InitializeTransform(0);
        currentObjIdx = 0;
    }

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (!m_TargetCamera || !m_GhostCameraLocator)
                return;
            if (PauseInteraction)
                return;
            ApplyAlwaysRotate();
            Vector3 pos = m_GhostCameraLocator.position;
            UpdateTransform();
            if (!ApplyTargetTransform()) { m_GhostCameraLocator.position = pos; }

        }
        mousepos = Input.mousePosition;

        if (Arrow.Length > 0)
        {
            if (MainController.GetCurrentSeq() == ArrowSeqNum)
            {
                Arrow[0].SetActive(true);
                Arrow[1].SetActive(true);
            }
            else
            {
                Arrow[0].SetActive(false);
                Arrow[1].SetActive(false);
            }
        }
    }

    public void InitializeTransformBySeqButton()
    {
        ChangeStructure(MainController.GetCurrentSeq());
    }
    
    // EDITOR COMMAND
    public void InitializeTransform(int a)
    {
        if (!m_ShootLocation || !m_GhostCameraLocator)
        {
            //m_RotationAngle = Vector3.zero;
            m_Translation = Vector3.zero;
            return;
        }

        // 초기 위치를 가져옴
        m_RotationAngle = seqTransforms[a].localEulerAngles;
        m_Translation = initialTranslations[a];
        
        ApplyTargetTransform();
    }

    public void ChangeStructure(int index)
    {
        // MainController.LoadSeq(index, true);

        transform.position = Centers[index];

        m_ShootLocation = targetObjects[index].transform;
        InitializeTransform(index);

        currentObjIdx = index;
    }
    
    // ------------------------------------------------------------------------------------------------------------
    // PRIVATE
    // ------------------------------------------------------------------------------------------------------------    
    void UpdateTransform()
    {
        float multiplier = GetKeyMulitplyFactor();
        // ROTATION - Mouse Left
        if (Input.GetMouseButton(0))
        {
            m_RotationAngle.y += multiplier * (m_LockAngleY ? 0 : Input.GetAxis("Mouse X") * m_RotationAngleSpeed.y);
            m_RotationAngle.x -= multiplier * (m_LockAngleX ? 0 : Input.GetAxis("Mouse Y") * m_RotationAngleSpeed.x);
        }

        else if (Input.GetMouseButton(2))
        {
            Vector3 multiplierVector = m_TranslationSpeed * m_Translation.z;
            multiplierVector *= multiplier;

            m_Translation.x -= (m_LockTranslationX ? 0 : Input.GetAxis("Mouse X") * multiplierVector.x);
            m_Translation.y -= (m_LockTranslationY ? 0 : Input.GetAxis("Mouse Y") * multiplierVector.y);
        }
        // ZOOM
        else
        {
            multiplier *= m_TranslationSpeed.z;
            m_Translation.z -= (m_LockZoom ? 0 : m_Translation.z * Input.GetAxis("Mouse ScrollWheel")) * multiplier;
        }
    }
    
    float GetKeyMulitplyFactor()
    {
        float multiplier = 1;
        if (Input.GetKey(KeyCode.LeftControl)) { multiplier *= m_CtrlMultipplier; }
        if (Input.GetKey(KeyCode.LeftShift)) { multiplier *= m_ShiftMultiplier; }
        return multiplier;
    }

    // 지정된 범위를 벗어나면 값을 조정한다.
    void ClampRotationAndTranslation()
    {
        m_RotationAngle = RegularAngle(m_RotationAngle);
        m_RotationAngle.x = Mathf.Clamp(m_RotationAngle.x, m_MinRotationAngle.x, m_MaxRotationAngle.x);
        m_RotationAngle.y = Mathf.Clamp(m_RotationAngle.y, m_MinRotationAngle.y, m_MaxRotationAngle.y);
        m_RotationAngle.z = 0;

        m_Translation.x = Mathf.Clamp(m_Translation.x, m_MinTranslation.x, m_MaxTranslation.x);
        m_Translation.y = Mathf.Clamp(m_Translation.y, m_MinTranslation.y, m_MaxTranslation.y);
        m_Translation.z = Mathf.Clamp(m_Translation.z, m_MinTranslation.z, m_MaxTranslation.z);
    }
    
    Vector3 RegularAngle(Vector3 angle)
    {
        // Set Value range to -180 to 180
        angle.x %= 360;
        angle.x = angle.x > 180 ? angle.x - 360 : angle.x;
        angle.x = angle.x < -180 ? angle.x + 360 : angle.x;

        angle.y %= 360;
        angle.y = angle.y > 180 ? angle.y - 360 : angle.y;
        angle.y = angle.y < -180 ? angle.y + 360 : angle.y;

        angle.z %= 360;
        angle.z = angle.z > 180 ? angle.z - 360 : angle.z;
        angle.z = angle.z < -180 ? angle.z + 360 : angle.z;
        return angle;
    }
    
    bool ApplyTransformTo(Transform target, Vector3 basePosition, Vector3 angle, Vector3 translation)
    {
        // Set to target location
        target.position = basePosition;
        // Set Angles
        target.eulerAngles = angle;
        // Set Translations

        target.position -= target.forward * translation.z;
        target.position += target.right * translation.x;
        target.position += target.up * translation.y;

        return true;
    }

    void DoSmoothGhostCameraFollow()
    {
        if (m_SmoothFollowTimeSpeed == 0 || !Application.isPlaying)
        {
            m_TargetCamera.position = m_GhostCameraLocator.position;
            m_TargetCamera.rotation = m_GhostCameraLocator.rotation;
            return;
        }
        Vector3 destAngle = m_GhostCameraLocator.eulerAngles;
        Vector3 currentAngle = m_TargetCamera.eulerAngles;
        float t = Time.deltaTime * 10 / m_SmoothFollowTimeSpeed;

        m_TargetCamera.position = Vector3.Lerp(m_TargetCamera.position, m_GhostCameraLocator.position, t);
        m_TargetCamera.eulerAngles = new Vector3(Mathf.LerpAngle(currentAngle.x, destAngle.x, t), Mathf.LerpAngle(currentAngle.y, destAngle.y, t), 0);
    }
}