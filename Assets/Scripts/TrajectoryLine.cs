using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRender;
    [SerializeField, Min(3)]
    private int segments = 10;
    [SerializeField, Min(1)]
    private float timeofFlight = 5;

    public void ShowTLine(Vector3 startpoint, Vector3 startVelocity)
    {
        float timeStep = timeofFlight / segments;

        Vector3[] lineRenderPoints = CalculateTrajLine(startpoint, startVelocity, timeStep);

        lineRender.positionCount = segments;
        lineRender.SetPositions(lineRenderPoints);


    }


    private Vector3[] CalculateTrajLine(Vector3 start, Vector3 velocity, float timeStep)
    {
        Vector3[] lineRendererPoints = new Vector3[segments];

        lineRendererPoints[0] = start;

        for (int i = 1; i < segments; i++)
        {
            float timeOffset = timeStep * i;

            Vector3 progressBeforeGrav = velocity * timeOffset;
            Vector3 gravOffset = Vector3.up * -0.5f * Physics.gravity.y * timeOffset * timeOffset;
            Vector3 newPos = start + progressBeforeGrav - gravOffset;
            lineRendererPoints[i] = newPos;
        }
        return lineRendererPoints;
    }

}
