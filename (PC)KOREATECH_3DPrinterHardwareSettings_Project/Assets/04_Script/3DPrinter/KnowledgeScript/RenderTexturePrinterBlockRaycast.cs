using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HighlightingSystem;
using System.Linq;

public class RenderTexturePrinterBlockRaycast : MonoBehaviour
{
    [System.Serializable]
    public struct CastTarget
    {
        public GameObject targetObject;
        public Highlighter highlighter;
    }

    public KnowledgeTabController tabController;
    
    public Camera UICamera;
    public RectTransform rawImageRectTrans;
    public Camera renderToTextureCamera;

    public KnowledgeHelpMeController helpMeController;
    public CastTarget[] targets;

    private IEnumerable<CastTarget> castResult;
    private string lastTargetTag = "Untagged";
    private bool isLastTarget3DObject = false;

    private void OnEnable()
    {
        SelectInitialize();
    }

    public void SelectInitialize()
    {
        isLastTarget3DObject = false;
        lastTargetTag = "Untagged"; // 초기화
    }

    public void HighlightOffAll()
    {
        foreach (CastTarget item in targets)
        {
            item.highlighter.Off();
        }
    }

    private void FixedUpdate()
    {
        if (tabController.IsStart)
        {
            // RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImageRectTrans, Input.mousePosition, UICamera, out localPoint);
            Vector2 normalizedPoint = new Vector2(Input.mousePosition.x - rawImageRectTrans.position.x, Input.mousePosition.y - rawImageRectTrans.position.y) / rawImageRectTrans.rect.size;
            normalizedPoint += new Vector2(0.5f, 0.5f);
            var renderRay = renderToTextureCamera.ViewportPointToRay(normalizedPoint);
            
            if (Physics.Raycast(renderRay, out var raycastHit))
            {
                if (!raycastHit.collider.gameObject.CompareTag(lastTargetTag))
                {
                    Debug.Log("Hit : " + raycastHit.collider.gameObject.tag);
                    castResult = targets.Where(itr => itr.targetObject.CompareTag(raycastHit.collider.gameObject.tag));

                    foreach (CastTarget item in targets)
                    {
                        item.highlighter.Off();
                    }

                    if (castResult.Count() > 0)
                    {
                        isLastTarget3DObject = true;
                        lastTargetTag = raycastHit.collider.gameObject.tag;

                        foreach (CastTarget item in castResult)
                        {
                            item.highlighter.ConstantOn();
                        }

                        helpMeController.SetContentsByTag(raycastHit.collider.gameObject.tag);
                        helpMeController.Show();
                    }
                }
            }
            else
            {
                if (isLastTarget3DObject)
                {
                    SelectInitialize();
                    helpMeController.Hide();

                    foreach (CastTarget item in targets)
                    {
                        item.highlighter.Off();
                    }
                }
            }
        }
    }
}
