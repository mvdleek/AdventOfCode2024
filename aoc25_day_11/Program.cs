namespace aoc25_day_11
{
    internal class Program
    {
        static string fname = "input.txt";
        static void Main(string[] args)
        {
            var nodegraph = ReadInput(fname);

            SolveP1(nodegraph); //497

            SolveP2(nodegraph); //358.564.784.931.864
        }

        private static void SolveP2(NodeGraph nodegraph)
        {
            var allFromSvrPathsToOut = FindForwardpathsFromTo("svr", "out", nodegraph, "all paths"); // 94.685.369.390.230.499

            nodegraph.RemoveNode("fft");
            var x2 = FindForwardpathsFromTo("svr", "dac", nodegraph, "all paths from svr to dac (no fft)"); //910.634.349.547
            MarkBackwardReachabilityFrom("out", "dac", nodegraph);
            MarkVisitableNodesFrom("dac", nodegraph);
            RemoveUnvisitableNodes(nodegraph);
            var x3 = FindForwardpathsViaReachabilityFromTo("dac", "out", nodegraph, "all paths from dac to out (no fft)"); //3.289.211.270.563.764

            var allPathsFromSvrToOutThroughDacNotThroughFft = x2 * x3;
            Console.WriteLine($"Paths from svr to out through dac, not through fft = {allPathsFromSvrToOutThroughDacNotThroughFft}");

            nodegraph = ReadInput(fname);
            nodegraph.RemoveNode("dac");
            var y2 = FindForwardpathsFromTo("svr", "fft", nodegraph, "all paths from svr to fft (no dac)"); //6262
            MarkBackwardReachabilityFrom("out", "fft", nodegraph);
            MarkVisitableNodesFrom("fft", nodegraph);
            RemoveUnvisitableNodes(nodegraph);
            var y3 = FindForwardpathsViaReachabilityFromTo("fft", "out", nodegraph, "all paths from fft to out (no dac)"); //8.948.669.094.917.292

            var allPathsFromSvrToOutThroughFftNoyThroughDac = y2 * y3;
            Console.WriteLine($"Paths from svr to out through fft, not through dac = {allPathsFromSvrToOutThroughFftNoyThroughDac}");

            nodegraph = ReadInput(fname);
            nodegraph.RemoveNode("fft");
            nodegraph.RemoveNode("dac");

            var allPathsWithoutFftAndDac = FindForwardpathsFromTo("svr", "out", nodegraph, "all paths without fft and dac"); //82.088.924.239.817.579

            var result = allFromSvrPathsToOut 
                - allPathsFromSvrToOutThroughDacNotThroughFft 
                - allPathsFromSvrToOutThroughFftNoyThroughDac 
                - allPathsWithoutFftAndDac;

            Console.WriteLine($"Total nr of paths (P2) = {result}");
        }

        private static void RemoveUnvisitableNodes(NodeGraph nodegraph)
        {
            var nodesToRemove = new List<Node>();
            foreach (var node in nodegraph.nodes.Values)
            {
                if (!node.visitable)
                {
                    nodesToRemove.Add(node);
                }
            }

            foreach (var node in nodesToRemove)
            {
                nodegraph.RemoveNode(node.name);
            }
        }

        private static void MarkVisitableNodesFrom(string from, NodeGraph nodegraph)
        {
            Console.WriteLine($"Starting MarkVisitableNodesFrom from {from}");
            var fromNode = nodegraph.nodes[from];
            fromNode.visitable = true;

            var q = new Queue<Node>();
            q.Enqueue(fromNode);
            while (true)
            {
                Node n = null;
                if (!q.TryDequeue(out n)) { break; }

                foreach (var forwardNode in n.reachabilityOutputs)
                {
                    if (!forwardNode.visitable)
                    {
                        forwardNode.visitable = true;
                        q.Enqueue(forwardNode);
                    }
                }
            }

        }

        private static long FindForwardpathsViaReachabilityFromTo(string from, string to, NodeGraph nodegraph, string extra = "")
        {
            // NB: all unvisitable nodes from fromNode must be removed from nodegraph
            //Console.WriteLine($"Starting FindForwardpathsViaReachabilityFromTo from {from} to {to} {extra}");
            nodegraph.ResetPathCounts();
            var fromNode = nodegraph.nodes[from];
            var toNode = nodegraph.nodes[to];
            fromNode.pathCount = 1;

            var q = new Queue<Node>();
            q.Enqueue(fromNode);
            while (true)
            {
                Node n = null;
                if (!q.TryDequeue(out n)) { break; }
                if (n == toNode)
                {
                    break;
                }

                foreach (var forwardNode in n.reachabilityOutputs)
                {
                    forwardNode.pathCount += n.pathCount;
                    forwardNode.PathsDoneCount++;
                    if (forwardNode.PathsDoneCount == forwardNode.reachabilityInputs.Count)
                    {
                        q.Enqueue(forwardNode);
                    }
                }
            }

            Console.WriteLine($"Path count from {from} to {to}  {extra} = {toNode.pathCount}");
            return toNode.pathCount;
        }

        private static void MarkBackwardReachabilityFrom(string from, string to, NodeGraph nodegraph)
        {
            Console.WriteLine($"Starting MarkBackwardReachabilityFrom from {from} to {to}");
            nodegraph.ResetPathCounts();
            var fromNode = nodegraph.nodes[from];
            var toNode = nodegraph.nodes[to];
            fromNode.pathCount = 1;

            var q = new Queue<Node>();
            q.Enqueue(fromNode);
            while (true)
            {
                Node n = null;
                if (!q.TryDequeue(out n)) { break; }


                foreach (var backNode in n.inputs)
                {
                    n.reachabilityInputs.Add(backNode);
                    backNode.reachabilityOutputs.Add(n);
                    if (backNode.reachabilityOutputs.Count == 1)
                    {
                        q.Enqueue(backNode);
                    }
                }

            }
            Console.WriteLine($"Done MarkBackwardReachabilityFrom from {from} to {to}");
        }

        private static long FindForwardpathsFromTo(string from, string to, NodeGraph nodegraph, string extra = "")
        {
            //Console.WriteLine($"Starting FindForwardpathsFromTo from {from} to {to} {extra}");
            nodegraph.ResetPathCounts();
            var fromNode = nodegraph.nodes[from];
            var toNode = nodegraph.nodes[to];
            fromNode.pathCount = 1;

            var q = new Queue<Node>();
            q.Enqueue(fromNode);
            while (true)
            {
                Node n = null;
                if (!q.TryDequeue(out n)) { break; }
                if (n == toNode)
                {
                    break;
                }

                foreach (var forwardNode in n.outputs)
                {
                    forwardNode.pathCount += n.pathCount;
                    forwardNode.PathsDoneCount++;
                    if (forwardNode.PathsDoneCount == forwardNode.inputs.Count)
                    {
                        q.Enqueue(forwardNode);
                    }
                }

            }

            Console.WriteLine($"Path count from {from} to {to} {extra} = {toNode.pathCount}");
            return toNode.pathCount;
        }

        private static void SolveP1(NodeGraph nodegraph)
        {
            // get nr of paths from end nodes to start nodes
            var node = nodegraph.nodes["you"];

            long pathCount = 0;

            CountPathsFrom(false, node, false, false, 0, ref pathCount);

            Console.WriteLine($"totalPath P1= {pathCount}");
        }

        static long endCount = 0;
        static long totalCount = 0;
        static int maxDepth = 0;

        private static void CountPathsFrom(bool isP2, Node node, bool visitedFft, bool visitedDac, int depth, ref long count)
        {

            if (node.outputs.Count == 0)
            {
                if (depth > maxDepth)
                {
                    maxDepth = depth;
                }
                endCount++;
                Console.WriteLine($"End found: {endCount} total={totalCount} maxdepth= {maxDepth}");
                if (visitedFft && visitedDac && isP2)
                {
                    count++;
                }
                return;
            }
            totalCount++;
            if (node.name == "fft")
            {
                visitedFft = true;
            }
            else if (node.name == "dac")
            {
                visitedDac = true;
            }
            foreach (var outputNode in node.outputs)
            {
                CountPathsFrom(isP2, outputNode, visitedFft, visitedDac, depth++, ref count);
            }
        }

        private static long CountPathsToStart(Node endNode)
        {
            Console.WriteLine(endNode.name);
            if (endNode.outputs.Count == 0)
            {
                Console.WriteLine("");
                return 1; // reached start node
            }
            long pathCount = 0;
            foreach (var inputNode in endNode.inputs)
            {
                pathCount += CountPathsToStart(inputNode);
            }
            return pathCount;
        }

        private static NodeGraph ReadInput(string filename)
        {
            var lines = File.ReadAllLines(filename);

            Dictionary<string, Node> nodes = new Dictionary<string, Node>();
            foreach (var line in lines)
            {
                // parse line into Line object
                var parts = line.Split(' ');
                var nodeName = parts[0].Substring(0, parts[0].Length - 1);
                Node node;
                if (nodes.ContainsKey(nodeName))
                {
                    node = nodes[nodeName];
                }
                else
                {
                    node = new Node(nodeName);
                    nodes.Add(nodeName, node);
                }

                // AI generated code, only 1 mistake in there
                //foreach (var outputName in parts[1..])
                for (var i = 1; i < parts.Length; i++)
                {
                    var outputName = parts[i];
                    if (!nodes.ContainsKey(outputName))
                    {
                        nodes[outputName] = new Node(outputName);
                    }
                    var outputNode = nodes[outputName];
                    node.outputs.Add(outputNode);
                    outputNode.inputs.Add(node);
                }
            }

            var ng = new NodeGraph();
            ng.nodes = nodes;
            return ng;
        }
    }

    internal class Node
    {
        public string name;
        public List<Node> outputs = new List<Node>();
        public List<Node> reachabilityOutputs = new List<Node>();
        public List<Node> reachabilityInputs = new List<Node>();
        public List<Node> inputs = new List<Node>();
        internal long pathCount;
        internal int PathsDoneCount = 0;
        internal bool visitable;

        public Node(string name)
        {
            this.name = name;
        }
    }

    internal class NodeGraph
    {
        public Dictionary<string, Node> nodes = new Dictionary<string, Node>();

        internal void RemoveNode(string v)
        {
            var nodeToRemove = nodes[v];
            foreach (var node in nodeToRemove.outputs)
            {
                node.inputs.Remove(nodeToRemove);
            }
            foreach (var node in nodeToRemove.inputs)
            {
                node.outputs.Remove(nodeToRemove);
            }
            foreach (var node in nodeToRemove.reachabilityOutputs)
            {
                node.reachabilityInputs.Remove(nodeToRemove);
            }
            foreach (var node in nodeToRemove.reachabilityInputs)
            {
                node.reachabilityOutputs.Remove(nodeToRemove);
            }
        }

        internal void ResetPathCounts()
        {
            foreach (var node in nodes.Values)
            {
                node.pathCount = 0;
                node.PathsDoneCount = 0;
            }
        }
    }
}
