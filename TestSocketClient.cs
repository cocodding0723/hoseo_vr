using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Net.Sockets;


public class TestSocketClient : MonoBehaviour
{

    private Socket _soket = null;

    // Start is called before the first frame update
    void Start()
    {
        _soket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ConnectSocket();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SendMSG();
        }
    }

    private static string tcpmsg = "Weldone";

    byte[] databyte = new byte[256];
    byte[] sendbyte = Encoding.Default.GetBytes(tcpmsg);


    void ConnectSocket()
    {
        _soket.Connect("127.0.0.1", 7775);
        Debug.Log(_soket + "Soket noti");
        
    }

    void SendMSG()
    {
        _soket.Send(sendbyte);
    }
}
