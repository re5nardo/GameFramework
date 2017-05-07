using UnityEngine;
using System.Collections.Generic;

public abstract class IGameRoom : MonoSingleton<IGameRoom>
{
    public abstract float GetElapsedTime();  //  Second
    public abstract int GetPlayerIndex();
}
