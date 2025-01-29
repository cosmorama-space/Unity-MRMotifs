using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.BuildingBlocks;
using UnityEngine.SceneManagement;

/// <summary>
/// The InvitationAcceptanceHandlerMotif class handles deep link invitations using the Oculus Platform SDK.
/// When the application is launched via a deep link (e.g., an invitation from a friend), it checks the launch
/// details to determine if the user should be directed to a specific destination.
/// </summary>
public class InvitationAcceptanceHandlerMotif : MonoBehaviour
{
    [SerializeField] private EntitlementCheck entitlementCheck;

    [Header("Destination API Names and their Scenes")]
    [Tooltip("A list of API to Scene mappings.")]
    [SerializeField] private List<ApiToSceneMapping> apiToSceneMappings = new();

    private Dictionary<string, string> _apiToSceneMap;

    private void Awake()
    {
        entitlementCheck.UserPassedEntitlementCheck += HandleDeepLinkInvitation;
        InitializeApiToSceneMap();
    }

    private void InitializeApiToSceneMap()
    {
        _apiToSceneMap = new Dictionary<string, string>();

        foreach (var mapping in apiToSceneMappings)
        {
            if (!_apiToSceneMap.ContainsKey(mapping.apiName))
            {
                _apiToSceneMap.Add(mapping.apiName, mapping.sceneName);
            }
        }
    }

    private void HandleDeepLinkInvitation()
    {
        var launchDetails = ApplicationLifecycle.GetLaunchDetails();
        if (launchDetails.LaunchType == LaunchType.Deeplink)
        {
            Debug.Log("App was launched via a deep link invitation.");
            var destinationApiName = launchDetails.DestinationApiName;

            if (_apiToSceneMap.TryGetValue(destinationApiName, out var sceneName))
            {
                SceneManager.LoadSceneAsync(sceneName);
                Debug.Log($"Connecting to {sceneName} session automatically via Auto-matchmaking.");
            }
            else
            {
                Debug.LogWarning("Unknown destination for deep link: " + destinationApiName);
            }
        }
        else
        {
            Debug.Log("No deep link invitation detected.");
        }
    }
}

[System.Serializable]
public class ApiToSceneMapping
{
    public string apiName;
    public string sceneName;
}
