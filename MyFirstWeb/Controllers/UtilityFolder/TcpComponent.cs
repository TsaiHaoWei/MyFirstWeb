using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyProcess
{
    public class TCPIP_Model
    { 
        public string IP { get; set; }
        public string Port { get; set; }
        public TCPIP_Model(string p1,string p2)
        {
            IP = p1;
            Port = p2;
        }
    }
    public class TcpUtility
    {
        public static TCPIP_Model GetTcpIpPort()
        {
            return new TCPIP_Model(
           ExternalFile.GetProfileString("SERVER", "IP", "DataServer", SystemParameterPath.TcpIniFile),
           ExternalFile.GetProfileString("SERVER", "Port", "DataServer", SystemParameterPath.TcpIniFile));
        }
        public static bool SetData(TCPIP_Model t)
        {
            try
            {
                ExternalFile.WriteProfileString("SERVER", "IP", t.IP, SystemParameterPath.TcpIniFile);
                ExternalFile.WriteProfileString("SERVER", "Port", t.Port, SystemParameterPath.TcpIniFile);
                return true;
            }
            catch 
            {
                return false;
            }


        }
    }
    public class TcpComponent
    {

        public void Start()
        {
            //TCP Connect occupy one thread, is a infinte loop ,one Thread to use it        
            Thread t = new Thread(Tcp);
            t.Start();
        }
        private TCPIP_Model tcpmodel;
        private void Tcp()
        {
            tcpmodel = TcpUtility.GetTcpIpPort();
            TcpServer();
            TcpClient();
        }

        private TcpListener GateWayListener;
        private TcpClient GateWayClient;
        public BinaryReader br;
        public BinaryWriter bw;
        private void TcpServer()
        {
            IPAddress ip = IPAddress.Parse(tcpmodel.IP);
            GateWayListener = new TcpListener(ip, int.Parse(tcpmodel.Port));
            GateWayListener.Start();
            GateWayClient = GateWayListener.AcceptTcpClient();
            while (true)
            {
                NetworkStream clientStream = GateWayClient.GetStream();
                br = new BinaryReader(clientStream);
                string receive = br.ReadString();
                Console.Write("接收到結果" + receive);
                if (!string.IsNullOrEmpty(receive))
                {
                    Byte[] Readdata = Encoding.UTF8.GetBytes(receive);
                }              
            }
        }
        private void TcpClient()
        {
            GateWayClient = new TcpClient(tcpmodel.IP, int.Parse(tcpmodel.Port));
            while (true)
            {
                NetworkStream clientStream = GateWayClient.GetStream();
                br = new BinaryReader(clientStream);
                string receive = br.ReadString();
                if (!string.IsNullOrEmpty(receive))
                {
                    Byte[] Readdata = Encoding.UTF8.GetBytes(receive);
                }
            }
        }
    }
}
