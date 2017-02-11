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
    class LZWIControll
    {
        LZWcomp_decomp lzwcomp_decomp = new LZWcomp_decomp();

        public void TestCompress(string name, TextBox tbinfo)
        {
            FileStream inputStream = new FileStream(name, FileMode.Open, FileAccess.Read);
            StreamReader inputReader = new StreamReader(inputStream);

            FileStream outputStream = new FileStream("CompressedLZW.lzw", FileMode.OpenOrCreate);
            BinaryWriter outputWriter = new BinaryWriter(outputStream);

            byte[] byteArray = File.ReadAllBytes(name);
            byte[] byteOutArray;

            byteOutArray = lzwcomp_decomp.Compresses(byteArray);
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
            FileStream inputStream = new FileStream("CompressedLZW.lzw", FileMode.Open, FileAccess.Read);
            BinaryReader inputReader = new BinaryReader(inputStream);

            FileStream outputStream = new FileStream("DecompressedLZW.bmp", FileMode.Create);
            StreamWriter outputWriter = new StreamWriter(outputStream, Encoding.ASCII);
            
            byte[] bytearray = File.ReadAllBytes("CompressedLZW.lzw");
            byte[] byteoutarray;

            byteoutarray = lzwcomp_decomp.Decompresses(bytearray);
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
            pb.Image = Image.FromFile("DecompressedLZW.bmp");
        }

    }
}
