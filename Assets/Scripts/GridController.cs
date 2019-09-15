using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private Image m_Grid;
    public Image Grid => m_Grid;

    [SerializeField]
    private Image m_GridMarker;
    public Image GridMarker => m_GridMarker;
}
