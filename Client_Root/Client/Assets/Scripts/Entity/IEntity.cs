using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class IEntity : PooledComponent
{
    public Vector3 m_vec3Velocity = Vector3.zero;

    protected int m_nID = -1;
    protected int m_nMasterDataID = -1;
    protected int m_nDefaultBehaviorID;

    protected EntityUI m_EntityUI = null;

    protected List<IBehavior> m_listBehavior = new List<IBehavior>();
    protected List<IState> m_listState = new List<IState>();

    public abstract float GetSpeed();
    public abstract float GetMaximumSpeed();
    public abstract void Initialize(int nID, int nMasterDataID, params object[] param);
    public abstract FBS.Data.EntityType GetEntityType();
    public abstract void NotifyGameEvent(IGameEvent gameEvent);
    public abstract void LateUpdateWorld(int nUpdateTick);

    public void UpdateBehaviors(int nUpdateTick)
    {
        List<IBehavior> listBehavior = GetActivatedBehaviors();
        foreach(IBehavior behavior in listBehavior)
        {
            behavior.Update(nUpdateTick);
        }
    }

    public void UpdateStates(int nUpdateTick)
    {
        List<IState> listState = GetStates();
        foreach(IState state in listState)
        {
            state.Update(nUpdateTick);
        }
    }

    public int GetID()
    {
        return m_nID;
    }

    public int GetMasterDataID()
    {
        return m_nMasterDataID;
    }

    public void Move(Vector3 vec3Motion)
    {
        m_EntityUI.Move(vec3Motion);
    }

    public bool IsGrounded()
    {
        return m_EntityUI.IsGrounded();
    }

    public Vector3 GetPosition()
    {
        return m_EntityUI.GetPosition();
    }

    public void SetPosition(Vector3 vec3Position)
    {
        m_EntityUI.SetPosition(vec3Position);
    }

    public Vector3 GetRotation()
    {
        return m_EntityUI.GetRotation();
    }

    public void SetRotation(Vector3 vec3Rotation)
    {
        m_EntityUI.SetRotation(vec3Rotation);
    }

    public Vector3 GetForward()
    {
        return m_EntityUI.GetForward();
    }

    public IBehavior GetBehavior(int nMasterDataID)
    {
        return m_listBehavior.Find(x => x.GetMasterDataID() == nMasterDataID);
    }

    public List<IBehavior> GetAllBehaviors()
    {
        return m_listBehavior;
    }

    public List<IBehavior> GetActivatedBehaviors()
    {
        return m_listBehavior.FindAll(x => x.IsActivated());
    }

    public bool IsBehavioring()
    {
        return m_listBehavior.Exists(x => x.IsActivated());
    }

    public IState GetState(int nMasterDataID)
    {
        return m_listState.Find(x => x.GetMasterDataID() == nMasterDataID);
    }

    public List<IState> GetStates()
    {
        return m_listState;
    }

    public void AddState(IState pState, int nTick)
    {
        m_listState.Add(pState);
    }

    public void RemoveState(int nMasterDataID, int nTick)
    {
        m_listState.RemoveAll(x => x.GetMasterDataID() == nMasterDataID);
    }

    public bool HasCoreState(CoreState coreState)
    {
        return m_listState.Exists(x => x.HasCoreState(coreState));
    }

    public Transform GetModelTransform()
    {
        return m_EntityUI.GetModelTransform();
    }

    public void Draw(int nTick)
    {
        m_EntityUI.Draw(nTick);
    }

    public void Play(string strAnimation, int nStartTick, bool bLoop = false, float fWeight = 1)
    {
        m_EntityUI.Play(strAnimation, nStartTick, bLoop, fWeight);
    }

    public void Stop(string strAnimation)
    {
        m_EntityUI.Stop(strAnimation);
    }
}