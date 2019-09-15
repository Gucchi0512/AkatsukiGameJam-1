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

    private AudioSource seSource;
    private Vector3 textOffset;

    private void Start() {
        seSource = GetComponent<AudioSource>();
    }
    public void OnDeleteWhiteUnit(GameObject grid, int combo) {
        string text = combo.ToString() + "Combo!!";
        Text combotxt = comboText.GetComponent<Text>();
        combotxt.text = text;
        textOffset = new Vector3(0, combo%10*100, 0);
        GameObject eff = (GameObject)Instantiate(renderTex, grid.transform.position, Quaternion.identity);
        eff.transform.SetParent(grid.transform);
        deleteEff.GetComponent<ParticleSystem>().Emit(3);
        seSource.Play();
        Destroy(eff, 2);
        if (combo > 0) {
            GameObject comboTextObj = (GameObject)Instantiate(comboText, grid.transform.position + textOffset, Quaternion.identity);
            comboTextObj.transform.SetParent(grid.transform);
            comboText.GetComponent<Animation>().Play();
            Destroy(comboTextObj, 2);
        }
    }

}
