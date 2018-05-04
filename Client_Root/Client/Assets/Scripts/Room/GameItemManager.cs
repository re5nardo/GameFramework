using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItemManager : IMonoTickUpdatable
{
    private int m_nGameItemSequence = 0;
    private int m_nSpawnInterval = 120;
    private int m_nLastSpawnTick = 0;

    private List<GameItem> m_listGameItem = new List<GameItem>();

    protected override void UpdateBody(int nUpdateTick)
    {
        if (m_nLastSpawnTick + m_nSpawnInterval == nUpdateTick)
        {
            SpawnGameItem(nUpdateTick);

            m_nLastSpawnTick = nUpdateTick;
        }

        //  Notice m_listGameItem can be modified during iterating
        for (int i = m_listGameItem.Count - 1; i >= 0; --i)
        {
            m_listGameItem[i].UpdateTick(nUpdateTick);
        }
    }

    private void SpawnGameItem(int nTick)
    {
        GameItem gameItem = Factory.Instance.CreateGameItem();
        m_listGameItem.Add(gameItem);

        float fTop = 0, fBottom = 0;
        BaeGameRoom2.Instance.GetPlayersHeight(ref fTop, ref fBottom);

        Vector3 vec3Start = new Vector3(Random.Range(0, 100) % 2 == 0 ? 45 : -45, Random.Range(fBottom, fTop), 0);
        Vector3 vec3End = new Vector3(-vec3Start.x, vec3Start.y, vec3Start.z);

        gameItem.Initialize(this, m_nGameItemSequence++, Random.Range(MasterDataDefine.GameItem.FirstID, MasterDataDefine.GameItem.LastID + 1), BaeGameRoom2.Instance.GetTickInterval(), vec3Start, vec3End, Random.Range(3, 10));
        gameItem.StartTick(nTick);
    }

    public void OnGameItemEnd(GameItem gameItem)
    {
        m_listGameItem.Remove(gameItem);
    }

	public void Save()
	{
		foreach(GameItem gameItem in m_listGameItem)
        {
			gameItem.Save();
        }
	}

	public void Restore()
	{
		foreach(GameItem gameItem in m_listGameItem)
        {
			gameItem.Restore();
        }
	}
}