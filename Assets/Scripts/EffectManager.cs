using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EffectManager : MonoBehaviour
{
    //public GameObject rawImage;
    public GameObject deleteEff;
    public GameObject renderTex;
    public GameObject comboText;
    [SerializeField] private Vector3 textOffset = new Vector3(); 

    public void OnDeleteWhiteUnit(GameObject grid, string text) {
        Text combotxt = comboText.GetComponent<Text>();
        combotxt.text = text;
        GameObject eff = (GameObject)Instantiate(renderTex, grid.transform.position, Quaternion.identity);
        GameObject comboTextObj = (GameObject)Instantiate(comboText, grid.transform.position + textOffset, Quaternion.identity);
        eff.transform.SetParent(grid.transform);
        comboTextObj.transform.SetParent(grid.transform);
        deleteEff.GetComponent<ParticleSystem>().Emit(3);
        comboText.GetComponent<Animation>().Play();
        Destroy(eff, 2);
        Destroy(comboTextObj, 2);
    }

}
