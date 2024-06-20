using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMove : MonoBehaviour
{
    [SerializeField] float mouse_sensitivity = 0.001f;
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
            dx = Input.GetAxis("Mouse X") * mouse_sensitivity;
            dy = Input.GetAxis("Mouse Y") * mouse_sensitivity;


            //ドラッグした分だけオブジェクトを動かします。
            this.transform.Translate(dx, dy, 0.0f);
            if (this.transform.position.x < -820)
            {
                this.transform.position = new Vector3(-820, this.transform.position.y, this.transform.position.z);
            }
            else if (this.transform.position.x > 2820)
            {
                this.transform.position = new Vector3(2820, this.transform.position.y, this.transform.position.z);
            }
            if (this.transform.position.y < -1950)
            {
                this.transform.position = new Vector3(this.transform.position.x, -1950, this.transform.position.z);
            }
            else if (this.transform.position.y > 2000)
            {
                this.transform.position = new Vector3(this.transform.position.x, 2000, this.transform.position.z);
            }
        }
    }
}
