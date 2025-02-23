using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine.EventSystems;
using System;
using System.Data;

public class ChatManager : MonoBehaviourPun, IOnEventCallback
{
    public ScrollRect scrollChatWindow;
    public TMP_Text text_chatContent;
    public TMP_InputField input_chat;

    const byte chattingEvent = 1;
    Image img_chatBackground;

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.AddCallbackTarget(this);
    }

    void Start()
    {
        input_chat.text = "";

        text_chatContent.text = "";

        // 인풋 필드의 제출 이벤트에 SendMessage 함수를 바인딩한다.
        input_chat.onSubmit.AddListener(SendMessage);

        // 좌측 하단으로 콘텐츠 오브젝트의  피봇을 변경한다.
        scrollChatWindow.content.pivot = Vector2.zero;

        img_chatBackground = scrollChatWindow.transform.GetComponent<Image>();
        img_chatBackground.color = new Color32(255, 255, 255, 10);
    }

    void Update()
    {

        // 탭 키를 누르면 인풋 필드를 선택하게 한다.
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            EventSystem.current.SetSelectedGameObject(input_chat.gameObject);
            input_chat.OnPointerClick(new PointerEventData(EventSystem.current));
        }
    }

    void SendMyMessage(string msg)
    {
        // 이벤트에 보낼 내용
        string currentTime = DateTime.Now.ToString("hh:mm:ss"); 

        object[] sendContent = new object[] { PhotonNetwork.NickName, msg,  currentTime};

        // 송신 옵션
        RaiseEventOptions eventOptions = new RaiseEventOptions();
        eventOptions.Receivers = ReceiverGroup.All;
        eventOptions.CachingOption = EventCaching.DoNotCache;

        // 이벤트 송신 시작
        PhotonNetwork.RaiseEvent(1, sendContent, eventOptions, SendOptions.SendUnreliable);

        EventSystem.current.SetSelectedGameObject(null);
    }

    // 같은 룸의 사용자로부터 이벤트가 왔을 때 실행되는 함수
    public void OnEvent(EventData photonEvent)
    {
        if(input_chat.text.Length > 0)
        {
            // 만일, 받은 이벤트가 채팅 이벤트라면...
            if (photonEvent.Code == chattingEvent)
            {
                // 받은 내용을 "닉네임: 채팅 내용" 형식으로 스크롤 뷰의 텍스트에 전달한다.
                object[] receiveObjects = (object[])photonEvent.CustomData;
                string receiveMessage = $"\n[{receiveObjects[2].ToString()}] {receiveObjects[0].ToString()} : {receiveObjects[1].ToString()}";


                text_chatContent.text += receiveMessage;

                input_chat.text = "";
            }
        }

        img_chatBackground.color = new Color32(255, 255, 255, 50);
        StopAllCoroutines();
        StartCoroutine(AlphaReturn(2.0f));
    }

    IEnumerator AlphaReturn(float time)
    {
        yield return new WaitForSeconds(time);
        img_chatBackground.color = new Color32(255, 255, 255, 10);
    }


    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.RemoveCallbackTarget(this);
    }
}