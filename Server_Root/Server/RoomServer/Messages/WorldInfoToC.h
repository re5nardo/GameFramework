#pragma once

#include "../../CommonSources/Message/IMessage.h"
#include "../../CommonSources/Message/MessageIDs.h"
#include <list>
#include <utility>

class IGameEvent;

using namespace std;

class WorldInfoToC : public IMessage
{
public:
	WorldInfoToC();
	virtual ~WorldInfoToC();

public:
	static const unsigned short MESSAGE_ID = WorldInfoToC_ID;

public:
	int m_nTick;
	float m_fStartTime;					//	Exclude
	float m_fEndTime;					//	Include
	list<IGameEvent*> m_listGameEvent;

public:
	unsigned short GetID() override;
	IMessage* Clone() override;
	const char* Serialize(int* pLength = NULL) override;
	bool Deserialize(const char* pChar) override;
};

