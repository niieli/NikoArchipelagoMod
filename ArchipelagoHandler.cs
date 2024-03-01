using System;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;

namespace NikoArchipelago;

public class ArchipelagoHandler
{
    private static ArchipelagoSession session = ArchipelagoSessionFactory.CreateSession("localhost", 38281);
    

    private static void Connect(string server, string user, string pass)
    {
        LoginResult result;

        try
        {
            result = session.TryConnectAndLogin("Here Comes Niko!", user, ItemsHandlingFlags.AllItems);
        }
        catch (Exception e)
        {
            result = new LoginFailure(e.GetBaseException().Message);
        }

        if (!result.Successful)
        {
            LoginFailure failure = (LoginFailure)result;
            string errorMessage = $"Failed to Connect to {server} as {user}:";
            foreach (string error in failure.Errors)
            {
                errorMessage += $"\n    {error}";
            }
            foreach (ConnectionRefusedError error in failure.ErrorCodes)
            {
                errorMessage += $"\n    {error}";
            }

            return; // Did not connect, show the user the contents of `errorMessage`
        }
    
        // Successfully connected, `ArchipelagoSession` (assume statically defined as `session` from now on) can now be used to interact with the server and the returned `LoginSuccessful` contains some useful information about the initial connection (e.g. a copy of the slot data as `loginSuccess.SlotData`)
        var loginSuccess = (LoginSuccessful)result;
    }
}