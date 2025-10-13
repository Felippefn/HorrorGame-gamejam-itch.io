using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    public bool loop = false;
    public List<Transform> points = new List<Transform>();
    void OnValidate()
    {
        Refresh();
    }

    void Reset()
    {
        Refresh();
    }
    
     public void Refresh()
    {
        points.Clear();
        foreach (Transform t in GetComponentsInChildren<Transform>())
            if (t != this.transform) points.Add(t);
    }
    void OnDrawGizmos()
    {
        if (points == null || points.Count < 2) return;
        Gizmos.matrix = Matrix4x4.identity;
        for (int i = 0; i < points.Count; i++)
        {
            var a = points[i].position;
            var b = points[(i + 1) % points.Count].position;
            Gizmos.DrawSphere(a, 0.1f);
            if (i < points.Count - 1 || loop) Gizmos.DrawLine(a, b);
        }
    }
}
