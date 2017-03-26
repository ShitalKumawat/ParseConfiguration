using Common;
using Model;
using System;
using System.IO;

namespace GenericConfiguration
{
    class Program
    {
        static void Main(string[] args)
        {
            var xmlProvider = new XmlFileProvider<StudentBatch>()
                                { FileName = Path.Combine(Environment.CurrentDirectory, "TestData.xml") };
            Console.WriteLine(xmlProvider.Read().ToString());

            var configFileProvider = new ConfigurationFileProvider<StudentBatch>();
            Console.WriteLine(configFileProvider.Read().ToString());

            Console.ReadLine();
        }
    }
}
