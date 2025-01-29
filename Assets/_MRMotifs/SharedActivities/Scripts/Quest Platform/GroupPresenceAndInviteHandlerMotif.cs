/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#if FUSION2
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using Oculus.Platform;
using Meta.XR.MultiplayerBlocks.Fusion;

/// <summary>
/// The GroupPresenceAndInviteHandlerMotif class is responsible for managing group presence
/// and launching the invite panel using the Oculus Platform SDK. It allows users to set
/// their presence in a joinable state and invite friends to join them in a multiplayer session.
/// </summary>
public class GroupPresenceAndInviteHandlerMotif : MonoBehaviour
{
    [Tooltip("Decide if you would like to use the Group Presence features for your experience, such as invites.")]
    [SerializeField] private bool setupGroupPresence = true;

    [Header("Destination and Session Info")]
    [Tooltip("Destination API Name, which can be found on the Developer Dashboard under Engagement > Destinations.")]
    [SerializeField] private string destinationApiName;

    [Tooltip("Lobby Session ID.")]
    [SerializeField] private string lobbySessionId;

    [Tooltip("Match Session ID.")]
    [SerializeField] private string matchSessionId;

    private Button _inviteFriendsButton;

    private void Awake()
    {
        if (!setupGroupPresence) return;
        FusionBBEvents.OnConnectedToServer += SetGroupPresence;
        SetupFriendsInvite();
    }

    private void OnDestroy()
    {
        if (!setupGroupPresence) return;
        FusionBBEvents.OnConnectedToServer -= SetGroupPresence;
        _inviteFriendsButton.onClick.RemoveListener(OpenInvitePanel);
    }

    private void SetupFriendsInvite()
    {
        _inviteFriendsButton = FindObjectOfType<MenuPanel>().FriendsInviteButton;
        _inviteFriendsButton.onClick.AddListener(OpenInvitePanel);
    }

    private void SetGroupPresence(NetworkRunner obj)
    {
        SetGroupPresence();
    }

    private void OpenInvitePanel()
    {
        LaunchInvitePanel();
    }

    /// <summary>
    /// Sets the group presence for the current user with the provided destination, lobby session ID,
    /// and match session ID. This makes the user's session joinable, allowing other users to join the game.
    /// </summary>
    public void SetGroupPresence()
    {
        var options = new GroupPresenceOptions();

        options.SetDestinationApiName(destinationApiName);
        options.SetLobbySessionId(lobbySessionId);
        options.SetMatchSessionId(matchSessionId);
        options.SetIsJoinable(true);

        GroupPresence.Set(options).OnComplete(message =>
        {
            if (message.IsError)
            {
                Debug.LogError("Error setting group presence: " + message.GetError().Message);
            }
            else
            {
                Debug.Log("Group presence successfully set for the Chess scene!");
            }
        });
    }

    /// <summary>
    /// Launches the invite panel, allowing the user to invite friends to join their current session.
    /// </summary>
    public void LaunchInvitePanel()
    {
        var options = new InviteOptions();

        GroupPresence.LaunchInvitePanel(options).OnComplete(message =>
        {
            if (message.IsError)
            {
                Debug.LogError("Error launching invite panel: " + message.GetError().Message);
            }
            else
            {
                Debug.Log("Invite panel successfully launched.");
            }
        });
    }
}
#endif
