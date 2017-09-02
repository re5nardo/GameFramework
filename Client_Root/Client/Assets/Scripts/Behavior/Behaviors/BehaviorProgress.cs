
namespace Behavior
{
    public class BehaviorProgress : IBehavior
    {
        private int m_nBehaviorID;

        public void Initialize(Entity entity, int nBehaviorID, float fTime)
        {
            m_Entity = entity;
            m_nBehaviorID = nBehaviorID;

            entity.SetBehaviorTime(nBehaviorID, fTime);
        }

        public override bool Update()
        {
            m_Entity.IncreaseBehaviorTime(m_nBehaviorID, BaeGameRoom.deltaTime);

            return true;
        }

        public int GetBehaviorID()
        {
            return m_nBehaviorID;
        }
    }
}