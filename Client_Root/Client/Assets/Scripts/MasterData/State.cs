using System.Collections.Generic;

namespace MasterData
{
    public class State : IMasterData
    {
        public string m_strName;
        public string m_strClassName;
        public float m_fLength;
        public List<string> m_listStringParam = new List<string>();
        public List<double> m_listDoubleParam1 = new List<double>();
        public List<double> m_listDoubleParam2 = new List<double>();
        public string  m_strFxName;
        public string  m_strAnimationName;

        public override void SetData(List<string> data)
        {
            Util.Convert(data[0], ref m_nID);
            m_strName = data[1];
            m_strClassName = data[2];
            Util.Convert(data[3], ref m_fLength);
            Util.Parse(data[4], ',', m_listStringParam);
            Util.Parse(data[5], ',', m_listDoubleParam1);
            Util.Parse(data[6], ',', m_listDoubleParam2);
            m_strFxName = data[7];
            m_strAnimationName = data[8];
        }
    }
}