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
    class TreeNode: IComparable<TreeNode>
    {
    public TreeNode left;
    public TreeNode right;
    public int quantity;
    public Byte character;
    public int size;

    public TreeNode(Byte character, int quantity) {
        //this();
        size = 3;
        this.character = character;
        this.quantity = quantity;
    }

    public TreeNode() {
        left = right = null;
        character = 0;//null
        size = 1;
        quantity = 0;
    }

    public TreeNode(TreeNode left, TreeNode right) {
       // this();
        this.left = left;
        this.right = right;
        this.quantity = left.quantity + right.quantity;
        this.size = 1 + left.size + right.size;
    }

    public int compareTo(TreeNode node) {
        return this.quantity - node.quantity;
    }
}
    }
