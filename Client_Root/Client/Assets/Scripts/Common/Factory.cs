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
        MasterData.Behavior masterData = null;
        MasterDataManager.Instance.GetData<MasterData.Behavior>(nMasterDataID, ref masterData);

        string strClassName = masterData.m_strClassName;

        if (masterData.m_strClassName == "Move")
        {
            return new Behavior.Move();
        }
        else if (masterData.m_strClassName == "Rotation")
        {
            return new Behavior.Rotation();
        }
        else if (masterData.m_strClassName == "Idle")
        {
            return new Behavior.Idle();
        }
        else if (masterData.m_strClassName == "Jump")
        {
            return new Behavior.Jump();
        }
        else if (masterData.m_strClassName == "Cast")
        {
            return new Behavior.Cast();
        }

        return null;
    }

    public IState CreateState(int nMasterDataID)
    {
        MasterData.State masterData = null;
        MasterDataManager.Instance.GetData<MasterData.State>(nMasterDataID, ref masterData);

        string strClassName = masterData.m_strClassName;

        if (masterData.m_strClassName == "ChallengerDisturbing")
        {
            return ObjectPool.instance.GetObject<State.ChallengerDisturbing>();
        }
        else if (masterData.m_strClassName == "Faint")
        {
            return ObjectPool.instance.GetObject<State.Faint>();
        }
        else if (masterData.m_strClassName == "General")
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

    public GameItem CreateGameItem()
    {
        GameObject goGameItem = ObjectPool.Instance.GetGameObject("ItemModel/GameItem");
        GameItem gameItem = goGameItem.GetComponent<GameItem>();

        return gameItem;
    }

    public IMagic CreateMagic()
    {
        GameObject goMagic = ObjectPool.Instance.GetGameObject("ItemModel/GameItem");
        IMagic magic = goMagic.GetComponent<IMagic>();

        return magic;
    }

    //CharacterAI*  CreateCharacterAI(BaeGameRoom* pGameRoom, int nID, int nMasterDataID);
//    public Projectile     CreateProjectile(BaeGameRoom* pGameRoom, int nProjectorID, int nID, int nMasterDataID);
//    public Item           CreateItem(BaeGameRoom* pGameRoom, long long lTime, int nID, int nMasterDataID);
}