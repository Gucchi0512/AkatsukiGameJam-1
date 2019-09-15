using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EffectManager : MonoBehaviour
{
    //public GameObject rawImage;
    public GameObject deleteEff;

    public void OnDeleteWhiteUnit(Vector3 pos) {
        var ray = RectTransformUtility.ScreenPointToRay(Camera.main, pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            GameObject eff = Instantiate(deleteEff, pos, Quaternion.identity);
            eff.GetComponent<ParticleSystem>().Play();
        }
    }
}
