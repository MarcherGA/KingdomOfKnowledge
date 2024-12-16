using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LandingUi : MonoBehaviour
{
    public UnityAction onOpenLoginClicked;
    public UnityAction onOpenRegisterClicked;

    [SerializeField] private Button _openLoginButton;
    [SerializeField] private Button _openRegisterButton;
    [SerializeField] private GameObject _logoGlow;

    private void Start()
    {
        _openLoginButton.gameObject.SetActive(false);
        _openRegisterButton.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _logoGlow.SetActive(true);
    }


    public void Close()
    {
        _logoGlow?.SetActive(false);
        gameObject.SetActive(false);
    }

    public void EnableButtons()
    {
        _openLoginButton.onClick.AddListener(onOpenLoginClicked);
        _openRegisterButton.onClick.AddListener(onOpenRegisterClicked);



        _openLoginButton.gameObject.SetActive(true);
        _openRegisterButton.gameObject.SetActive(true);
    }
}
