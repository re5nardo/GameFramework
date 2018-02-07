using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ICharacterAI : ITickUpdatable
{
    protected int m_nMasterDataID = -1;
    protected Character m_character;

    protected float m_fTickInterval = 0;
    protected int m_nStartTick = -1;

    public virtual void StartTick(int nStartTick, params object[] param)
    {
        m_nStartTick = nStartTick;
    }

    public virtual void Initialize(int nMasterDataID, float fTickInterval)
    {
        m_fTickInterval = fTickInterval;

//        int nEntityID = 0;
//        Character character = null;
//
//        MasterData.Character masterCharacter = null;
//        MasterDataManager.Instance.GetData<MasterData.Character>(m_nMasterDataID, ref masterCharacter);
//
//        CharacterStatus status = new CharacterStatus(masterCharacter.m_nHP, masterCharacter.m_nHP, masterCharacter.m_nMP, masterCharacter.m_nMP, masterCharacter.m_fMaximumSpeed, 0, masterCharacter.m_fMPChargeRate, 0);
//
//        BaeGameRoom2.Instance.CreateCharacter(nMasterDataID, ref nEntityID, ref m_character, Character.Role.Disturber, status);
    }

    public int GetMasterDataID()
    {
        return m_nMasterDataID;
    }

    public Character GetCharacter()
    {
        return m_character;
    }
}
