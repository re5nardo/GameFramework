using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterDataManager : MonoSingleton<MasterDataManager>
{
    private Dictionary<string, Dictionary<int, IMasterData>> m_dicData = new Dictionary<string, Dictionary<int, IMasterData>>();

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void DownloadMasterData(string strUrl)
    {
        //  Temp.. load local files
        SetCharacter();
        SetSkill();
        SetBehavior();
        SetState();
        SetProjectile();
    }

    private void SetCharacter()
    {
        string strTypeName = typeof(MasterData.Character).Name;
        m_dicData[strTypeName] = new Dictionary<int, IMasterData>();

        List<List<string>> listData = Util.ReadCSV("MasterData/Character");
        for (int row = 1; row < listData.Count; ++row)
        {
            MasterData.Character character = new MasterData.Character();
            character.SetData(listData[row]);

            m_dicData[strTypeName][character.m_nID] = character;
        }
    }

    private void SetSkill()
    {
        string strTypeName = typeof(MasterData.Skill).Name;
        m_dicData[strTypeName] = new Dictionary<int, IMasterData>();

        List<List<string>> listData = Util.ReadCSV("MasterData/Skill");
        for (int row = 1; row < listData.Count; ++row)
        {
            MasterData.Skill skill = new MasterData.Skill();
            skill.SetData(listData[row]);

            m_dicData[strTypeName][skill.m_nID] = skill;
        }
    }

    private void SetBehavior()
    {
        string strTypeName = typeof(MasterData.Behavior).Name;
        m_dicData[strTypeName] = new Dictionary<int, IMasterData>();

        List<List<string>> listData = Util.ReadCSV("MasterData/Behavior");
        for (int row = 1; row < listData.Count; ++row)
        {
            MasterData.Behavior behavior = new MasterData.Behavior();
            behavior.SetData(listData[row]);

            m_dicData[strTypeName][behavior.m_nID] = behavior;
        }
    }

    private void SetState()
    {
        string strTypeName = typeof(MasterData.State).Name;
        m_dicData[strTypeName] = new Dictionary<int, IMasterData>();

        List<List<string>> listData = Util.ReadCSV("MasterData/State");
        for (int row = 1; row < listData.Count; ++row)
        {
            MasterData.State state = new MasterData.State();
            state.SetData(listData[row]);

            m_dicData[strTypeName][state.m_nID] = state;
        }
    }

    private void SetProjectile()
    {
        string strTypeName = typeof(MasterData.Projectile).Name;
        m_dicData[strTypeName] = new Dictionary<int, IMasterData>();

        List<List<string>> listData = Util.ReadCSV("MasterData/Projectile");
        for (int row = 1; row < listData.Count; ++row)
        {
            MasterData.Projectile projectile = new MasterData.Projectile();
            projectile.SetData(listData[row]);

            m_dicData[strTypeName][projectile.m_nID] = projectile;
        }
    }

    public bool GetData<T>(int nID, ref T data) where T : IMasterData
    {
        string strTypeName = typeof(T).Name;

        if (m_dicData.ContainsKey(strTypeName))
        {
            if (m_dicData[strTypeName].ContainsKey(nID))
            {
                data = (T)m_dicData[strTypeName][nID];

                return true;
            }
        }

        return false;
    }
}
