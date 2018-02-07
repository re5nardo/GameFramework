using UnityEngine;

namespace Behavior
{
    public class Rotation : IBehavior
    {
        private Vector3 m_vec3Start;
        private Vector3 m_vec3Target;

        public override void Initialize(IEntity entity, int nMasterDataID, float fTickInterval)
        {
            m_Entity = entity;
            m_nMasterDataID = nMasterDataID;
            m_fTickInterval = fTickInterval;
        }

        public override void StartTick(int nStartTick, params object[] param)
        {
            base.StartTick(nStartTick, param);

            m_vec3Start = m_Entity.GetRotation();
            m_vec3Target = (Vector3)param[0];
        }

        float fRotSpeed = 0.1f;
        protected override void UpdateBody(int nUpdateTick)
        {
//            float fTime = (nUpdateTick - m_nStartTick + 1) * m_fTickInterval;
//            float fValue = fTime / fRotSpeed;

            m_Entity.SetRotation(m_vec3Target);
            Stop();

//            m_Entity.SetRotation(Quaternion.Lerp(Quaternion.Euler(m_vec3Start), Quaternion.Euler(m_vec3Target), fValue).eulerAngles);

//            if (fValue >= 1)
//            {
//                Stop();
//            }
        }
    }
}