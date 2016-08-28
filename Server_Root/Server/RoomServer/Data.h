#pragma once

typedef struct
{
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