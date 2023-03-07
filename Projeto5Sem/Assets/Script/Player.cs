using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 5;
    [SerializeField] private float turnSpeed = 360;

    
    public GameObject espadaAtack;
    public float CDAtack;

    private Vector3 input;

    void Start()
    {
        rb.GetComponent<Rigidbody>();
    }
    void Update()
    {
        GatherInput();
        Look();
        AtackMelee();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void GatherInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

    }
    void Look()
    {
        if( input != Vector3.zero)
        {

            var relative = (transform.position + input.toIso()) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation,rot,turnSpeed); //rotação mais demorada (...turnSpeed * Time.deltatime)
        }

    }

    void Move()
    {
        rb.MovePosition(transform.position + (transform.forward * input.magnitude)* speed * Time.deltaTime);
    }

    void AtackMelee()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            StartCoroutine("CDAtackMelee");
        }
    }

    IEnumerator CDAtackMelee()
    {
        espadaAtack.SetActive(true);
        yield return new WaitForSeconds(CDAtack);
        espadaAtack.SetActive(false);
    }
}
