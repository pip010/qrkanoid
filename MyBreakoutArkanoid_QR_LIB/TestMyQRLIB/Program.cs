using MyQRLIB;
using System;


// A version of the classic "Hello World" program
class Program
{
    static void Main()
    {
        var final_qr = MyQRLIB.GenQR.Generate("myveryssecrettext");

        Console.WriteLine(String.Format("QR: {0}", final_qr));
    }
}


