using PhysicsAStar.Primitives;

namespace PhysicsAStar.Parts
{
    public class Grid
    {
        private readonly Node[,] Nodes;

        public readonly int Columns;
        public readonly int Rows;

        public readonly GridVec topLeft;
        
        public static Grid CreateGridWithLateralConnections(GridVec gridSize, GridVec topLeft)
        {
            var grid = new Grid(gridSize, topLeft);

            grid.CreateLateralConnections();

            return grid;
        }

        public static Grid CreateGridWithDiagonalConnections(GridVec gridSize, GridVec topLeft)
        {
            var grid = new Grid(gridSize, topLeft);

            grid.CreateDiagonalConnections();

            return grid;
        }

        public static Grid CreateGridWithLateralAndDiagonalConnections(GridVec gridSize, GridVec topLeft, bool cutCorners = false)
        {
            var grid = new Grid(gridSize, topLeft);

            grid.CreateDiagonalConnections();
            grid.CreateLateralConnections();

            return grid;
        }
        
        private Grid(GridVec gridSize, GridVec topLeft)
        {
            this.Nodes = new Node[gridSize.x, gridSize.y];
            this.Columns = gridSize.x;
            this.Rows = gridSize.y;
            this.topLeft = topLeft;
            this.CreateNodes(topLeft);
        }

        private void CreateNodes(GridVec topLeft)
        {
            for (var x = 0; x < this.Columns; x++)
            {
                for (var y = 0; y < this.Rows; y++)
                {
                    this.Nodes[x, y] = new Node(new GridVec(x + topLeft.x, y + topLeft.y));
                }
            }
        }
        
        private void CreateLateralConnections()
        {
            for (var x = 0; x < this.Columns; x++)
            {
                for (var y = 0; y < this.Rows; y++)
                {
                    var node = this.Nodes[x, y];

                    if (x < this.Columns - 1)
                    {
                        var rightNode = this.Nodes[x + 1, y];
                        node.Right = rightNode;
                        rightNode.Left = node;
                    }

                    if (y < this.Rows - 1)
                    {
                        var downNode = this.Nodes[x, y + 1];
                        node.Down = downNode;
                        downNode.Up = node;
                    }
                }
            }
        }

        private void CreateDiagonalConnections()
        {
            for (var x = 0; x < this.Columns; x++)
            {
                for (var y = 0; y < this.Rows; y++)
                {
                    var node = this.Nodes[x, y];

                    if (x < this.Columns - 1 && y < this.Rows - 1)
                    {
                        var downRightNode = this.Nodes[x + 1, y + 1];
                        node.DownRight = downRightNode;
                        downRightNode.UpLeft = node;
                    }

                    if (x > 0 && y < this.Rows - 1)
                    {
                        var downLeftNode = this.Nodes[x - 1, y + 1];
                        node.DownLeft = downLeftNode;
                        downLeftNode.UpRight = node;
                    }
                }
            }
        }
        
        public void DisconnectNode(GridVec position)
        {

            position -= topLeft;
            
            var node = this.Nodes[position.x, position.y];

            node.DisconnectAll();
        }
        
        public void RemoveCornerCutting(GridVec position)
        {

            position -= topLeft;
            
            var left = new GridVec(position.x - 1, position.y);
            var up = new GridVec(position.x, position.y - 1);
            var right = new GridVec(position.x + 1, position.y);
            var down = new GridVec(position.x, position.y + 1);

            if (this.IsInsideGrid(left) && this.IsInsideGrid(up))
            {
                Node leftNode = Nodes[left.x, left.y];
                Node upNode = leftNode.Up;
                leftNode.UpRight = null;
                upNode.DownLeft = null;
            }
            
            if (this.IsInsideGrid(down) && this.IsInsideGrid(left))
            {
                Node leftNode = Nodes[left.x, left.y];
                Node downNode = leftNode.Down; 
                leftNode.DownRight = null;
                downNode.UpLeft = null;
            }

            if (this.IsInsideGrid(up) && this.IsInsideGrid(right))
            {
                Node rightNode = Nodes[right.x, right.y];
                Node upNode = rightNode.Up; 
                rightNode.UpLeft = null;
                upNode.DownRight = null;
            }

            if (this.IsInsideGrid(right) && this.IsInsideGrid(down))
            {
                Node rightNode = Nodes[right.x, right.y];
                Node downNode = rightNode.Down; 
                rightNode.DownLeft = null;
                downNode.UpRight = null;
            }
        }
        
        private bool IsInsideGrid(GridVec position) => position.x >= 0 && position.x < this.Columns && position.y >= 0 && position.y < this.Rows;

        public Node GetNode(GridVec pos)
        {
            pos -= topLeft;
            if (IsInsideGrid(pos))
                return Nodes[pos.x, pos.y];
            return null;
        }
    }
}