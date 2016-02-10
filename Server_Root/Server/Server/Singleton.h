#pragma once

template <class T>
class Singleton
{
private:
	Singleton();
	virtual ~Singleton();

public:
	static T* Instance();
	static void Destroy();

private:
	static T* m_pInstance;
};

template <class T> T* Singleton<T>::m_pInstance = NULL;
