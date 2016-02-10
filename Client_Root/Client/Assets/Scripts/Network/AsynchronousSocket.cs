using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections.Generic;

// State object for receiving data from remote device.
public class StateObject
{
	// Client socket.
	public Socket workSocket = null;
	// Size of receive buffer.
	public const int BufferSize = 256;
	// Receive buffer.
	public byte[] buffer = new byte[BufferSize];
	// Received data string.
	public StringBuilder sb = new StringBuilder();
}

public class AsynchronousSocket
{
	private Socket m_socket;
	public Queue<IMessage> m_MessagesReceived = new Queue<IMessage>();

	public BoolHandler m_OnConnectCallback;
	public BoolHandler m_OnSendCallback;

	// The response from the remote device.
	private String response = String.Empty;

	public void Connect(string strIP, int nPort)
	{
		IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(strIP), nPort);

		// Create a TCP/IP socket.
		m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		// Connect to the remote endpoint.
		m_socket.BeginConnect( remoteEP, new AsyncCallback(ConnectCallback), m_socket);
	}

	public void Close()
	{
		// Release the socket.
		m_socket.Shutdown(SocketShutdown.Both);
		m_socket.Close();
	}
		
	private void ConnectCallback(IAsyncResult ar)
	{
		try
		{
			// Retrieve the socket from the state object.
			Socket client = (Socket) ar.AsyncState;

			// Complete the connection.
			client.EndConnect(ar);

			if(m_OnConnectCallback != null)
			{
				m_OnConnectCallback(true);
			}
		} 
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());

			if(m_OnConnectCallback != null)
			{
				m_OnConnectCallback(false);
			}
		}
	}

	private void Receive(Socket client)
	{
		try
		{
			// Create the state object.
			StateObject state = new StateObject();
			state.workSocket = client;

			// Begin receiving the data from the remote device.
			client.BeginReceive( state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
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
			Socket client = state.workSocket;

			// Read data from the remote device.
			int bytesRead = client.EndReceive(ar);

			if (bytesRead > 0)
			{
				// There might be more data, so store the data received so far.
				state.sb.Append(Encoding.ASCII.GetString(state.buffer,0,bytesRead));

				// Get the rest of the data.
				client.BeginReceive(state.buffer,0,StateObject.BufferSize,0, new AsyncCallback(ReceiveCallback), state);
			}
			else
			{
				// All the data has arrived; put it in response.
				if (state.sb.Length > 1)
				{
					response = state.sb.ToString();
				}
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
		}
	}

	public void Send(byte[] byteData)
	{
		try
		{
			// Convert the string data to byte data using ASCII encoding.
			//byte[] byteData = Encoding.ASCII.GetBytes(data);

			// Begin sending the data to the remote device.
			m_socket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), m_socket);
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

			if(m_OnSendCallback != null)
			{
				m_OnSendCallback(true);
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());

			if(m_OnSendCallback != null)
			{
				m_OnSendCallback(false);
			}
		}
	}
}