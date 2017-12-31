using FlatBuffers;
using System.Collections.Generic;
using UnityEngine;

public class TickInfoToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.TickInfoToC_ID;

    public int m_nTick;
    public List<IPlayerInput> m_listPlayerInput = new List<IPlayerInput>();

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
        return null;
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.TickInfoToC.GetRootAsTickInfoToC(buf);

        m_nTick = data.Tick;
        for (int i = 0; i < data.PlayerInputsLength; ++i)
        {
            FBS.PlayerInputData playerInputData = data.GetPlayerInputs(i);

            List<byte> listByte = new List<byte>();
            for (int j = 0; j < playerInputData.DataLength; ++j)
            {
                listByte.Add((byte)playerInputData.GetData(j));
            }

            IPlayerInput playerInput = null;
            if (playerInputData.Type == FBS.PlayerInputType.Rotation)
            {
                playerInput = new PlayerInput.Rotation();
            }
            else
            {
                UnityEngine.Debug.LogError("Invalid type!, PlayerInputType : " + playerInputData.Type.ToString());
            }

            playerInput.Deserialize(listByte.ToArray());

            m_listPlayerInput.Add(playerInput);
        }

        return true;
    }

    public override void OnReturned()
    {
        m_listPlayerInput.Clear();
    }

    public override string ToString()
    {
        string strText = "";

        strText += string.Format("[TickInfoToC] Tick : {0}\n", m_nTick);

        foreach(IPlayerInput input in m_listPlayerInput)
        {
            strText += input.ToString();
            strText += '\n';
        }

        return strText;
    }
}
