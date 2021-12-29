using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 贝塞尔曲线
/// </summary>
public class BezierCurves 
{
    /// <summary>
    /// 三次贝塞尔
    /// </summary>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3 Bezier_3(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) { 
        return (1 - t) * ((1 - t) * ((1 - t) * p0 + t * p1) + t * ((1 - t) * p1 + t * p2)) + t * ((1 - t) * ((1 - t) * p1 + t * p2) + t * ((1 - t) * p2 + t * p3));
    }
    /// <summary>
    /// 二次贝塞尔
    /// </summary>
    public static Vector3 Bezier_2(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return (1 - t) * ((1 - t) * p0 + t * p1) + t * ((1 - t) * p1 + t * p2);
    }

    /// <summary>
    /// 获取曲线的坐标,count:两点之间分段的数量
    /// </summary>
    /// <param name="targets"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static List<Vector3> GetDrawPoints(List<Vector3> targets,int count) {
        List<Vector3> points = new List<Vector3>();
        for (int i=0;i<targets.Count-3;i+=3) {
            Vector3 p0 = targets[i];
            Vector3 p1 = targets[i+1];
            Vector3 p2 = targets[i+2];
            Vector3 p3 = targets[i+3];
            if (i==0) { //获取第一个点
                points.Add(Bezier_3(p0, p1, p2, p3, 0));
            }
            for (int j=1;j<count;j++) {
                float t = j / (float)count;
                points.Add(Bezier_3(p0, p1, p2, p3, t));
            }
        }
        return points;
    }
    public static List<Vector3> GetDrawPoints_2(List<Vector3> targets, int count)
    {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < targets.Count - 2; i += 2)
        {
            Vector3 p0 = targets[i];
            Vector3 p1 = targets[i + 1];
            Vector3 p2 = targets[i + 2];
            if (i == 0)
            { //获取第一个点
                points.Add(Bezier_2(p0, p1, p2, 0));
            }
            for (int j = 1; j < count; j++)
            {
                float t = j / (float)count;
                points.Add(Bezier_2(p0, p1, p2,  t));
            }
        }
        return points;
    }
    
}
