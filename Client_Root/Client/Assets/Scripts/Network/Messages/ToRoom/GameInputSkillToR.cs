using UnityEngine;
using FlatBuffers;
using System.Collections.Generic;

public class GameInputSkillToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameInputSkillToR_ID;

    public int m_nPlayerIndex;
    public int m_nSkillID;
    public List<Vector3> m_listVector3 = new List<Vector3>();
    public List<int> m_listInt = new List<int>();
    public List<float> m_listFloat = new List<float>();
    public FBS.InputType m_InputType;

    public override ushort GetID()
    {
        return MESSAGE_ID;
    }

    public override IMessage Clone()
    {
        return null; 
    }

    public override byte[] Serialize()
    {
        FBS.GameInputSkillToR.StartVector3sVector(m_Builder, m_listVector3.Count);
        foreach(Vector3 vec3 in m_listVector3)
        {
            FBS.Data.Vector3.CreateVector3(m_Builder, vec3.x, vec3.y, vec3.z);
        }
        var vector3s = m_Builder.EndVector();
        var ints = FBS.GameInputSkillToR.CreateIntsVector(m_Builder, m_listInt.ToArray());
        var floats = FBS.GameInputSkillToR.CreateFloatsVector(m_Builder, m_listFloat.ToArray());

        FBS.GameInputSkillToR.StartGameInputSkillToR(m_Builder);
        FBS.GameInputSkillToR.AddPlayerIndex(m_Builder, m_nPlayerIndex);
        FBS.GameInputSkillToR.AddSkillID(m_Builder, m_nSkillID);
        FBS.GameInputSkillToR.AddVector3s(m_Builder, vector3s);
        FBS.GameInputSkillToR.AddInts(m_Builder, ints);
        FBS.GameInputSkillToR.AddFloats(m_Builder, floats);
        FBS.GameInputSkillToR.AddInput(m_Builder, m_InputType);
        var data = FBS.GameInputSkillToR.EndGameInputSkillToR(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.GameInputSkillToR.GetRootAsGameInputSkillToR(buf);

        m_nPlayerIndex = data.PlayerIndex;
        m_nSkillID = data.SkillID;
        for (int i = 0; i < data.Vector3sLength; ++i)
        {
            FBS.Data.Vector3 vec3 = data.GetVector3s(i);
            m_listVector3.Add(new Vector3(vec3.X, vec3.Y, vec3.Z));
        }
        for (int i = 0; i < data.IntsLength; ++i)
        {
            m_listInt.Add(data.GetInts(i));
        }
        for (int i = 0; i < data.FloatsLength; ++i)
        {
            m_listFloat.Add(data.GetFloats(i));
        }
        m_InputType = data.Input;

        return true;
    }

    public override void OnReturned()
    {
        m_listVector3.Clear();
        m_listInt.Clear();
        m_listFloat.Clear();
    }
}
