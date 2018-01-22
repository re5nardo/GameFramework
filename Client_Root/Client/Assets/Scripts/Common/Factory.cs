using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoSingleton<Factory>
{
//    public ISkill         CreateSkill(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID);
    public IBehavior CreateBehavior(IEntity entity, int nMasterDataID)
    {
        MasterData.Behavior pMasterBehavior = null;
        MasterDataManager.Instance.GetData<MasterData.Behavior>(nMasterDataID, ref pMasterBehavior);

        string strClassName = pMasterBehavior.m_strClassName;

        if (pMasterBehavior.m_strName == "Move")
        {
            Behavior.Move move = new Behavior.Move();

            move.Initialize(entity, nMasterDataID);

            return move;
        }
        else if (pMasterBehavior.m_strName == "Rotation")
        {
            Behavior.Rotation rotation = new Behavior.Rotation();

            rotation.Initialize(entity, nMasterDataID);

            return rotation;
        }
        else if (pMasterBehavior.m_strName == "Idle")
        {
            Behavior.Idle idle = new Behavior.Idle();

            idle.Initialize(entity, nMasterDataID);

            return idle;
        }
        else if (pMasterBehavior.m_strName == "Jump")
        {
            Behavior.Jump jump = new Behavior.Jump();

            jump.Initialize(entity, nMasterDataID);

            return jump;
        }
        else if (pMasterBehavior.m_strName == "BlobFire")
        {
            Behavior.Fire fire = new Behavior.Fire();

            fire.Initialize(entity, nMasterDataID);

            return fire;
        }

        return null;
    }

    public IState CreateState(IEntity entity, int nMasterDataID)
    {
        MasterData.State pMasterState = null;
        MasterDataManager.Instance.GetData<MasterData.State>(nMasterDataID, ref pMasterState);

        string strClassName = pMasterState.m_strClassName;

        if (pMasterState.m_strName == "ChallengerDisturbing")
        {
            State.ChallengerDisturbing challengerDisturbing = new State.ChallengerDisturbing();

            challengerDisturbing.Initialize(entity, nMasterDataID);

            return challengerDisturbing;
        }

        return null;
    }

    public Character CreateCharacter(int nMasterDataID, Character.Role role)
    {
        GameObject goCharacter = ObjectPool.Instance.GetGameObject("CharacterModel/Character");
        Character character = goCharacter.GetComponent<Character>();
//        character.Initialize(nID, nMasterDataID, role);

        return character;
    }

    public Projectile CreateProjectile(int nProjectorID, int nMasterDataID)
    {
        GameObject goProjectile = ObjectPool.Instance.GetGameObject("ProjectileModel/Projectile");
        Projectile projectile = goProjectile.GetComponent<Projectile>();

        return projectile;
    }

    //CharacterAI*  CreateCharacterAI(BaeGameRoom* pGameRoom, int nID, int nMasterDataID);
//    public Projectile     CreateProjectile(BaeGameRoom* pGameRoom, int nProjectorID, int nID, int nMasterDataID);
//    public Item           CreateItem(BaeGameRoom* pGameRoom, long long lTime, int nID, int nMasterDataID);
}