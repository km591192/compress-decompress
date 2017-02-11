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
using System.Collections.ObjectModel;

namespace comp_decomp
{
    static class Extensions
    {
        public static IList<T> Clone<T>(this IList<T> list) where T : ICloneable
        {
            return list.Select(i => (T)i.Clone()).ToList();
        }
    }

    class LZWcomp_decomp
    {
    /*    public byte[] Compresses(byte[] data);
        public byte[] Decompresses(byte[] data);*/

        private short CLEAR_CODE = 257;
        private short EMPTY_CODE = 258;
        private short END_CODE = 259;
        private short POOL_START = 260;
        private int MAX_CODE_LENGTH = 12;
        private int MAGIC_NUMBER = 8191;
        private int TABLE_CAPACITY = 2 << 13 + 1;
        private int START_BITS_TO_WRITE = 9;
        private int[] THRESHOLDS = { 511, 1023, 2047, 4095, 8191 };
        private int CLEARTABLESIZE = 4095;

        public byte[] buffer;
        private int nextCode;
        private int bitsInCode;
        private int prevCode;
        private int valueCode;


        public byte[] Compresses(byte[] data)
        {

            BinaryIO writer = initBufferAndBinaryWriter(data);
            ListOfInts[] table = new ListOfInts[TABLE_CAPACITY];

            resetTable(table);
            writeClearCode(writer);

            codeDataBytes(data, writer, table);

            finishWritingCodes(writer);
            byte[] result = getUsedSubarrayOfBuffer(writer);
            return result;
        }

        private void codeDataBytes(byte[] data, BinaryIO writer, ListOfInts[] table)
        {
            for (int i = 0; i < data.Length; ++i)
            {
                int currentByte = getCurrentByte(data[i]);
                int key = MakeKey(prevCode, currentByte);
                int hash = GetHash(key);

                bool contains = getValueCodeIfContainsCurrentChain(table[hash], currentByte);

                if (contains)
                {
                    prevCode = valueCode;
                }
                else
                {
                    createNewEntryAndWriteItsCode(writer, table, currentByte, hash);
                }
            }
        }

        private void createNewEntryAndWriteItsCode(BinaryIO writer, ListOfInts[] table, int currentByte, int hash)
        {
            writer.WriteBits(prevCode, bitsInCode);
            addNewElementToHashTable(table, currentByte, hash);

            LinkedList<int> collisionsList = getCollisionListByPrevCodeAndCurByte(EMPTY_CODE, currentByte, table);
            prevCode = EMPTY_CODE;
            foreach (int value in collisionsList)
            {
                if (areEqualValues(prevCode, currentByte, value))
                {
                    prevCode = GetCode(value);
                    break;
                }
            }

            ++nextCode;
            checkCodeRestrictions(writer, table);
        }

        private LinkedList<int> getCollisionListByPrevCodeAndCurByte(short previousCode, int currentByte, ListOfInts[] table)
        {
            int prevKey = MakeKey(previousCode, currentByte);
            return getCollisionListByKey(table, prevKey);
        }

        private LinkedList<int> getCollisionListByKey(ListOfInts[] table, int prevKey)
        {
            int prevHash = GetHash(prevKey);
            return getCollisionsList(table, prevHash);
        }

        private void checkCodeRestrictions(BinaryIO writer, ListOfInts[] table)
        {
            if (codeIsAThreshold(nextCode))
            {
                ++bitsInCode;
            }
            if (reachedCodeSizeLimit())
            {
                writer.WriteBits(prevCode, bitsInCode);
                writeClearCode(writer);
                resetTable(table);
            }
        }

        private BinaryIO initBufferAndBinaryWriter(byte[] data)
        {
            buffer = new byte[2 * (data.Length + 2)];
            return new BinaryIO(buffer);
        }

        private void resetTable(ListOfInts[] table)
        {
            initHashTable(table);
            nextCode = POOL_START;
            bitsInCode = START_BITS_TO_WRITE;
            prevCode = EMPTY_CODE;
        }

        void initHashTable(ListOfInts[] table)
        {
            initListsInTable(table);
            addInTableSingleByteChains(table);
        }

        private void addInTableSingleByteChains(ListOfInts[] table)
        {
            for (int codedByte = 0; codedByte < 256; ++codedByte)
            {
                addSingleByteChainToTable(table, codedByte);
            }
        }

        private void addSingleByteChainToTable(ListOfInts[] table, int i)
        {
            LinkedList<int> collisionsList = getCollisionListByPrevCodeAndCurByte(EMPTY_CODE, i, table);
            int newElement = GetNewElement(i, EMPTY_CODE, i);
            addNewElementToCollisionsList(newElement, collisionsList);
        }

        int GetHash(int key)
        {
            int hash = (((key) >> MAX_CODE_LENGTH) ^ key) & (MAGIC_NUMBER);
            return hash;
        }

        int MakeKey(int prevKey, int lastByte)
        {
            int result = (prevKey << 8) | (lastByte & 0xFF);
            return result;
        }

        int GetNewElement(int curCode, int prevCode, int cur)
        {
            return (((curCode << 12) | prevCode) << 8) | (cur & 0xFF);
        }

        private LinkedList<int> getCollisionsList(ListOfInts[] table, int hash)
        {
            return table[hash].list;
        }

        private void addNewElementToCollisionsList(int newElement, LinkedList<int> collisionsList)
        {
            collisionsList.AddLast(newElement);
        }

        private void initListsInTable(ListOfInts[] table)
        {
            for (int i = 0; i < TABLE_CAPACITY; ++i)
            {
                table[i] = new ListOfInts();
            }
        }

        private void writeClearCode(BinaryIO writer)
        {
            writer.WriteBits(CLEAR_CODE, bitsInCode);
        }

        private bool reachedCodeSizeLimit()
        {
            return nextCode == CLEARTABLESIZE;
        }

        private int getCurrentByte(byte b)
        {
            return b & 0xFF;
        }

        //sets member variable valueCode
        private bool getValueCodeIfContainsCurrentChain(ListOfInts listOfInts, int currentByte)
        {
            valueCode = 0;

            bool contains = false;


            for (int j = 0; j < listOfInts.list.Count && !contains; ++j)
            {
                int value = listOfInts.list.ElementAt(j);
                valueCode = value >> 20;
                contains = areEqualValues(prevCode, currentByte, value);
            }
            return contains;
        }
        private bool areEqualValues(int previousCode, int currentByte, int value)
        {
            int valuePrevCode = GetPrevCode(value);
            int valueLastSymbol = GetLastSymbol(value);
            bool result = previousCode == valuePrevCode && currentByte == valueLastSymbol;
            return result;
        }

        int GetLastSymbol(int value)
        {

            return (value & 0xFF);
        }

        int GetPrevCode(int value)
        {
            return (value << 12) >> 20;
        }

        int GetCode(int value)
        {
            return value >> 20;
        }

        private void addNewElementToHashTable(ListOfInts[] table, int currentByte, int hash)
        {
            int newElement = GetNewElement(nextCode, prevCode, currentByte);
            LinkedList<int> collisionsList = getCollisionsList(table, hash);
            addNewElementToCollisionsList(newElement, collisionsList);
        }

        private void finishWritingCodes(BinaryIO writer)
        {
            writer.WriteBits(prevCode, bitsInCode);
            writer.WriteBits(END_CODE, bitsInCode);
            writer.Flush();
        }

        private byte[] getUsedSubarrayOfBuffer(BinaryIO writer)
        {
            int count = writer.GetTotalBytesProceeded();
            byte[] outs = new byte[count];
            if (outs.Length != count)
            buffer.CopyTo(outs, 0);
            return outs;
          //  return Array.CopyOf(buffer,count);
        }


        public byte[] Decompresses(byte[] data)
        {

            buffer = new byte[(2 + data.Length) * 2];
            int bytesCount = 0;
            bitsInCode = START_BITS_TO_WRITE;
            nextCode = POOL_START;
            BinaryIO reader = new BinaryIO(data);
            ListOfBytes[] table = new ListOfBytes[TABLE_CAPACITY];
            InitCodeArray(table);

            bytesCount = rebuildDataWithTable(bytesCount, reader, table);
            
            byte[] outs = new byte[bytesCount];
           // if (outs.Length != bytesCount)
            buffer.CopyTo(outs, 0);
            return outs;

            //return Array.CopyOf(buffer, bytesCount);
        }

        private int rebuildDataWithTable(int bytesCount, BinaryIO reader, ListOfBytes[] table)
        {
            valueCode = reader.ReadBits(bitsInCode);
            prevCode = EMPTY_CODE;
            while (isNotEndCode())
            {
                if (isClearCode())
                {
                    resetCodeArrayAndParams(table);
                    readNextCode(reader);
                    if (isEndCode())
                    {
                        break;
                    }
                    bytesCount = addDataByCode(bytesCount, table);
                }
                else
                {
                    bytesCount = handleNonClearCode(bytesCount, table);
                }
                valueCode = reader.ReadBits(bitsInCode);
            }
            return bytesCount;
        }

        private int handleNonClearCode(int bytesCount, ListOfBytes[] table)
        {
            if (tableIncludesValueCode(table))
            {

                bytesCount = createNewEntryInTable(bytesCount, table);
                prevCode = valueCode;
            }
            else
            {
                valueCode = nextCode;
                bytesCount = createNewEntryInTable(bytesCount, table);
                prevCode = valueCode;
            }
            return bytesCount;
        }


   
        private int createNewEntryInTable(int bytesCount, ListOfBytes[] table)
        {
            byte[] prev = table[prevCode].list.ToArray();
            LinkedList<Byte> tab = new LinkedList<Byte>(prev);
          //  table[prevCode].list.CopyTo(prev, prevCode);

            table[nextCode] = new ListOfBytes();
           
            table[nextCode].list = (LinkedList<Byte>)table[prevCode].list;

           // table[nextCode].list = (LinkedList<Byte>)table[prevCode].list.Clone();
            //table[nextCode].list = (LinkedList<Byte>)table[prevCode].list.Select(item => item.Clone()).ToList();
            table[nextCode].list.AddLast(table[valueCode].list.ElementAt(0));
            bytesCount += AddData(bytesCount, valueCode, table);
            nextCode += 1;
            checkNextCodeSize();
            return bytesCount;
        }

        private void checkNextCodeSize()
        {
            if (codeIsAThreshold(nextCode + 1))
            {
                ++bitsInCode;
            }
        }

        private bool codeIsAThreshold(int code)
        {
            return Array.BinarySearch(THRESHOLDS, code) >= 0;
        }

        private bool tableIncludesValueCode(ListOfBytes[] table)
        {
            return table[valueCode] != null;
        }

        private void resetCodeArrayAndParams(ListOfBytes[] table)
        {
            InitCodeArray(table);
            nextCode = POOL_START;
            bitsInCode = START_BITS_TO_WRITE;
        }

        private void readNextCode(BinaryIO reader)
        {
            valueCode = reader.ReadBits(bitsInCode);
            valueCode = ((byte)valueCode) & 0xFF;
        }

        private int addDataByCode(int bytesCount, ListOfBytes[] table)
        {
            bytesCount += AddData(bytesCount, valueCode, table);
            prevCode = valueCode;
            return bytesCount;
        }

        public int AddData(int start, int code, ListOfBytes[] table)
        {

            ListOfBytes elem = table[code];
            int size = elem.list.Count;
            if (start + size >= buffer.Length)
            {
                int c = (start + size) / buffer.Length + 1;
                byte[] newBuf = new byte[buffer.Length * c];
                System.Array.Copy(buffer, 0, newBuf, 0, buffer.Length);
                buffer = newBuf;
            }
            for (int i = 0; i < size; ++i)
            {
                buffer[start + i] = elem.list.ElementAt(i);
            }
            return size;
        }

        private bool isEndCode()
        {
            return valueCode == END_CODE;
        }

        private bool isNotEndCode()
        {
            return valueCode != END_CODE;
        }

        private bool isClearCode()
        {
            return valueCode == CLEAR_CODE;
        }

        void InitCodeArray(ListOfBytes[] table)
        {
            for (int i = 0; i < table.Length; ++i)
            {
                table[i] = null;
            }
            table[CLEAR_CODE] = new ListOfBytes();
            for (int i = 0; i < 256; ++i)
            {
                table[i] = new ListOfBytes();
                table[i].list.AddLast((byte)(i & 0xFF));
            }
        }

        public class ListOfInts
        {
            public LinkedList<int> list;

            public ListOfInts()
            {
                list = new LinkedList<int>();
            }
        }

        public class ListOfBytes
        {
            public LinkedList<Byte> list;

            public ListOfBytes()
            {
                list = new LinkedList<Byte>();
            }
        }
    }
}
