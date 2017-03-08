using UnityEngine;
using System.Collections.Generic;

public class MisterBae : IEntity
{
    [SerializeField] private BehaviorAnimationInfo[] m_arrBehaviorAnimationInfo;

    public override void Initialize(params object[] arrParam)
    {
        m_DefaultStat = m_CurrentStat = (Stat)arrParam[0];

        //  Temp
        m_arrBehaviorAnimationInfo = new BehaviorAnimationInfo[2];
        m_arrBehaviorAnimationInfo[0] = new BehaviorAnimationInfo();
        m_arrBehaviorAnimationInfo[0].behaviorKey = "0";
        m_arrBehaviorAnimationInfo[0].clipName = "WAIT00";

        m_arrBehaviorAnimationInfo[1] = new BehaviorAnimationInfo();
        m_arrBehaviorAnimationInfo[1].behaviorKey = "1";
        m_arrBehaviorAnimationInfo[1].clipName = "RUN00_F";
    }

    protected override void CreateUI()
    {
        m_EntityUI = (Instantiate(Resources.Load("CharacterUI/MisterBaeUI")) as GameObject).GetComponent<MisterBaeUI>();
    }

    public override void SampleBehaviors(Dictionary<string, KeyValuePair<float, float>> dicBehaviors, float fInterpolationValue, float fTickInterval, float fEmptyValue)
    {
        foreach(KeyValuePair<string, KeyValuePair<float, float>> behavior in dicBehaviors)
        {
            float fTime = 0f;
            float fWeight = 0f;
            if (behavior.Value.Key != fEmptyValue && behavior.Value.Value != fEmptyValue)
            {
                fTime = Mathf.Lerp(behavior.Value.Key, behavior.Value.Value, fInterpolationValue);
                fWeight = 1f;
            }
            else if(behavior.Value.Key == fEmptyValue)
            {
                fTime = Mathf.Lerp(0f, behavior.Value.Value, fInterpolationValue);
                fWeight = fInterpolationValue;
            }
            else if(behavior.Value.Value == fEmptyValue)
            {
                fTime = Mathf.Lerp(behavior.Value.Key, behavior.Value.Key + fTickInterval, fInterpolationValue);
                fWeight = 1f - fInterpolationValue;
            }

            m_EntityUI.SampleAnimation(GetClipName(behavior.Key), fTime % 1f, fWeight);
        }
    }

    private string GetClipName(string strBehaviorKey)
    {
        foreach(BehaviorAnimationInfo info in m_arrBehaviorAnimationInfo)
        {
            if (info.behaviorKey == strBehaviorKey)
                return info.clipName;
        }

        Debug.LogWarning("No clip found!, strBehaviorKey : " + strBehaviorKey);
        return "";
    }
}