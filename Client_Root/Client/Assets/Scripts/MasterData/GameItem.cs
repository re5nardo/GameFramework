using System.Collections.Generic;

namespace MasterData
{
    public class GameItem : IMasterData
    {
        public string m_strName;
        public string m_strClassName;
        public string m_strModelResName;
        public global::GameItem.Type m_Type;
        public float m_fLifespan;

        public override void SetData(List<string> data)
        {
            Util.Convert(data[0], ref m_nID);
            m_strName = data[1];
            m_strClassName = data[2];
            Util.Convert(data[3], ref m_Type);
            m_strModelResName = data[4];
            Util.Convert(data[5], ref m_fLifespan);
        }
    }
}