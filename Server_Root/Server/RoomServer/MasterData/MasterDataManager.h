#pragma once

#include "../../CommonSources/Singleton.h"
#include <map>
#include "libxl.h"
#include "Skill.h"
#include "Character.h"
#include "Behavior.h"
#include "State.h"
#include "Projectile.h"
#include "Item.h"
#include "CharacterSpeedVariation.h"

using namespace std;
using namespace libxl;

class MasterDataManager : public Singleton<MasterDataManager>
{
public:
	MasterDataManager();
	virtual ~MasterDataManager();

private:
	Book* m_Book = NULL;

private:
	map<string, map<int, IMasterData*>> m_mapData;

public:
	void DownloadMasterData(string strUrl);

private:
	void SetCharacter();
	void SetSkill();
	void SetBehavior();
	void SetState();
	void SetProjectile();
	void SetItem();
	void SetCharacterSpeedVariation();

public:
	template <typename T>
	bool GetData(int nID, T*& pData)
	{
		string strTypeName = typeid(T).name();
		map<string, map<int, IMasterData*>>::iterator it = m_mapData.find(strTypeName);

		if (it != m_mapData.end())
		{
			map<int, IMasterData*>::iterator iter = it->second.find(nID);

			if (iter != it->second.end())
			{
				pData = (T*)iter->second;

				return true;
			}
		}

		return false;
	}
};