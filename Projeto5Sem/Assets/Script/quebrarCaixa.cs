using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quebrarCaixa : MonoBehaviour
{

    public MeshRenderer caixa;
    public Collider col;
    public GameObject effect;
    public GameObject destrocosSpawn;
    public GameObject destrocosSpawn1;
    public GameObject destrocosSpawn2;
    public GameObject destrocos;
    public GameObject destrocos1;
    public GameObject destrocos2;
    public GameObject radiacao;

    void Start()
    {
        effect.SetActive(false);
        destrocos.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "espada")
        {
            caixa.enabled = false;
            col.enabled = false;
            destrocos.SetActive(true);
            Instantiate(destrocos, destrocosSpawn.transform.position, destrocosSpawn.transform.rotation);
            Instantiate(destrocos1, destrocosSpawn1.transform.position, destrocosSpawn.transform.rotation);
            Instantiate(destrocos2, destrocosSpawn2.transform.position, destrocosSpawn.transform.rotation);
            Instantiate(radiacao, destrocosSpawn2.transform.position, destrocosSpawn.transform.rotation);
            animacao();
        }
    }
    
    IEnumerator animacao()
    {
        effect.SetActive(true);
        yield return new WaitForSeconds(1);
        effect.SetActive(false);
        Destroy(gameObject, 5);
    }
}
