using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace comp_decomp
{
    class Hcontrol
    {
        Huffmancd hcd = new Huffmancd();

        public void TestCompress(string name, TextBox tbinfo)
        {
            FileStream inputStream = new FileStream(name, FileMode.Open, FileAccess.Read);
            StreamReader inputReader = new StreamReader(inputStream);

            FileStream outputStream = new FileStream("CompressedH.huffman", FileMode.OpenOrCreate);
            BinaryWriter outputWriter = new BinaryWriter(outputStream);

            byte[] byteArray = File.ReadAllBytes(name);
            byte[] byteOutArray;

            byteOutArray = hcd.Compresses(byteArray);

            for (int i = 0; i < byteOutArray.Length; i++)
            {
                byte ibyte = byteOutArray[i];
                outputStream.WriteByte(ibyte);
            }
            //Compress(inputReader, outputWriter, tbinfo);
            MessageBox.Show("Compressed");


            float k = (float)inputStream.Length / outputStream.Length;
            tbinfo.Text += " name primary: " + inputStream.Name + " length primary: " + inputStream.Length
                + " name compressed: " + outputStream.Name + " length compressed: " + outputStream.Length
                        + " K= " + k;

            outputWriter.Close();
            outputStream.Close();

            inputReader.Close();
            inputStream.Close();

        }


        public void TestDecompress(PictureBox pb, TextBox tbinfo)
        {
            FileStream inputStream = new FileStream("CompressedH.huffman", FileMode.Open, FileAccess.Read);
            BinaryReader inputReader = new BinaryReader(inputStream);

            FileStream outputStream = new FileStream("DecompressedH.bmp", FileMode.Create);
            StreamWriter outputWriter = new StreamWriter(outputStream, Encoding.ASCII);

            byte[] bytearray = File.ReadAllBytes("CompressedH.huffman");
            byte[] byteoutarray;

            byteoutarray = hcd.Decompresses(bytearray);

            for (int i = 0; i < byteoutarray.Length; i++)
            {
                byte ibyte = byteoutarray[i];
                outputStream.WriteByte(ibyte);
            }


            //Decompress(inputReader, outputWriter, tbinfo);
            MessageBox.Show("Decompressed");

            outputWriter.Close();
            outputStream.Close();

            inputReader.Close();
            inputStream.Close();

            pb.SizeMode = PictureBoxSizeMode.CenterImage;
            pb.Image = Image.FromFile("DecompressedH.bmp");
        }

    }
}
