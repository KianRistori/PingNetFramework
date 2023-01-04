using System;
using System.IO;
using System.Threading;
using System.Net.NetworkInformation;
using System.Net;

namespace Ping1._0
{
    class Program
    {
        static byte[] buffer = BitConverter.GetBytes(32);
        static int lSize = 32;
        static int iTTL = 55;
        static int wtimeout = 0;
        static int count = 4;
        static void Main(string[] args)
        {
            string sAppPath = Environment.CurrentDirectory;
            while (true)
            {
                buffer = BitConverter.GetBytes(32);
                lSize = 32;
                iTTL = 55;
                wtimeout = 0;
                count = 4;
                Console.Write(sAppPath + @":\>");
                string tmp = Console.ReadLine();
                string[] command = Command(tmp);
                bool ok = ControllCommand(command);
                if (ok)
                    CommandReserch(command);
            }
        }

        static void CommandHost(string command)
        {
            string hostname = command;
            int timeout = 10000;
            byte[] transmit = BitConverter.GetBytes(0);
            int send = 0;
            int lost = 0;
            Ping ping = new Ping();
            PingOptions pingOptions = new PingOptions() { Ttl = iTTL };
            transmit = buffer;
            int[] timeTrip = new int[count];
            try
            {
                for (int i = 0; i < count; i++)
                {
                    PingReply pingreply = ping.Send(hostname, timeout, buffer, pingOptions);
                    if (pingreply.Status == IPStatus.Success)
                    {
                        Console.WriteLine("Risposta da: " + pingreply.Address + " byte " + pingreply.Buffer.GetValue(0) + " durata=" + pingreply.RoundtripTime +"ms"+ " TTL=" + pingreply.Options.Ttl);
                        timeTrip[i] = (int)pingreply.RoundtripTime;
                        send++;
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Console.WriteLine("Request timed out");
                        lost++;
                    }
                    if (pingreply.Status==IPStatus.DestinationNetworkUnreachable)
                    {
                        Console.WriteLine("Errore Generale");
                    }
                }
            }
            catch { Console.WriteLine("Host sconosciuto"); }

            Console.WriteLine("Statistiche Ping per {0}", hostname);
            Console.WriteLine("   Pacchetti: Trasmessi = {0}, Ricevuti = {1}, Persi = {2} ({3}% persi)", count, send, lost, (double)lost / (double)count * 100);
            Console.WriteLine("Tempo apprpssimativo percorsi andata/ritorno in millisencondi");
            Console.WriteLine("   Minimo = {0}ms, Massimo = {1}ms, Medio = {2}ms", GetLower(timeTrip), GetHigh(timeTrip), GetAverge(timeTrip));
            Console.WriteLine("");
        }

        static int GetAverge(int[] vett)
        {
            int ris = 0;
            for (int i = 0; i < vett.Length; i++)
            {
                ris = ris + vett[i];
            }
            return ris/vett.Length;
        }
        static int GetHigh(int[] vett)
        {
            int ris = vett[0];
            for (int i = 0; i < vett.Length; i++)
            {
                if (vett[i]>ris)
                {
                    ris = vett[i];
                }
            }
            return ris;
        }
        static int GetLower(int[] vett)
        {
            int ris = vett[0];
            for (int i = 0; i < vett.Length; i++)
            {
                if (vett[i]<ris)
                {
                    ris = vett[i];
                }
            }
            return ris;
        }

        static void CommandReserch(string[] command)
        {
            if (command[1] == "/?")
            {
                Console.WriteLine("Sintassi: ping [-t] [-a] [-n count] [-l size] [-f] [-i TTL] [-v TOS] \n [-r count][-s count][[-j host - list] | [-k host - list]] \n [-w timeout][-R][-S srcaddr][-c compartment] \n [-4][-6] target_name");
            }

            for (int i = 0; i < command.Length; i++)
            {
                if (command[i].StartsWith("-n"))
                {
                    string[] commandN = command[i].Split(':');
                    count = int.Parse(commandN[1]);
                }
                if (command[i].StartsWith("-l"))
                {
                    string[] commandN = command[i].Split(':');
                    lSize = int.Parse(commandN[1]);
                }
                if (command[i].StartsWith("-i"))
                {
                    string[] commandN = command[i].Split(':');
                    iTTL = int.Parse(commandN[1]);
                }
                if (command[i].StartsWith("-w"))
                {
                    string[] commandN = command[i].Split(':');
                    wtimeout = int.Parse(commandN[1]);
                }
            }
            CommandHost(command[command.Length-1]);
        }

        static string[] Command(string command)
        {
            return command.Split(' ');
        }

        static bool ControllCommand(string[] command)
        {
            if (command[0] == "ping")
                return true;
            return false;
        }
    }
}
