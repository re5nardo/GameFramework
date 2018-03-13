using FlatBuffers;
using System.Collections.Generic;

public class GameResultToR : IMessage
{
    public const ushort MESSAGE_ID = MessageID.GameResultToR_ID;

    public List<PlayerRankInfo> m_listPlayerRankInfo = new List<PlayerRankInfo>();

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
        FBS.GameResultToR.StartPlayerRanksVector(m_Builder, m_listPlayerRankInfo.Count);
        foreach(PlayerRankInfo playerRankInfo in m_listPlayerRankInfo)
        {
            FBS.Data.PlayerRankInfo.CreatePlayerRankInfo(m_Builder, playerRankInfo.m_nPlayerIndex, playerRankInfo.m_nRank, playerRankInfo.m_fHeight);
        }
        var playerRanks = m_Builder.EndVector();

        FBS.GameResultToR.StartGameResultToR(m_Builder);
        FBS.GameResultToR.AddPlayerRanks(m_Builder, playerRanks);
        var data = FBS.GameResultToR.EndGameResultToR(m_Builder);

        m_Builder.Finish(data.Value);

        return m_Builder.SizedByteArray();
    }

    public override bool Deserialize(byte[] bytes)
    {
        var buf = new ByteBuffer(bytes);

        var data = FBS.GameResultToR.GetRootAsGameResultToR(buf);

        for (int i = 0; i < data.PlayerRanksLength; ++i)
        {
            m_listPlayerRankInfo.Add(new PlayerRankInfo(data.GetPlayerRanks(i)));
        }

        return true;
    }

    public override void OnReturned()
    {
        m_listPlayerRankInfo.Clear();
    }
}