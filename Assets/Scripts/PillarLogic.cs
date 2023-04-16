
using Mirror;
using UnityEngine;

public class PillarLogic : NetworkBehaviour
{
    MeshRenderer m_meshRenderer;

    [SyncVar]
    Color m_color;

    private void Awake()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        if(m_meshRenderer)
        {
            m_color = m_meshRenderer.material.color;
        }
    }
    
    void Start()
    {
        
    }

    public override void OnStartClient()
    {
        SetColor(m_color);
    }

    [ClientRpc]
    public void RpcSetColor(Color color)
    {
        m_color = color;
        SetColor(m_color);
    }

    void SetColor(Color color)
    {
        m_meshRenderer.material.color = color;
    }
}
