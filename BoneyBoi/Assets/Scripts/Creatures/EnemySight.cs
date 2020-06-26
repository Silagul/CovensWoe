using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField]
    private LayerMask layerMask;    //layerMask used to determine what fov hits
    private Mesh mesh;  //the mesh that shows the field of vision
    private Vector3 origin;  //origin point of the fov
    private float angle = 0f;   //the angle of the fov
    private float angleIncrease;    //the value for how much the angle is increased after every ray
    private float timeInSight = 0f;
    private bool killingChild = false;
    //private bool calmingDown = false;

    public float timeToDie;
    public float startingAngle;    //starting angle of the fov
    public float fov = 90f;    //the fov of the enemy
    public int rayCount = 50;  //the amount of rays, adding more makes the fov more rounded and more accurate
    public float viewDistance = 5f;    //length of the fov
    public static bool shouldDie = false;

    void Start()
    {
        gameManager = GameObject.Find("Game").GetComponent<GameManager>();
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        angleIncrease = fov / rayCount;   //this is used to count how much we need to increase the angle for the next ray
        //StartCoroutine("CalmingDown");
    }

    private void Update()
    {
        //Debug.Log(timeInSight);
        origin = transform.position;
        angle = startingAngle;
        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero;

        bool childVisible = false;
        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, gameManager.GetVectorFromAngle(angle), viewDistance, layerMask);

            if (raycastHit2D.collider == null)  //no hit
            {
                vertex = gameManager.GetVectorFromAngle(angle) * viewDistance;
            }

            else   //hit
            {
                vertex = transform.InverseTransformPoint(raycastHit2D.point);
                if (raycastHit2D.collider.name == "Human")
                {
                    Debug.Log("Seeing Player");
                    if (killingChild == false)
                        childVisible = true;
                }

                //else
                //{
                //    if (calmingDown == false && killingChild == false)
                //    {
                //        StartCoroutine("CalmingDown");
                //    }
                //}
            }

            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1; ;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        if (childVisible)
            shouldDie = true;
        else
            shouldDie = false;
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    private void FixedUpdate()
    {
        if(timeInSight >= 0f)
        {
            timeInSight -= 0.01f;
        }
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection)   //this sets the starting angle of the fov to the middle of the direction that the enemy is facing
    {
        startingAngle = gameManager.GetAngleFromVectorFloat(aimDirection) - fov / 2f;
    }

    private IEnumerator KillingChild()
    {
        killingChild = true;
        Creature.dying = true;
        if (timeInSight < timeToDie)
        {
            yield return new WaitForSeconds(1);
            timeInSight += 1f;
        }
        else
        {
            //Debug.Log("Wasted");
            GameObject.Find("Player").GetComponent<PlayerTest>().Wasted(); //replace this with the players death function
        }
        killingChild = false;
    }

    //private IEnumerator CalmingDown()
    //{
    //    calmingDown = true;
    //    if (timeInSight > 0)
    //    {
    //        yield return new WaitForSeconds(1);
    //        timeInSight -= 1f;
    //        StartCoroutine("CalmingDown");
    //    }
    //    calmingDown = false;
    //}
}
