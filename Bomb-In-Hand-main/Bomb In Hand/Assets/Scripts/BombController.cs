using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController: MonoBehaviour
{
    public float delay = 3f;
    public float radius = 5f;
    bool hasExploded = false;
    public float force = 700f;

    public float g = -10;           // 重力加速度

    public Vector3 speed;          // 初速度向量
    private Vector3 Gravity;        // 重力向量
    private float dTime = 0;        // 时间线 (一直在增长)
    private int go; // whether throwing
    private Transform m_Transform;

    public GameObject exposioneffect;

    float countdown;
    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
        Gravity = Vector3.zero;
        go = 0;
        m_Transform = gameObject.GetComponent<Transform>();
        speed = new Vector3(2, 2, 2);

    }

    // Update is called once per frame
    void Update()
    {

        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            hasExploded = true;
            Explode();
        }

        if (Input.GetKey(KeyCode.K))
        {
            speed.z = speed.z + 1;
        }
        if (Input.GetKey(KeyCode.L))
        {
            go = 1;
        }
        if (go == 1)
        {
            // 重力模拟
            Gravity.y = g * (dTime += Time.deltaTime);  //v=gt
            // 模拟位移
            transform.Translate(speed * Time.deltaTime);
            transform.Translate(Gravity * Time.deltaTime);

        }

    }
    void Explode()
    {
        Instantiate(exposioneffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }
        }

        Debug.Log("bomb");

        Destroy(gameObject);
    }
}
