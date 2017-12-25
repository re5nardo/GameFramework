#include "stdafx.h"
#include "MasterDataManager.h"
#include <typeinfo>
#include "IMasterData.h"
#include "../Util.h"

MasterDataManager::MasterDataManager()
{
}

MasterDataManager::~MasterDataManager()
{
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
	SetItem();
	SetCharacterSpeedVariation();
}

void MasterDataManager::SetCharacter()
{
	string strTypeName = typeid(MasterData::Character).name();
	m_mapData[strTypeName] = map<int, IMasterData*>();

	vector<vector<string>> vectorData = Util::ReadCSV("Character.csv");
	for (int row = 1; row < vectorData.size(); ++row)
	{
		MasterData::Character* pCharacter = new MasterData::Character();
		pCharacter->SetData(vectorData[row]);

		m_mapData[strTypeName][pCharacter->m_nID] = pCharacter;
	}
}

void MasterDataManager::SetSkill()
{
	string strTypeName = typeid(MasterData::Skill).name();
	m_mapData[strTypeName] = map<int, IMasterData*>();

	vector<vector<string>> vectorData = Util::ReadCSV("Skill.csv");
	for (int row = 1; row < vectorData.size(); ++row)
	{
		MasterData::Skill* pSkill = new MasterData::Skill();
		pSkill->SetData(vectorData[row]);

		m_mapData[strTypeName][pSkill->m_nID] = pSkill;
	}
}

void MasterDataManager::SetBehavior()
{
	string strTypeName = typeid(MasterData::Behavior).name();
	m_mapData[strTypeName] = map<int, IMasterData*>();

	vector<vector<string>> vectorData = Util::ReadCSV("Behavior.csv");
	for (int row = 1; row < vectorData.size(); ++row)
	{
		MasterData::Behavior* pBehavior = new MasterData::Behavior();
		pBehavior->SetData(vectorData[row]);

		m_mapData[strTypeName][pBehavior->m_nID] = pBehavior;
	}
}

void MasterDataManager::SetState()
{
	string strTypeName = typeid(MasterData::State).name();
	m_mapData[strTypeName] = map<int, IMasterData*>();

	vector<vector<string>> vectorData = Util::ReadCSV("State.csv");
	for (int row = 1; row < vectorData.size(); ++row)
	{
		MasterData::State* pState = new MasterData::State();
		pState->SetData(vectorData[row]);

		m_mapData[strTypeName][pState->m_nID] = pState;
	}
}

void MasterDataManager::SetProjectile()
{
	string strTypeName = typeid(MasterData::Projectile).name();
	m_mapData[strTypeName] = map<int, IMasterData*>();

	vector<vector<string>> vectorData = Util::ReadCSV("Projectile.csv");
	for (int row = 1; row < vectorData.size(); ++row)
	{
		MasterData::Projectile* pProjectile = new MasterData::Projectile();
		pProjectile->SetData(vectorData[row]);

		m_mapData[strTypeName][pProjectile->m_nID] = pProjectile;
	}
}

void MasterDataManager::SetItem()
{
	string strTypeName = typeid(MasterData::Item).name();
	m_mapData[strTypeName] = map<int, IMasterData*>();

	vector<vector<string>> vectorData = Util::ReadCSV("Item.csv");
	for (int row = 1; row < vectorData.size(); ++row)
	{
		MasterData::Item* pItem = new MasterData::Item();
		pItem->SetData(vectorData[row]);

		m_mapData[strTypeName][pItem->m_nID] = pItem;
	}
}

void MasterDataManager::SetCharacterSpeedVariation()
{
	string strTypeName = typeid(MasterData::CharacterSpeedVariation).name();
	m_mapData[strTypeName] = map<int, IMasterData*>();

	vector<vector<string>> vectorData = Util::ReadCSV("CharacterSpeedVariation.csv");
	for (int row = 1; row < vectorData.size(); ++row)
	{
		MasterData::CharacterSpeedVariation* pCharacterSpeedVariation = new MasterData::CharacterSpeedVariation();
		pCharacterSpeedVariation->SetData(vectorData[row]);

		m_mapData[strTypeName][pCharacterSpeedVariation->m_nID] = pCharacterSpeedVariation;
	}
}