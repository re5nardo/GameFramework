using System.Collections.Generic;

namespace MasterData
{
    public class GameItemEffect : IMasterData
    {
        public enum Type
        {
            Behavior,
        }

        public Type m_Type;
        public int m_nTargetID;

        public override void SetData(List<string> data)
        {
            Util.Convert(data[0], ref m_nID);
            Util.Convert(data[1], ref m_Type);
            Util.Convert(data[2], ref m_nTargetID);
        }
    }
}