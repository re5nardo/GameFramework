using System;
using System.Text;

public class Network : MonoSingleton<Network>
{
	private AsynchronousSocket m_socket = new AsynchronousSocket ();
	public MessageHandler m_OnReceived;

	#region MonoSingleton
	#endregion

	private void Update()
	{
		lock (m_socket.m_MessagesReceived)
		{
			if (m_socket.m_MessagesReceived.Count > 0)
			{
				if (m_OnReceived != null)
				{
					m_OnReceived(m_socket.m_MessagesReceived.Dequeue ());
				}
			}
		}

	}

	public void ConnectToServer(BoolHandler ConnectCallback)
	{
		m_socket.m_OnConnectCallback = ConnectCallback;

		m_socket.Connect ("127.0.0.1", 9110);
	}

	public void Send(IMessage msg)
	{
		string strSerializedData = msg.Serialize ();
		byte[] byteSerializedData = Encoding.Default.GetBytes (strSerializedData); 
		int nTotalSize = 2 + 2 + byteSerializedData.Length;		//	msg id(2) + msg length info(2)
		byte[] byteData = new byte[nTotalSize];

		byteData [0] = BitConverter.GetBytes (msg.GetID ()) [1];
		byteData [1] = BitConverter.GetBytes (msg.GetID ()) [0];
		byteData [2] = BitConverter.GetBytes (nTotalSize)[1];
		byteData [3] = BitConverter.GetBytes (nTotalSize)[0];

		int nIndex = 4;
		foreach(byte data in byteSerializedData)
		{
			byteData [nIndex++] = data;
		}
			
		m_socket.Send (byteData);
	}

	private void OnDestroy()
	{
		m_socket.Close ();
	}

	#region Socket Event Handler
	#endregion
}
