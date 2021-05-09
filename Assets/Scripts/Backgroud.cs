using UnityEngine;
using System.Collections;

public class Backgroud : MonoBehaviour
{
    public float ScrollSpeed = 0.1f;
    private MeshRenderer render;
    private float Offset;

    //렌더러 설정
    private void Start()
    {
        render = GetComponent<MeshRenderer>();
    }
    //바탕화면을 움직임
    void Update()
    {
        Offset += Time.deltaTime * ScrollSpeed;
        render.material.mainTextureOffset = new Vector2(Offset, 0);
    }
}
