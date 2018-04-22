using System.Collections.Generic;

namespace MasterData
{
    public class Magic : IMasterData
    {
        public struct Action
        {
            public string m_strID;
            public float m_fTime;
            public List<string> m_listParams;
        };

        public string m_strName;
        public string m_strClassName;
        public float m_fLength;
        public string m_strModelResName;
        public List<MasterData.Magic.Action> m_listAction = new List<Action>();
        public global::Magic.TargetType m_TargetType;

        public override void SetData(List<string> data)
        {
            Util.Convert(data[0], ref m_nID);
            m_strName = data[1];
            m_strClassName = data[2];
            Util.Convert(data[3], ref m_fLength);
            m_strModelResName = data[4];

            if (data[5] != "")
            {
                List<string> listString = new List<string>();
                List<string> listString2 = new List<string>();
                Util.Parse(data[5], ',', listString);
                foreach(string str in listString)
                {
                    Util.Parse(str, ':', listString2);

                    Action action;
                    action.m_strID = listString2[0];
                    action.m_fTime = float.Parse(listString2[1]);
                    action.m_listParams = new List<string>();

                    for (int i = 2; i < listString2.Count; ++i)
                    {
                        action.m_listParams.Add(listString2[i]);
                    }

                    m_listAction.Add(action);
                }
            }

            Util.Convert(data[6], ref m_TargetType);
        }
    }
}