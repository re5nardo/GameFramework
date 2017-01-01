using UnityEngine;
using System.Collections;

public class MisterBaeTeleportBehavior : IBehavior
{
    public enum State
    {
        Charge,
        Start,
    }

    private ICharacter      m_Character = null;
    private State           m_State = State.Charge;
    private Vector3         m_vec3Start = Vector3.zero;
    private Vector3         m_vec3Dest = Vector3.zero;

    public MisterBaeTeleportBehavior(ICharacter Character, State state, Vector3 vec3Start, Vector3 vec3Dest) : base(Character)
    {
        m_Character = Character;
        m_State = state;
        m_vec3Start = vec3Start;
        m_vec3Dest = vec3Dest;
    }

    protected override IEnumerator Body()
    {
        if (m_State == State.Charge)
        {
            m_SubCoroutine = m_Performer.StartCoroutine(TeleportCharge());
            yield return m_SubCoroutine;

            m_SubCoroutine = m_Performer.StartCoroutine(TeleportStart());
            yield return m_SubCoroutine;
        }
        else if (m_State == State.Start)
        {
            m_SubCoroutine = m_Performer.StartCoroutine(TeleportStart());
            yield return m_SubCoroutine;
        }
    }

    private IEnumerator TeleportCharge()
    {
        float fClipLength = m_Character.m_CharacterUI.GetAnimationClipLegth("HANDUP00_R");
        float fElapsedTime = 0f;
        float fChargeTime = 1f;
        bool bSendPacket = false;

        while (m_State == State.Charge)
        {
            m_Character.m_CharacterUI.SampleAnimation("HANDUP00_R", (fElapsedTime % fClipLength) / fClipLength);

            if (fElapsedTime >= fChargeTime && !bSendPacket)
            {
                GameEventTeleportToR reqToR = new GameEventTeleportToR();
                reqToR.m_nPlayerIndex = IGameRoom.Instance.GetPlayerIndex();
                reqToR.m_vec3Start = m_vec3Start;
                reqToR.m_vec3Dest = m_vec3Dest;
                reqToR.m_nState = (int)State.Start;

                RoomNetwork.Instance.Send(reqToR);

                bSendPacket = true;
            }

            yield return null;

            fElapsedTime += Time.deltaTime;
        }
    }

    private IEnumerator TeleportStart()
    {
        m_Character.SetPosition(new Vector3(m_vec3Dest.x, m_vec3Dest.y, m_vec3Dest.z));

        yield break;
    }

    public void SetState(State state)
    {
        m_State = state;
    }
}