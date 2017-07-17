using UnityEngine;
using FlatBuffers;
using System.Collections.Generic;

public class GameInputSkillToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameInputSkillToR_ID;

    public enum InputType
    {
        Click = 0,
        Press,
        Release,
    }

    public int m_nPlayerIndex;
    public int m_nSkillID;
    public List<Vector3> m_listVector3 = new List<Vector3>();
    public List<int> m_listInt = new List<int>();
    public List<float> m_listFloat = new List<float>();
    public InputType m_InputType;

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
        GameInputSkillToR_Data.StartVector3sVector(m_Builder, m_listVector3.Count);
        foreach(Vector3 vec3 in m_listVector3)
        {
            FBSData.Vector3.CreateVector3(m_Builder, vec3.x, vec3.y, vec3.z);
        }
        var vector3s = m_Builder.EndVector();
        var ints = GameInputSkillToR_Data.CreateIntsVector(m_Builder, m_listInt.ToArray());
        var floats = GameInputSkillToR_Data.CreateFloatsVector(m_Builder, m_listFloat.ToArray());

        GameInputSkillToR_Data.StartGameInputSkillToR_Data(m_Builder);
        GameInputSkillToR_Data.AddPlayerIndex(m_Builder, m_nPlayerIndex);
        GameInputSkillToR_Data.AddSkillID(m_Builder, m_nSkillID);
        GameInputSkillToR_Data.AddVector3s(m_Builder, vector3s);
        GameInputSkillToR_Data.AddInts(m_Builder, ints);
        GameInputSkillToR_Data.AddFloats(m_Builder, floats);
        GameInputSkillToR_Data.AddInputType(m_Builder, (int)m_InputType);
        var data = GameInputSkillToR_Data.EndGameInputSkillToR_Data(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = GameInputSkillToR_Data.GetRootAsGameInputSkillToR_Data(buf);

        m_nPlayerIndex = data.PlayerIndex;
        m_nSkillID = data.SkillID;
        for (int i = 0; i < data.Vector3sLength; ++i)
        {
            FBSData.Vector3 vec3 = data.GetVector3s(i);
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
        m_InputType = (InputType)data.InputType;

        return true;
    }
}
