using UnityEngine;
using System.Collections;
//for flipping textures inwards
using System.Linq;

public class OuterCylendarMovement : MonoBehaviour {

    public float vSpeedCalibration;

    private Vector2 materialOffset = Vector2.zero;
    private Material material;
    [SerializeField]
    private float vSpeed;

    void Start()
    {
        this.material = GetComponent<Renderer>().material;
        materialOffset = material.GetTextureOffset("_MainTex");

        //flip the texture to appear on the inside
        var mesh = (transform.GetComponent("MeshFilter") as MeshFilter).mesh;
        mesh.uv = mesh.uv.Select(o => new Vector2(1 - o.x, o.y)).ToArray();
        mesh.triangles = mesh.triangles.Reverse().ToArray();
        mesh.normals = mesh.normals.Select(o => -o).ToArray();
    }

    void Update()
    {
        materialOffset.y += vSpeedCalibration * vSpeed * Time.deltaTime;
        material.SetTextureOffset("_MainTex", materialOffset);
    }
}
