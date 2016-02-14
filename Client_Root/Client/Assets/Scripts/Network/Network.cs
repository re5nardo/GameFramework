﻿using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

public class Network : MonoSingleton<Network>
{
	private Socket m_Socket;
	private Queue<IMessage> m_MessagesReceived = new Queue<IMessage>();

	private BoolHandler m_ConnectCallback;
	private MessageHandler m_RecvMessageCallback;

	private void Update()
	{
		lock (m_MessagesReceived)
		{
			if (m_MessagesReceived.Count > 0)
			{
				if (m_RecvMessageCallback != null)
				{
					m_RecvMessageCallback(m_MessagesReceived.Dequeue ());
				}
			}
		}
	}


	public void ConnectToServer(string strIP, int nPort, BoolHandler connectHandler, MessageHandler recvMessageHandler)
	{
		Close ();
		lock (m_MessagesReceived)
		{
			m_MessagesReceived.Clear ();
		}

		m_ConnectCallback = connectHandler;
		m_RecvMessageCallback = recvMessageHandler;

		Connect ("127.0.0.1", 9110);
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

			if(m_ConnectCallback != null)
			{
				m_ConnectCallback(true);
			}
		} 
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());

			if(m_ConnectCallback != null)
			{
				m_ConnectCallback(false);
			}
		}
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

			if(bytesRead == 0)	//	EOF
			{
				//	close socket
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
					state.CurMessage = new byte[state.TotalSize];
					state.CurPos++;
				}
				else
				{
					//	last index
					if (state.CurPos == state.TotalSize - 1)
					{
						state.CurMessage[state.CurPos - NetworkDefines.MESSAGE_HEADER_SIZE] = state.Buffer[i];

						if (m_RecvMessageCallback != null)
						{
							m_RecvMessageCallback(GetIMessage(state.CurMessageID, Encoding.Default.GetString(state.CurMessage)));
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

	private IMessage GetIMessage(ushort nMessageID, string strJson)
	{
		IMessage msg = null;

		if (nMessageID == (ushort)Messages.TEST_MESSAGE_ID)
		{
			msg = new TestMessage();
		}

		if (msg != null)
		{
			msg.Deserialize(strJson);
		}

		return msg;

	}

	public void Send(IMessage msg)
	{
		try
		{
			string strSerializedData = msg.Serialize ();
			byte[] byteSerializedData = Encoding.Default.GetBytes (strSerializedData); 
			int nTotalSize = NetworkDefines.MESSAGE_HEADER_SIZE + byteSerializedData.Length;		//	msg id(2) + msg length info(2)
			byte[] byteData = new byte[nTotalSize];

			byteData [0] = BitConverter.GetBytes (msg.GetID ()) [1];
			byteData [1] = BitConverter.GetBytes (msg.GetID ()) [0];
			byteData [2] = BitConverter.GetBytes (nTotalSize)[1];
			byteData [3] = BitConverter.GetBytes (nTotalSize)[0];

			int nIndex = NetworkDefines.MESSAGE_HEADER_SIZE;
			foreach(byte data in byteSerializedData)
			{
				byteData [nIndex++] = data;
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
		}
	}
	#endregion
}
