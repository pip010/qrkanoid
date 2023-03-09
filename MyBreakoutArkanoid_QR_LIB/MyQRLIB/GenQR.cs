//original source: https://github.com/codebude/QRCoder

using System;
using System.Collections.Generic;


namespace MyQRLIB
{
    public class GenQR
    {
        internal static bool[,] BreakQrCode(QRCodeData qrdata)
        {
            var sideLength = qrdata.ModuleMatrix.Count;

            bool[,] result = new bool[sideLength, sideLength];

            for (var y = 0; y < sideLength; y++)
            {
                for (var x = 0; x < sideLength; x++)
                {
                    var module = qrdata.ModuleMatrix[x][y];
                    result[x, y] = module ? true : false;
                }
            }

            return result;
        }

        static public bool[,] Generate(String text)
        {
            bool[,] result;

            using (var gen = new QRCodeGenerator())
            {
                using (var data = gen.CreateQrCode(text, QRCodeGenerator.ECCLevel.H,true,false,QRCodeGenerator.EciMode.Utf8))
                {
                    //result = data.GetRawData(QRCodeData.Compression.Uncompressed);
                    result = BreakQrCode(data);
                }
            }


            return result;
        }
    }
}