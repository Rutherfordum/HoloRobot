using System;
using System.ComponentModel;
using System.Threading;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.Protocols;

namespace HoloRobot.Connector
{
    public sealed class RosConnector: IRosConnector
    {
        public RosConnector(string url, Protocol protocol, RosSocket.SerializerEnum serializer)
        {
            this.m_rosBridgeServerUrl = url;
            this.m_protocol = protocol;
            this.m_serializer = serializer;
        }

        public RosSocket Socket { get => m_rosSocket; set => throw new NotSupportedException(); }
        public ManualResetEvent IsConnected { get => m_isConnected; set => throw new NotSupportedException(); }
        public EventHandler<bool> ConnectedHandler { get; set; }
        public bool isConnected
        {
            get => _isConnected;
            set { }
        }

        private bool _isConnected;
        private int m_secondsTimeout = 10;
        private RosSocket m_rosSocket;
        private RosSocket.SerializerEnum m_serializer;
        private Protocol m_protocol;
        private string m_rosBridgeServerUrl = "ws://localhost:9090";
        private ManualResetEvent m_isConnected;

        public void Connect()
        {
#if WINDOWS_UWP
            // overwrite selection
            m_protocol = m_protocol.WebSocketUWP;
#endif
            m_isConnected = new ManualResetEvent(false);
            //#if WINDOWS_UWP
            //            ConnectAndWait();
            //#else
            new Thread(ConnectAndWait).Start();
            //#endif
        }

        public void Disconnect()
        {
            m_rosSocket.Close();
        }

        public RosSocket GetRosSocket()
        {
            return m_rosSocket;
        }

        private void ConnectAndWait()
        {
            m_rosSocket = ConnectToRos(m_protocol, m_rosBridgeServerUrl, OnConnected, OnClosed, m_serializer);

            if (!m_isConnected.WaitOne(m_secondsTimeout * 1000))
            {
                ConnectedHandler?.Invoke(this, false);
                throw new WarningException("Failed to connect to RosBridge at: " + m_rosBridgeServerUrl);
            }
        }

        private RosSocket ConnectToRos(Protocol protocolType, string serverUrl, EventHandler onConnected = null, EventHandler onClosed = null, RosSocket.SerializerEnum serializer = RosSocket.SerializerEnum.Microsoft)
        {
            IProtocol protocol = ProtocolInitializer.GetProtocol(protocolType, serverUrl);
            protocol.OnConnected += onConnected;
            protocol.OnClosed += onClosed;
            return new RosSocket(protocol, serializer);
        }

        private IProtocol GetProtocol(Protocol protocol, string rosBridgeServerUrl)
        {

#if WINDOWS_UWP
            Debug.Log("Defaulted to UWP m_protocol");
            return new RosBridgeClient.Protocols.WebSocketUWPProtocol(m_rosBridgeServerUrl);
#else
            switch (protocol)
            {
                case Protocol.WebSocketNET:
                    return new WebSocketNetProtocol(rosBridgeServerUrl);
                case Protocol.WebSocketSharp:
                    return new WebSocketSharpProtocol(rosBridgeServerUrl);
                case Protocol.WebSocketUWP:
                    //"WebSocketUWP only works when deployed to HoloLens, defaulting to WebSocketNetProtocol"
                    return new WebSocketNetProtocol(rosBridgeServerUrl);
                default:
                    return null;
            }
#endif
        }

        private void OnConnected(object sender, EventArgs e)
        {
            m_isConnected.Set();
            _isConnected = true;
            //  Debug.Log("Connected to RosBridge: " + m_rosBridgeServerUrl);
            ConnectedHandler?.Invoke(this,true);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            m_isConnected.Reset();
            _isConnected = false;
            //  Debug.Log("Disconnected from RosBridge: " + m_rosBridgeServerUrl);
            ConnectedHandler?.Invoke(this,false);
        }
    }
}