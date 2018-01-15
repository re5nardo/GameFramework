#include "stdafx.h"
#include "PlayerInputToR.h"
#include "../../PlayerInput/PlayerInputs/Rotation.h"
#include "../../PlayerInput/PlayerInputs/Position.h"

PlayerInputToR::PlayerInputToR()
{
}

PlayerInputToR::~PlayerInputToR()
{
	if (m_PlayerInput != NULL)
	{
		delete m_PlayerInput;
	}
}

unsigned short PlayerInputToR::GetID()
{
	return MESSAGE_ID;
}

IMessage* PlayerInputToR::Clone()
{
	PlayerInputToR* pClone = new PlayerInputToR();

	pClone->m_Type = m_Type;
	pClone->m_PlayerInput = m_PlayerInput->Clone();

	return pClone;
}

const char* PlayerInputToR::Serialize(int* pLength)
{
	int nLength = 0;
	const char* pData = m_PlayerInput->Serialize(&nLength);

	auto inputData = m_Builder.CreateVector<int8_t>((const signed char*)(pData), nLength);

	FBS::PlayerInputToRBuilder data_builder(m_Builder);
	data_builder.add_Type(m_Type);
	data_builder.add_Data(inputData);
	auto data = data_builder.Finish();

	m_Builder.Finish(data);

	*pLength = m_Builder.GetSize();

	return (char*)m_Builder.GetBufferPointer();
}

bool PlayerInputToR::Deserialize(const char* pChar)
{
	auto data = flatbuffers::GetRoot<FBS::PlayerInputToR>((const void*)pChar);

	m_Type = data->Type();

	if (m_Type == FBS::PlayerInputType::PlayerInputType_Rotation)
	{
		PlayerInput::Rotation* playerInput = new PlayerInput::Rotation();

		vector<char> vecChar;
		const flatbuffers::Vector<int8_t>* vecInt8 = data->Data();
		for (flatbuffers::Vector<int8_t>::iterator it = vecInt8->begin(); it != vecInt8->end(); ++it)
		{
			vecChar.push_back(*it);
		}

		playerInput->Deserialize(&vecChar[0]);

		m_PlayerInput = playerInput;
	}
	else if (m_Type == FBS::PlayerInputType::PlayerInputType_Position)
	{
		PlayerInput::Position* playerInput = new PlayerInput::Position();

		vector<char> vecChar;
		const flatbuffers::Vector<int8_t>* vecInt8 = data->Data();
		for (flatbuffers::Vector<int8_t>::iterator it = vecInt8->begin(); it != vecInt8->end(); ++it)
		{
			vecChar.push_back(*it);
		}

		playerInput->Deserialize(&vecChar[0]);

		m_PlayerInput = playerInput;
	}

	return true;
}