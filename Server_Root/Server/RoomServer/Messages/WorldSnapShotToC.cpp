#include "stdafx.h"
#include "WorldSnapShotToC.h"
#include "WorldSnapShotToC_Data_generated.h"

WorldSnapShotToC::WorldSnapShotToC()
{
}

WorldSnapShotToC::~WorldSnapShotToC()
{
}

unsigned short WorldSnapShotToC::GetID()
{
	return MESSAGE_ID;
}

IMessage* WorldSnapShotToC::Clone()
{
	return NULL;
}

const char* WorldSnapShotToC::Serialize(int* pLength)
{
	//	최적화 고려.. 포인터 사용하는 방향.. (복사 속도때문에)

	list<Offset<EntityState>> listEntityState;
	for (map<int, IEntity*>::iterator it = m_mapEntity.begin(); it != m_mapEntity.end(); ++it)
	{
		int nPlayerIndex = it->first;
		IEntity* pEntity = it->second;

		FBSData::Vector3 pos(pEntity->GetPosition().x(), pEntity->GetPosition().y(), pEntity->GetPosition().z());
		FBSData::Vector3 rot(pEntity->GetRotation().x(), pEntity->GetRotation().y(), pEntity->GetRotation().z());

		vector<int> vecKeys;
		vector<float> vecValues;
		list<IBehavior*> listBehavior = pEntity->GetActivatedBehaviors();
		for (list<IBehavior*>::iterator it = listBehavior.begin(); it != listBehavior.end(); ++it)
		{
			vecKeys.push_back((*it)->GetMasterDataID());
			vecValues.push_back((*it)->GetTime());
		}

		auto BehaviorsMapKey = m_Builder.CreateVector(vecKeys);
		auto BehaviorsMapValue = m_Builder.CreateVector(vecValues);

		EntityStateBuilder entityState_builder(m_Builder);
		entityState_builder.add_PlayerIndex(nPlayerIndex);
		entityState_builder.add_Position(&pos);
		entityState_builder.add_Rotation(&rot);
		entityState_builder.add_BehaviorsMapKey(BehaviorsMapKey);
		entityState_builder.add_BehaviorsMapValue(BehaviorsMapValue);
		auto entityState = entityState_builder.Finish();

		listEntityState.push_back(entityState);
	}

	vector<Offset<EntityState>> vecOffsetEntityState;
	for (list<Offset<EntityState>>::iterator it = listEntityState.begin(); it != listEntityState.end(); ++it)
	{
		vecOffsetEntityState.push_back(*it);
	}
	auto entityStates = m_Builder.CreateVector(vecOffsetEntityState);

	WorldSnapShotToC_DataBuilder data_builder(m_Builder);
	data_builder.add_Tick(m_nTick);
	data_builder.add_Time(m_fTime);
	data_builder.add_EntityStates(entityStates);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool WorldSnapShotToC::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<WorldSnapShotToC_Data>((const void*)pChar);

	m_nTick = data->Tick();
	m_fTime = data->Time();

	//	Not necessary.. (Deserialize() is never called in server side..)
	for (int i = 0; i < data->EntityStates()->size(); ++i)
	{
		
	}

	return true;
}