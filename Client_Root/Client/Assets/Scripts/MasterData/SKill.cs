using System.Collections.Generic;

namespace MasterData
{
    public class Skill : IMasterData
    {
        public string m_strName;
        public string m_strClassName;
        public List<KeyValuePair<int, float>> m_listBehavior = new List<KeyValuePair<int, float>>();
        public List<KeyValuePair<int, float>> m_listState = new List<KeyValuePair<int, float>>();
        public string m_strStringParams;
        public float m_fLength;
        public float m_fCoolTime;
        public float m_fMP;
        public string m_strResourceName;

        public override void SetData(List<string> data)
        {
            Util.Convert(data[0], ref m_nID);
            m_strName = data[1];
            m_strClassName = data[2];

            List<string> listString = new List<string>();
            List<string> listString2 = new List<string>();
            Util.Parse(data[3], ',', listString);
            foreach(string strText in listString)
            {
                Util.Parse(strText, ':', listString2);

                int nResult = 0; float fResult = 0;
                if (Util.Convert(listString2[0], ref nResult) && Util.Convert(listString2[1], ref fResult))
                {
                    m_listBehavior.Add(new KeyValuePair<int, float>(nResult, fResult));
                }
            }

            Util.Parse(data[4], ',', listString);
            foreach(string strText in listString)
            {
                Util.Parse(strText, ':', listString2);

                int nResult = 0; float fResult = 0;
                if (Util.Convert(listString2[0], ref nResult) && Util.Convert(listString2[1], ref fResult))
                {
                    m_listState.Add(new KeyValuePair<int, float>(nResult, fResult));
                }
            }

            m_strStringParams = data[5];
            Util.Convert(data[6], ref m_fLength);
            Util.Convert(data[7], ref m_fCoolTime);
            Util.Convert(data[8], ref m_fMP);
            m_strResourceName = data[9];
        }
    }
}