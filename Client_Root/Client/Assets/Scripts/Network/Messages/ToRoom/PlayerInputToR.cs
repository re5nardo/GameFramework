using UnityEngine;
using FlatBuffers;
using System.Collections.Generic;

public class PlayerInputToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.PlayerInputToR_ID;

    public FBS.PlayerInputType m_Type;
    public byte[] m_Data;

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
        List<sbyte> listData = new List<sbyte>();
        foreach(byte b in m_Data)
        {
            listData.Add((sbyte)b);
        }
        var inputData = FBS.PlayerInputToR.CreateDataVector(m_Builder, listData.ToArray());

        FBS.PlayerInputToR.StartPlayerInputToR(m_Builder);
        FBS.PlayerInputToR.AddType(m_Builder, m_Type);
        FBS.PlayerInputToR.AddData(m_Builder, inputData);
        var data = FBS.PlayerInputToR.EndPlayerInputToR(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.PlayerInputToR.GetRootAsPlayerInputToR(buf);

        m_Type = data.Type;
        List<byte> listData = new List<byte>();
        for (int i = 0; i < data.DataLength; ++i)
        {
            listData.Add((byte)data.GetData(i));
        }
        m_Data = listData.ToArray();

        return true;
    }
}
