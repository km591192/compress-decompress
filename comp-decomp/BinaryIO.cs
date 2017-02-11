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
    class BinaryIO
    {
        public static  int BYTESININT = 4;
    private  int SIZEOFBYTE = 8;
    private int SIZEOFINT = 32;
    private int MASK = 0xFF;
    int bytesProceeded;
    int buffer;
    int bitsInBuffer;
    private byte[] data;
    private int dataPointer;

    public BinaryIO(byte[] dest) {
        data = dest;
        dataPointer = 0;
        bytesProceeded = 0;
        bitsInBuffer = 0;
        clearBuffer();
    }

    private void clearBuffer() {
        buffer = 0;
    }
    public void WriteBits(int value, int bitsCount)
    {
        if (bufferHasFreeSpaceMoreThan(bitsCount))
        {
            writeValueToBuffer(value, bitsCount);
        }
        else
        {
            int shift = freeBitsInBuffer();
            fillUpBufferWithPartOfValueBits(value, bitsCount, shift);
            copyFullBufferToData();
            copyRemainingBitsOfValueToBuffer(value, bitsCount, shift);
        }
    }

    private void copyRemainingBitsOfValueToBuffer(int value, int bitsCount, int shift) {
        buffer = value << (SIZEOFINT - bitsCount + shift);
        bitsInBuffer = bitsCount - shift;
        buffer >>= (freeBitsInBuffer());

    }

    private void copyFullBufferToData()
    {
        byte[] bufferAsBytes = bufferToByteArray();

        for (int i = (BYTESININT - 1); i >= 0; --i, dataPointer++)
        {
            data[dataPointer] = bufferAsBytes[i];
        }
        bytesProceeded += BYTESININT;
    }

    private byte[] bufferToByteArray() {
        byte[] tmp = new byte[BYTESININT];
        tmp[0] = (byte) (((buffer << 3 * SIZEOFBYTE) >> 3 * SIZEOFBYTE) & MASK);
        tmp[1] = (byte) (((buffer << 2 * SIZEOFBYTE) >> 3 * SIZEOFBYTE) & MASK);
        tmp[2] = (byte) (((buffer << SIZEOFBYTE) >> 3 * SIZEOFBYTE) & MASK);
        tmp[3] = (byte) (((buffer << 0) >> 3 * SIZEOFBYTE) & MASK);
        return tmp;
    }

    private void fillUpBufferWithPartOfValueBits(int value, int bitsCount, int shift) {
        prepareBuffer(shift);
        value >>= (bitsCount - shift);
        buffer |= value;
    }

    private void prepareBuffer(int shift)
    {
        if (bufferIsEmpty(shift))
        {
            clearBuffer();
        }
        else
        {
            shiftBitsInBufferToTheLeft(shift);
        }
    }

    private void shiftBitsInBufferToTheLeft(int shift)
    {
        buffer <<= shift;
    }

    private bool bufferIsEmpty(int shift)
    {
        return shift == SIZEOFINT;
    }

    private void writeValueToBuffer(int value, int bitsCount)
    {
        buffer <<= bitsCount;
        buffer |= value;
        bitsInBuffer += bitsCount;
    }

    private bool bufferHasFreeSpaceMoreThan(int bitsCount)
    {
        return freeBitsInBuffer() > bitsCount;
    }

    private int freeBitsInBuffer()
    {
        return SIZEOFINT - bitsInBuffer;
    }

    public void Flush()
    {
        if (bufferIsEmpty())
        {
            return;
        }
        shiftBitsInBufferToTheLeft(freeBitsInBuffer());
        copyRemainingBufferToData();
    }

    private void copyRemainingBufferToData()
    {
        byte[] bufferAsBytes = bufferToByteArray();

        int bytesCount = (bitsInBuffer + SIZEOFBYTE - 1) / SIZEOFBYTE;
        for (int i = 3; i >= BYTESININT - bytesCount; --i, dataPointer++)
        {
            data[dataPointer] = bufferAsBytes[i];
        }
        bytesProceeded += bytesCount;
    }

    private bool bufferIsEmpty()
    {
        return bitsInBuffer == 0;
    }


    public int ReadBits(int bitsCount) {
        Debug.Assert (bitsCount <= SIZEOFINT);
        fillBufferWithData();
        int result = readBitsFromBuffer(bitsCount);
        return result;
    }

    private void fillBufferWithData()
    {
        int scannedByte = 0;
        while (isSpaceForByteInBuffer() && HaveMoreData())
        {
            scannedByte = readByte();
            writeValueToBuffer(scannedByte, SIZEOFBYTE);
        }
    }

    private bool isSpaceForByteInBuffer()
    {
        return bitsInBuffer <= SIZEOFINT - SIZEOFBYTE;
    }

    private int readByte()
    {
        int scannedByte;
        scannedByte = 0;
        scannedByte |= (data[dataPointer] & MASK);
        dataPointer += 1;
        return scannedByte;
    }

    public bool HaveMoreData()
    {
        return dataPointer < data.Length;
    }

    private int readBitsFromBuffer(int bitsCount) {
        int shift = freeBitsInBuffer();

        int copy = buffer;
        int result = ((copy << shift) >> (SIZEOFINT - bitsCount));
        buffer <<= (bitsCount + shift);
        bitsInBuffer -= bitsCount;
        buffer >>= (bitsCount + shift);
        return result;
    }

    public int GetTotalBytesProceeded()
    {
        return bytesProceeded;
    }
   
    }
}
