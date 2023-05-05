using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObj/CreateEnemyData", order = int.MaxValue)]
public class EnemyScriptble : ScriptableObject
{
    [SerializeField]
    private ENEMY_KIND enemy_Kind;
    public ENEMY_KIND GetKind()
    { return enemy_Kind; }
    [SerializeField]
    private float sensing;
    public float GetSensing()
    { return sensing; }
    [SerializeField]
    private float attackLate;
    public float GetAttackLate()
    { return attackLate; }
    [SerializeField]
    private float attackRange;
    public float GetAttackRange()
    { return attackRange; }
    [SerializeField]
    private float effective_distance;
    public float GetEffecive()
    { return effective_distance; }
    [SerializeField]
    private float damage;
    public float GetDamage()
    { return damage; }
}
