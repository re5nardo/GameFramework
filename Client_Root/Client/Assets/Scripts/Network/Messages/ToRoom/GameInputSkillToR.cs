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

    public ushort GetID()
    {
        return MESSAGE_ID;
    }

    public IMessage Clone()
    {
        return null; 
    }

    public byte[] Serialize()
    {
        FlatBufferBuilder builder = new FlatBufferBuilder(1024);

        GameInputSkillToR_Data.StartVector3sVector(builder, m_listVector3.Count);
        foreach(Vector3 vec3 in m_listVector3)
        {
            FBSData.Vector3.CreateVector3(builder, vec3.x, vec3.y, vec3.z);
        }
        var vector3s = builder.EndVector();
        var ints = GameInputSkillToR_Data.CreateIntsVector(builder, m_listInt.ToArray());
        var floats = GameInputSkillToR_Data.CreateFloatsVector(builder, m_listFloat.ToArray());

        GameInputSkillToR_Data.StartGameInputSkillToR_Data(builder);
        GameInputSkillToR_Data.AddPlayerIndex(builder, m_nPlayerIndex);
        GameInputSkillToR_Data.AddSkillID(builder, m_nSkillID);
        GameInputSkillToR_Data.AddVector3s(builder, vector3s);
        GameInputSkillToR_Data.AddInts(builder, ints);
        GameInputSkillToR_Data.AddFloats(builder, floats);
        var data = GameInputSkillToR_Data.EndGameInputSkillToR_Data(builder);

        builder.Finish(data.Value);

        return builder.SizedByteArray();
    }

    public bool Deserialize(byte[] bytes)
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

        return true;
    }
}
