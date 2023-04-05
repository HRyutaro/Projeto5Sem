using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParedeDinamica : MonoBehaviour
{
    private ParedeTransparente paredeTransScript;
    private Renderer renderMaterial = new Renderer();
    public List<GameObject> paredesTransparentes;

    void Start()
    {
        GameObject transp = GameObject.Find("Main Camera");
        paredeTransScript = transp.GetComponent<ParedeTransparente>();

        renderMaterial = gameObject.GetComponent<Renderer>();
        GameObject[] paredes = GameObject.FindGameObjectsWithTag("ParedeTransparente");
        foreach (GameObject parede in paredes)
        {
            paredesTransparentes.Add(parede);
        }
    }


    void Update()
    {
        foreach (GameObject parede in paredesTransparentes)
        {
            Renderer renderMaterial = parede.GetComponent<Renderer>();
            for (int m = 0; m < renderMaterial.materials.Length; m ++)
            {
                if(paredeTransScript.hitPoint.transform == transform)
                {
                    if(renderMaterial.materials[m].color.a > 0.5f)
                    {
                        Color cor = renderMaterial.materials[m].color;
                        cor.a -= 0.5f;

                        renderMaterial.materials[m].color = cor;
                    }
                }
                else if (renderMaterial.materials[m].color.a < 1)
                {
                    Color cor = renderMaterial.materials[m].color;
                    cor.a += 0.5f;

                    renderMaterial.materials[m].color = cor;
                }
            }
        }
    }
}
