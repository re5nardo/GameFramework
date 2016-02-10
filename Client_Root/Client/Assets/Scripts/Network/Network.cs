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
		byte[] byteSerializedData = Encoding.ASCII.GetBytes (strSerializedData);
		int nCount = 2 + 2 + byteSerializedData.Length;
		byte[] byteData = new byte[nCount];

		byteData [0] = BitConverter.GetBytes (msg.GetID ()) [0];
		byteData [1] = BitConverter.GetBytes (msg.GetID ()) [1];
		byteData [2] = BitConverter.GetBytes ((ushort)msg.Serialize ().Length)[0];
		byteData [3] = BitConverter.GetBytes ((ushort)msg.Serialize ().Length)[1];

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
