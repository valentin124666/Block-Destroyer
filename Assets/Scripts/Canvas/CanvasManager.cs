using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _menuUI, _inGameUI, _wimIU, _lostUI;
    [SerializeField]
    private Image _levelBar;
    private Transform _finishPos, _playerTransform;
    [SerializeField]
    private Text _textLevelWin, _textLevelCurent, _textLevelTarget,_crystalCounter;
    private float _distens;
    private float _distensTraveled
    { get { return _finishPos.position.y - _playerTransform.position.y; } }

    private void Start()
    {
        _crystalCounter.text = PlayerPrefs.GetInt("Crystal").ToString();

        _finishPos = GridSystem.Instance.transform;
        _playerTransform = PlayerMove.Transform;


        _distens = _finishPos.position.y - _playerTransform.position.y - 1f;

        _textLevelWin.text = "Level " + PlayerPrefs.GetInt("Level").ToString();
        _textLevelCurent.text = PlayerPrefs.GetInt("Level").ToString();
        _textLevelTarget.text = (PlayerPrefs.GetInt("Level") + 1).ToString();
    }
    private void FixedUpdate()
    {
        if (GameStage.IsGameFlowe)
            AmoutDistensTraveled();
    }
    private void AmoutDistensTraveled()
    {
        if (_playerTransform != null)
        {
            float amoutDistens = 1 - _distensTraveled / _distens;
            _levelBar.fillAmount = Mathf.Lerp(_levelBar.fillAmount, amoutDistens, 0.7f);
        }
    }
    public void GameStageWindow(Stage stageGame)
    {
        switch (stageGame)
        {
            case Stage.StartGame:

                _menuUI.SetActive(true);
                _inGameUI.SetActive(false);
                break;

            case Stage.StartLevel:

                _menuUI.SetActive(false);
                _inGameUI.SetActive(true);
                break;

            case Stage.WinGame:

                _inGameUI.SetActive(false);
                _wimIU.SetActive(true);
                //впиши сюда поднятие уровня и сцены 
                break;

            case Stage.LostGame:

                _lostUI.SetActive(true);
                break;
        }
    }
    public void CrystalCollector(int namber)
    {
        PlayerPrefs.SetInt("Crystal", PlayerPrefs.GetInt("Crystal") + 1);
        _crystalCounter.text = PlayerPrefs.GetInt("Crystal").ToString();
    }

}
