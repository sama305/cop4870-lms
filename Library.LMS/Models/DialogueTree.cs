using System;
namespace Library.LMS.Models
{
	public class Node
	{
		public Node? Parent { get; }
		public List<Node> Children { get; }
		public string? Data { get; set; }


		// creating a root node
		public Node()
		{
			Parent = null;
			Data = null;
			Children = new List<Node>();
		}

		// private constructor for children
		private Node(Node p, string d)
        {
            Children = new List<Node>();
			Data = d;
        }

		// methods
		public Node createChild(string d)
		{
			Node child = new Node(this, d);
			Children.Add(child);
			return child;
		}

		public Node createMultiChild(string d)
		{
			Node child = new Node(this, d);
			Children.Add(child);
			return this;
		}
	}

	public class DialogueTree
	{
		public Node RootNode { get; set; }
		public Node CurrentNode { get; set; }

		public DialogueTree()
		{
            RootNode = new Node();
            CurrentNode = RootNode;
		}

        public void displayChildren(Node? p = null, bool numbered=false)
		{
			if (p == null) { p = CurrentNode; }

			List<Node> children = p.Children;
			foreach (Node n in children)
			{
				Console.Write((numbered) ? (children.IndexOf(n)+1) + ".\t" : "");
				Console.WriteLine(n.Data);
			}
		}

        public Node chooseChildFromMenu(Node? p = null, bool inclParent = false)
        {
			if (p.Parent == null) { inclParent = false; }
			var length = p.Children.Count() + ((inclParent) ? 1 : 0);
            if (p == null) { p = CurrentNode; }

            displayChildren(p, true);
			if (inclParent) { Console.WriteLine(length + ".\tBack"); }
			while (true)
			{
				Console.Write(">> ");
				if (int.TryParse(Console.ReadLine(), out int select))
				{
					--select;
					if (select < length && select >= 0)
                    {
						if (select == length) { return p.Parent; }
                        return p.Children[select];
                    }
				}
			}
		}

		public Node goUp()
		{
            CurrentNode = CurrentNode.Parent ?? CurrentNode;
			return CurrentNode;
		}
	}
}

