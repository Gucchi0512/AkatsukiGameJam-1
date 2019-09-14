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
        m_grids = new Image[UnitFieldData.FIELD_HEIGHT-UnitFieldData.FIELD_TOP_OFFSET, UnitFieldData.FILED_WIDTH];
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
        for(int height=3;height<UnitFieldData.FIELD_HEIGHT;height++)
        {
            for(int width=0;width<UnitFieldData.FILED_WIDTH;width++)
            {
                var grid = m_grids[height - UnitFieldData.FIELD_TOP_OFFSET, width];
                switch (m_unitFieldData.Units[height, width].GetDisplayColor())
                {

                    case (ColorData.Blue):
                        grid.color = Color.blue;
                        break;
                    case (ColorData.Green):
                        grid.color = Color.green;
                        break;
                    case (ColorData.Red):
                        grid.color = Color.green;
                        break;
                    case (ColorData.Cyan):
                        grid.color = Color.cyan;
                        break;
                    case (ColorData.Magenta):
                        grid.color = Color.magenta;
                        break;
                    case (ColorData.Yellow):
                        grid.color = Color.yellow;
                        break;
                    case (ColorData.White):
                        grid.color = Color.white;
                        break;
                    case (ColorData.None):
                        grid.color = new Color(0, 0, 0, 0);
                        break;
                }
            }
        }
        
    }

    public void OnStartState() {
        var gameState = GameManager.Instance.CurrentState;
        switch (gameState) {
            case GameManagerState.None:
            break;
            case GameManagerState.GameStart:
            unitFieldData = GameManager.Instance.LogicManager.FieldDataPlayer1;
            break;
            case GameManagerState.Input:
            break;
            case GameManagerState.Put:
            break;
            case GameManagerState.AutoDrop:
            break;
            case GameManagerState.CheckGameOver:
            break;
            case GameManagerState.GameEnd:
            break;
        }
    }
}
