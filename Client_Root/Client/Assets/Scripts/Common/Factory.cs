using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoSingleton<Factory>
{
//    public ISkill         CreateSkill(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID)
//    {
//    }

    public IBehavior CreateBehavior(int nMasterDataID)
    {
        MasterData.Behavior pMasterBehavior = null;
        MasterDataManager.Instance.GetData<MasterData.Behavior>(nMasterDataID, ref pMasterBehavior);

        string strClassName = pMasterBehavior.m_strClassName;

        if (pMasterBehavior.m_strName == "Move")
        {
            return new Behavior.Move();
        }
        else if (pMasterBehavior.m_strName == "Rotation")
        {
            return new Behavior.Rotation();
        }
        else if (pMasterBehavior.m_strName == "Idle")
        {
            return new Behavior.Idle();
        }
        else if (pMasterBehavior.m_strName == "Jump")
        {
            return new Behavior.Jump();
        }
        else if (pMasterBehavior.m_strName == "BlobFire")
        {
            return new Behavior.Fire();
        }

        return null;
    }

    public IState CreateState(int nMasterDataID)
    {
        MasterData.State pMasterState = null;
        MasterDataManager.Instance.GetData<MasterData.State>(nMasterDataID, ref pMasterState);

        string strClassName = pMasterState.m_strClassName;

        if (pMasterState.m_strClassName == "ChallengerDisturbing")
        {
            return ObjectPool.instance.GetObject<State.ChallengerDisturbing>();
        }
        else if (pMasterState.m_strClassName == "Faint")
        {
            return ObjectPool.instance.GetObject<State.Faint>();
        }
        else if (pMasterState.m_strClassName == "General")
        {
            return ObjectPool.instance.GetObject<State.General>();
        }

        return null;
    }

    public Character CreateCharacter(int nMasterDataID)
    {
        GameObject goCharacter = ObjectPool.Instance.GetGameObject("CharacterModel/Character");
        Character character = goCharacter.GetComponent<Character>();

        return character;
    }

    public Projectile CreateProjectile(int nMasterDataID)
    {
        GameObject goProjectile = ObjectPool.Instance.GetGameObject("ProjectileModel/Projectile");
        Projectile projectile = goProjectile.GetComponent<Projectile>();

        return projectile;
    }

    //CharacterAI*  CreateCharacterAI(BaeGameRoom* pGameRoom, int nID, int nMasterDataID);
//    public Projectile     CreateProjectile(BaeGameRoom* pGameRoom, int nProjectorID, int nID, int nMasterDataID);
//    public Item           CreateItem(BaeGameRoom* pGameRoom, long long lTime, int nID, int nMasterDataID);
}