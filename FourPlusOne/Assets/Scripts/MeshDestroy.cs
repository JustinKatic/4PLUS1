using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based on a tutorial by DitzelGames on youtube

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshDestroy : MonoBehaviour
{
    private bool edgeSet = false;
    private Vector3 edgeVertex = Vector3.zero;
    private Vector2 edgeUV = Vector2.zero;
    private Plane edgePlane = new Plane();

    public AudioClip ClipToPlayOnBreak;


    [Range(1, 5)]
    [Tooltip("How many cuts, usually recommend 2-3")]
    public int CutCascades = 1;
    public float ExplodeForce = 0;

    [Tooltip("If a piece is created that's small enough (size vector  < 0.1f) destroys it.")]
    public bool DestoryTinyPieces = true;

    [HideInInspector]
    public SlapDetection LastSlapSource;

    public void DestoryMesh(SlapDetection slapSource)
    {
        if(ClipToPlayOnBreak != null && SoundPooler.SharedInstance != null)
        {
            SoundPooler.SoundObject sobj = SoundPooler.SharedInstance.GetPooledObject("SoundObject");

            sobj.WorldObject.transform.position = transform.position;
            sobj.WorldObject.SetActive(true);
            sobj.Source.PlayOneShot(ClipToPlayOnBreak);
        }

        LastSlapSource = slapSource;

        Mesh originalMesh = GetComponent<MeshFilter>().mesh;
        if (originalMesh == null) return; // Don't run method if we have no mesh

        originalMesh.RecalculateBounds();
        List<PartMesh> parts = new List<PartMesh>();
        List<PartMesh> subParts = new List<PartMesh>();

        PartMesh mainPart = new PartMesh();

        // Set parts variables
        mainPart.UV = originalMesh.uv;
        mainPart.Verticies = originalMesh.vertices;
        mainPart.Normals = originalMesh.normals;
        mainPart.Triangles = new int[originalMesh.subMeshCount][];
        mainPart.ObjectBounds = originalMesh.bounds;

        // Add Triangles to Main Part
        for (int i = 0; i < originalMesh.subMeshCount; i++)
        {
            mainPart.Triangles[i] = originalMesh.GetTriangles(i);
        }

        parts.Add(mainPart);

        for (int c = 0; c < CutCascades; c++)
        {
            for (int i = 0; i < parts.Count; i++)
            {
                Bounds bounds = parts[i].ObjectBounds;
                bounds.Expand(0.5f);

                //Randomly construct a plane to slice the mesh 
                Plane plane = new Plane(Random.onUnitSphere, new Vector3(Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y),
                    Random.Range(bounds.min.z, bounds.max.z)));

                subParts.Add(GenerateMesh(parts[i], plane, true));
                subParts.Add(GenerateMesh(parts[i], plane, false));
            }
            parts = new List<PartMesh>(subParts);
            subParts.Clear();
        }

        for (int i = 0; i < parts.Count; i++)
        {
            parts[i].MakeGameObject(this);

            //Give force to the parts
            if (parts[i].SpawnedObject.WorldObject != null)
                parts[i].SpawnedObject.Body.AddForceAtPosition(parts[i].ObjectBounds.center * ExplodeForce, transform.position);
        }

        Destroy(gameObject);
        parts.Clear();
        subParts.Clear();
    }

    private PartMesh GenerateMesh(PartMesh original, Plane plane, bool left)
    {
        PartMesh partMesh = new PartMesh();
        Ray ray1 = new Ray();
        Ray ray2 = new Ray();

        for (int i = 0; i < original.Triangles.Length; i++)
        {
            int[] triangles = original.Triangles[i];
            edgeSet = false;

            for (int j = 0; j < triangles.Length; j += 3)
            {
                bool sideA = plane.GetSide(original.Verticies[triangles[j]]) == left;
                bool sideB = plane.GetSide(original.Verticies[triangles[j + 1]]) == left;
                bool sideC = plane.GetSide(original.Verticies[triangles[j + 2]]) == left;

                int sideCount = ((sideA) ? 1 : 0) + ((sideB) ? 1 : 0) + ((sideC) ? 1 : 0);

                if (sideCount == 0) continue;

                if (sideCount == 3)
                {
                    partMesh.AddTriangle(i, original.Verticies[triangles[j]], original.Verticies[triangles[j + 1]], original.Verticies[triangles[j + 2]],
                        original.Normals[triangles[j]], original.Normals[triangles[j + 1]], original.Normals[triangles[j + 2]],
                        original.UV[triangles[j]], original.UV[triangles[j + 1]], original.UV[triangles[j + 2]]);

                    continue;
                }

                // Cut Points
                int singleIndex = (sideB == sideC) ? 0 : ((sideA == sideC) ? 1 : 2);

                int triIndex1 = triangles[j + singleIndex];
                int triIndex2 = triangles[j + ((singleIndex + 1) % 3)];
                int triIndex3 = triangles[j + ((singleIndex + 2) % 3)];

                ray1.origin = original.Verticies[triIndex1];
                Vector3 dir1 = original.Verticies[triIndex2] - original.Verticies[triIndex1];
                ray1.direction = dir1;
                plane.Raycast(ray1, out float enter1);
                float lerp1 = enter1 / dir1.magnitude;

                ray2.origin = original.Verticies[triIndex1];
                Vector3 dir2 = original.Verticies[triIndex3] - original.Verticies[triIndex1];
                ray2.direction = dir2;
                plane.Raycast(ray2, out float enter2);
                float lerp2 = enter2 / dir2.magnitude;

                //First Vertex is an anchor
                AddEdge(i, partMesh, (left) ? plane.normal * -1f : plane.normal,
                    ray1.origin + ray1.direction.normalized * enter1, ray2.origin + ray2.direction.normalized * enter2,
                    Vector2.Lerp(original.UV[triIndex1], original.UV[triIndex2], lerp1),
                    Vector2.Lerp(original.UV[triIndex1], original.UV[triIndex3], lerp1));

                if (sideCount == 1)
                {
                    partMesh.AddTriangle(i, original.Verticies[triIndex1],
                        ray1.origin + ray1.direction.normalized * enter1, ray2.origin + ray2.direction.normalized * enter2,
                        original.Normals[triIndex1],
                        Vector3.Lerp(original.Normals[triIndex1], original.Normals[triIndex2], lerp1),
                        Vector3.Lerp(original.Normals[triIndex1], original.Normals[triIndex3], lerp2),
                        original.UV[triIndex1],
                        Vector2.Lerp(original.UV[triIndex1], original.UV[triIndex2], lerp1),
                        Vector2.Lerp(original.UV[triIndex1], original.UV[triIndex3], lerp2));

                    continue;
                }

                if (sideCount == 2)
                {
                    partMesh.AddTriangle(i, ray1.origin + ray1.direction.normalized * enter1,
                        original.Verticies[triIndex2], original.Verticies[triIndex3],
                        Vector3.Lerp(original.Normals[triIndex1], original.Normals[triIndex2], lerp1),
                        original.Normals[triIndex2],
                        original.Normals[triIndex3],
                        Vector2.Lerp(original.UV[triIndex1], original.UV[triIndex2], lerp1),
                        original.UV[triIndex2],
                        original.UV[triIndex3]);

                    partMesh.AddTriangle(i, ray1.origin + ray1.direction.normalized * enter1,
                        original.Verticies[triIndex3],
                        ray2.origin + ray2.direction.normalized * enter2,
                        Vector3.Lerp(original.Normals[triIndex1], original.Normals[triIndex3], lerp1),
                        original.Normals[triIndex3],
                        Vector3.Lerp(original.Normals[triIndex1], original.Normals[triIndex3], lerp2),
                        Vector2.Lerp(original.UV[triIndex1], original.UV[triIndex2], lerp1),
                        original.UV[triIndex3],
                        Vector2.Lerp(original.UV[triIndex1], original.UV[triIndex3], lerp2));

                    continue;
                }
            }
        }
        partMesh.FillArrays();
        return partMesh;
    }

    private void AddEdge(int subMesh, PartMesh partMesh, Vector3 normal, Vector3 vertex1, Vector3 vertex2, Vector2 uv1, Vector2 uv2)
    {
        if (!edgeSet)
        {
            edgeSet = true;
            edgeVertex = vertex1;
            edgeUV = uv1;
        }
        else
        {
            edgePlane.Set3Points(edgeVertex, vertex1, vertex2);

            partMesh.AddTriangle(subMesh, edgeVertex, edgePlane.GetSide(edgeVertex + normal) ? vertex1 : vertex2,
                edgePlane.GetSide(edgeVertex + normal) ? vertex2 : vertex1, normal, normal, normal, edgeUV, uv1, uv2);
        }
    }

    public class PartMesh
    {
        private List<Vector3> lVerticies = new List<Vector3>();
        private List<Vector3> lNormals = new List<Vector3>();
        private List<List<int>> lTriangles = new List<List<int>>();
        private List<Vector2> lUVs = new List<Vector2>();

        public Vector3[] Verticies;
        public Vector3[] Normals;
        public int[][] Triangles;
        public Vector2[] UV;
        public DebriePooler.Debrie SpawnedObject;
        public Bounds ObjectBounds = new Bounds();

        public void AddTriangle(int submesh, Vector3 vert1, Vector3 vert2, Vector3 vert3, Vector3 normal1, Vector3 normal2, Vector3 normal3,
            Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            if (lTriangles.Count - 1 < submesh)
            {
                //Add triangles data to lists
                lTriangles.Add(new List<int>());
            }

            lTriangles[submesh].Add(lVerticies.Count);
            lVerticies.Add(vert1);
            lTriangles[submesh].Add(lVerticies.Count);
            lVerticies.Add(vert2);
            lTriangles[submesh].Add(lVerticies.Count);
            lVerticies.Add(vert3);

            lNormals.Add(normal1);
            lNormals.Add(normal2);
            lNormals.Add(normal3);

            lUVs.Add(uv1);
            lUVs.Add(uv2);
            lUVs.Add(uv3);

            //Resize object bounds to fit the new verticies
            ObjectBounds.min = Vector3.Min(ObjectBounds.min, vert1);
            ObjectBounds.min = Vector3.Min(ObjectBounds.min, vert2);
            ObjectBounds.min = Vector3.Min(ObjectBounds.min, vert3);
            ObjectBounds.max = Vector3.Min(ObjectBounds.max, vert1);
            ObjectBounds.max = Vector3.Min(ObjectBounds.max, vert2);
            ObjectBounds.max = Vector3.Min(ObjectBounds.max, vert3);
        }

        public void FillArrays()
        {
            Verticies = lVerticies.ToArray();
            Normals = lNormals.ToArray();
            UV = lUVs.ToArray();

            //Move nested triangles list into 2D array
            Triangles = new int[lTriangles.Count][];
            for (int i = 0; i < lTriangles.Count; i++)
            {
                Triangles[i] = lTriangles[i].ToArray();
            }
        }

        public void MakeGameObject(MeshDestroy original)
        {
            SpawnedObject = DebriePooler.SharedInstance.GetPooledObject("Debrie");
            SpawnedObject.WorldObject.transform.position = original.transform.position;
            SpawnedObject.WorldObject.transform.rotation = original.transform.rotation;
            SpawnedObject.WorldObject.transform.localScale = original.transform.lossyScale;

            Mesh mesh = new Mesh();
            MeshFilter originalFilter = original.GetComponent<MeshFilter>();
            mesh.name = originalFilter.mesh.name;

            //Fill mesh with new data
            mesh.vertices = Verticies;
            mesh.normals = Normals;
            mesh.uv = UV;

            for (int i = 0; i < Triangles.Length; i++)
            {
                mesh.SetTriangles(Triangles[i], i, true);
            }

            ObjectBounds = mesh.bounds;

            if (ObjectBounds.size.magnitude < 0.1f)
            {
                SpawnedObject.WorldObject.SetActive(false);
                SpawnedObject.WorldObject = null;
                return;
            }

            SpawnedObject.WorldObject.SetActive(true);

            Rigidbody originalBody = original.GetComponent<Rigidbody>();

            //Add Correct Components to game object
            SpawnedObject.Renderer.materials = original.GetComponent<MeshRenderer>().materials;
            SpawnedObject.Filter.mesh = mesh;
            SpawnedObject.Collider.sharedMesh = mesh;

            SpawnedObject.Collider.convex = true; // Needs to be true so rigidbodies work with mesh colliders

            if (originalBody != null)
            {
                SpawnedObject.Body.AddRelativeForce(originalBody.velocity + ((original.LastSlapSource != null) ? original.LastSlapSource.PreviousHitVelocity : Vector3.zero), ForceMode.Impulse);
                float massMulti = (ObjectBounds.size.magnitude / originalFilter.mesh.bounds.size.magnitude);
                SpawnedObject.Body.mass = originalBody.mass * massMulti; // Gives a very rough apporximation of the objects mass
            }
        }
    }
}
