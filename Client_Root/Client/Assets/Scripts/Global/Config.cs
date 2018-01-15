using UnityEngine;
using System.Xml;

public class Config : Singleton<Config>
{
    private const string CONFIG_FILE_NAME = "Config.xml";

    private string m_strLobbyServerIP;
    private int m_nLobbyServerPort;

    private string m_strRoomServerIP;
    private int m_nRoomServerPort;

    public Config()
    {
        SetServerInfo();
    }

    private void SetServerInfo()
    {
//        XmlDocument xmlDoc = XmlEditor.Instance.LoadXml(Application.dataPath + System.IO.Path.DirectorySeparatorChar + CONFIG_FILE_NAME);
        XmlDocument xmlDoc = XmlEditor.Instance.LoadXmlFromResources("Config");

        XmlNode ServerInfo = xmlDoc.SelectSingleNode("ServerInfo");
        XmlNode LobbyServerInfo = ServerInfo.SelectSingleNode("LobbyServerInfo");
        XmlNode RoomServerInfo = ServerInfo.SelectSingleNode("RoomServerInfo");
      
        XmlNode LobbyServerIP = LobbyServerInfo.SelectSingleNode("IP");
        XmlNode LobbyServerPort = LobbyServerInfo.SelectSingleNode("Port");

        XmlNode RoomServerIP = RoomServerInfo.SelectSingleNode("IP");
        XmlNode RoomServerPort = RoomServerInfo.SelectSingleNode("Port");

        m_strLobbyServerIP = LobbyServerIP.InnerText;
        m_nLobbyServerPort = int.Parse(LobbyServerPort.InnerText);

        m_strRoomServerIP = RoomServerIP.InnerText;
        m_nRoomServerPort = int.Parse(RoomServerPort.InnerText);
    }

    public string GetLobbyServerIP()
    {
        return m_strLobbyServerIP;
    }

    public int GetLobbyServerPort()
    {
        return m_nLobbyServerPort;
    }

    public string GetRoomServerIP()
    {
        return m_strRoomServerIP;
    }

    public int GetRoomServerPort()
    {
        return m_nRoomServerPort;
    }
}
