using System.Collections.Generic;

namespace MasterData
{
    public class Item : IMasterData
    {
        public string m_strName;
        public string m_strClassName;
        public List<int> m_listBehaviorID = new List<int>();
        public float m_fSize;
        public string m_strModelResName;

        public override void SetData(List<string> data)
        {
            Util.Convert(data[0], ref m_nID);
            m_strName = data[1];
            m_strClassName = data[2];
            Util.Parse(data[3], ',', m_listBehaviorID);
            Util.Convert(data[4], ref m_fSize);
            m_strModelResName = data[5];
        }
    }
}