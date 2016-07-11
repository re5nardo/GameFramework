#pragma once

#include "IGameEvent.h"

class GameEvent_Move_ToC : public IGameEvent
{
public:
	GameEvent_Move_ToC();
	virtual ~GameEvent_Move_ToC();

public:
	Vector3 m_vec3Dest;   //  json field name : Pos_X, Pos_Y, Pos_Z

public:
	unsigned short GetID() override;
	string Serialize() override;
	bool Deserialize(string strJson) override;
	void GetJSONObject(Document* pJsonObj) override;
	bool SetByJSONObject(Document* pJsonObj) override;
};

