using UnityEngine;

[CreateAssetMenu(fileName = "PaData", menuName = "ScriptableObj/CreatePaData", order = int.MaxValue)]
public class PaScriptble : ScriptableObject
{
    [SerializeField]
    private float Hp;
    public float GetHp()
    {
        return Hp;
    }





    [SerializeField]
    private Sprite sprite;
    public Sprite GetSprite()
    {
        return sprite;
    }
}
