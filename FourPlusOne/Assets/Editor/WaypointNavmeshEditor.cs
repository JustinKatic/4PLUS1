using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(WaypointNavmesh))]
public class WaypointNavmeshEditor : Editor
{
    WaypointNavmesh wayPoints;
    SerializedObject obj;
    GUIStyle uiStyle = new GUIStyle();

    public int selectedNode = 0;

    private void OnEnable()
    {
        wayPoints = (WaypointNavmesh)target;
        obj = new SerializedObject(target);

        uiStyle.alignment = TextAnchor.MiddleCenter;
        uiStyle.fontStyle = FontStyle.Bold;

        if(wayPoints.WayPoints == null || wayPoints.WayPoints.Count <= 0)
        {
            wayPoints.WayPoints.Add(wayPoints.transform.position + wayPoints.transform.forward);
        }
    }

    public override void OnInspectorGUI()
    {
        obj.Update();

        EditorGUILayout.PropertyField(obj.FindProperty("WayPoints"));

        if (GUILayout.Button("Add Point"))
        {
            int pointCount = wayPoints.WayPoints.Count;
            if (pointCount <= 1)
            {
                wayPoints.WayPoints.Add(wayPoints.WayPoints[0] + new Vector3(0, 1));
            }
            else
            {
                Vector3 normal = (wayPoints.WayPoints[pointCount - 1] - wayPoints.WayPoints[pointCount - 2]).normalized;
                wayPoints.WayPoints.Add(wayPoints.WayPoints[pointCount - 1] + normal);
            }
            SceneView.RepaintAll();
        }

        obj.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(wayPoints);
            EditorSceneManager.MarkSceneDirty(wayPoints.gameObject.scene);
        }
    }

    private void OnSceneGUI()
    {
        Event guiEvent = Event.current;

        for (int i = 0; i < wayPoints.WayPoints.Count; i++)
        {
            Handles.color = Color.white;

            if (i != 0)
            {
                if (!Application.isPlaying)
                {
                    wayPoints.WayPoints[i] = (Handles.PositionHandle(wayPoints.WayPoints[i], Quaternion.identity));
                    //Physics.Raycast(wayPoints.WayPoints[i],Vector3.down,out RaycastHit data,8);
                    //wayPoints.WayPoints[i] = data.point;
                }
            }
            else
            {
                if (!Application.isPlaying)
                {
                    wayPoints.WayPoints[i] = (wayPoints.transform.position);
                }
            }

            Handles.SphereHandleCap(0,wayPoints.WayPoints[i],Quaternion.identity, .25f,EventType.Repaint);

            if (i != wayPoints.WayPoints.Count - 1)
            {
                Handles.DrawDottedLine(wayPoints.WayPoints[i], wayPoints.WayPoints[i + 1], 8);
            }

            Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
            float drawPlaneDepth = 0;
            float dstToDrawPlane = (drawPlaneDepth - mouseRay.origin.y) / mouseRay.direction.y;
            Vector2 worldPos = mouseRay.GetPoint(dstToDrawPlane);

            //Handles.DrawSolidDisc(worldPos, Vector3.forward, 2);

            // wayPoints.WayPoints.Add(worldPos);


            Handles.color = Color.black;
            Handles.Label(wayPoints.WayPoints[i], i.ToString(), uiStyle);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(wayPoints);
            EditorSceneManager.MarkSceneDirty(wayPoints.gameObject.scene);
        }
    }
}
