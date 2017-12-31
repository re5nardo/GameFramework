#include "stdafx.h"
#include "TickInfoToC.h"
#include "../../PlayerInput/IPlayerInput.h"
#include "../../../FBSFiles/TickInfoToC_generated.h"

TickInfoToC::TickInfoToC()
{
}

TickInfoToC::~TickInfoToC()
{
	for (list<IPlayerInput*>::iterator it = m_listPlayerInput.begin(); it != m_listPlayerInput.end(); ++it)
	{
		delete *it;
	}
}

unsigned short TickInfoToC::GetID()
{
	return MESSAGE_ID;
}

IMessage* TickInfoToC::Clone()
{
	return NULL;
}

const char* TickInfoToC::Serialize(int* pLength)
{
	vector<Offset<FBS::PlayerInputData>> vecPlayerInput;
	for (list<IPlayerInput*>::iterator it = m_listPlayerInput.begin(); it != m_listPlayerInput.end(); ++it)
	{
		int nLength;
		const signed char* pData = (const signed char*)(*it)->Serialize(&nLength);
		auto data = m_Builder.CreateVector<int8_t>(pData, nLength);

		FBS::PlayerInputDataBuilder gameEvent_builder(m_Builder);
		gameEvent_builder.add_Type((*it)->GetType());
		gameEvent_builder.add_Data(data);
		auto playerInput = gameEvent_builder.Finish();

		vecPlayerInput.push_back(playerInput);
	}

	Offset<Vector<Offset<FBS::PlayerInputData>>> playerInputs;

	if (vecPlayerInput.size() > 0)
	{
		playerInputs = m_Builder.CreateVector<Offset<FBS::PlayerInputData>>(&vecPlayerInput[0], vecPlayerInput.size());
	}

	FBS::TickInfoToCBuilder data_builder(m_Builder);
	data_builder.add_Tick(m_nTick);
	data_builder.add_PlayerInputs(playerInputs);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool TickInfoToC::Deserialize(const char* pChar)
{
	//	Not necessary.. (Deserialize() is never called in server side..)
	return true;
}