using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStat", menuName = "Scriptable Objects/EnemyStat")]
public class EnemyStat : ScriptableObject
{
    public int MaxHealth;
    public int Damage;
    public int Speed;
}
