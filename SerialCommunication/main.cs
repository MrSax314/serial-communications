using System;
using System.IO.Ports;
using System.Threading;

public class main
{
    static bool _continue;

    static string portName = "COM7";
    static int baudRate = 250000;
    static SerialPort _serialPort;

    // Stores info for comms
    static int size = 6;
    static byte[] input = new byte[size];
    static int z;
    static char[] ch = new char[1];
    static byte b;
    static byte[] output = new byte[6] {10,20,30,1,1,0};
    static int i;

    public static void Main()
    {
        // Create new thread for reading input data
        Thread readThread = new Thread(Read);
        Thread writeThread = new Thread(Write);

        // Create a new SerialPort object with default settings
        _serialPort = new SerialPort(portName, baudRate, Parity.None, 8);

        _serialPort.Open();     // Open port
        readThread.Start();     // Start thread
        writeThread.Start();
        _continue = true;       // Latch loop
        Thread.Sleep(1000);     // Wait for input data to come in brefily

        while (_continue)
        {
            //printVals();      // Prints value of read encoder angles
            //Console.WriteLine(i);
            i++;
        }
        readThread.Join();
        writeThread.Join();
        _serialPort.Close();
    }

    public static void Read()
    {
        _serialPort.DiscardInBuffer();          // Clear buffer
        while (_serialPort.ReadByte() > 0)      // Check for '0'
        {
            Thread.Sleep(100);
        }
        while (_continue)                       // Start once '0' read
        {
            if (_serialPort.BytesToRead > 6)
                _serialPort.Read(input, 0, 6);  // Read only if 6 bytes available
        }
    }

    public static void printVals()
    {
        for (int i = 0; i < 5; i++)
        {
            Console.Write("{0}, ", input[i]);
        }
        Console.Write("\n");
    }

    public static void Write()
    {
        while (_continue)
        {
            _serialPort.Write(output, 0, 6);
        }
    }
}