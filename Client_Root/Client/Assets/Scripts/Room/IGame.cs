using UnityEngine;
using System.Collections.Generic;

public abstract class IGame : MonoBehaviour
{
    [SerializeField] protected MapManager           m_MapManager = null;
    [SerializeField] protected InputManager         m_InputManager = null;
    [SerializeField] protected Camera               m_CameraMain = null;
    [SerializeField] protected CameraController     m_CameraController;

    protected Dictionary<long, ICharacter>          m_dicCharacter = new Dictionary<long, ICharacter>();

    public abstract void OnRecvMessage(IMessage msg);
}
