﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    public class Super : IMagic
    {
        public override void Initialize(int nCasterID, int nID, int nMasterDataID, float fTickInterval)
        {
            m_nCasterID = nCasterID;
            m_nID = nID;
            m_nMasterDataID = nMasterDataID;
            m_fTickInterval = fTickInterval;

            MasterData.Magic masterMagic = null;
            MasterDataManager.Instance.GetData<MasterData.Magic>(nMasterDataID, ref masterMagic);

            m_fLength = masterMagic.m_fLength;
            m_listAction = masterMagic.m_listAction;
            m_TargetType = masterMagic.m_TargetType;
        }

        protected override void UpdateBody(int nUpdateTick)
        {
			foreach (MasterData.Magic.Action action in m_listAction)
            {
                int nTick = (int)(action.m_fTime / m_fTickInterval);

                if (nUpdateTick == nTick + m_nStartTick)
                {
                    if (action.m_strID == "AddState")
                    {
                        int nStateID = 0;
						Util.Convert(action.m_listParams[0], ref nStateID);

						if(m_TargetType == TargetType.AllButSelf)
						{
							List<Character> listCharacter = BaeGameRoom2.Instance.GetAllCharacters();

							foreach(Character character in listCharacter)
							{
								if(character.GetID() == m_nCasterID)
									continue;

								IState state = Factory.Instance.CreateState(nStateID);
								state.Initialize(character, nStateID, BaeGameRoom2.Instance.GetTickInterval());

								character.AddState(state, nUpdateTick);

				                state.StartTick(nUpdateTick);
				                state.UpdateTick(nUpdateTick);
							}
						}
                    }
                }
            }

            if (m_nEndTick != -1 && nUpdateTick == m_nEndTick)
            {
                BaeGameRoom2.Instance.DestroyMagic(this);
            }
        }
    }
}