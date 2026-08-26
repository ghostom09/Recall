using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "Scriptable Objects/PlayerStat")]
public class PlayerStat : ScriptableObject
{
    public int MaxHealth;
    public int Damage;
    public float Speed;
    public float DashCooldown;
    public float jumpPower;
}
