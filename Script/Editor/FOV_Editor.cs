using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Enemy))]
public class FOV_Editor : Editor
{
    private void OnSceneGUI()
    {
        Enemy enemy = (Enemy)target;

        Vector3 fromAnglePos = enemy.CirclePoint(-enemy.enemyScriptble.GetViewAngle() * 0.5f);

        Handles.color = new Color(1, 1, 1, 0.2f);

        Handles.DrawWireDisc(enemy.transform.position, Vector3.up, enemy.enemyScriptble.GetSensing());

        Handles.DrawSolidArc(enemy.transform.position, Vector3.up, fromAnglePos, enemy.enemyScriptble.GetViewAngle(), enemy.enemyScriptble.GetSensing());

        Handles.Label(enemy.transform.position + (enemy.transform.forward * 2.0f), enemy.enemyScriptble.GetViewAngle().ToString());
    }
}
