using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class IEntity : MonoBehaviour
{
    protected IEntityUI             m_EntityUI = null;
    protected Vector3               m_vec3Position = Vector3.zero;
    protected Vector3               m_vec3Rotation = Vector3.zero;

    private Dictionary<int, float> dicBehavior = new Dictionary<int, float>();
    private Dictionary<int, Coroutine> dicCoroutine = new Dictionary<int, Coroutine>();

    protected abstract void CreateUI();
    public abstract void Initialize();
    public abstract void SampleBehaviors(Dictionary<string, KeyValuePair<float, float>> dicBehaviors, float fInterpolationValue, float fTickInterval, float fEmptyValue);

    protected virtual void Awake()
    {
        CreateUI();
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

    protected virtual void OnDestroy()
    {
        StopAllCoroutines();

        if (m_EntityUI != null)
        {
            Destroy(m_EntityUI.gameObject);
        }
    }

    public void ProcessGameEvent(IGameEvent iGameEvent)
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

        dicCoroutine[gameEvent.m_nBehaviorID] = StartCoroutine(BehaviorStart(gameEvent, fTime));
    }

    private IEnumerator BehaviorStart(GameEvent.BehaviorStart gameEvent, float fTime)
    {
        dicBehavior[gameEvent.m_nBehaviorID] = fTime;

        while (true)
        {
            yield return null;

            dicBehavior[gameEvent.m_nBehaviorID] += BaeGameRoom.deltaTime;
        }
    }

    private void ProcessBehaviorEnd(GameEvent.BehaviorEnd gameEvent)
    {
        StopCoroutine(dicCoroutine[gameEvent.m_nBehaviorID]);
        dicCoroutine.Remove(gameEvent.m_nBehaviorID);
        dicBehavior.Remove(gameEvent.m_nBehaviorID);
    }

    private void ProcessPosition(GameEvent.Position gameEvent)
    {
        StopCoroutine("Position");
        StartCoroutine("Position", gameEvent);
    }

    private IEnumerator Position(GameEvent.Position gameEvent)
    {
        while (gameEvent.m_fEndTime > BaeGameRoom.Instance.GetElapsedTime())
        {
            float t = 1 - (gameEvent.m_fEndTime - BaeGameRoom.Instance.GetElapsedTime()) / (gameEvent.m_fEndTime - gameEvent.m_fStartTime);

            SetPosition(Vector3.Lerp(gameEvent.m_vec3StartPosition, gameEvent.m_vec3EndPosition, t));

            yield return null;
        }

        SetPosition(gameEvent.m_vec3EndPosition);
    }

    private void ProcessRotation(GameEvent.Rotation gameEvent)
    {
        StopCoroutine("Rotation");
        StartCoroutine("Rotation", gameEvent);
    }

    private IEnumerator Rotation(GameEvent.Rotation gameEvent)
    {
        while (gameEvent.m_fEndTime > BaeGameRoom.Instance.GetElapsedTime())
        {
            float t = 1 - (gameEvent.m_fEndTime - BaeGameRoom.Instance.GetElapsedTime()) / (gameEvent.m_fEndTime - gameEvent.m_fStartTime);

            Vector3 vec3PrevRotation = gameEvent.m_vec3StartRotation;
            Vector3 vec3NextRotation = gameEvent.m_vec3EndRotation;

            if (vec3NextRotation.y < vec3PrevRotation.y)
            {
                vec3NextRotation.y += 360;
            }

            //  For lerp calculation
            //  Clockwise rotation
            if (vec3NextRotation.y - vec3PrevRotation.y <= 180)
            {
                if (vec3PrevRotation.y > vec3NextRotation.y)
                    vec3NextRotation.y += 360;
            }
            //  CounterClockwise rotation
            else
            {
                if (vec3PrevRotation.y < vec3NextRotation.y)
                    vec3PrevRotation.y += 360;
            }

            Vector3 vecRotation;
            vecRotation.x = Mathf.Lerp(vec3PrevRotation.x, vec3NextRotation.x, t);
            vecRotation.y = Mathf.Lerp(vec3PrevRotation.y, vec3NextRotation.y, t);
            vecRotation.z = Mathf.Lerp(vec3PrevRotation.z, vec3NextRotation.z, t);

            SetRotation(vecRotation);

            yield return null;
        }

        SetRotation(gameEvent.m_vec3EndRotation);
    }

    public void Sample()
    {
        m_EntityUI.Sample(dicBehavior);
    }
}
