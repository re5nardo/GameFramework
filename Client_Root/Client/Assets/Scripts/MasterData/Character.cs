using System.Collections.Generic;

namespace MasterData
{
    public class Character : IMasterData
    {
        public string m_strName;
        public string m_strClassName;
        public List<int> m_listSkillID = new List<int>();
        public List<int> m_listBehaviorID = new List<int>();
        public int m_nHP;
        public int m_nMP;
        public float m_fMaximumSpeed;
        public float m_fSize;
        public string m_strModelResName;

        public override void SetData(List<string> data)
        {
            Util.Convert(data[0], ref m_nID);
            m_strName = data[1];
            m_strClassName = data[2];
            Util.Parse(data[3], ',', m_listSkillID);
            Util.Parse(data[4], ',', m_listBehaviorID);
            Util.Convert(data[5], ref m_nHP);
            Util.Convert(data[6], ref m_nMP);
            Util.Convert(data[7], ref m_fMaximumSpeed);
            Util.Convert(data[8], ref m_fSize);
            m_strModelResName = data[9];
        }
    }
}