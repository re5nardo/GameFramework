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
        else if (masterData.m_strClassName == "Die")
        {
            return new Behavior.Die();
        }

        return null;
    }

    public IState CreateState(int nMasterDataID)
    {
        MasterData.State masterData = null;
        MasterDataManager.Instance.GetData<MasterData.State>(nMasterDataID, ref masterData);

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
        else if (masterData.m_strClassName == "Boost")
        {
            return ObjectPool.instance.GetObject<State.Boost>();
        }
        else if (masterData.m_strClassName == "Shield")
        {
            return ObjectPool.instance.GetObject<State.Shield>();
        }
		else if (masterData.m_strClassName == "Flicker")
        {
            return ObjectPool.instance.GetObject<State.Flicker>();
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

    public IMagic CreateMagic(int nMasterDataID)
    {
        MasterData.Magic masterData = null;
        MasterDataManager.Instance.GetData<MasterData.Magic>(nMasterDataID, ref masterData);

        if (masterData.m_strClassName == "Meteor")
        {
            return new Magic.Meteor();
        }
        else if (masterData.m_strClassName == "WaterJail")
        {
            return new Magic.WaterJail();
        }
        else if (masterData.m_strClassName == "Shield")
        {
            return new Magic.Shield();
        }
        else if (masterData.m_strClassName == "Boost")
        {
            return new Magic.Boost();
        }
        else if (masterData.m_strClassName == "Weight")
        {
            return new Magic.Weight();
        }
        else if (masterData.m_strClassName == "Thunder")
        {
            return new Magic.Thunder();
        }
        else if (masterData.m_strClassName == "Super")
        {
            return new Magic.Super();
        }

        return null;
    }

    public IMagicObject CreateMagicObject(int nMasterDataID)
    {
        MasterData.MagicObject masterData = null;
        MasterDataManager.Instance.GetData<MasterData.MagicObject>(nMasterDataID, ref masterData);

        if (masterData.m_strClassName == "WeightObject")
        {
            return new MagicObject.WeightObject();
        }
		else if (masterData.m_strClassName == "MeteorObject")
        {
            return new MagicObject.MeteorObject();
        }
		else if (masterData.m_strClassName == "WaterJailObject")
        {
            return new MagicObject.WaterJailObject();
        }

        return null;
    }

    //CharacterAI*  CreateCharacterAI(BaeGameRoom* pGameRoom, int nID, int nMasterDataID);
//    public Projectile     CreateProjectile(BaeGameRoom* pGameRoom, int nProjectorID, int nID, int nMasterDataID);
//    public Item           CreateItem(BaeGameRoom* pGameRoom, long long lTime, int nID, int nMasterDataID);
}