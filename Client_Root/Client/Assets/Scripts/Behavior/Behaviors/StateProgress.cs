
namespace Behavior
{
    public class StateProgress : IBehavior
    {
        private int m_nStateID;

        public void Initialize(Entity entity, int nStateID, float fTime)
        {
            m_Entity = entity;
            m_nStateID = nStateID;

            entity.SetStateTime(nStateID, fTime);
        }

        public override bool Update()
        {
            m_Entity.IncreaseStateTime(m_nStateID, BaeGameRoom.deltaTime);

            return true;
        }

        public int GetStateID()
        {
            return m_nStateID;
        }
    }
}