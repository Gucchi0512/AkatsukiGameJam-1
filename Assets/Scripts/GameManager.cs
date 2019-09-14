using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private LogicManager m_LogicManager;

    public GameManagerState CurrentState { get; private set; }

    public static GameManager Instance {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            OnAwake();
        } else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Destroy(gameObject);
        }
    }

    private void OnAwake()
    {
        m_LogicManager = new LogicManager();
    }

    private void Start()
    {

    }

    private void Update()
    {

    }
}
