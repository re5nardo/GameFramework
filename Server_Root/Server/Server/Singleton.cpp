#include "stdafx.h"
#include "Singleton.h"

template <class T>
T* Singleton<T>::Instance()
{
	if (m_pInstance == NULL)
	{
		m_pInstance = new T;
	}

	assert(m_pInstance != NULL);
	return m_pInstance;
}

template <class T>
void Singleton<T>::Destroy()
{
	if (m_pInstance != NULL)
	{
		delete m_pInstance;
		m_pInstance = NULL;
	}
}
