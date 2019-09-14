using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GridColorManager : MonoBehaviour
{

    public UnitFieldData unitFieldData;
    public GameObject[] unitLine;

    [SerializeField]private Image[,] m_grids;
    private UnitFieldData m_unitFieldData => unitFieldData;

    // Start is called before the first frame update
    void Start()
    {
        m_grids = new Image[16, 8];
        for(int i = 0; i < unitLine.Length; i++) 
        {
            foreach(Transform child in unitLine[i].transform) {
                int childIndex = child.GetSiblingIndex();
                m_grids[i, childIndex] = child.gameObject.GetComponent<Image>();
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*for(int i=3;i<UnitFieldData.FIELD_HEIGHT;i++)
        {
            for(int j=0;j<UnitFieldData.FILED_WIDTH;j++)
            {
                switch(m_unitFieldData.)
            }
        }*/
         
    }
}
