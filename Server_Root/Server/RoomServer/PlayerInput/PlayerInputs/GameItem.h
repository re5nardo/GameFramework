#pragma once

#include "../IPlayerInput.h"

namespace PlayerInput
{
	class GameItem : public IPlayerInput
	{
	public:
		GameItem();
		virtual ~GameItem();

	public:
		int m_nPlayerIndex;
		int m_nGameItemID;

	public:
		FBS::PlayerInputType GetType() override;
		IPlayerInput* Clone() override;
		const char* Serialize(int* pLength = NULL) override;
		bool Deserialize(const char* pChar) override;
	};
}