using System.Collections.Generic;

namespace MasterData
{
    public class Character : IMasterData
    {
        public string m_strName;
        public string m_strClassName;
        public Dictionary<int,int> m_dicGameItemEffect = new Dictionary<int,int>();
        public List<int> m_listBehaviorID = new List<int>();
        public int m_nHP;
        public int m_nMP;
        public float m_fMaximumSpeed;
        public float m_fMPChargeRate;
        public int m_nJumpCount;
        public float m_fJumpRegenerationTime;
        public float m_fSize;
        public string m_strModelResName;

        public override void SetData(List<string> data)
        {
            Util.Convert(data[0], ref m_nID);
            m_strName = data[1];
            m_strClassName = data[2];

            List<string> listTemp = new List<string>();
            Util.Parse(data[3], ',', listTemp);
            foreach(string text in listTemp)
            {
                List<string> listTemp2 = new List<string>();
                Util.Parse(text, ':', listTemp2);

                int nGameItemID = 0;
                int nGameItemEffectID = 0;

                Util.Convert(listTemp2[0], ref nGameItemID);
                Util.Convert(listTemp2[1], ref nGameItemEffectID);

                m_dicGameItemEffect.Add(nGameItemID, nGameItemEffectID);
            }

            Util.Parse(data[4], ',', m_listBehaviorID);
            Util.Convert(data[5], ref m_nHP);
            Util.Convert(data[6], ref m_nMP);
            Util.Convert(data[7], ref m_fMaximumSpeed);
            Util.Convert(data[8], ref m_fMPChargeRate);
            Util.Convert(data[9], ref m_nJumpCount);
            Util.Convert(data[10], ref m_fJumpRegenerationTime);
            Util.Convert(data[11], ref m_fSize);
            m_strModelResName = data[12];
        }
    }
}