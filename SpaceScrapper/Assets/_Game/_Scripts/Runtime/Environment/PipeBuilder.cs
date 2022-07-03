using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeBuilder : MonoBehaviour
{
    [SerializeField]
    MeshFilter filter;
    Mesh mesh;

    //SETTINGS FOR GENERATION:
    [SerializeField, Range(4, 25)]
    int divisions = 20;
    [SerializeField, Range(0.01f, 1f)]
    float radius = 0.25f;
    [SerializeField]
    [UnityEngine.Serialization.FormerlySerializedAs("points")]
    public Vector3[] controlPoints = { Vector3.zero, Vector3.forward };
    [SerializeField, Range(5, 30)]
    float angleLimit = 10f;
    [SerializeField, Range(0.1f, 10f)]
    float cornerRadius = 1f;
    [SerializeField]
    bool useAverageDirection = true;
    [SerializeField]
    bool loop = false;

    private int vertsPerRing = 0;
    private float vertsPerRingF = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Build();
    }

    private void OnValidate() 
    {
        //Check for two or more duplicate points in a row. would cause issues.
        for(int i = 0; i < controlPoints.Length-1; i++)
        {
            if(controlPoints[i] == controlPoints[i+1])
            {
                controlPoints[i+1] += new Vector3(0,0,0.5f); 
            }
        }
        //TODO: Check if changes have been made.
        Build();
    }

    public void Build()
    {
        filter = filter ?? GetComponent<MeshFilter>();
        if(mesh)
            mesh.Clear();
        else
            mesh = new Mesh();
        Vector3[] validatedPoints;
        if(loop)
        {
            Vector3[] buffer = new Vector3[controlPoints.Length+1];
            int i = 0;
            for(; i < controlPoints.Length; i++)
                buffer[i] = controlPoints[i];
            buffer[i] = controlPoints[0];
            validatedPoints = ValidatePoints(buffer);
        }
        else
            validatedPoints = ValidatePoints(controlPoints);
        vertsPerRing = divisions + 1;
        vertsPerRingF = (float) vertsPerRing;
        CreatePipeMesh(validatedPoints);
    }

    //Validate the points in the points array. "insert" more points to make smooth corners.
    Vector3[] ValidatePoints(Vector3[] originalPoints)
    {
        List<Vector3> validatedPoints = new List<Vector3>(originalPoints.Length);
        validatedPoints.Add(originalPoints[0]);
        for(int i = 1; i < originalPoints.Length-1; i++)
        {
            Vector3 prevDir = originalPoints[i-1] - originalPoints[i]; prevDir.Normalize();
            Vector3 nextDir = originalPoints[i+1] - originalPoints[i]; nextDir.Normalize();
            float innerAngle = Vector3.Angle(prevDir, nextDir);
            //the angle of the turn.
            float turnAngle = 180f - innerAngle;
            //check whether the smallest angle (180-inner) is greater than the limit for it to be considered a corner.
            if(turnAngle >= angleLimit)
            {
                //The normal of the plane created by prevDir, nextDir
                Vector3 planeNormal = (Vector3.Cross(nextDir, prevDir)).normalized;
                //The vector that divides the angle between the plane vectors.
                Vector3 dividingVector = (prevDir + nextDir) * 0.5f;
                dividingVector.Normalize();
                //the angle between the dividingVector and any of the plane vectors.
                float beta = innerAngle * 0.5f;
                //The distance between the controlPoint and the center of the curve M
                float distanceCPM = cornerRadius / Mathf.Sin(beta * Mathf.Deg2Rad);
                //The center of circle that defines the roundness of the corner 
                Vector3 cornerCenter = originalPoints[i] + distanceCPM * dividingVector;
                //The angle at the center of the circle.
                float gamma = 180f - (90f + beta);
                //the distance between the corner vertex and the tangent of the circle.
                float tangentPointDistance = Mathf.Sin(gamma * Mathf.Deg2Rad) * distanceCPM;
                //The point where the tangent touches the circle.
                Vector3 tangentPoint = originalPoints[i] + prevDir * tangentPointDistance;
                
                //The offset from the center of the circle where the first vertex will be placed.
                Vector3 startOffset = tangentPoint - cornerCenter;
                //add the tangentPoint.
                validatedPoints.Add(tangentPoint);
                //Now add points throughout the curve.
                //points are "rotated" around the center, so the center angle is important in this case.
                float curveAngleBounds = 2f * gamma;
                for(float offsetAngle = (curveAngleBounds % angleLimit) * 0.5f; offsetAngle < curveAngleBounds; offsetAngle += angleLimit)
                {
                    Vector3 vertex = cornerCenter + Quaternion.AngleAxis(offsetAngle, planeNormal) * startOffset;
                    validatedPoints.Add(vertex);
                }
                //Add the other tangentPoint
                validatedPoints.Add(cornerCenter + Quaternion.AngleAxis(curveAngleBounds, planeNormal) * startOffset);
            }
            else
            {
                //not a hard corner. just add the control point.
                validatedPoints.Add(originalPoints[i]);
            }
        }
        validatedPoints.Add(originalPoints[originalPoints.Length-1]);
        return validatedPoints.ToArray();
    }

    //Creates the pipe mesh with the points its given.
    void CreatePipeMesh(Vector3[] points)
    {
        if(points.Length < 2)
            return;

        int n = vertsPerRing * points.Length;
        float angleOffset = 360f / (float)divisions;
        Vector3[] vertices = new Vector3[n];
        Vector3[] normals = new Vector3[n];
        int[] tris = new int[(points.Length * divisions) * 6];

        //Make the first set of vertices.
        Vector3 offsetDirection = Vector3.up;
        Vector3 forwardDirection = Vector3.forward;
        Vector3 previousForward = Vector3.forward;
        Quaternion rotationQuat = Quaternion.AngleAxis(angleOffset, forwardDirection);
        //the plane of the backward and the forward directions.
        for(int i = 0; i < points.Length; i++)
        {
            //figure out the forward direction from the current node to the next.
            if(i < points.Length -1)
                forwardDirection = points[i+1]-points[i];
            else
                forwardDirection = points[i]-points[i-1];
            forwardDirection.Normalize();

            //average the direction from the previous to the next nodes.
            //This helps smooth it out a bit.
            Vector3 avg = forwardDirection;
            if(i > 0)
            {
                avg = (previousForward + forwardDirection) * 0.5f;
                avg.Normalize();
            }
            previousForward = forwardDirection;

            //set up the rotation quaternion and the offset direction(normal) of the node.
            rotationQuat = Quaternion.AngleAxis(angleOffset, useAverageDirection? avg : forwardDirection);
            offsetDirection = Quaternion.LookRotation(useAverageDirection? avg : forwardDirection, Vector3.up) * Vector3.up; 
            int startVertex = i*vertsPerRing;
            //place vertices around the node in a regular pattern.
            for(int j = 0; j < divisions; j++)
            {
                vertices[startVertex + j] = points[i] + offsetDirection * radius;
                normals[startVertex + j] = offsetDirection;
                offsetDirection = rotationQuat * offsetDirection;
            }
            //place a duplicate of the starting vertex of the ring.
            vertices[startVertex + divisions] = vertices[startVertex];
            normals[startVertex + divisions] = normals[startVertex];
        }

        //TODO: Detect previously impossible angles, where the pipe would be twisted.
        //1. check the turn angle
        //2. offset the tris for the segment by an amount that approximates the turn in degrees.

        //Make the triangles.
        int index = 0;
        for(int i = 0; i < points.Length-1; i++)
        {
            //the min and max vertex index of the current ring.
            int ringStart = i * vertsPerRing;
            int ringEnd = ringStart + divisions; 

            for(int v = ringStart; v < ringEnd; v++)
            {
                tris[index    ] = v ;
                tris[index + 1] = v + 1;
                tris[index + 2] = v + 1 + vertsPerRing;
                tris[index + 3] = v;
                tris[index + 4] = v + 1 + vertsPerRing;
                tris[index + 5] = v + vertsPerRing;

                index += 6;
            }
        }

        //Create the UVs
        Vector2[] uv = new Vector2[vertices.Length];
        float uvOffsetY = 0f;
        //the baseline ring.
        for(float vert = 0; vert < vertsPerRingF; vert++)
            uv[(int)vert] = new Vector2(vert/(float)divisions, 0f);
        //every ring afterwards.
        for(int ringIndex = 1; ringIndex < points.Length; ringIndex++)
        {
            //max distance between vertices.
            int ringStart = ringIndex*vertsPerRing;
            int ringEnd = ringStart + vertsPerRing-1;
            float maxSqrDist = -1f;
            //loop through
            for(int i = ringStart; i <= ringEnd; i++)
            {
                //distance from current to previous vertex
                float cDist = Vector3.SqrMagnitude(vertices[i] - vertices[i-vertsPerRing]);
                maxSqrDist = (maxSqrDist > cDist? maxSqrDist : cDist);
            }
            //increase offset on Y.
            uvOffsetY += Mathf.Sqrt(maxSqrDist);
            for(float i = 0; i < vertsPerRingF; i++)
            {
                int vert = ringStart + (int)i;
                uv[vert] = new Vector2(i/(float)divisions, uvOffsetY);
            }
        }

        //assign the mesh and load it.
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.uv = uv;
        
        mesh.UploadMeshData(false);
        mesh.normals = normals;
        //mesh.RecalculateNormals();
        filter.sharedMesh = mesh;
    }

    //Verify that the vertex is on the correct ring of vertices. loop back to first / last when out of bounds.
    int LoopVertex(int ringStart, int ringEnd, int vertex)
    {
        if(vertex > ringEnd)
            return vertex - divisions;
        if(vertex < ringStart)
            return vertex + divisions;
        return vertex;
    }
}
