using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class GridColorManager : MonoBehaviour {

    public Sprite minoTexture;
    public UnitFieldData unitFieldData;
    public GameObject[] unitLineField;
    public GameObject[] unitLineNext;
    public GameObject[] unitLineNextNext;
    public EffectManager effectManager;
    public Text Score;
    public Image lasar;


    [SerializeField] private Image[,] m_grids;
    [SerializeField] private Image[,] m_nextGrid;
    [SerializeField] private Image[,] m_nextNextGrid;
    private UnitFieldData m_unitFieldData => unitFieldData;

    // Start is called before the first frame update
    public void OnStart() {
        m_grids = new Image[UnitFieldData.FIELD_HEIGHT - UnitFieldData.FIELD_TOP_OFFSET, UnitFieldData.FILED_WIDTH];
        m_nextGrid = new Image[MinoData.MINO_HEIGHT, MinoData.MINO_WIDTH];
        m_nextNextGrid = new Image[MinoData.MINO_HEIGHT, MinoData.MINO_WIDTH];

        for (int i = 0; i < unitLineField.Length; i++) {
            foreach (Transform child in unitLineField[i].transform) {
                int childIndex = child.GetSiblingIndex();
                m_grids[i, childIndex] = child.gameObject.GetComponent<Image>();
                m_grids[i, childIndex].sprite = minoTexture;

            }
        }
        for (int i = 0; i < unitLineNext.Length; i++) {
            foreach (Transform child in unitLineNext[i].transform) {
                int childIndex = child.GetSiblingIndex();
                var image = child.gameObject.GetComponent<Image>();
                m_nextGrid[i, childIndex] = image;
                m_nextGrid[i, childIndex].sprite = minoTexture;
            }
        }
        for (int i = 0; i < unitLineNextNext.Length; i++) {
            foreach (Transform child in unitLineNextNext[i].transform) {
                int childIndex = child.GetSiblingIndex();
                m_nextNextGrid[i, childIndex] = child.gameObject.GetComponent<Image>();
                m_nextNextGrid[i, childIndex].sprite = minoTexture;
            }
        }
        
    }

    // Update is called once per frame
    public void OnUpdate() {
        var gameState = GameManager.Instance.CurrentState;
        UpdateField();
        ShowCurrentMino();
        ShowNextMino();
        Score.text = unitFieldData.Score.ToString();

    }

    public void OnStartState() {
        var gameState = GameManager.Instance.CurrentState;
        switch (gameState) {
            case GameManagerState.None:
            break;
            case GameManagerState.GameStart:
            if (this.gameObject.tag == "Player1") {
                unitFieldData = GameManager.Instance.LogicManager.FieldDataPlayer1;
            } else {
                unitFieldData = GameManager.Instance.LogicManager.FieldDataPlayer2;
            }
            unitFieldData.AddStartStateAction(EffectPlay);
            unitFieldData.AddStartStateAction(LasarPlay);
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
            case GameManagerState.Disapper:
            break;
        }
    }

    public void ShowCurrentMino() {
        MinoData currentMino = unitFieldData.CurrentMino;
        int posX = unitFieldData.CurrentMino.Pos.x;
        int posY = unitFieldData.CurrentMino.Pos.y - UnitFieldData.FIELD_TOP_OFFSET;
        for (int height = posY; height < posY + MinoData.MINO_HEIGHT; height++) {
            for (int width = posX; width < posX + MinoData.MINO_WIDTH; width++) {
                if (height < 0) continue;
                var currentColor = currentMino.Units[height - posY, width - posX].GetDisplayColor();
                if (currentColor == ColorData.None) {
                    continue;
                } else {
                    var grid = m_grids[height, width];
                    grid.color = GridColorChange(currentColor);
                }
            }
        }
    }

    public void UpdateField() {
        for (int height = 3; height < UnitFieldData.FIELD_HEIGHT; height++) {
            for (int width = 0; width < UnitFieldData.FILED_WIDTH; width++) {
                var grid = m_grids[height - UnitFieldData.FIELD_TOP_OFFSET, width];
                grid.color = GridColorChange(m_unitFieldData.Units[height, width].GetDisplayColor());

            }
        }
    }
    public void ShowNextMino() {
        MinoData nextMino = m_unitFieldData.NextMino;
        MinoData nextNextMino = m_unitFieldData.NextNextMino;
        for (int height = 0; height < MinoData.MINO_HEIGHT; height++) {
            for (int width = 0; width < MinoData.MINO_WIDTH; width++) {
                var nextGrid = m_nextGrid[height, width];
                var nextNextGrid = m_nextNextGrid[height, width];
                nextGrid.color = GridColorChange(nextMino.Units[height, width].GetDisplayColor());
                nextNextGrid.color = GridColorChange(nextNextMino.Units[height, width].GetDisplayColor());
            }
        }
    }

    private Color GridColorChange(ColorData colorData) {
        Color chColor = new Color();
        switch (colorData) {
            case (ColorData.Blue):
            chColor = Color.blue;
            break;
            case (ColorData.Green):
            chColor = Color.green;
            break;
            case (ColorData.Red):
            chColor = Color.red;
            break;
            case (ColorData.Cyan):
            chColor = Color.cyan;
            break;
            case (ColorData.Magenta):
            chColor = Color.magenta;
            break;
            case (ColorData.Yellow):
            chColor = Color.yellow;
            break;
            case (ColorData.White):
            chColor = Color.white;
            break;
            case (ColorData.None):
            chColor = new Color(0, 0, 0, 0);
            break;
        }
        return chColor;
    }

    public void EffectPlay() {
        if (unitFieldData.CurrentState == UnitFieldState.Delete) {
            if (unitFieldData.ComboDataList != null) {
                List<ComboData> dataList = unitFieldData.ComboDataList;
                foreach (var item in dataList) {
                    int posX = item.Pos.x;
                    int posY = item.Pos.y - UnitFieldData.FIELD_TOP_OFFSET;
                    effectManager.OnDeleteWhiteUnit(m_grids[posY, posX].gameObject, item.ComboCount);
                }
                
            }
        }
    }

    public void LasarPlay() {
        if(unitFieldData.CurrentState == UnitFieldState.StartLaser) {

        }
    }
}
