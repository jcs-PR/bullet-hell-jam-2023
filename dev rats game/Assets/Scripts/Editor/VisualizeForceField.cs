using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ForceField))]
public class VisualizeForceField : Editor
{
    void OnSceneGUI()
    {
        ForceField field = (ForceField)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(field.transform.position, Vector3.forward, Vector3.up, 360f, field.radius);
        Vector3 viewAngleA = field.DirFromAngle(-field.angle / 2);
        Vector3 viewAngleB = field.DirFromAngle(field.angle / 2);

        Handles.DrawLine(field.transform.position, field.transform.position + viewAngleA * field.radius);
        Handles.DrawLine(field.transform.position, field.transform.position + viewAngleB * field.radius);
    }
}