using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze1
{
    /// <summary>
    /// A class representing a node inside a grid graph
    /// </summary>
    class GridNode
    {

        #region Class fields and properties

        private List<GridNode> edges = new List<GridNode>();
        private int i, j;

        public List<GridNode> Edges
        {
            get { return edges; }
            set { edges = value; }
        }

        #endregion


        #region Class constructors
        ///<summary> A public constructor for a grid node at index (i,j) </summary>
        public GridNode(int i_pos, int j_pos) {
            i = i_pos;
            j = j_pos;
        }
        #endregion


        #region Methods: isConnectedTo and removeEdge

        /// <summary> Check if this node is connected to the specified node </summary>
        public bool isConnectedTo(GridNode node)
        {
            return edges.Contains(node);
        }

        ///<summary> Remove an edge between this node and the specified node </summary>
        public void removeEdge(GridNode node)
        {
            edges.Remove(node);
        }

        #endregion


        #region Operator and method overrides

        //just in case
        public static bool operator ==(GridNode g1, GridNode g2)
        {
            return (g1.i == g2.i) && (g1.j == g2.j);
        }

        public static bool operator !=(GridNode g1, GridNode g2)
        {
            return (g1.i != g2.i) && (g1.j != g2.j);
        }

        /// <summary> Equals method. I wrote this so IList.Remove would work. </summary>
        public override bool Equals(object obj)
        {
            // If parameter cannot be cast to GridNode return false.
            GridNode g2 = obj as GridNode;
            if ((System.Object)g2 == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (i == g2.i) && (j == g2.j);
        }

        // For debugging
        public override string ToString()
        {
            return "(" + i + ", " + j + ")";
        }

        #endregion
    }
}
