﻿using System;
using System.Threading.Tasks;

using UnityEditor;

using Codice.Client.Common;
using Codice.Client.Common.Connection;
using Codice.Client.Common.Threading;
using Codice.CM.Common;
using PlasticGui;
using Unity.PlasticSCM.Editor.UI;
using Unity.PlasticSCM.Editor.WebApi;

namespace Unity.PlasticSCM.Editor.Configuration
{
    internal class CredentialsUiImpl : AskCredentialsToUser.IGui
    {
        AskCredentialsToUser.DialogData AskCredentialsToUser.IGui.AskUserForCredentials(
            string servername)
        {
            AskCredentialsToUser.DialogData result = null;

            GUIActionRunner.RunGUIAction(delegate
            {
                result = CredentialsDialog.RequestCredentials(
                        servername, ParentWindow.Get());
            });

            return result;
        }

        void AskCredentialsToUser.IGui.ShowSaveProfileErrorMessage(
            string message)
        {
            GUIActionRunner.RunGUIAction(delegate
            {
                GuiMessage.ShowError(string.Format(
                    PlasticLocalization.GetString(
                        PlasticLocalization.Name.CredentialsErrorSavingProfile),
                    message));
            });
        }

        AskCredentialsToUser.DialogData AskCredentialsToUser.IGui.AskUserForSSOCredentials(
            string cloudServer)
        {
            AskCredentialsToUser.DialogData result = null;

            GUIActionRunner.RunGUIAction(delegate
            {
                result = RunSSOCredentialsRequest(
                    cloudServer, CloudProjectSettings.accessToken);
            });

            return result;
        }

        AskCredentialsToUser.DialogData RunSSOCredentialsRequest(
            string cloudServer,
            string unityAccessToken)
        {
            if (string.IsNullOrEmpty(unityAccessToken))
            {
                return SSOCredentialsDialog.RequestCredentials(
                    cloudServer, ParentWindow.Get());
            }

            TokenExchangeResponse tokenExchangeResponse =
                WaitUntilTokenExchange(unityAccessToken);

            // There is no internet connection, so no way to get credentials
            if (tokenExchangeResponse == null)
            {
                return new AskCredentialsToUser.DialogData(
                    false, null, null, false,
                    SEIDWorkingMode.SSOWorkingMode);
            }

            if (tokenExchangeResponse.Error == null)
            {
                return new AskCredentialsToUser.DialogData(
                    true, 
                    tokenExchangeResponse.User,
                    tokenExchangeResponse.AccessToken, 
                    false,
                    SEIDWorkingMode.SSOWorkingMode);
            }

            return SSOCredentialsDialog.RequestCredentials(
                cloudServer, ParentWindow.Get());
        }

        static TokenExchangeResponse WaitUntilTokenExchange(
            string unityAccessToken)
        {
            TokenExchangeResponse result = null;

            Task.Run(() =>
            {
                try
                {
                    result = WebRestApiClient.PlasticScm.
                        TokenExchange(unityAccessToken);
                }
                catch (Exception ex)
                {
                    ExceptionsHandler.LogException(
                        "CredentialsUiImpl", ex);
                }
            }).Wait();

            return result;
        }
    }
}
