
public abstract class IGameRoom : MonoSingleton<IGameRoom>
{
    public abstract float GetElapsedTime();

    public virtual float GetCorrectionTime()
    {
        return 0.2f;
    }

    public virtual float GetCorrectionThreshold()
    {
        return 1f;
    }
}
