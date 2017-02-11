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
    class LZW_cd_i
    {
        private const int NumBytesPerCode = 2;

        static int ReadCode(BinaryReader reader)
        {
            int code = 0;
            int shift = 0;

            for (int i = 0; i < NumBytesPerCode; i++)
            {
                byte nextByte = reader.ReadByte();
                code += nextByte << shift;
                shift += 8;
            }

            return code;
        }

        static void WriteCode(BinaryWriter writer, int code)
        {
            int shift = 0;
            int mask = 0xFF;

            for (int i = 0; i < NumBytesPerCode; i++)
            {
                byte nextByte = (byte)((code >> shift) & mask);
                writer.Write(nextByte);
                shift += 8;
            }
        }

        static void Compress(StreamReader input, BinaryWriter output, TextBox tb)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            LZW_table table = new LZW_table(NumBytesPerCode);

            char first = (char)input.Read();
            string match = first.ToString();

            while (input.Peek() != -1)
            {
                char next = (char)input.Read();
                string nextMatch = match + next;

                if (table.Contains(nextMatch))
                {
                    match = nextMatch;
                }
                else
                {
                    WriteCode(output, table.GetCode(match));
                    table.AddCode(nextMatch);
                    match = next.ToString();
                }
            }

            WriteCode(output, table.GetCode(match));

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            tb.Text += " time compressed : " + elapsedTime;
        }

        static void Decompress(BinaryReader input, StreamWriter output, TextBox tb)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<string> table = new List<string>();

            for (int i = 0; i < 256; i++)
            {
                char ch = (char)i;
                table.Add(ch.ToString());
            }

            int firstCode = ReadCode(input);
            char matchChar = (char)firstCode;
            string match = matchChar.ToString();

            output.Write(match);

            while (input.PeekChar() != -1)
            {
                int nextCode = ReadCode(input);

                string nextMatch;
                if (nextCode < table.Count)
                    nextMatch = table[nextCode];
                else
                    nextMatch = match + match[0];

                output.Write(nextMatch);

                table.Add(match + nextMatch[0]);
                match = nextMatch;
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            tb.Text += " time decompressed : " + elapsedTime;
        }

        public void TestCompress(string name,  TextBox tbinfo)
        {
            FileStream inputStream = new FileStream(name, FileMode.Open);
            StreamReader inputReader = new StreamReader(inputStream);

            FileStream outputStream = new FileStream("CompressedLZW.lzw", FileMode.OpenOrCreate);
            BinaryWriter outputWriter = new BinaryWriter(outputStream);

            Compress(inputReader, outputWriter, tbinfo);
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

        public void TestDecompres(PictureBox pb, TextBox tbinfo)
        {
            FileStream inputStream = new FileStream("CompressedLZW.lzw", FileMode.Open);
            BinaryReader inputReader = new BinaryReader(inputStream);

            FileStream outputStream = new FileStream("DecompressedLZW.bmp", FileMode.Create);
            StreamWriter outputWriter = new StreamWriter(outputStream, Encoding.ASCII);

            Decompress(inputReader, outputWriter, tbinfo);
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
