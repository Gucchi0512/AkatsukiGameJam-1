using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EffectManager : MonoBehaviour
{
    //public GameObject rawImage;
    public GameObject deleteEff;
    public GameObject renderTex;

    public void OnDeleteWhiteUnit(GameObject grid) {
        
        GameObject eff = (GameObject)Instantiate(renderTex, grid.transform.position, Quaternion.identity);
        eff.transform.parent = grid.transform;
        deleteEff.GetComponent<ParticleSystem>().Play();
    }
}
