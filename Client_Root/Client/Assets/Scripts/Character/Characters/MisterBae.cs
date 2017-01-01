using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MisterBae : ICharacter
{
#region ICharacter
    protected override void CreateUI()
    {
        m_CharacterUI = (Instantiate(Resources.Load("CharacterUI/MisterBaeUI")) as GameObject).GetComponent<MisterBaeUI>();
    }

    public override void Initialize(params object[] arrParam)
    {
        m_DefaultStat = m_CurrentStat = (Stat)arrParam[0];
    }

    public override void Idle()
    {
        StopAllBehaviors();

        StartBehavior(new IdleBehavior(this, "WAIT01", m_LastBehavior is IdleBehavior));
    }

    public override void Stop()
    {
        StopAllBehaviors();

        StartBehavior(new StopBehavior(this));
    }

    public override void Move(LinkedList<Node> listPath, float fEventTime, System.Action callback = null)
    {
        List<IBehavior> listBehavior = m_listBehavior.FindAll(a => a is IdleBehavior || a is MoveBehavior || a is PatrolBehavior);
        foreach(IBehavior bh in listBehavior)
        {
            bh.Stop();
        }

        StartBehavior(new MoveBehavior(this, listPath, "RUN00_F", m_LastBehavior is MoveBehavior, fEventTime), callback);
    }

    public override void GameEvent(IMessage iMsg)
    {
        if (iMsg is GameEventTeleportToC)
        {
            Teleport((GameEventTeleportToC)iMsg);
        }
    }

    public override void Emotion()
    {
        
    }
#endregion

    public void Teleport(GameEventTeleportToC data)
    {
        if (m_listBehavior.Exists(a => a is MisterBaeTeleportBehavior))
        {
            (m_listBehavior.Find(a => a is MisterBaeTeleportBehavior) as MisterBaeTeleportBehavior).SetState((MisterBaeTeleportBehavior.State)data.m_nState);
        }
        else
        {
            List<IBehavior> listBehavior = m_listBehavior.FindAll(a => a is IdleBehavior || a is MoveBehavior);
            foreach(IBehavior bh in listBehavior)
            {
                bh.Stop();
            }

            StartBehavior(new MisterBaeTeleportBehavior(this, (MisterBaeTeleportBehavior.State)data.m_nState, data.m_vec3Start, data.m_vec3Dest));
        }
    }


    public void Patrol(Vector3 vec3Start, Vector3 vec3Dest)
    {
        List<IBehavior> listBehavior = m_listBehavior.FindAll(a => a is IdleBehavior || a is MoveBehavior);
        foreach(IBehavior bh in listBehavior)
        {
            bh.Stop();
        }

        StartBehavior(new PatrolBehavior(this, vec3Start, vec3Dest, "RUN00_F"));
    }
}
