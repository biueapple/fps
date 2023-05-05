using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Codice.Client.BaseCommands;

[CustomEditor(typeof(CreateUnit))]
public class Editor_CreateUnit : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CreateUnit createUnit = (CreateUnit)target;
        if (GUILayout.Button("View Position"))
        {
            if(createUnit.GetViewTransform() == null)
            {
                GameObject obj = new GameObject();
                obj.name = "Position";
                obj.transform.parent = createUnit.transform;
                createUnit.SetViewTransform(obj.transform);
            }
        }
        if (GUILayout.Button("Create Unit"))
        {
            createUnit.Init();
            if(createUnit.GetViewTransform() != null)
            {
                Enemy enemy = createUnit.GetCreateEnemy(createUnit.kind);
                enemy.transform.position = createUnit.GetViewTransform().position;
            }
            else
            {
                createUnit.GetCreateEnemy(createUnit.kind);
            }
        }
        if (GUILayout.Button("√ ±‚»≠"))
        {
            createUnit.Init_Force();
        }
    }
}
