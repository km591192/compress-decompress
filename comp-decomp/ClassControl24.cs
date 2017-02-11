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
    class ClassControl24
    {
        public string Encode()
        {
            StringBuilder sb = new StringBuilder();
            int count = 1;
            string sout = "";
            string[] str = File.ReadAllLines("a.txt");
            char[] s = String.Join("", str).ToCharArray();
            char current = s[0];
            for (int i = 1; i < s.Length; i++)
            {
                if (current == s[i])
                {
                    count++;
                }
                else
                {
                    sout += count.ToString() + current.ToString();
                    count = 1;
                    current = s[i];
                }
            }
            sout += count.ToString() + current.ToString();
            File.WriteAllText("compressedletter.rle", sout);
            MessageBox.Show("compressed");
            return sb.ToString();
        }

        public string Decode()
        {
            string a = "",sout = "";
            int count = 0;
            StringBuilder sb = new StringBuilder();
            string[] str = File.ReadAllLines("compressedletter.rle");
            char[] s = String.Join("", str).ToCharArray();
            char current = char.MinValue;
            for (int i = 0; i < s.Length; i++)
            {
                current = s[i];
                if (char.IsDigit(current))
                    a += current;
                else
                {
                    count = int.Parse(a);
                    a = "";
                    for (int j = 0; j < count; j++)
                        sout += current.ToString();
                    //sb.Append(current);
                }
            }
            int NumberChars = sout.Length;
          byte[] bytes = new byte[NumberChars / 2];
          for (int i = 0; i < NumberChars; i += 2)
            bytes[i / 2] = Convert.ToByte(sout.Substring(i, 2), 16);
            File.WriteAllBytes("decompressedletter.bmp",bytes);
            MessageBox.Show("decompressed");
            return sb.ToString();
        }



      private void singlebyte(FileStream write, byte b)
        {
            if (b < (byte)192)
            {
                write.WriteByte(b);
            }
            else if (b >= (byte)192)
            {
                write.WriteByte((byte)193);
                write.WriteByte(b);
            }
        }
        private void comp (FileStream r, byte currentr, FileStream outfile,byte bcurrentr)
        {
            if (currentr == bcurrentr)
                {
                    currentr =  alotbyte(r,currentr,outfile);
                }
                else
                {
                    singlebyte(outfile, currentr);
                    currentr = bcurrentr;
                }
                if (r.Position == r.Length)
                {
                    singlebyte(outfile, bcurrentr);
                }
                bcurrentr = (byte)r.ReadByte();
                Application.DoEvents();
        }

    private byte alotbyte(FileStream infile, byte current, FileStream outfile)    
    {
                    int counter = 2;
                    byte block = Convert.ToByte(192);
                    byte checker = (byte)infile.ReadByte();
                    while (checker == current)
                    {
                        if (infile.Position >= infile.Length)
                        {
                            break;
                        }
                        checker = (byte)infile.ReadByte();
                        counter++;
                    }
                    if (counter <= 63)
                    {
                        block += (byte)counter;
                        outfile.WriteByte(block);
                        outfile.WriteByte(current);
                    }
                    else if (counter > 63)
                    {
                        while (counter > 0)
                        {
                            if (counter - 63 < 0)
                            {
                                block += (byte)counter;
                                outfile.WriteByte(block);
                                outfile.WriteByte(current);
                            }
                            else if (counter - 63 >= 0)
                            {
                                outfile.WriteByte(255);
                                outfile.WriteByte(current);
                            }
                            counter -= 63;
                        }
                    }
                    return checker;
        }
    private void decom(byte read,BinaryReader binarread,int counter,BinaryWriter binarwrite)
    {
        read = binarread.ReadByte();
        if (read > (byte)192)
        {
            read -= (byte)192;
            counter = (int)read;
            read = binarread.ReadByte();
            for (int i = 0; i < counter; i++)
            {
                binarwrite.Write(read);
            }
            counter = 0;
        }
        else
        {
            binarwrite.Write(read);
        }
    }

    public void compressrle24(string infr, string infg, string infb, TextBox tb)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            FileStream r = new FileStream(infr, FileMode.Open, FileAccess.Read);
            FileStream g = new FileStream(infg, FileMode.Open, FileAccess.Read);
            FileStream b = new FileStream(infb, FileMode.Open, FileAccess.Read);
            FileStream outfile = new FileStream("compressed24.rle", FileMode.Create, FileAccess.Write);
            byte currentr = (byte)r.ReadByte();
            byte bcurrentr = (byte)r.ReadByte();
            byte currentg = (byte)g.ReadByte();
            byte bcurrentg = (byte)g.ReadByte();
            byte currentb = (byte)b.ReadByte();
            byte bcurrentb = (byte)b.ReadByte();
            do
            {
                comp(r, currentr, outfile, bcurrentr);
                comp(g, currentg, outfile, bcurrentg);
                comp(b, currentb, outfile, bcurrentb);
            }
            while (b.Position != b.Length);

            //singlebyte(outfile, current); singlebyte(outfile, bcurrent);
            MessageBox.Show("compressed");
            float k = (float)(r.Length + g.Length + b.Length) / outfile.Length;
            tb.Text += " name compressed: " + outfile.Name + " length compressed: " + outfile.Length + " K= " + k;
            r.Dispose();
            g.Dispose();
            b.Dispose();
            r.Close();
        g.Close();
        b.Close();
        outfile.Dispose();
        outfile.Close();
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            tb.Text += " time compressed : " + elapsedTime;
        }

        public void decompressrle24(PictureBox pb, TextBox tb)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            FileStream compf = new FileStream("compressed24.rle", FileMode.Open, FileAccess.Read);
            BinaryReader binarread = new BinaryReader(compf);
            FileStream decompf = new FileStream("decompressed24.bmp", FileMode.Create, FileAccess.Write);
            BinaryWriter binarwrite = new BinaryWriter(decompf);
            byte read = 0;
            int counter = 0;
            do
            {
                decom(read, binarread,counter, binarwrite);
            }
            while (compf.Position != compf.Length);
            compf.Dispose();
            decompf.Dispose();
            compf.Close();
            decompf.Close();
            MessageBox.Show("decompressed");
            pb.SizeMode = PictureBoxSizeMode.CenterImage;
            pb.Image = Image.FromFile("decompressed24.bmp");
            //  File.Delete("compressed.rle");
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            tb.Text += " time decompressed : " + elapsedTime;
        }
    }
}
