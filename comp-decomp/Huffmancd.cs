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
      public static class ArrayExtensions {
    public static void Fill<T>(this T[] originalArray, T with) {
        for(int i = 0; i < originalArray.Length; i++){
            originalArray[i] = with;
        }
    }  
}
    class Huffmancd
    {

    public static  int START_CODE = 0;
    public static  int START_CODE_LENGTH = 0;
    public static  int LEFT_BIT = 0;
    private static int BYTES_COUNT = 256;
    private static int SIZE_OF_BYTE = 8;
    private static int SIZE_OF_INT = 32;


    private int[] codes;
    private int[] codeLengths;
    private int[] quantities;
    private int differentBytesCount;
    private BinaryIO binaryIO;
    private TreeNode root;
    private int bitsCount;

    public Huffmancd() {
        codes = new int[BYTES_COUNT];
        codeLengths = new int[BYTES_COUNT];
        quantities = new int[BYTES_COUNT];
        quantities.Fill(0);
        codeLengths.Fill(-1);
       // Arrays.fill(quantities, 0);
        //Arrays.fill(codeLengths, -1);
        differentBytesCount = 0;
        bitsCount = 0;
    }

    public byte[] Compress(byte[] data) {
        byte[] buffer = prepareBuffer(data);
        CalculateQuantities(data);
        BuildTree();
        Tour(root, START_CODE, START_CODE_LENGTH);
        writeQuantities();
        writeCodes(data);
        int totalLength = getTotalLengthOfBuffer();
        writeBitsCount(buffer);
        return Arrays.copyOf(buffer, totalLength);
    }

    private int getTotalLengthOfBuffer() {
        return binaryIO.GetTotalBytesProceeded();
    }

    private void writeBitsCount(byte[] buffer) {
        binaryIO = new BinaryIO(buffer);
        binaryIO.WriteBits(bitsCount, SIZE_OF_INT);
        binaryIO.Flush();
    }

    private void writeQuantities() {
        binaryIO.WriteBits(differentBytesCount - 1, SIZE_OF_BYTE);
        for (int i = 0; i < BYTES_COUNT; ++i) {
            if (codeLengths[i] >= 0) {
                binaryIO.WriteBits(i, SIZE_OF_BYTE);
                binaryIO.WriteBits(quantities[i], SIZE_OF_INT);
            }
        }

    }

    private void writeCodes(byte[] data) {
        for (int i = 0; i < data.Length; ++i) {
            int index = data[i] & 0xFF;
            binaryIO.WriteBits(codes[index], codeLengths[index]);
            bitsCount += codeLengths[index];
        }
        binaryIO.Flush();
    }

    private byte[] prepareBuffer(byte[] data) {
        byte[] buffer = new byte[SIZE_OF_INT + data.Length + (SIZE_OF_BYTE + SIZE_OF_INT) * BYTES_COUNT];
        binaryIO = new BinaryIO(buffer);
        binaryIO.WriteBits(0, SIZE_OF_INT);
        return buffer;
    }


    public byte[] Decompress(byte[] data) {
        byte[] buffer = createBuffer(data);
        binaryIO = new BinaryIO(data);
        readBitsCount();
        ReadQuantities();
        BuildTree();
        int bufferPointer = readDataFromTreeToBuffer(buffer);

        byte[] outs = new byte[bufferPointer];
        // if (outs.Length != bytesCount)
        buffer.CopyTo(outs, 0);
        return outs;

       // return Arrays.copyOf(buffer, bufferPointer);
    }

    private int readDataFromTreeToBuffer(byte[] buffer) {
        TreeNode cur = root;
        int bufferPointer = 0;
        while (bitsCount > 0) {
            bitsCount -= 1;
            int bit = binaryIO.ReadBits(1);
            if (bit == LEFT_BIT) {
                cur = cur.left;
            } else {
                cur = cur.right;
            }
            if (cur.character != null) {
                buffer[bufferPointer] = cur.character;
                bufferPointer += 1;
                cur = root;
            }
        }
        return bufferPointer;
    }

    private void readBitsCount() {
        bitsCount = binaryIO.ReadBits(SIZE_OF_INT);
    }

    private byte[] createBuffer(byte[] data) {
        return new byte[SIZE_OF_INT + 8 * data.Length + (SIZE_OF_BYTE + SIZE_OF_INT) * BYTES_COUNT];
    }

    private void BuildTree() {
        PriorityQueue<TreeNode> queue = new PriorityQueue<TreeNode>();

        createTreeNodesFromQuantities(queue);
        if (queue.size() == 1) {
            addFakeNode(queue);
        }
        while (queue.size() > 1) {
            unionLastTwoNodesInQueue(queue);
        }
        root = queue.poll();
    }

    private void unionLastTwoNodesInQueue(PriorityQueue<TreeNode> queue)
    {
        TreeNode first = queue.poll();
        TreeNode second = queue.poll();
        TreeNode union = new TreeNode(first, second);
        queue.add(union);
    }

    private void createTreeNodesFromQuantities(PriorityQueue<TreeNode> queue)
    {
        for (int i = 0; i < BYTES_COUNT; ++i) {
            if (quantities[i] > 0) {
                TreeNode node = new TreeNode((byte)i, quantities[i]);
                queue.add(node);
            }
        }
    }

    private void addFakeNode(PriorityQueue<TreeNode> queue)
    {
        TreeNode fake = new TreeNode((byte)(queue.peek().character + 1), queue.peek().quantity + 1);
        queue.add(fake);
        differentBytesCount += 1;
    }

    private void ReadQuantities() {
        int differentBytesCount = binaryIO.ReadBits(SIZE_OF_BYTE) + 1;
        for (int i = 0; i < differentBytesCount; ++i) {
            int index = binaryIO.ReadBits(SIZE_OF_BYTE);
            int quantity = binaryIO.ReadBits(SIZE_OF_INT);
            quantities[index] = quantity;
        }
    }

    private void CalculateQuantities(byte[] data) {
        for (int i = 0; i < data.Length; ++i) {
            int index = data[i] & 0xFF;
            if (quantities[index] == 0) {
                differentBytesCount += 1;
            }
            quantities[index] += 1;
        }
    }

    private void Tour(TreeNode cur, int curCode, int curCodeLength) {
        if (cur.character != null) {
            byte b = cur.character.byteValue();
            int index = b & 0xFF;
            codes[index] = curCode;
            codeLengths[index] = curCodeLength;
        }
        if (cur.left != null) {
            Tour(cur.left, curCode << 1, curCodeLength + 1);
        }
        if (cur.right != null) {
            Tour(cur.right, (curCode << 1) | 1, curCodeLength + 1);
        }
    }
    }
}
