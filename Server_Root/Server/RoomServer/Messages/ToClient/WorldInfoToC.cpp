#include "stdafx.h"
#include "WorldInfoToC.h"
#include "../../GameEvent/IGameEvent.h"
#include "../../../FBSFiles/WorldInfoToC_generated.h"

WorldInfoToC::WorldInfoToC()
{
}

WorldInfoToC::~WorldInfoToC()
{
}

unsigned short WorldInfoToC::GetID()
{
	return MESSAGE_ID;
}

IMessage* WorldInfoToC::Clone()
{
	return NULL;
}

const char* WorldInfoToC::Serialize(int* pLength)
{
	vector<Offset<FBS::GameEventData>> vecGameEvent;
	for (list<IGameEvent*>::iterator it = m_listGameEvent.begin(); it != m_listGameEvent.end(); ++it)
	{
		int nLength;
		const signed char* pData = (const signed char*)(*it)->Serialize(&nLength);
		auto data = m_Builder.CreateVector<int8_t>(pData, nLength);

		FBS::GameEventDataBuilder gameEvent_builder(m_Builder);
		gameEvent_builder.add_Type((*it)->GetType());
		gameEvent_builder.add_EventTime((*it)->m_fEventTime);
		gameEvent_builder.add_Data(data);
		auto gameEvent = gameEvent_builder.Finish();

		vecGameEvent.push_back(gameEvent);
	}

	Offset<Vector<Offset<FBS::GameEventData>>> gameEvents;

	if (vecGameEvent.size() > 0)
	{
		gameEvents = m_Builder.CreateVector<Offset<FBS::GameEventData>>(&vecGameEvent[0], vecGameEvent.size());
	}

	FBS::WorldInfoToCBuilder data_builder(m_Builder);
	data_builder.add_Tick(m_nTick);
	data_builder.add_StartTime(m_fStartTime);
	data_builder.add_EndTime(m_fEndTime);
	data_builder.add_GameEvents(gameEvents);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool WorldInfoToC::Deserialize(const char* pChar)
{
	//	Not necessary.. (Deserialize() is never called in server side..)
	return true;
}