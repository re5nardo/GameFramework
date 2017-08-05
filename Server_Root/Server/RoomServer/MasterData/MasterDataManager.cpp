#include "stdafx.h"
#include "MasterDataManager.h"
#include <typeinfo>
#include "IMasterData.h"
#include "Skill.h"
#include "Character.h"
#include "Behavior.h"
#include "State.h"
#include "Projectile.h"

MasterDataManager::MasterDataManager()
{
	m_Book = xlCreateBook();
}

MasterDataManager::~MasterDataManager()
{
	if (m_Book != NULL)
	{
		m_Book->release();
		//delete m_Book;		Does release() delete instance?
	}

	for (map<string, map<int, IMasterData*>>::iterator it = m_mapData.begin(); it != m_mapData.end(); ++it)
	{
		for (map<int, IMasterData*>::iterator iter = it->second.begin(); iter != it->second.end(); ++it)
		{
			delete iter->second;
		}
		it->second.clear();
	}
	m_mapData.clear();
}

void MasterDataManager::DownloadMasterData(string strUrl)
{
	//	Temp.. load local files
	SetCharacter();
	SetSkill();
	SetBehavior();
	SetState();
	SetProjectile();
}

void MasterDataManager::SetCharacter()
{
	string strTypeName = typeid(MasterData::Character).name();
	m_mapData[strTypeName] = map<int, IMasterData*>();

	if (m_Book->load("Character.xls"))
	{
		Sheet* pSheet = m_Book->getSheet(0);
		for (int row = 1; row < pSheet->lastRow(); ++row)
		{
			MasterData::Character* pCharacter = new MasterData::Character();
			pCharacter->SetData(pSheet, row);

			m_mapData[strTypeName][pCharacter->m_nID] = pCharacter;
		}
	}
	else
	{
		printf("Fail to load Character.xls!");
	}
}

void MasterDataManager::SetSkill()
{
	string strTypeName = typeid(MasterData::Skill).name();
	m_mapData[strTypeName] = map<int, IMasterData*>();

	if (m_Book->load("Skill.xls"))
	{
		Sheet* pSheet = m_Book->getSheet(0);
		for (int row = 1; row < pSheet->lastRow(); ++row)
		{
			MasterData::Skill* pSkill = new MasterData::Skill();
			pSkill->SetData(pSheet, row);

			m_mapData[strTypeName][pSkill->m_nID] = pSkill;
		}
	}
	else
	{
		printf("Fail to load Skill.xls!");
	}
}

void MasterDataManager::SetBehavior()
{
	string strTypeName = typeid(MasterData::Behavior).name();
	m_mapData[strTypeName] = map<int, IMasterData*>();

	if (m_Book->load("Behavior.xls"))
	{
		Sheet* pSheet = m_Book->getSheet(0);
		for (int row = 1; row < pSheet->lastRow(); ++row)
		{
			MasterData::Behavior* pBehavior = new MasterData::Behavior();
			pBehavior->SetData(pSheet, row);

			m_mapData[strTypeName][pBehavior->m_nID] = pBehavior;
		}
	}
	else
	{
		printf("Fail to load Behavior.xls!");
	}
}

void MasterDataManager::SetState()
{
	string strTypeName = typeid(MasterData::State).name();
	m_mapData[strTypeName] = map<int, IMasterData*>();

	if (m_Book->load("State.xls"))
	{
		Sheet* pSheet = m_Book->getSheet(0);
		for (int row = 1; row < pSheet->lastRow(); ++row)
		{
			MasterData::State* pState = new MasterData::State();
			pState->SetData(pSheet, row);

			m_mapData[strTypeName][pState->m_nID] = pState;
		}
	}
	else
	{
		printf("Fail to load State.xls!");
	}
}

void MasterDataManager::SetProjectile()
{
	string strTypeName = typeid(MasterData::Projectile).name();
	m_mapData[strTypeName] = map<int, IMasterData*>();

	if (m_Book->load("Projectile.xls"))
	{
		Sheet* pSheet = m_Book->getSheet(0);
		for (int row = 1; row < pSheet->lastRow(); ++row)
		{
			MasterData::Projectile* pProjectile = new MasterData::Projectile();
			pProjectile->SetData(pSheet, row);

			m_mapData[strTypeName][pProjectile->m_nID] = pProjectile;
		}
	}
	else
	{
		printf("Fail to load Projectile.xls!");
	}
}