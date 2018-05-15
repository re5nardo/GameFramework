using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    public class Meteor : IMagic
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
                    if (action.m_strID == "MagicObject")
                    {
                        int nMagicObjectID = 0;
                        Util.Convert(action.m_listParams[0], ref nMagicObjectID);

                        int nEntityID = 0;
                        IMagicObject magicObject = null;
						IGameRoom.Instance.CreateMagicObject(nMagicObjectID, ref nEntityID, ref magicObject, m_nCasterID, m_nID);

						MagicObject.MeteorObject meteorObject = magicObject as MagicObject.MeteorObject;

						Vector3 vec3Offset = new Vector3(float.Parse(action.m_listParams[1]), float.Parse(action.m_listParams[2]), float.Parse(action.m_listParams[3]));
						Vector3 vec3Direction = new Vector3(float.Parse(action.m_listParams[4]), float.Parse(action.m_listParams[5]), float.Parse(action.m_listParams[6]));
						float fSpeed = float.Parse(action.m_listParams[7]);

						meteorObject.SetData(vec3Direction, fSpeed);

						Character caster = IGameRoom.Instance.GetCharacter(m_nCasterID);

                        Rigidbody rigidbody = magicObject.GetRigidbody();
                        rigidbody.isKinematic = true;

						GameObject goMagicObjectModel = meteorObject.GetModel();

						goMagicObjectModel.transform.position = caster.GetPosition() + vec3Offset;
                        goMagicObjectModel.transform.rotation = Quaternion.identity;
                        goMagicObjectModel.transform.localScale = Vector3.one;

                        rigidbody.isKinematic = false;

                        magicObject.StartTick(nUpdateTick);
                        magicObject.UpdateTick(nUpdateTick);
                    }
                }
            }

            if (m_nEndTick != -1 && nUpdateTick == m_nEndTick)
            {
				IGameRoom.Instance.DestroyMagic(this);
            }
        }
    }
}