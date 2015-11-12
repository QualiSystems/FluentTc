using System;
using System.Collections;

namespace FluentTc.Samples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            new RemoteTc()
                .Connect(_ => _.ToHost(""))
                .GetAgents(_ => _.Enabled().Authorized().Disconnected())
                .ForEach(a => Console.WriteLine("{0}", a.Name));
        }
    }
}