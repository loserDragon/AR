using System;
using BestHTTP;
using com.ootii.Messages;
using UnityEngine;
using LitJson;
/// <summary>
/// 识别到之后，打开发送http请求的按钮
/// </summary>
public class JT_RecvMsg : MonoBehaviour
{
    public GameObject btn;

    private string bookId = null;
    private void Awake()
    {
        MessageDispatcher.AddListener("ON_TRACKING_FOUND", OnRecvFound);
        MessageDispatcher.AddListener("ON_TRACKING_LOST", OnRecvLost);

        UIEventListener.Get(btn).onClick += OnClickBtn;

    }

    private void OnDestroy()
    {
        MessageDispatcher.RemoveListener("ON_TRACKING_FOUND", OnRecvFound);
        MessageDispatcher.RemoveListener("ON_TRACKING_LOST", OnRecvLost);
    }


    private void OnClickBtn(GameObject go)
    {
        JT_HTTPSENDMSG _HTTPSENDMSG = new JT_HTTPSENDMSG();
        _HTTPSENDMSG.callback = GetPopupList;
        _HTTPSENDMSG.interface_name = "user";
        JsonData date = new JsonData();
        date["action"] = "sendmsg";
        date["mobile"] = JT_Core.instance.userName;
        date["page"] = bookId;

        //Debug.Log("======>>>>" + date.ToJson());
        _HTTPSENDMSG.RawData = date.ToJson();
        MessageDispatcher.SendMessage(this, JT_VR_MessageInfo.ON_SEND_HTTP_MSG.ToString(), _HTTPSENDMSG, EnumMessageDelay.IMMEDIATE);
    }

    private void GetPopupList(HTTPRequest originalRequest, HTTPResponse response)
    {
        //Debug.Log("---->>>>"+response.DataAsText.ToString());
    }

    private void OnRecvLost(IMessage rMessage)
    {
        if(btn!=null && btn.activeSelf )
            btn.SetActive(false);
    }

    private void OnRecvFound(IMessage rMessage)
    {
        if(rMessage != null)
        bookId = rMessage.Data.ToString();

        //JT_Core.instance.userSignal = "2";
        if (JT_Core.instance.userSignal =="2")//是老师才可以打开按钮
            btn.SetActive(true);
    }


}

