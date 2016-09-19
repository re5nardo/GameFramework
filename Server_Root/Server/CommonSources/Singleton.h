#pragma once
#include <assert.h>

template <class T>
class Singleton
{
public:
	static T* Instance()
	{
		if (m_pInstance == NULL)
		{
			m_pInstance = new T;
		}

		assert(m_pInstance != NULL);
		return m_pInstance;
	}

	static void Destroy()
	{
		if (m_pInstance != NULL)
		{
			delete m_pInstance;
			m_pInstance = NULL;
		}
	}

private:
	static T* m_pInstance;
};

template <class T> T* Singleton<T>::m_pInstance = NULL;
