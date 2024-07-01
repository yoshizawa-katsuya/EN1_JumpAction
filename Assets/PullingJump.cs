using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//音楽：魔王魂

public class PullingJump : MonoBehaviour
{
    public int itemCount = 0;
    public Text itemCountText;
    public string newTag;

    private Rigidbody rb;

    public float squashFactor = 0.5f; // 潰れ具合の倍率
    public float lerpSpeed = 5.0f; // スムーズな変化の速度

    private Vector3 otherNormal;
    private Vector3 originalScale; // 元のスケール
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        originalScale = transform.localScale;
    }
    private Vector3 clickPosition;
    private bool isCanJump;
    [SerializeField]
    private float jumpPower = 10.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickPosition = Input.mousePosition;
        }
        if (isCanJump && Input.GetMouseButtonUp(0))
        {
            //クリックした座標と離した座標の差分を取得
            Vector3 dist = clickPosition - Input.mousePosition;
            //クリックとリリースが同じ座標ならば無視
            if (dist.sqrMagnitude == 0) { return; }
            //差分を標準化し、jumpPowerをかけ合わせた値を移動量とする。
            rb.velocity = dist.normalized * jumpPower;

            
        }

        if (isCanJump && Input.GetMouseButton(0))
        {
            Vector3 dist = clickPosition - Input.mousePosition;
            // 法線に基づいてスケールを調整
            Vector3 localNormal = transform.InverseTransformDirection(dist.normalized);
            Vector3 adjustedScale = originalScale;
            if (localNormal.y < 0)
            {
                localNormal.y *= -1;
            }
            if (localNormal.x < 0)
            {
                localNormal.x *= -1;
            }
            adjustedScale.y *= Mathf.Clamp01(1 - localNormal.y * squashFactor);
            adjustedScale.x *= Mathf.Clamp01(1 - localNormal.x * squashFactor);



            // スムーズにスケールを変更
            transform.localScale = Vector3.Lerp(transform.localScale, adjustedScale, Time.deltaTime * lerpSpeed);
        }

        if (Input.GetMouseButtonUp(0))
        {

            transform.localScale = originalScale;

        }

            itemCountText.text = "アイテム数:" + itemCount.ToString();

    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("衝突した");
    }

    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log("接触中");
        //衝突している点の情報が複数格納されている
        ContactPoint[] contacts = collision.contacts;
        //0番目の衝突情報から、衝突している点の法線を取得
        otherNormal = contacts[0].normal;
        //上方向を示すベクトル。長さは1
        Vector3 upVector = new Vector3(0, 1, 0);
        //上方向と法線の内積。二つのベクトルはともに長さが1なので、conθの結果がdotUN変数に入る。
        float dotUN = Vector3.Dot(upVector, otherNormal);
        //内積値に逆三角形関数arccosを掛けて角度を算出。それを度数法へ変換する。これで角度が算出できた。
        float dotDeg = Mathf.Acos(dotUN) * Mathf.Rad2Deg;
        //二つのベクトルがなす角度が45度より小さければ再びジャンプ可能とする。
        if (dotDeg <= 45)
        {
            isCanJump = true;
        }

        

    }

    private void OnCollisionExit(Collision collision)
    {
        //Debug.Log("離脱した");
        isCanJump = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            itemCount++;
            other.gameObject.tag = newTag;
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("JumpArea"))
        {
            isCanJump = true;
        }
        if (other.gameObject.CompareTag("FloatArea"))
        {
            rb.AddForce(-Physics.gravity * (rb.mass * 2.0f));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("JumpArea"))
        {
            isCanJump = false;
        }
    }

}
