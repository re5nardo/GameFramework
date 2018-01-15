using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoSingleton<Factory>
{
//    public ISkill         CreateSkill(BaeGameRoom* pGameRoom, IEntity* pEntity, int nMasterDataID);
    public IBehavior CreateBehavior(IEntity pEntity, int nMasterDataID)
    {
        MasterData.Behavior pMasterBehavior = null;
        MasterDataManager.Instance.GetData<MasterData.Behavior>(nMasterDataID, ref pMasterBehavior);

        string strClassName = pMasterBehavior.m_strClassName;

        if (pMasterBehavior.m_strName == "Move")
        {
            Behavior.Move move = new Behavior.Move();

            move.Initialize(pEntity, nMasterDataID);

            return move;
        }
        else if (pMasterBehavior.m_strName == "Rotation")
        {
            Behavior.Rotation rotation = new Behavior.Rotation();

            rotation.Initialize(pEntity, nMasterDataID);

            return rotation;
        }
        else if (pMasterBehavior.m_strName == "Idle")
        {
            Behavior.Idle idle = new Behavior.Idle();

            idle.Initialize(pEntity, nMasterDataID);

            return idle;
        }
        else if (pMasterBehavior.m_strName == "Jump")
        {
            Behavior.Jump jump = new Behavior.Jump();

            jump.Initialize(pEntity, nMasterDataID);

            return jump;
        }

        return null;
    }

    public IState CreateState(IEntity pEntity, int nMasterDataID, long lStartTime)
    {
        MasterData.State pMasterState = null;
        MasterDataManager.Instance.GetData<MasterData.State>(nMasterDataID, ref pMasterState);

//        string strClassName = pMasterState->m_strClassName;
//
//        if (Acceleration::NAME.compare(strClassName) == 0)
//        {
//            return new Acceleration(pGameRoom, pEntity, nMasterDataID, lStartTime);
//        }
//        else if (Shield::NAME.compare(strClassName) == 0)
//        {
//            return new Shield(pGameRoom, pEntity, nMasterDataID, lStartTime);
//        }
//        else if (FireBehavior::NAME.compare(strClassName) == 0)
//        {
//            return new FireBehavior(pGameRoom, pEntity, nMasterDataID, lStartTime);
//        }
//        else if (GeneralState::NAME.compare(strClassName) == 0)
//        {
//            return new GeneralState(pGameRoom, pEntity, nMasterDataID, lStartTime);
//        }
//        else if (ChallengerDisturbing::NAME.compare(strClassName) == 0)
//        {
//            return new ChallengerDisturbing(pGameRoom, pEntity, nMasterDataID, lStartTime);
//        }

        return null;
    }

    public Character CreateCharacter(int nID, int nMasterDataID, Character.Role role)
    {
        GameObject goCharacter = ObjectPool.Instance.GetGameObject("CharacterModel/Character");
        Character character = goCharacter.GetComponent<Character>();
        character.Initialize(nID, nMasterDataID, role);

        return character;
    }

    //CharacterAI*  CreateCharacterAI(BaeGameRoom* pGameRoom, int nID, int nMasterDataID);
//    public Projectile     CreateProjectile(BaeGameRoom* pGameRoom, int nProjectorID, int nID, int nMasterDataID);
//    public Item           CreateItem(BaeGameRoom* pGameRoom, long long lTime, int nID, int nMasterDataID);
}