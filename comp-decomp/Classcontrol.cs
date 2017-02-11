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
  
        public class Classcontrol
        {    

            public void compressrle(string inf,TextBox tb)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                FileStream infile = new FileStream(inf, FileMode.Open, FileAccess.Read);
                FileStream outfile = new FileStream("compressed.rle", FileMode.Create, FileAccess.Write);
                byte current = (byte)infile.ReadByte();
                byte bcurrent = (byte)infile.ReadByte();
                do
                {
                    if (current == bcurrent)
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
                            current = checker;
                     }
                    else
                    {
                        if (current < (byte)192)
                        {
                            outfile.WriteByte(current);
                        }
                        else if (current >= (byte)192)
                        {
                            outfile.WriteByte((byte)193);
                            outfile.WriteByte(current);
                        }
                        current = bcurrent;
                    }
                    if (infile.Position == infile.Length)
                    {
                        if (bcurrent < (byte)192)
                        {
                            outfile.WriteByte(bcurrent);
                        }
                        else if (bcurrent >= (byte)192)
                        {
                            outfile.WriteByte((byte)193);
                            outfile.WriteByte(bcurrent);
                        }
                    }
                    bcurrent = (byte)infile.ReadByte();
                    Application.DoEvents();
                }
                while (infile.Position != infile.Length);
                if (current < (byte)192)
                {
                    outfile.WriteByte(current);
                }
                else if (current >= (byte)192)
                {
                    outfile.WriteByte((byte)193);
                    outfile.WriteByte(current);
                }
                if (bcurrent < (byte)192)
                {
                    outfile.WriteByte(bcurrent);
                }
                else if (bcurrent >= (byte)192)
                {
                    outfile.WriteByte((byte)193);
                    outfile.WriteByte(bcurrent);
                }
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
                    ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds );
                tb.Text += " time compressed : " + elapsedTime;     
            }

            public void decompressrle(PictureBox pb,TextBox tb)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                FileStream compf = new FileStream("compressed.rle", FileMode.Open, FileAccess.Read);
                BinaryReader binarread = new BinaryReader(compf);
                FileStream decompf = new FileStream("decompressed.bmp", FileMode.Create, FileAccess.Write);
                BinaryWriter binarwrite = new BinaryWriter(decompf);
                byte read = 0;
                int counter = 0;
                do
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
                while (compf.Position != compf.Length);
                compf.Dispose();
                decompf.Dispose();
                compf.Close();
                decompf.Close();
                MessageBox.Show("decompressed");
                pb.SizeMode = PictureBoxSizeMode.CenterImage;
                pb.Image = Image.FromFile("decompressed.bmp");
              //  File.Delete("compressed.rle");
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
                tb.Text += " time decompressed : " + elapsedTime;  
            }

            
        }
    }


