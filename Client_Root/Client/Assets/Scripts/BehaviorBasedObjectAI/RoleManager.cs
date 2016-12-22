using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour
{
    [SerializeField] BehaviorBasedObject m_Target = null;

    private Role m_Role = Role.NPC;

    public enum Role
    {
        Challenger,
        Obstacle,
        NPC,
    }

    public void SetRole(Role role)
    {
        m_Role = role;
    }

    public Role GetRole()
    {
        return m_Role;
    }
}
