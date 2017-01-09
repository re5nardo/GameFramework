#pragma once

typedef struct
{
public:
	float x;
	float y;
	float z;
}Vector3;

enum GameEventType
{
	None,
	Idle,
	Move,
	Stop,
	Skill,
	Gesture,
	GetItem,
	Collision,
	Die,
};