#pragma once

#include "IMessage.h"
#include "Defines.h"

class ReqMove : public IMessage
{
public:
	ReqMove();
	virtual ~ReqMove();

public:
	Vector3		m_vec3Position;

	//	json field name : pos_x, pos_y, pos_z

public:
	unsigned short GetID() override;
	string Serialize() override;
	bool Deserialize(string strJson) override;
};