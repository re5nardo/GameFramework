using UnityEngine;

namespace Behavior
{
    public class Position : IBehavior
    {
        private GameEvent.Position m_GameEvent;

        public void Initialize(Entity entity, GameEvent.Position gameEvent)
        {
            m_Entity = entity;
            m_GameEvent = gameEvent;

            m_Entity.SetPosition(m_GameEvent.m_vec3StartPosition);
        }

        public override bool Update()
        {
            if (m_GameEvent.m_fEndTime > BaeGameRoom.Instance.GetElapsedTime())
            {
                float t = 1 - (m_GameEvent.m_fEndTime - BaeGameRoom.Instance.GetElapsedTime()) / (m_GameEvent.m_fEndTime - m_GameEvent.m_fStartTime);

                m_Entity.SetPosition(Vector3.Lerp(m_GameEvent.m_vec3StartPosition, m_GameEvent.m_vec3EndPosition, t));

                return true;
            }
            else
            {
                m_Entity.SetPosition(m_GameEvent.m_vec3EndPosition);

                return false;
            }
        }
    }
}