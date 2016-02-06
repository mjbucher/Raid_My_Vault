using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Motive.Core.Diagnostics;
using Motive.Core.WebServices;
using Motive.Unity.Utilities;

public class AccountPanel : Panel
{
    public GameObject RegisterPanel;
    public GameObject LoginPanel;

    public InputField RegisterUserName;
    public InputField RegisterEmail;
    public InputField RegisterPassword;
    public InputField RegisterConfirmPassword;

    public InputField LoginUserName;
    public InputField LoginPassword;

    public Text StatusText;

	Motive.Core.Diagnostics.Logger m_logger;

    public override void DidShow(object data)
    {
        base.DidShow(data);

        if (MotiveAuthenticator.Instance.IsUserAuthenticated)
        {
            StatusText.text = "Logged in as " + MotiveAuthenticator.Instance.UserName;
        }
    }

    void Start()
    {
		m_logger = new Motive.Core.Diagnostics.Logger(this);

        ShowRegister();
    }
    public void ShowRegister()
    {
        RegisterPanel.SetActive(true);
        LoginPanel.SetActive(false);
    }

    public void ShowLogin()
    {
        RegisterPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    void Login(string username, string password)
    {
        WebServices.Instance.UserManager.Login(
            WebServices.Instance.FullUserDomain,
            username,
            password,
            (user) =>
            {
                ThreadHelper.Instance.CallOnMainThread(() =>
                    {
                        if (user != null)
                        {
                            StatusText.text = "Logged in as " + user.UserName;
                        }
                        else
                        {
                            StatusText.text = "Failed to log in as " + username;
                        }

                        m_logger.Debug("Login success {0} for {1}", (user != null), username);
                    });
            });
    }

    public void Login()
    {
        Login(LoginUserName.text, LoginPassword.text);
    }

    public void Register()
    {
        WebServices.Instance.UserManager.RegisterUser(
            WebServices.Instance.FullUserDomain,
            RegisterUserName.text,
            RegisterEmail.text,
            RegisterPassword.text,
            RegisterConfirmPassword.text,
            (registerSuccess) =>
            {
                ThreadHelper.Instance.CallOnMainThread(() =>
                    {
                        if (registerSuccess)
                        {
                            StatusText.text = "Registration successful!";

                            Login(RegisterUserName.text, RegisterPassword.text);
                        }
                        else
                        {
                            StatusText.text = "Failed to register user";
                        }
                    });
            });
    }
}
