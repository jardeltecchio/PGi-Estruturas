using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    public class Cycle
    {
        int _generated_from;
        List<int> _p_cycle;
        List<int> _p_edge_list;

        public Cycle(List<int> path_vx, List<int> path_vy)
        {
            _p_cycle = null;
            _p_edge_list = null;
            _generated_from = 0;

            int i;
            int item_vx = path_vx[0];
            int item_vy = path_vy[0];


            // checks if both path start at 'v'
            if (item_vx == item_vy)
            {
                _generated_from = item_vx;

                // creates a new array
                _p_cycle = new List<int>();

                for (i = 0; i < path_vx.Count; i++)
                {
                    item_vx = path_vx[i];
                    _p_cycle.Add(item_vx);
                }


                for (i = 1; i <= path_vy.Count; i++)
                {
                    item_vy = path_vy[path_vy.Count - i];
                    _p_cycle.Add(item_vy);
                }

                _p_edge_list = new List<int>();
            }
        }

        public int GetVertexCount()
        {
            return _p_cycle != null ? _p_cycle.Count : 0;
        }

        public int GetEdgeCount()
        {
            return _p_edge_list != null ? _p_edge_list.Count() : 0;
        }

        public int GetLength()
        {
            return GetVertexCount();
        }

        public static int CompareOrder(Cycle p_c1, Cycle p_c2)
        {
            if (p_c1.GetLength() == p_c2.GetLength())
                return p_c1._generated_from - p_c2._generated_from;

            return p_c1.GetLength() - p_c2.GetLength();
        }

        public void AddEdge(int edge_id) 
        { 
            if (_p_edge_list!=null) 
                _p_edge_list.Add(edge_id); 
        }

        public int GetVertex(int number) 
        {
            if (_p_cycle!=null)
                if (_p_cycle.Count() > number)
                    return _p_cycle[number];
          
            return 0;
		}
        public int GetEdge(int number) 
        { 
             if (_p_edge_list!=null)
                if (_p_edge_list.Count() > number)
                    return _p_edge_list[number];
          
            return 0;
		}
    }

    public class CycleSet
    {
        List<Cycle> _p_cycles_array;
        public CycleSet()
        {
            _p_cycles_array = null;
        }

        public void AddCycle(ref Cycle c)
        {
            if (_p_cycles_array != null)
                //_p_cycles_array = new CyclesArray(Cycle::CompareOrder);
                _p_cycles_array = new List<Cycle>();

            if (c != null)
                _p_cycles_array.Add(c);
        }

        /*@return the number of cycles existing in this cycle set*/
        public int GetCount()
        {
            return _p_cycles_array != null ? _p_cycles_array.Count() : 0;
        }

        /*@return the cycle at the position indicated, NULL if does not exists*/
        public Cycle GetCycle(int number)
        {
            if (_p_cycles_array != null)
                if (_p_cycles_array.Count > number)
                    return _p_cycles_array[number];

            return null;
        }

        /* @desc Select independent cycles from cycle set*/
        public void SelectCycles()
        {
            int c;
            bool independent_cycle;

            // creation of incidence matrix 
            IncidenceMatrix incidence_matrix = new IncidenceMatrix();

            for (c=0; c < GetCount();c++) 
               incidence_matrix.AddCycleToEdgePool(_p_cycles_array[c]);

            incidence_matrix.CreateMatrix();

            for (c=0; c<GetCount();)  
            {		
              independent_cycle = incidence_matrix.IndependentCycle(_p_cycles_array[c]);
		
              if (!independent_cycle) 
              {
                 Cycle cycle = _p_cycles_array[c];
                 _p_cycles_array.RemoveAt(c);		
                cycle = null;
              }
              else 
                c++;
            }
            incidence_matrix = null;    
        }

        public void Sort()
        {
	       if (_p_cycles_array!=null)
		     _p_cycles_array.Sort(Cycle.CompareOrder);
        } 
    }

    public class MatrixModuloTwo
    {
        public MatrixModuloTwo(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;

            // create the matrix

            // first allocates memory
            int dim = _rows * _cols;
            _matrix = new int[dim];
        }

        public int Get(int _base, int offset)
        {
            return _matrix[_base + offset];
        }
        public void Set(int _base, int offset, int value)
        {
            _matrix[_base + offset] = value;
        }

        public int GetAt(int row, int col)
        {
            return _matrix[GetAddress(row, col)];
        }

        public void SetAt(int row, int col, int value)
        {
            _matrix[GetAddress(row, col)] = value;
        }
        private void SwapMatrixRows(int row_a, int row_b)
        {
            int t; // variable to store temporarily the value

            int row_a_offset = row_a * _cols;
            int row_b_offset = row_b * _cols;

            for (int k = 0; k < _cols; k++)
            {
                t = _matrix[row_a_offset + k];
                _matrix[row_a_offset + k] = _matrix[row_b_offset + k];
                _matrix[row_b_offset + k] = t;
            }
        }
        public void GaussianElimination(int rows)
        {
            int c, r, k, max; //, address;

            int pivot_row = 0;


            for (c = 0; c < _cols; c++)
            {
                max = pivot_row;

                // in this case no substitution is needed
                if (_matrix[pivot_row * _cols + c] != 0x01)
                {

                    // otherwise, lets found if its needed any substitution
                    for (r = pivot_row + 1; r < rows; r++)
                    {
                        if (_matrix[r * _cols + c] == 1)
                        {
                            max = r;
                            break;
                        }
                    }

                    // if pivot row is zero and other column is one
                    // then they must change			
                    if (max != pivot_row)
                        SwapMatrixRows(max, pivot_row);
                }

                // now lets make the elimination
                if (_matrix[pivot_row * _cols + c] == 0x01)
                {
                    for (r = pivot_row + 1; r < rows; r++)
                        if (_matrix[r * _cols + c] == 0x01)
                            for (k = c; k < _cols; k++)
                                _matrix[r * _cols + k] ^= _matrix[pivot_row * _cols + k];

                    pivot_row++;
                }
            }
        }

        private int GetAddress(int row, int col)
        {
            return row * _cols + col;
        }

        private int _rows;
        private int _cols;

        int[] _matrix;
    }
    public class VertexGraph
    {
        int _id;
        public VertexGraph(int id)
        {
            this._id = id;
        }
    }
    public class Edge
    {
        public int _id;
        public int _vertex_a;
		public int _vertex_b;
        public Edge(int id)
        {
            this._id = id;
        }

        public bool Equals(int vertex_a, int vertex_b) 
        {
			return (vertex_a ==_vertex_a && vertex_b==_vertex_b) || 
				   (vertex_a==_vertex_b && vertex_b==_vertex_a); 
		}
		public int GetId() { return _id; }
		public void SetVertices(int vertex_a, int vertex_b) 
        {
			_vertex_a = vertex_a;
			_vertex_b = vertex_b;
		}
    }
    public class IncidenceMatrix
    {
        int _edge_count;
        int _independent_cycle_count;
      	List<Edge> _edge_pool;

        MatrixModuloTwo _p_incidence_matrix;

        public IncidenceMatrix()
        {
            _p_incidence_matrix = null;
            _edge_count = 0;
            _independent_cycle_count = 0;
            _edge_pool = new List<Edge>();
        }

        public bool IndependentCycle(Cycle c)
        {
            bool independent_cycle = true;
            int offset = _independent_cycle_count * _edge_count;
            int edge;

            // add cycle to matrix
            for (int i = 0; i < c.GetEdgeCount(); i++)
            {
                edge = c.GetEdge(i);

                _p_incidence_matrix.Set(offset, edge, 0x1);
            }
            _independent_cycle_count++;

            if (_independent_cycle_count > 1)
            {
                // check independency

                // first perform gaussian elimination
                _p_incidence_matrix.GaussianElimination(_independent_cycle_count);

                // then see if added row is all zeros
                independent_cycle = false;
                for (int i = 0; i < _edge_count && !independent_cycle; i++)
                    independent_cycle = (_p_incidence_matrix.Get(offset, i) != 0x00);
            }

            // if this is an independent cycle, increment the row counter
            if (!independent_cycle)
                _independent_cycle_count--;

            return independent_cycle;
        }

        /***
        * @desc add a cycle in order to construct the edge pool, 
        * @note this must be done before creating the matrix
        * @note IMPORTANT: creates an edge list in cycle
        */
        public void AddCycleToEdgePool(Cycle cycle)
        {
            int current_vertex, first_vertex, previous_vertex = 0;
            bool first = true;
            int edge_number;

            for (int i = 0; i < cycle.GetVertexCount(); i++)
            {
                current_vertex = cycle.GetVertex(i);

                // in case this is not the frst vertex, finds the edge number
                // and adds it to the edge list in cycle
                if (!first)
                {
                    edge_number = GetEdgeNumber(previous_vertex, current_vertex);
                    cycle.AddEdge(edge_number);
                }
                else
                {
                    first = false;
                    first_vertex = current_vertex;
                }

                previous_vertex = current_vertex;
            }

            // the vertex list in cycle already contains the first vertex at the end of the list,
            // so we do not need the ollowing lines
            //	if (i>0) {
            //		edge_number = GetEdgeNumber(current_vertex, first_vertex);
            //		cycle->AddEdge(edge_number);
            //	}
            //
        }

        /***
* @return the edge number of given pair of vertices
*/
        public int GetEdgeNumber(int vertex_a, int vertex_b)
        {
            Edge e;

            for (int i = 0; i < _edge_pool.Count(); i++)
            {
                e = _edge_pool[i];
                if (e.Equals(vertex_a, vertex_b))
                    return e.GetId();
            }

            // if arrives here, there are no such edge in edge pool
            // so we must create a new edge
            e = new Edge(_edge_count++);
            e.SetVertices(vertex_a, vertex_b);

            // and add it to the edge pool
            _edge_pool.Add(e);

            return e.GetId();
        }

        public void CreateMatrix()
        {
	      _p_incidence_matrix = new MatrixModuloTwo(_edge_pool.Count(), _edge_count) ;
        }
    }

    public class PolygonGraph
    {
        List<TPonto> vertexes;
        public PolygonGraph()
        {
            vertexes = new List<TPonto>();
        }

        public void AddVertex(TPonto p)
        {
            vertexes.Add(p);
        }
    }

    public class PolygonSet
    {
        public List<PolygonGraph> _polygons_array;
        public List<TPonto> _all_points_array;
        public PolygonSet()
        {
            _all_points_array = new List<TPonto>();
            _polygons_array = new List<PolygonGraph>();

        }
        public void Clear()
        {
            _polygons_array.Clear();
        }
public void CreatePointsArray(List<TLinha> line_set)
{	
	if (line_set!=null) 
    {
		_all_points_array.Clear();
  
		for (int i=0; i< line_set.Count(); i++)
        {
			TLinha line = line_set[i];
			TPonto p = line.pIni;
            p.SetIndex(i);
			_all_points_array.Add(p);
		
			p = line.pFin;
	        p.SetIndex(i);
		    _all_points_array.Add(p);			
		}

      //  TPonto pf = line_set[line_set.Count - 1].pFin;
     //   pf.SetIndex(line_set.Count);
    //    _all_points_array.Add(pf);
		
        // the points are sorted in order to allow fast
		// identification of coincident points
		_all_points_array.Sort(TPonto.CompareOrder);

		// at the end we update the index on all points
		for(int i=0; i<_all_points_array.Count();i++)
  		  _all_points_array[i].SetIndex(i);
	}
}
public  Graph LinesToGraph(List<TLinha> line_set)
{

	// first, create the points array from the current line set
	CreatePointsArray(line_set);

	// then create the graph
	Graph G = new Graph(GetPointCount());

	// because of using GetPointCount we are sure that all points 
	// already have an ID, so we can add lines to the graph

	TLinha line;

	for (int i=0; i<line_set.Count(); i++ ) {
		line = line_set[i];
		if (line!=null)
			G.SetAdjacency(line.pIni.GetID(), line.pFin.GetID());
	}
	
	return G;
}

public int GetPointCount()
{
	int id = 0;
    TPonto current, previous = null;

    for (int i = 0; i < _all_points_array.Count(); i++)
    {
		current = _all_points_array[i];

        if (((Object)current != (Object)previous) || (Object)previous != null) 
        {		
			id++;
			previous = current;
		}	
		current.SetID(id-1);		
	}

	return id;
}

/***
* @desc creates polygons from cycles
* @para cycle set
*/
public void CyclesToPolygons(CycleSet cycle_set)
{

	if (cycle_set!=null) {
		// then create polygons
		for (int i=0; i<cycle_set.GetCount();i++)
        {
			Cycle cycle = cycle_set.GetCycle(i);

			if (cycle.GetVertexCount()>2) 
            {
				PolygonGraph plg = new PolygonGraph();

                for (int j = 0; j < cycle.GetVertexCount(); j++)
                {
					TPonto p = PointByID(cycle.GetVertex(j));
					if (p!=null)
					  plg.AddVertex(new TPonto(p));
				}

				// adds new polygon to polygons array
				_polygons_array.Add(plg);							

				// update polygon first and last point, plus closed status

//plg->CalculateFirstAndLastPoint();


				// simplify the polygon, removing unnecessary vertices
				// IMPORTANT NOTICE: This should not be called here,
				// because we need all vertices for remove contained polygons 
				// with single adjacencies. This simplification is done
				// later, within the polygon set simplification
				// plg->Simplify();

			}			
		}
	}
}

TPonto PointByID(int id)
{
	TPonto p;

	// note: the position is at least equal to id
	for (int i=id; i<_all_points_array.Count();i++) {
		p = _all_points_array[i];
		if (p.GetID()==id)
			return p;
	}

	return null;
}


    }

    public class Graph
    {
        public Graph(int vertices) 
        {
            _p_adjacency_matrix = null;
            _predecessor_matrix = null;
            _d_matrix           = null;
	        _vertex_count       = vertices;	
	        _p_adjacency_matrix = new MatrixModuloTwo(vertices, vertices);	

        }

/***
* @desc indicates that two vertices (v1 and v2) are adjacent
* @param v1, v2 vertex numbers
*/
        public void SetAdjacency(int v1, int v2)
        {
	       _p_adjacency_matrix.SetAt(v1,v2, 1);
	       _p_adjacency_matrix.SetAt(v2,v1,1);
        }

        /***
* @desc tells if two vertices (v1 and v2) are adjacent
* @param v1, v2 vertex numbers
* @return boolean true if they are adjacent, false otherwise
*/
        public bool isAdjacent(int v1, int v2)
        {
	         return (_p_adjacency_matrix.GetAt(v1,v2)==1) || (_p_adjacency_matrix.GetAt(v2,v1)==1);
        } 

/***
* @desc count the edges of the graph
*/
public int GetEdgeCount()
{
	int i, j, result = 0;
	
	
	for(i=0; i<GetVertexCount(); i++) {
		for(j=i+1;j<GetVertexCount(); j++)		
			if (isAdjacent(i,j))
				result++;
	}
	
	return result;
}

        public int GetVertexCount() { return _vertex_count; } 

        public int MatrixOffset(int v1, int v2)
        {
            return (v1 * _vertex_count + v2);
        }
        const int MAX_VERTICES = 65535;
public void InitializeFloydWarshall()
{
	
	_predecessor_matrix = new int[_vertex_count*_vertex_count];
	_d_matrix = new int[_vertex_count*_vertex_count];

	int i, j, offset;
	
	for (i=0; i<_vertex_count; i++) 
		for (j=0;j<_vertex_count; j++){			
			offset = i*_vertex_count+j;
			_d_matrix[offset] = _p_adjacency_matrix.Get(offset,0)==1?1:MAX_VERTICES;
			_predecessor_matrix[offset] =  (_p_adjacency_matrix.Get(offset,0)==1 && (i!=j))?i:MAX_VERTICES;
		}
	
}

public void FloydWarshall()
{		
	int k, i, j, offset;
	int n = GetVertexCount();

	InitializeFloydWarshall();
	
	// 'd' matrices
	int [] previous_d_matrix = _d_matrix;
	int [] current_d_matrix = new int[_vertex_count*_vertex_count];

	// 'pi' matrices
	int [] previous_pi_matrix = _predecessor_matrix;
	int [] current_pi_matrix = new int[_vertex_count*_vertex_count];
	
	// initialize current matrix
	for (i=0;i<n;i++)
		for (j=0;j<n;j++) {
			current_d_matrix[i*n+j]=MAX_VERTICES;
			current_pi_matrix[i*n+j]=MAX_VERTICES;
		}
	
	// apply floyd warshall algorithm
	for (k=0; k<n; k++)
    {	
		for (i=0; i<n; i++) 
        {
			for (j=0; j<n; j++)
            {
				if (i!=j) 
                {				
					offset = MatrixOffset(i,j);			
					
					int previous_d_ij = previous_d_matrix[offset];
                    int previous_d_ik = previous_d_matrix[MatrixOffset(i, k)];
                    int previous_d_kj = previous_d_matrix[MatrixOffset(k, j)]; 

					// start of 'd' calculation
					if (previous_d_ik==MAX_VERTICES || previous_d_kj==MAX_VERTICES)
						current_d_matrix[offset]=previous_d_ij;
					else
						current_d_matrix[offset]=Math.Min(previous_d_ij, previous_d_ik+previous_d_kj);
					// end of 'd' calculation

					
					// start of 'PI' calculation		
					if (previous_d_ij <= (previous_d_ik+previous_d_kj))
						current_pi_matrix[offset] = previous_pi_matrix[offset];
					else
						current_pi_matrix[offset] = previous_pi_matrix[MatrixOffset(k,j)];
					// end of 'PI' calculation					
				}
				
			}				
		}
		previous_d_matrix = current_d_matrix;
		previous_pi_matrix = current_pi_matrix;
	}
	
	_d_matrix = null;
	_predecessor_matrix = null;
	
	_d_matrix = current_d_matrix;
	_predecessor_matrix = current_pi_matrix;	

}
/***
* @desc returns the shortest path between 'i' and 'j'
*       this method is based on the PrintAllPairsShorthestPath algorithm
* @see Corman, T, Leiserson, C, Rivest, R, "Introduction to Algorithms", pp.551
*/
public List<int> GetShortestPath(int i, int j)
{

    List<int> path = null;

	// in case we end reach the end of the path
	if (i==j)
    {
		path = new List<int>();
		path.Add(i);
	}
	else
    {
		int offset_ij = MatrixOffset(i,j); 			
		if (_predecessor_matrix[offset_ij]==MAX_VERTICES)
		  return null;
		
		path = GetShortestPath(i, _predecessor_matrix[offset_ij]);

		if (path!=null)
			path.Add(j);

		return  path;
	}
			
	return path;
}

public bool IsOnlyCommonPointInPaths(int v, ref List<int> p1, ref List<int> p2)
{
    int i, j;
    int item_p1;
    int item_p2;
	bool v_exists_in_p1 = false;
	bool v_exists_in_p2 = false;

	for (i=0; i<p1.Count(); i++) {
		item_p1 = p1[i];

		// checks if v exists in p1
		v_exists_in_p1 |= (item_p1 == v);
		
		for (j=0; j<p2.Count();j++) 
        {
			item_p2 = p2[j];
		
			if (item_p1 == item_p2 && item_p1 != v)
				return false;

			// checks if v exists in p2
			v_exists_in_p2 |= (item_p2 == v);
		}
	}
			
	return v_exists_in_p2 && v_exists_in_p1;
}

public bool IsTiermanCompliant(int v,ref List<int>  path_vx, ref List<int> path_vy)
{
    int i;
    int item_vx = path_vx[0];
    int item_vy = path_vy[0];

	// checks if both path start at 'v'
	if (item_vx != v || item_vy != v)
		return false;

	// checks if a cycle only contains vertices that precede v 
	for (i=1; i< path_vx.Count(); i++) {
		item_vx = path_vx[i];
		if (item_vx<=v)
			return false;
	}
	for (i=1; i< path_vy.Count(); i++) {
		item_vy = path_vy[i];
		if (item_vy<=v)
			return false;
	}

	return true;
}

/***
* @desc implements the Horton's algorithm for minimum cycle base
* @return pointer to a miminum cycle base
* @note DELETE the cycle base returned. It wont be deleted by the graph class
*       when it were destroyed
* @see Horton, J.D., "A polynomial-time algorithm to find the shortest cycle basis of a graph", 
*      SIAm J. Comput. 16(2):pp.358-366, 1987
* @see Vismara, P., "Union if all minimum cycle bases of a graph", Electronic Journal of 
*      Combinatronics 4:73--87, 1997 (Paper No. #R9 )
*/
public CycleSet  Horton()
{

	// creates a new cycles set	
	CycleSet p_cycle_set = new CycleSet();

	List<int> path_vx, path_vy;
	
	Cycle cycle;

	int v, x, y;

	// visit all vertices on the graph
	for(v=0; v<GetVertexCount() ;v++) 
    {
		for(x=v+1; x<GetVertexCount();x++) 
        {
			path_vx = GetShortestPath(v,x);
			
			for (y=x+1; y<GetVertexCount(); y++)
            {								
				path_vy = GetShortestPath(v,y);		

				// if paths exists and points x and y are adjacent
				if (path_vx!=null && path_vy!=null && isAdjacent(x,y))
					if (IsOnlyCommonPointInPaths(v, ref path_vx, ref path_vy) &&
						IsTiermanCompliant(v,ref path_vx, ref path_vy))
                    {												
						cycle = new Cycle(path_vx, path_vy);
						if (cycle.GetLength()>0)  
							p_cycle_set.AddCycle(ref cycle);
						else
							cycle = null;
					}

                path_vy = null;			
			}
            path_vx = null;
		}
	}	

	// sort the cycles
	p_cycle_set.Sort();
	
	// lets apply the gaussian elimination
	if (p_cycle_set!=null) 
    {
		p_cycle_set.SelectCycles();
	}
	
	return p_cycle_set;
}
        int _vertex_count;
        MatrixModuloTwo _p_adjacency_matrix;
        int[] _d_matrix;
        int[] _predecessor_matrix;
    }


}
