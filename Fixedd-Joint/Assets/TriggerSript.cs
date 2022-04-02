using UnityEngine;

public class TriggerSript : MonoBehaviour
{

    public GameObject treeBase1;
    public GameObject treeBody1;
    // public GameObject test1;

    // Start is called before the first frame update
    void Start()
    {
        treeBase1 = GameObject.Find("base");
        treeBody1 = GameObject.Find("treeBody");

        float volumeTreeBody = VolumeOfMesh(treeBase1);
        float volumeTreeBase = VolumeOfMesh(treeBody1);


        string msg = "The volume of the Tree Body mesh is " + volumeTreeBody + " cube units.";
        string msg2 = "The volume of the Tree Base mesh is " + volumeTreeBase + " cube units.";

        Debug.Log(msg);
        Debug.Log(msg2);

        float treeDensity = 0.68f; // Плотность дерева ~ 680 кг/м^3

        treeBody1.GetComponent<Rigidbody>().mass = volumeTreeBody * treeDensity;
        treeBase1.GetComponent<Rigidbody>().mass = volumeTreeBase * treeDensity;

        CalculateSurfaceArea(treeBody1);

        // msg = "Mass of the Tree Body is set to " + volumeTreeBody * treeDensity;
        // msg2 = "Mass of the Tree Base is set to " + volumeTreeBase * treeDensity;

        // Debug.Log(msg);
        // Debug.Log(msg2);
    }
    // Mesh about volume конвес
    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Bumped in!");
        setComponentFixedJoint(treeBody1, treeBase1);
    }

    private void setComponentFixedJoint(GameObject first, GameObject second)
    {
        if (!first.GetComponent<FixedJoint>())
        {
            FixedJoint joint = first.AddComponent<FixedJoint>();
            joint.connectedBody = second.GetComponent<Rigidbody>();
            float massFirst = treeBody1.GetComponent<Rigidbody>().mass;
            float massSecond = treeBase1.GetComponent<Rigidbody>().mass;
            joint.breakForce = massFirst / massSecond * 1000f; // Поперечное сечение !!!!!!!!
            // Debug.Log("Break Froce is set to " + massFirst / massSecond);
        }
    }

    public float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;

        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }

    public float VolumeOfMesh(GameObject gameObject)
    {
        float volume = 0;

        Mesh mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);

        }
        return Mathf.Abs(volume) * gameObject.transform.localScale.x * gameObject.transform.localScale.y * gameObject.transform.localScale.z;
    }

    public void CalculateSurfaceArea(GameObject gameObject)
    {
        RaycastHit hit;
        // RaycastHit hit2;
        
        Physics.Raycast(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.down), out hit);
        
        // Ray ray = new Ray(hit.point, gameObject.transform.TransformDirection(Vector3.forward));
        Collider collider = hit.rigidbody.GetComponentInParent<Collider>();

        // collider.Raycast(ray, out hit2, 50f);

        Debug.Log(hit.point);
        Debug.Log(hit.rigidbody.name);
        // Debug.Log(hit2.point);
        // Debug.Log(hit.triangleIndex);
        // Debug.Log(hit.);


        Mesh mesh = hit.rigidbody.GetComponentInParent<MeshFilter>().sharedMesh;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        // Debug.Log(triangles[2]);

    }

}