using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Highlighter))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(MeshRenderer))]
public class PPSlicerObjectController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private bool isInitialized = false;

    private bool isCollocated = false;
    public bool IsCollocated => isCollocated;
    public string rotateAxis;
    public Camera camera;

    private bool bSlicerMoveMode = false;
    private bool bSlicerRotateMode = false;
    private Highlighter highlighter = null;
    private Collider collider = null;
    private MeshRenderer meshRenderer = null;

    private Vector3 InitPosition;
    private Quaternion InitRotation;

    public Collider trayRange;
    public Material whenCorrectPos;
    public Material whenIncorrectPos;

    public int rotateCount = 0;

    public void Init()
    {
        if (!isInitialized)
        {
            collider = GetComponent<Collider>();
            highlighter = GetComponent<Highlighter>();
            meshRenderer = GetComponent<MeshRenderer>();

            // 초기 위치 저장
            InitPosition = transform.position;
            InitRotation = transform.rotation;

            isInitialized = true;
        }
    }

    public void SetCamera(Camera cam)
    {
        camera = cam;
    }

    public void EditModeExit()
    {
        bSlicerMoveMode = false;
        bSlicerRotateMode = false;
    }
    public void SetMoveMode()
    {
        bSlicerMoveMode = true;
        bSlicerRotateMode = false;
    }
    public void SetRotateMode()
    {
        bSlicerMoveMode = false;
        bSlicerRotateMode = true;
    }

    public void ResetPosition()
    {
        transform.position = InitPosition;
        transform.rotation = InitRotation;
    }

    public Collider GetCollider()
    {
        return collider;
    }

    public void CollocatedOff()
    {
        isCollocated = false;
        meshRenderer.material = whenIncorrectPos;
    }
    public void CollocatedOn()
    {
        isCollocated = true;
        meshRenderer.material = whenCorrectPos;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!bSlicerMoveMode && !bSlicerRotateMode)
            return;

        highlighter.ConstantOn();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!bSlicerMoveMode && !bSlicerRotateMode)
            return;

        highlighter.Off();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!bSlicerRotateMode)
            return;

        rotateCount++;
        rotateCount = rotateCount % 2;

        // 기준 회전 축
        if (rotateAxis.Equals("x") || rotateAxis.Equals("X"))
        {
            transform.Rotate(new Vector3(transform.rotation.x + 90f, transform.rotation.y, transform.rotation.z));
            return;
        }

        if (rotateAxis.Equals("y") || rotateAxis.Equals("Y"))
        {
            transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y + 90f, transform.rotation.z));
            return;
        }

        transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + 90f));
    }

    /*
    IEnumerator OnMouseDown()
    {
        if (bSlicerMoveMode)
        {
            Vector3 scrSpace = camera.WorldToScreenPoint(transform.position);
            Vector3 offset = transform.position - camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, scrSpace.z));

            while (Input.GetMouseButton(0))
            {
                Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, scrSpace.z);
                Vector3 curPosition = camera.ScreenToWorldPoint(curScreenSpace) + offset;
                transform.position = curPosition;
                yield return null;
            }
        }
    }
    */
}
