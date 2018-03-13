using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

public class RoomNetwork : MonoSingleton<RoomNetwork>
{
    private Socket m_Socket;
    private Queue<IMessage> m_MessagesReceived = new Queue<IMessage>();

    private bool m_bConnectResult;
    private bool m_bConnectCallbacked;
    private BoolHandler m_ConnectCallback;
    private MessageHandler m_RecvMessageCallback;

    private void Update()
    {
        lock (m_MessagesReceived)
        {
            while (m_MessagesReceived.Count > 0)
            {
                IMessage msg = m_MessagesReceived.Dequeue();

                if (m_RecvMessageCallback != null)
                {
                    m_RecvMessageCallback(msg);
                }
            }
        }

        if (m_bConnectCallbacked)
        {
            if (m_ConnectCallback != null)
            {
                m_ConnectCallback(m_bConnectResult);
            }

            m_bConnectCallbacked = false;
        }
    }
        
    public void ConnectToServer(string strIP, int nPort, BoolHandler connectHandler, MessageHandler recvMessageHandler)
    {
        Close ();
        lock (m_MessagesReceived)
        {
            m_MessagesReceived.Clear ();
        }

        AddConnectHandler(connectHandler);
        AddRecvMessageHandler(recvMessageHandler);

        Connect (strIP, nPort);
    }

    public void AddConnectHandler(BoolHandler connectHandler)
    {
        m_ConnectCallback += connectHandler;
    }

    public void RemoveConnectHandler(BoolHandler connectHandler)
    {
        m_ConnectCallback -= connectHandler;
    }

    public void AddRecvMessageHandler(MessageHandler recvMessageHandler)
    {
        m_RecvMessageCallback += recvMessageHandler;
    }

    public void RemoveRecvMessageHandler(MessageHandler recvMessageHandler)
    {
        m_RecvMessageCallback -= recvMessageHandler;
    }

    private void OnDestroy()
    {
        Close ();
    }

    #region Asynchronous Socket
    private void Connect(string strIP, int nPort)
    {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(strIP), nPort);

        // Create a TCP/IP socket.
        m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // Connect to the remote endpoint.
        m_Socket.BeginConnect( remoteEP, new AsyncCallback(ConnectCallback), m_Socket);
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.
            Socket client = (Socket) ar.AsyncState;

            // Complete the connection.
            client.EndConnect(ar);

            ReceiveStart ();

            m_bConnectResult = true;
        } 
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());

            m_bConnectResult = false;
        }

        m_bConnectCallbacked = true;
    }

    private void ReceiveStart()
    {
        try
        {
            // Create the state object.
            StateObject state = new StateObject();
            state.WorkSocket = m_Socket;

            // Begin receiving the data from the remote device.
            m_Socket.BeginReceive( state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private void ReceiveCallback( IAsyncResult ar )
    {
        try
        {
            // Retrieve the state object and the client socket 
            // from the asynchronous state object.
            StateObject state = (StateObject) ar.AsyncState;
            Socket client = state.WorkSocket;

            // Read data from the remote device.
            int bytesRead = client.EndReceive(ar);

            if(bytesRead == 0)  //  EOF
            {
                //  close socket
                return;
            }

            for (int i = 0; i < bytesRead; ++i)
            {
                if (state.CurPos == 0)
                {
                    state.CurMessageID = (ushort)(state.Buffer[i] << 8);
                    state.CurPos++;
                }
                else if (state.CurPos== 1)
                {
                    state.CurMessageID += (ushort)state.Buffer[i];
                    state.CurPos++;
                }
                else if (state.CurPos == 2)
                {
                    state.TotalSize = (ushort)(state.Buffer[i] << 8);
                    state.CurPos++;
                }
                else if (state.CurPos == 3)
                {
                    state.TotalSize += (ushort)state.Buffer[i];
                    state.CurMessage = new byte[state.TotalSize - NetworkDefines.MESSAGE_HEADER_SIZE];
                    state.CurPos++;
                }
                else
                {
                    //  last index
                    if (state.CurPos == state.TotalSize - 1)
                    {
                        state.CurMessage[state.CurPos - NetworkDefines.MESSAGE_HEADER_SIZE] = state.Buffer[i];

                        IMessage msg = GetIMessage(state.CurMessageID, state.CurMessage);
                        lock (m_MessagesReceived)
                        {
                            m_MessagesReceived.Enqueue(msg);
                        }

                        state.CurMessage = null;
                        state.CurPos = 0;
                    }
                    else
                    {
                        state.CurMessage[state.CurPos - NetworkDefines.MESSAGE_HEADER_SIZE] = state.Buffer[i];
                        state.CurPos++;
                    }
                }
            }

            client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private IMessage GetIMessage(ushort nMessageID, byte[] data)
    {
        try
        {
            IMessage msg = null;

            if (nMessageID == EnterRoomToC.MESSAGE_ID)
            {
                msg = ObjectPool.Instance.GetObject<EnterRoomToC>();
            }
            else if (nMessageID == PreparationStateToC.MESSAGE_ID)
            {
                msg = ObjectPool.Instance.GetObject<PreparationStateToC>();
            }
            else if (nMessageID == GameStartToC.MESSAGE_ID)
            {
                msg = ObjectPool.Instance.GetObject<GameStartToC>();
            }
            else if (nMessageID == PlayerEnterRoomToC.MESSAGE_ID)
            {
                msg = ObjectPool.Instance.GetObject<PlayerEnterRoomToC>();
            }
            else if (nMessageID == WorldSnapShotToC.MESSAGE_ID)
            {
                msg = ObjectPool.Instance.GetObject<WorldSnapShotToC>();
            }
            else if (nMessageID == WorldInfoToC.MESSAGE_ID)
            {
                msg = ObjectPool.Instance.GetObject<WorldInfoToC>();
            }
            else if (nMessageID == TickInfoToC.MESSAGE_ID)
            {
                msg = ObjectPool.Instance.GetObject<TickInfoToC>();
            }
            else if (nMessageID == GameEndToC.MESSAGE_ID)
            {
                msg = ObjectPool.Instance.GetObject<GameEndToC>();
            }

            if (msg != null)
            {
                msg.Deserialize(data);
            }

            return msg;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());

            return null;
        }
    }

    public void Send(IMessage msg)
    {
        try
        {
            byte[] byteSerializedData = msg.Serialize();
            int nTotalSize = NetworkDefines.MESSAGE_HEADER_SIZE + byteSerializedData.Length;        //  msg id(2) + msg length info(2)
            byte[] byteData = new byte[nTotalSize];

            byteData [0] = BitConverter.GetBytes(msg.GetID())[1];
            byteData [1] = BitConverter.GetBytes(msg.GetID())[0];
            byteData [2] = BitConverter.GetBytes(nTotalSize)[1];
            byteData [3] = BitConverter.GetBytes(nTotalSize)[0];

            int nIndex = NetworkDefines.MESSAGE_HEADER_SIZE;
            foreach(byte data in byteSerializedData)
            {
                byteData[nIndex++] = data;
            }

            // Begin sending the data to the remote device.
            m_Socket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), m_Socket);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private void SendCallback(IAsyncResult ar)
    {
        try {
            // Retrieve the socket from the state object.
            Socket socket = (Socket) ar.AsyncState;

            // Complete sending the data to the remote device.
            int bytesSent = socket.EndSend(ar);
            Console.WriteLine("Sent {0} bytes to server.", bytesSent);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private void Close()
    {
        // Release the socket.
        if (m_Socket != null)
        {
            m_Socket.Shutdown(SocketShutdown.Both);
            m_Socket.Close();
            m_Socket = null;
        }

        m_MessagesReceived.Clear();
        m_bConnectResult = false;
        m_bConnectCallbacked = false;
        m_ConnectCallback = null;
        m_RecvMessageCallback = null;
    }
    #endregion
}
