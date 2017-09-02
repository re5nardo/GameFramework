using UnityEngine;

namespace Behavior
{
    public class Rotation : IBehavior
    {
        private GameEvent.Rotation m_GameEvent;

        public void Initialize(Entity entity, GameEvent.Rotation gameEvent)
        {
            m_Entity = entity;
            m_GameEvent = gameEvent;
        }

        public override bool Update()
        {
            if (m_GameEvent.m_fEndTime > BaeGameRoom.Instance.GetElapsedTime())
            {
                float t = 1 - (m_GameEvent.m_fEndTime - BaeGameRoom.Instance.GetElapsedTime()) / (m_GameEvent.m_fEndTime - m_GameEvent.m_fStartTime);

                Vector3 vec3PrevRotation = m_GameEvent.m_vec3StartRotation;
                Vector3 vec3NextRotation = m_GameEvent.m_vec3EndRotation;

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

                m_Entity.SetRotation(vecRotation);

                return true;
            }
            else
            {
                m_Entity.SetRotation(m_GameEvent.m_vec3EndRotation);

                return false;
            }
        }
    }
}