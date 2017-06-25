using System.Collections.Generic;

public abstract class IMasterData
{
    public int m_nID;

    public abstract void SetData(List<string> data);
}