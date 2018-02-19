#include "stdafx.h"
#include "GameItem.h"
#include "../../../FBSFiles/PlayerInput/GameItem_generated.h"

namespace PlayerInput
{
	GameItem::GameItem()
	{
	}

	GameItem::~GameItem()
	{
	}

	FBS::PlayerInputType GameItem::GetType()
	{
		return FBS::PlayerInputType::PlayerInputType_GameItem;
	}

	IPlayerInput* GameItem::Clone()
	{
		GameItem* pClone = new GameItem();

		pClone->m_nPlayerIndex = m_nPlayerIndex;
		pClone->m_nGameItemID = m_nGameItemID;

		return pClone;
	}

	const char* GameItem::Serialize(int* pLength)
	{
		FBS::PlayerInput::GameItemBuilder data_builder(m_Builder);
		data_builder.add_PlayerIndex(m_nPlayerIndex);
		data_builder.add_GameItemID(m_nGameItemID);
		auto data = data_builder.Finish();

		m_Builder.Finish(data);

		*pLength = m_Builder.GetSize();

		return (char*)m_Builder.GetBufferPointer();
	}

	bool GameItem::Deserialize(const char* pChar)
	{
		auto data = flatbuffers::GetRoot<FBS::PlayerInput::GameItem>((const void*)pChar);

		m_nPlayerIndex = data->PlayerIndex();
		m_nGameItemID = data->GameItemID();

		return true;
	}
}

