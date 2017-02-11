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
    class LZWcd
    {
        private const int NumBytesPerCode = 2;
        public int sizefile {get; set;}

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

        static void Compress(StreamReader input, BinaryWriter output,TextBox tb)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
           
            LZW_table table = new LZW_table(NumBytesPerCode);

            char firstChar = (char)input.Read();
            string match = firstChar.ToString();

            while (input.Peek() != -1)
            {
                char nextChar = (char)input.Read();
                string nextMatch = match + nextChar;

                if (table.Contains(nextMatch))
                {
                    match = nextMatch;
                }
                else
                {
                    WriteCode(output, table.GetCode(match));
                    table.AddCode(nextMatch);
                    match = nextChar.ToString();
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

       public  void TestCompress(string name,TextBox tb, TextBox tbinfo)
        {
            FileStream inputStream = new FileStream(name, FileMode.Open);
            StreamReader inputReader = new StreamReader(inputStream);

            FileStream outputStream = new FileStream("CompressedLZW.txt", FileMode.OpenOrCreate);
            BinaryWriter outputWriter = new BinaryWriter(outputStream);

            sizefile = (int)inputStream.Length;
            Compress(inputReader, outputWriter,tbinfo);
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

       public void TestDecompres(TextBox tb,TextBox tbinfo)
        {
            FileStream inputStream = new FileStream("CompressedLZW.txt", FileMode.Open);
            BinaryReader inputReader = new BinaryReader(inputStream);

            FileStream outputStream = new FileStream("DecompressedLZW.txt", FileMode.Create);
            StreamWriter outputWriter = new StreamWriter(outputStream, Encoding.ASCII);

            Decompress(inputReader, outputWriter,tbinfo);

            MessageBox.Show("Decompressed");

            outputWriter.Close();
            outputStream.Close();

            inputReader.Close();
            inputStream.Close();

                tb.Text = File.ReadAllText("DecompressedLZW.txt");

                FileStream inf = new FileStream("DecompressedLZW.txt", FileMode.Open);
                StreamReader infw = new StreamReader(inf, Encoding.ASCII);
                int Flags = 0;
                long fs = inf.Length;
                if (fs <= sizefile)
                    Flags = 1;
           if (Flags != 1)
           {
                FileStream outf = new FileStream("DecompressedLZW1.txt", FileMode.Create);
                BinaryWriter outfw = new BinaryWriter(outf, Encoding.ASCII);
                int count = 1;
                string matches ="";

                while (count <= sizefile)
                {
                    char firstChar = (char)infw.Read();
                    matches += firstChar.ToString();
                    count++;
                }
                outfw.Write(matches);
                outfw.Close();
                outf.Close();
           }
                infw.Close();
                inf.Close();
           if (Flags != 1)
                tb.Text = File.ReadAllText("DecompressedLZW1.txt");
            }
        }
 
    }

