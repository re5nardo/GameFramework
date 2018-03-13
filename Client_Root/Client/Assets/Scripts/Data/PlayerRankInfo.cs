using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerRankInfo
{
    public PlayerRankInfo(FBS.Data.PlayerRankInfo playerRankInfo)
    {
        m_nPlayerIndex = playerRankInfo.PlayerIndex;
        m_nRank = playerRankInfo.Rank;
        m_fHeight = playerRankInfo.Height;
    }
    public PlayerRankInfo(int nPlayerIndex, int nRank, float fHeight)
    {
        m_nPlayerIndex = nPlayerIndex;
        m_nRank = nRank;
        m_fHeight = fHeight;
    }

    public int m_nPlayerIndex;
    public int m_nRank;
    public float m_fHeight;
}