using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    [SerializeField] float mouse_sensitivity = 0.001f;
    [SerializeField] int x;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //ドラッグ中もしくはスワイプ中
        if (Input.GetMouseButton(1))
        {
            float dx, dy;

            //PCマウスの場合

            dy = Input.GetAxis("Mouse Y") * mouse_sensitivity;

            //ドラッグした分だけオブジェクトを動かします。
            this.transform.Translate(0, dy, 0.0f);

            if (this.transform.position.x < -820)
            {
                this.transform.position = new Vector3(x, this.transform.position.y, this.transform.position.z);
            }
            else if (this.transform.position.x > 2820)
            {
                this.transform.position = new Vector3(x, this.transform.position.y, this.transform.position.z);
            }
        }
    }
}
