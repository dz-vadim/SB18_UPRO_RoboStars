using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class LobbyAutoButtleManager : MonoBehaviourPunCallbacks
{
    public static LobbyAutoButtleManager instance;
    [SerializeField] private TMP_Text waitButtleText;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void ToButtle()
    {
        WindowsManager.Layout.OpenLayout("AutomaticButtle");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        if (returnCode == (short)ErrorCode.NoRandomMatchFound)
        {
            waitButtleText.text = "No Match Found, Creating new room";
            CreateNewRoom();
        }
    }
    private void CreateNewRoom()
    {
        RoomOptions currentRoom = new RoomOptions();
        currentRoom.IsOpen = true;
        currentRoom.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(RoomNameGenerator(), currentRoom);
    }

    private string RoomNameGenerator()
    {
        short codeLength = 12;
        string roomCode = "";
        for (short i = 0 ; i < codeLength; i++)
        {
            char symbol = (char)Random.Range('a', 'z');
            roomCode += symbol;
        }
        return roomCode;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        if (returnCode == (short)ErrorCode.GameIdAlreadyExists)
        {
            CreateNewRoom();
        }
    }

    public override void OnCreatedRoom()
    {
        waitButtleText.text = "Waiting for the second player...";
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PhotonNetwork.IsMasterClient)
        {
            return;
        }
        waitButtleText.text = "The battle begin! Get ready!";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        Room currentRoom = PhotonNetwork.CurrentRoom;
        currentRoom.IsOpen = false;
        waitButtleText.text = "The battle begin! Get ready!";
        Invoke(nameof(LoadingGameScene), 3f);
    }
    private void LoadingGameScene()
    {
        PhotonNetwork.LoadLevel(1);
    }
    public void StopFindButtle()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        WindowsManager.Layout.OpenLayout("MainMenu");
    }
}
