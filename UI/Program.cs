using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading;
using Plossum.CommandLine;
using PortScanner;

namespace UI
{
    class Program
    {
        private static Scanner _scanner;
        private static Options _options;

        static void Main(string[] args)
        {
            _options = new Options();
            var parser = new CommandLineParser(_options);
            parser.Parse();

            Console.WriteLine(parser.UsageInfo.GetHeaderAsString(78));
            if (_options.Help)
            {
                Console.WriteLine(parser.UsageInfo.GetOptionsAsString(78));
                Console.ReadKey();
                return;
            }
            if (parser.HasErrors)
            {
                Console.WriteLine(parser.UsageInfo.GetErrorsAsString(78));
                Output.ShowUsage();
                Console.ReadKey();
                return;
            }

            var ipRange = new IPRange(_options.StartIP, _options.EndIP);
            _scanner = new Scanner(new TcpScan(), new[] { ipRange }, _options.PortsAsInt32);

            Output.ShowInputInfo(_options, _scanner.TotalEndPoints);
            Thread.Sleep(4000);

            _scanner.Scan.ResponsedEndpoints.CollectionChanged += ResponsedEndpoints_CollectionChanged;
            _scanner.Scan.SendedEndpoints.CollectionChanged += SendedEndpoints_CollectionChanged;
            _scanner.Start();

            Console.ReadKey();
        }

        private static void SendedEndpoints_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var collection = (ObservableCollection<IP4EndPoint>)sender;
            if (e.Action != NotifyCollectionChangedAction.Add)
                return;
            foreach (IP4EndPoint endpoint in e.NewItems)
                Output.ShowEndPointInfo("Sended\t\t\t", endpoint, collection.Count);
            if (collection.Count >= _scanner.TotalEndPoints)
                Output.ShowSendIsDone();
        }

        private static void ResponsedEndpoints_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var collection = (ObservableCollection<IP4EndPoint>)sender;
            if (e.Action != NotifyCollectionChangedAction.Add)
                return;
            foreach (IP4EndPoint endpoint in e.NewItems)
            {
                Output.ShowEndPointInfo("Response\t\t", endpoint, collection.Count);
                Helper.WriteToFile(_options.Filepath, endpoint.AddressStr,
                    endpoint.Port.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}
