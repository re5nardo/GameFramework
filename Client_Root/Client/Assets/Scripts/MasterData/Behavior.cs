using System.Collections.Generic;

namespace MasterData
{
    public class Behavior : IMasterData
    {
        public string  m_strName;
        public string  m_strClassName;
        public float   m_fLength;
        public string  m_strStringParams;
        public string  m_strAnimationName;

        public override void SetData(List<string> data)
        {
            Util.Convert(data[0], ref m_nID);
            m_strName = data[1];
            m_strClassName = data[2];
            Util.Convert(data[3], ref m_fLength);
            m_strStringParams = data[4];
            m_strAnimationName = data[5];
        }
    }
}