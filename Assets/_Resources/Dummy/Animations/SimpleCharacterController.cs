using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharacterController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //float moveX = 0f;
        //float moveZ = 0f;

        //if(Input.GetKey(KeyCode.W))
        //{
        //    moveZ += 1f;
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    moveZ -= 1f;
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    moveX -= 1f;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    moveZ += 1f;
        //}

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        // 유니티 엔진 1 단위는 1미터
        transform.Translate(new Vector3(moveX, 0f, moveZ).normalized * Time.deltaTime * 5f);
    }
}
