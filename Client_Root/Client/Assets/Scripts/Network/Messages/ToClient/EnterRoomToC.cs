using System.Collections.Generic;
using FlatBuffers;

public class EnterRoomToC : IMessage
{
    public const ushort MESSAGE_ID = MessageID.EnterRoomToC_ID;

    public int m_nResult;
    public int m_nUserPlayerIndex;
    public List<FBS.PlayerInfo> m_listPlayerInfo = new List<FBS.PlayerInfo>();

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
        FBS.EnterRoomToC.StartPlayersVector(m_Builder, m_listPlayerInfo.Count);
        foreach(FBS.PlayerInfo playerInfo in m_listPlayerInfo)
        {
            FBS.PlayerInfo.CreatePlayerInfo(m_Builder, playerInfo.PlayerIndex, playerInfo.MasterDataID, playerInfo.EntityID, playerInfo.Status.MaximumHP, playerInfo.Status.HP,
                playerInfo.Status.MaximumMP, playerInfo.Status.MP, playerInfo.Status.MaximumSpeed, playerInfo.Status.Speed, playerInfo.Status.MPChargeRate, playerInfo.Status.MovePoint);
        }
        var players = m_Builder.EndVector();

        FBS.EnterRoomToC.StartEnterRoomToC(m_Builder);
        FBS.EnterRoomToC.AddResult(m_Builder, m_nResult);
        FBS.EnterRoomToC.AddUserPlayerIndex(m_Builder, m_nUserPlayerIndex);
        FBS.EnterRoomToC.AddPlayers(m_Builder, players);
        var data = FBS.EnterRoomToC.EndEnterRoomToC(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.EnterRoomToC.GetRootAsEnterRoomToC(buf);

        m_nResult = data.Result;
        m_nUserPlayerIndex = data.UserPlayerIndex;
        for (int i = 0; i < data.PlayersLength; ++i)
        {
            m_listPlayerInfo.Add(data.GetPlayers(i));
        }

        return true;
    }

    public override void OnReturned()
    {
        m_listPlayerInfo.Clear();
    }
}
