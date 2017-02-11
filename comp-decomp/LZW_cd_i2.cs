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
    class LZW_cd_i2_table
    {
        public LZW_cd_i2_table(int numBytesPerCode)
            {
                maxCode = (1 << (8 * numBytesPerCode)) - 1;
            }

            public void AddCode(byte s)
            {
                if (nextAvailableCode <= maxCode)
                {
                    if ( !table.ContainsKey(s))
                        table[s] = nextAvailableCode++;
                }
                else
                {
                    throw new Exception("LZW string table overflow");
                }
            }

            public int GetCode(byte s)
            {
                    return table[s];
            }

            public bool Contains(byte s)
            {
                return  table.ContainsKey(s);
            }

            private Dictionary<byte, int> table = new Dictionary<byte, int>();
            private int nextAvailableCode = 256;
            private int maxCode;


    }


    class LZW_cd_i2
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

        static void WriteCode(BinaryWriter writer, byte code)
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

        public void compress(string inf, TextBox tb)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            FileStream infile = new FileStream(inf, FileMode.Open, FileAccess.Read);
            FileStream outfile = new FileStream("compressed2.lzw", FileMode.Create, FileAccess.Write);
            BinaryWriter binarwrite = new BinaryWriter(outfile);

            LZW_cd_i2_table table = new LZW_cd_i2_table(NumBytesPerCode);

            byte first = (byte)infile.ReadByte();


            while (infile.Position != infile.Length) 
            {
                byte next = (byte)infile.ReadByte();

                if (table.Contains(next))
                {
                    first = next;
                }
                else
                {

                    table.AddCode(next);
                    WriteCode(binarwrite, first);
                    first = next;
                }
            }

            WriteCode(binarwrite, first);
         
            MessageBox.Show("compressed");
            float k = (float)infile.Length / outfile.Length;
            tb.Text += " name primary: " + infile.Name + " length primary: " + infile.Length
                + " name compressed: " + outfile.Name + " length compressed: " + outfile.Length
                        + " K= " + k;
            infile.Dispose();
            outfile.Dispose();
            infile.Close();
            outfile.Close();
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            tb.Text += " time compressed : " + elapsedTime;
        }

        public void decompress(PictureBox pb, TextBox tb)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            FileStream compf = new FileStream("compressed2.lzw", FileMode.Open, FileAccess.Read);
            BinaryReader binarread = new BinaryReader(compf);
            FileStream decompf = new FileStream("decompressed2lzw.bmp", FileMode.Create, FileAccess.Write);
            BinaryWriter binarwrite = new BinaryWriter(decompf);
            StreamWriter outputWriter = new StreamWriter(decompf);

            List<string> table = new List<string>();


            int firstCode = ReadCode(binarread);
            char matchChar = (char)firstCode;
            string match = matchChar.ToString();

            //binarwrite.Write(match);
            outputWriter.Write(match);

            while (compf.Position != compf.Length) 
            {
                int nextCode = ReadCode(binarread);

                string nextMatch;
                if (nextCode < table.Count)
                    nextMatch = table[nextCode].ToString();
                else
                    nextMatch = match + match[0];

                //binarwrite.Write(nextMatch);
                outputWriter.Write(nextMatch);
                string s = match + nextMatch[0];
                table.Add(s);
                match = nextMatch;
            }
            
            MessageBox.Show("decompressed");
            pb.SizeMode = PictureBoxSizeMode.CenterImage;
            pb.Image = Image.FromFile("decompressed2lzw.bmp");
            //  File.Delete("compressed.rle");
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            tb.Text += " time decompressed : " + elapsedTime;
        }
    }
}
