#pragma once

struct Vector3
{
public:
	Vector3(){ x = 0.0f; y = 0.0f; z = 0.0f; };
	Vector3(float x, float y, float z){ this->x = x; this->y = y; this->z = z; };
public:
	float x;
	float y;
	float z;
};

//enum GameEventType
//{
//	None,
//	Idle,
//	Move,
//	Stop,
//	Skill,
//	Gesture,
//	GetItem,
//	Collision,
//	Die,
//};

struct Stat
{
public:
	Stat(){ fSpeed = 0.0f; fAgility = 0.0f; };
	Stat(float fSpeed, float fAgility){ this->fSpeed = fSpeed; this->fAgility = fAgility; };
public:
	float fSpeed;
	float fAgility;
};