using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    //Part one of  2 part tutorial by Sebastian Lague https://www.youtube.com/watch?v=rQG9aUWarwE "Field Of view visualisation(E01)" uploaded Dec 26 2015
    //Part 2 https://www.youtube.com/watch?v=73Dc5JTCmKI
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();

    public float meshResolution;
    public float edgeDstThreshhold;
    public int rayResolveIterations;

    public MeshFilter viewMeshFilter;

    Mesh viewMesh;

    private void Start()
    {
        
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        StartCoroutine(FindTargetsWithDelay(.2f));
    }
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    void FindVisibleTargets() // finds any targets within the view angle 
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                    foreach(Transform transform in visibleTargets)
                    {
                        MeshRenderer renderer = transform.gameObject.GetComponent<MeshRenderer>();
                        Material material = renderer.material;
                        renderer.enabled = true;
                        if(transform.tag == "NPC")
                        {
                            transform.GetComponent<EnemyController>().enemyNameText.gameObject.SetActive(true);
                            transform.GetComponent<EnemyController>().weapon.gameObject.SetActive(true);
                        }
                        
                        
                    }
                }
            }
        }
    }
    void DrawFieldOfView() // draws a field of view between the angle if the radius
    {
        int rayCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float rayAngleSize = viewAngle / rayCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= rayCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + rayAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshhold;
            if (i > 0)
            {
                
                if(oldViewCast.hit != newViewCast.hit)// used to find the info of corners to stop the view glitching round corners
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if(edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
            //Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * viewRadius, Color.red);
        }
        int vertexCount = viewPoints.Count + 1;// gets all the vertexs to draw the mesh
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }
    ViewCastInfo ViewCast(float globalAngle) // raycasts finding the point that was hit and the distance
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;
        if (Physics.Raycast(transform.position,dir,out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
            
        }
        else
        {
            return new ViewCastInfo(false, transform.position +dir * viewRadius, viewRadius, globalAngle);
        }
    }
    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast) // finds the edge and gets the min and max points raycast out from the player
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;
        for (int i = 0; i < rayResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);
            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshhold;
            if (newViewCast.hit == minViewCast.hit)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }
        return new EdgeInfo(minPoint, maxPoint);
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo // stores infos
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;
        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }
    public struct EdgeInfo // stores edge
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
