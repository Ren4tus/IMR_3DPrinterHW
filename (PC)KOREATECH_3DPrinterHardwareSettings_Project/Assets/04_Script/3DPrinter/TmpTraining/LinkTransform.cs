using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkTransform : MonoBehaviour
{
    public Transform m_LinkTarget;
    public string m_ObjectPathName;
    public Vector3 m_Offset;

    public Transform LinkedTarget
    {
        get => m_LinkTarget;
        set => m_LinkTarget = value;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTransform();
    }

    void LateUpdate()
    {
        UpdateTransform();
    }

    void FixedUpdate()
    {
        UpdateTransform();
    }

    void UpdateTransform()
    {
        if (!m_LinkTarget)
        {
            GameObject g = GameObject.Find(m_ObjectPathName);
            if (g) { m_LinkTarget = g.transform; }
        }
        if (!m_LinkTarget) { return; }
        transform.position = m_LinkTarget.position;
        transform.position += m_Offset;
        if (!Application.isPlaying && m_LinkTarget)
        {
            // 다른 Scene의 Object가 Link되어 있는 경우에는 실행중이 아니면 해제 해버린다.
            if (gameObject.scene != m_LinkTarget.gameObject.scene)
            {
                m_LinkTarget = null;
            }
        }
    }

    void OnValidate()
    {
        GameObject g = GameObject.Find(m_ObjectPathName);
        if (g)
        {
            m_LinkTarget = g.transform;
            gameObject.name = g.name + " Linked";
        }
        else
        {
            m_LinkTarget = null;
        }
        UpdateTransform();
    }
}