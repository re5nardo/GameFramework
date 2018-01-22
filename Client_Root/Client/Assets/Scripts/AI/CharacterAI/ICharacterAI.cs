using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ICharacterAI : ITickUpdatable
{
    protected int m_nMasterDataID = -1;
    protected Character m_character;

    public virtual void Initialize(int nMasterDataID)
    {
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
