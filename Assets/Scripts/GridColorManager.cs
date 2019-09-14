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
    public void OnStart()
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
    public void OnUpdate()
    {
        for(int i=3;i<UnitFieldData.FIELD_HEIGHT;i++)
        {
            for(int j=0;j<UnitFieldData.FILED_WIDTH;j++)
            {
                switch (m_unitFieldData.Units[i, j].GetDisplayColor())
                {
                    case (ColorData.Blue):
                        m_grids[i, j].color = Color.blue;
                        break;
                    case (ColorData.Green):
                        m_grids[i, j].color = Color.green;
                        break;
                    case (ColorData.Red):
                        m_grids[i, j].color = Color.green;
                        break;
                    case (ColorData.Cyan):
                        m_grids[i, j].color = Color.cyan;
                        break;
                    case (ColorData.Magenta):
                        m_grids[i, j].color = Color.magenta;
                        break;
                    case (ColorData.Yellow):
                        m_grids[i, j].color = Color.yellow;
                        break;
                    case (ColorData.White):
                        m_grids[i, j].color = Color.white;
                        break;
                    case (ColorData.None):
                        m_grids[i, j].color = new Color(0, 0, 0, 0);
                        break;
                }
            }
        }
        
    }
}
