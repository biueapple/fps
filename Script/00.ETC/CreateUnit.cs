using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class CreateUnit : MonoBehaviour
{
    private Unit[] characters;
    private Unit[] enemies;
    private Transform position;
    [Header("¸¸µé Àû")]
    public ENEMY_KIND kind;

    private void Awake()
    {
        if(characters == null)
            characters = Resources.LoadAll<Unit>("Character");
        if(enemies == null)
            enemies = Resources.LoadAll<Unit>("Enemy");
    }
    public void Init()
    {
        if (characters == null)
            characters = Resources.LoadAll<Unit>("Character");
        if (enemies == null)
            enemies = Resources.LoadAll<Unit>("Enemy");
    }
    public void Init_Force()
    {
        characters = Resources.LoadAll<Unit>("Character");
        enemies = Resources.LoadAll<Unit>("Enemy");
    }
    public Enemy GetCreateEnemy(ENEMY_KIND kind)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].GetComponent<Enemy>().enemyScriptble.GetKind() == kind)
            {
                Enemy enemy = Instantiate(enemies[i].GetComponent<Enemy>());
                enemy.Init();
                return enemy;
            }
        }
        return null;
    }
    public Enemy[] GetCreateEnemy(ENEMY_KIND kind, int count)
    {
        Enemy[] list = new Enemy[count];
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].GetComponent<Enemy>().enemyScriptble.GetKind() == kind)
            {
                for (int j = 0; j < count; j++)
                {
                    list[j] = Instantiate(enemies[i].GetComponent<Enemy>());
                    list[j].Init();
                }
                break;
            }
        }
        return list;
    }
    public Ch GetCreateCharacter(Ch ch)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (ch == characters[i])
            {
                Ch c = Instantiate(characters[i].GetComponent<Ch>());
                c.Init();
                return c;
            }
        }
        return null;
    }
    public Ch GetCreateCharacter(int index)
    {
        if(index < 0 || index >= characters.Length)
            return null;
        Ch c = Instantiate(characters[index].GetComponent<Ch>());
        c.Init();
        return c;
    }

    public Transform GetViewTransform()
    {
        return position;   
    }
    public void SetViewTransform(Transform t)
    {
        position = t;
    }
    public Unit[] GetCharacters()
    {
        return characters;
    }
    public Unit[] GetEnemys()
    {
        return enemies;
    }
}
