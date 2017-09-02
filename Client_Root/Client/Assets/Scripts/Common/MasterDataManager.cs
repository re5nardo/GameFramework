using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterDataManager : MonoSingleton<MasterDataManager>
{
    private Dictionary<System.Type, Dictionary<int, IMasterData>> m_dicData = new Dictionary<System.Type, Dictionary<int, IMasterData>>();

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
        m_dicData[typeof(MasterData.Character)] = new Dictionary<int, IMasterData>();

        List<List<string>> listData = Util.ReadCSV("MasterData/Character");
        for (int row = 1; row < listData.Count; ++row)
        {
            MasterData.Character character = new MasterData.Character();
            character.SetData(listData[row]);

            m_dicData[typeof(MasterData.Character)][character.m_nID] = character;
        }
    }

    private void SetSkill()
    {
        m_dicData[typeof(MasterData.Skill)] = new Dictionary<int, IMasterData>();

        List<List<string>> listData = Util.ReadCSV("MasterData/Skill");
        for (int row = 1; row < listData.Count; ++row)
        {
            MasterData.Skill skill = new MasterData.Skill();
            skill.SetData(listData[row]);

            m_dicData[typeof(MasterData.Skill)][skill.m_nID] = skill;
        }
    }

    private void SetBehavior()
    {
        m_dicData[typeof(MasterData.Behavior)] = new Dictionary<int, IMasterData>();

        List<List<string>> listData = Util.ReadCSV("MasterData/Behavior");
        for (int row = 1; row < listData.Count; ++row)
        {
            MasterData.Behavior behavior = new MasterData.Behavior();
            behavior.SetData(listData[row]);

            m_dicData[typeof(MasterData.Behavior)][behavior.m_nID] = behavior;
        }
    }

    private void SetState()
    {
        m_dicData[typeof(MasterData.State)] = new Dictionary<int, IMasterData>();

        List<List<string>> listData = Util.ReadCSV("MasterData/State");
        for (int row = 1; row < listData.Count; ++row)
        {
            MasterData.State state = new MasterData.State();
            state.SetData(listData[row]);

            m_dicData[typeof(MasterData.State)][state.m_nID] = state;
        }
    }

    private void SetProjectile()
    {
        m_dicData[typeof(MasterData.Projectile)] = new Dictionary<int, IMasterData>();

        List<List<string>> listData = Util.ReadCSV("MasterData/Projectile");
        for (int row = 1; row < listData.Count; ++row)
        {
            MasterData.Projectile projectile = new MasterData.Projectile();
            projectile.SetData(listData[row]);

            m_dicData[typeof(MasterData.Projectile)][projectile.m_nID] = projectile;
        }
    }

    public bool GetData<T>(int nID, ref T data) where T : IMasterData
    {
        System.Type type = data == null ? typeof(T) : data.GetType();

        Dictionary<int, IMasterData> dicvalue = null;

        if (m_dicData.TryGetValue(type, out dicvalue))
        {
            IMasterData value = null;
            if (dicvalue.TryGetValue(nID, out value))
            {
                data = (T)value;

                return true;
            }
        }

        return false;
    }
}
