using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Entity : PooledComponent
{
    protected EntityUI    m_EntityUI = null;
    private Vector3       m_vec3Position = Vector3.zero;
    private Vector3       m_vec3Rotation = Vector3.zero;

    private Dictionary<int, float> m_dicBehavior = new Dictionary<int, float>();
    private Dictionary<int, float> m_dicState = new Dictionary<int, float>();
    private LinkedList<IBehavior> m_listBehavior = new LinkedList<IBehavior>();

    private IEnumerator BehaviorLoop()
    {
        LinkedList<IBehavior> toRemove = new LinkedList<IBehavior>();

        while (true)
        {
            foreach (IBehavior behavior in m_listBehavior)
            {
                if (!behavior.Update())
                {
                    toRemove.AddLast(behavior);
                }
            }

            foreach (IBehavior behavior in toRemove)
            {
                m_listBehavior.Remove(behavior);

                ObjectPool.Instance.ReturnObject(behavior);
            }
            toRemove.Clear();

            yield return null;
        }
    }


    public virtual void Initialize(FBS.Data.EntityType entityType, int nID, int nMasterDataID)
    {
        GameObject goEntityUI = ObjectPool.Instance.GetGameObject("CharacterModel/EntityUI");
        EntityUI entityUI = goEntityUI.GetComponent<EntityUI>();
        entityUI.Initialize(entityType, nID, nMasterDataID);

        m_EntityUI = entityUI;

        StartCoroutine("BehaviorLoop");
    }

    public void SampleBehaviors(Dictionary<string, KeyValuePair<float, float>> dicBehaviors, float fInterpolationValue, float fTickInterval, float fEmptyValue)
    {
        foreach(KeyValuePair<string, KeyValuePair<float, float>> behavior in dicBehaviors)
        {
            float fTime = 0f;
            float fWeight = 0f;
            if (behavior.Value.Key != fEmptyValue && behavior.Value.Value != fEmptyValue)
            {
                fTime = Mathf.Lerp(behavior.Value.Key, behavior.Value.Value, fInterpolationValue);
                fWeight = 1f;
            }
            else if(behavior.Value.Key == fEmptyValue)
            {
                fTime = Mathf.Lerp(0f, behavior.Value.Value, fInterpolationValue);
                fWeight = fInterpolationValue;
            }
            else if(behavior.Value.Value == fEmptyValue)
            {
                fTime = Mathf.Lerp(behavior.Value.Key, behavior.Value.Key + fTickInterval, fInterpolationValue);
                fWeight = 1f - fInterpolationValue;
            }

            MasterData.Behavior behaviorMasterData = null;
            MasterDataManager.Instance.GetData<MasterData.Behavior>(int.Parse(behavior.Key), ref behaviorMasterData);

            m_EntityUI.SampleAnimation(behaviorMasterData.m_strAnimationName, fTime % 1f, fWeight);
        }
    }

    public void SetPosition(Vector3 vec3Position)
    {
        m_vec3Position = vec3Position;

        m_EntityUI.SetPosition(vec3Position);
    }

    public Vector3 GetPosition()
    {
        return m_vec3Position;
    }

    public void SetRotation(Vector3 vec3Rotation)
    {
        m_vec3Rotation = vec3Rotation;

        m_EntityUI.SetRotation(vec3Rotation);
    }

    public Vector3 GetRotation()
    {
        return m_vec3Rotation;
    }

    public Transform GetUITransform()
    {
        return m_EntityUI.transform;
    }

    public virtual void ProcessGameEvent(IGameEvent iGameEvent)
    {
        if (iGameEvent.GetEventType() == FBS.GameEventType.BehaviorStart)
        {
            GameEvent.BehaviorStart gameEvent = (GameEvent.BehaviorStart)iGameEvent;

            ProcessBehaviorStart(gameEvent);
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.BehaviorEnd)
        {
            GameEvent.BehaviorEnd gameEvent = (GameEvent.BehaviorEnd)iGameEvent;

            ProcessBehaviorEnd(gameEvent);
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.StateStart)
        {
            GameEvent.StateStart gameEvent = (GameEvent.StateStart)iGameEvent;

            ProcessStateStart(gameEvent);
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.StateEnd)
        {
            GameEvent.StateEnd gameEvent = (GameEvent.StateEnd)iGameEvent;

            ProcessStateEnd(gameEvent);
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.Position)
        {
            GameEvent.Position gameEvent = (GameEvent.Position)iGameEvent;

            ProcessPosition(gameEvent);
        }
        else if (iGameEvent.GetEventType() == FBS.GameEventType.Rotation)
        {
            GameEvent.Rotation gameEvent = (GameEvent.Rotation)iGameEvent;

            ProcessRotation(gameEvent);
        }
    }

    private void ProcessBehaviorStart(GameEvent.BehaviorStart gameEvent)
    {
        float fTime = BaeGameRoom.Instance.GetElapsedTime() - gameEvent.m_fStartTime;

        Behavior.BehaviorProgress behavior = ObjectPool.Instance.GetObject<Behavior.BehaviorProgress>();
        behavior.Initialize(this, gameEvent.m_nBehaviorID, fTime);

        m_listBehavior.AddLast(behavior);
    }

    private void ProcessBehaviorEnd(GameEvent.BehaviorEnd gameEvent)
    {
        m_dicBehavior.Remove(gameEvent.m_nBehaviorID);

        IBehavior target = null;
        foreach(IBehavior behavior in m_listBehavior)
        {
            if (behavior is Behavior.BehaviorProgress && ((Behavior.BehaviorProgress)behavior).GetBehaviorID() == gameEvent.m_nBehaviorID)
            {
                target = behavior;
            }
        }

        m_listBehavior.Remove(target);

        ObjectPool.Instance.ReturnObject(target);
    }

    private void ProcessStateStart(GameEvent.StateStart gameEvent)
    {
        float fTime = BaeGameRoom.Instance.GetElapsedTime() - gameEvent.m_fStartTime;

        Behavior.StateProgress behavior = ObjectPool.Instance.GetObject<Behavior.StateProgress>();
        behavior.Initialize(this, gameEvent.m_nStateID, fTime);

        m_listBehavior.AddLast(behavior);
    }

    private void ProcessStateEnd(GameEvent.StateEnd gameEvent)
    {
        m_dicState.Remove(gameEvent.m_nStateID);

        IBehavior target = null;
        foreach(IBehavior behavior in m_listBehavior)
        {
            if (behavior is Behavior.StateProgress && ((Behavior.StateProgress)behavior).GetStateID() == gameEvent.m_nStateID)
            {
                target = behavior;
            }
        }

        m_listBehavior.Remove(target);

        ObjectPool.Instance.ReturnObject(target);
    }

    private void ProcessPosition(GameEvent.Position gameEvent)
    {
        IBehavior target = null;
        foreach(IBehavior behavior in m_listBehavior)
        {
            if (behavior is Behavior.Position)
            {
                target = behavior;
            }
        }

        if (target == null)
        {
            Behavior.Position behavior = ObjectPool.Instance.GetObject<Behavior.Position>();
            behavior.Initialize(this, gameEvent);

            m_listBehavior.AddLast(behavior);
        }
        else
        {
            ((Behavior.Position)target).Initialize(this, gameEvent);
        }

        // 기존 Behavior를 재사용하면 순서가 꼬이는데... 수정하자..
    }

    private void ProcessRotation(GameEvent.Rotation gameEvent)
    {
        IBehavior target = null;
        foreach(IBehavior behavior in m_listBehavior)
        {
            if (behavior is Behavior.Rotation)
            {
                target = behavior;
            }
        }

        if (target == null)
        {
            Behavior.Rotation behavior = ObjectPool.Instance.GetObject<Behavior.Rotation>();
            behavior.Initialize(this, gameEvent);

            m_listBehavior.AddLast(behavior);
        }
        else
        {
            ((Behavior.Rotation)target).Initialize(this, gameEvent);
        }
    }

    public void Sample()
    {
        m_EntityUI.Sample(m_dicBehavior);
    }

    public void Clear()
    {
        if (m_EntityUI != null)
        {
            ObjectPool.Instance.ReturnGameObject(m_EntityUI.gameObject);
            m_EntityUI = null;
        }

        m_vec3Position = Vector3.zero;
        m_vec3Rotation = Vector3.zero;

        m_dicBehavior.Clear();

        StopCoroutine("BehaviorLoop");
        foreach (IBehavior behavior in m_listBehavior)
        {
            ObjectPool.Instance.ReturnObject(behavior);
        }
        m_listBehavior.Clear();
    }

    public void SetBehaviorTime(int nBehaviorID, float fTime)
    {
        m_dicBehavior[nBehaviorID] = fTime;
    }

    public void IncreaseBehaviorTime(int nBehaviorID, float fTime)
    {
        m_dicBehavior[nBehaviorID] += fTime;
    }

    public void SetStateTime(int nStateID, float fTime)
    {
        m_dicState[nStateID] = fTime;
    }

    public void IncreaseStateTime(int nStateID, float fTime)
    {
        m_dicState[nStateID] += fTime;
    }

    public override void OnReturned()
    {
        Clear();
    }
}