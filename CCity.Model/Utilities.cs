using Priority_Queue;

namespace CCity.Model
{
    public static class Utilities
    {
        /// <summary>
        /// Gets points around a field within a given radius
        /// </summary>
        /// <param name="f">The field</param>
        /// <param name="r">The radius</param>
        /// <returns>Enumerator of the points</returns>
        public static IEnumerable<(int X, int Y)> GetPointsInRadius(Field f, int r)
        {
            var size = r * 2 + 1;
            var startX = f.X - r;
            var startY = f.Y - r;

            var result = new List<(int X, int Y)>();

            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    var currentX = startX + i;
                    var currentY = startY + j;

                    var distance =
                        Convert.ToInt32(Math.Round(SquareDistance(f.X, f.Y, currentX, currentY)));

                    if (distance <= r)
                        result.Add((currentX, currentY));
                }
            }

            return result;
        }

        /// <summary>
        /// Gets points around a field within a given radius, weighted by the distance from the center of the circle
        /// </summary>
        /// <param name="f">The field</param>
        /// <param name="r">The radius</param>
        /// <returns>Enumerator of the points</returns>
        public static IEnumerable<(int X, int Y, double Weight)> GetPointsInRadiusWeighted(Field f, int r) => 
            from point in GetPointsInRadius(f, r) 
            select (
                point.X, 
                point.Y, 
                Math.Sin(Math.Round(r - SquareDistance(f.X, f.Y, point.X, point.Y)) / r * Math.PI / 2)
            );

        
        /// <summary>
        /// Square distance between 2 placeables
        /// </summary>
        /// <param name="p1">Placeable 1</param>
        /// <param name="p2">Placeable 2</param>
        /// <returns>The square distance between placeable 1 and placeable 2</returns>
        public static double SquareDistance(Placeable p1, Placeable p2) => SquareDistance(p1.Owner, p2.Owner);

        /// <summary>
        /// Square distance between 2 fields
        /// </summary>
        /// <param name="p1">Field 1</param>
        /// <param name="p2">Field 2</param>
        /// <returns>The square distance between field 1 and field 2</returns>
        public static double SquareDistance(Field? f1, Field? f2) => (f1, f2) switch
        {
            (not null, not null) => SquareDistance(f1.X, f1.Y, f2.X, f2.Y),
            _ => 0
        };

        /// <summary>
        /// Square distance between 2 points
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>The square distance between point 1 and point 2</returns>
        public static double SquareDistance(int x1, int y1, int x2, int y2) =>
            Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));

        /// <summary>
        /// Absolute distance between 2 placeables
        /// </summary>
        /// <param name="p1">Placeable 1</param>
        /// <param name="p2">Placeable 2</param>
        /// <returns>The absolute distance between placeable 1 and placeable 2</returns>
        public static int AbsoluteDistance(Placeable? p1, Placeable? p2) => AbsoluteDistance(p1?.Owner, p2?.Owner);

        /// <summary>
        /// Absolute distance between 2 fields
        /// </summary>
        /// <param name="p1">Field 1</param>
        /// <param name="p2">Field 2</param>
        /// <returns>The absolute distance between field 1 and field 2</returns>
        public static int AbsoluteDistance(Field? f1, Field? f2) => (f1, f2) switch
        {
            (not null, not null) =>  AbsoluteDistance(f1.X, f1.Y, f2.X, f2.Y),
            _ => int.MaxValue
        };

        /// <summary>
        /// Absolute distance between 2 points
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>The absolute distance between point 1 and point 2</returns>
        public static int AbsoluteDistance(int x1, int y1, int x2, int y2) => Math.Abs(x1 - x2) + Math.Abs(y1 - y2);

        public static List<(int, int)> GetPointsBetween(Field s, Field t)
        {
            const double errorBound = 0.00;

            double dX = s.X - t.X;
            double dY = s.Y - t.Y;
            double density = (Math.Abs(dX) + 1) * (Math.Abs(dY) + 1);

            double stepX = dX / density;
            double stepY = dY / density;

            HashSet<(int, int)> points = new();
            for (int i = 0; i < density; i++)
            {
                (double X, double Y) = (s.X - i * stepX, s.Y - i * stepY);
                for (int j = -1; j <= 1; j++)
                    for (int k = -1; k <= 1; k++)
                        points.Add(((int)Math.Round(X + j * errorBound), (int)Math.Round(Y + k * errorBound)));
            }
            points.Remove((s.X, s.Y));
            points.Remove((t.X, t.Y));

            return points.ToList();
        }
        
        /// <summary>
        /// Returns the shortest path on roads between two fields using Dijkstra's algorithm.
        /// This method should be provided a starter field and set of fields as destinations.
        /// The first field to be found using Dijkstra's algorithm from the set of goals will be the the destination used in the shortest road. 
        /// </summary>
        /// <param name="fields">The fields of the map.</param>
        /// <param name="width">The width of the map.</param>
        /// <param name="height">The height of the map.</param>
        /// <param name="s">The starting field.</param>
        /// <param name="goals">A set of goal fields.</param>
        /// <returns>
        /// A linked list of fields representing the shortest road between the starting field and one of the goal fields.
        /// The first item in this list is the road next to the starting field and the last item is the destination field.
        /// If the list is empty, there is no road connecting the two fields.
        /// </returns>
        public static LinkedList<Field> ShortestRoad(Field[,] fields, int width, int height, Field s, HashSet<Field> goals)
        {
            var result = new LinkedList<Field>();
            
            // Dijkstra's algorithm to find the shortest road
            var nodes = new FieldNode[width, height];
            var q = new FastPriorityQueue<FieldNode>(width * height);
            
            var d = new float[width, height];
            var pi = new Field?[width, height];

            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
            {
                d[i, j] = float.PositiveInfinity;
                pi[i, j] = null;

                nodes[i, j] = new FieldNode(fields[i, j]);
                q.Enqueue(nodes[i, j], d[i, j]);
            }

            d[s.X, s.Y] = 0;
            q.UpdatePriority(nodes[s.X, s.Y], d[s.X, s.Y]);

            var u = q.Dequeue().Field;
            
            while (d[u.X, u.Y] < float.PositiveInfinity && q.Any())
            {
                if (goals.Contains(u))
                    break;
                
                var neighbors = new List<FieldNode>();

                if (u.X - 1 > 0 && (fields[u.X - 1, u.Y].Placeable is Road || goals.Contains(fields[u.X - 1, u.Y])))
                    neighbors.Add(nodes[u.X - 1, u.Y]);

                if (u.Y - 1 > 0 && (fields[u.X, u.Y - 1].Placeable is Road || goals.Contains(fields[u.X, u.Y - 1])))
                    neighbors.Add(nodes[u.X, u.Y - 1]);

                if (u.X + 1 < width && (fields[u.X + 1, u.Y].Placeable is Road || goals.Contains(fields[u.X + 1, u.Y])))
                    neighbors.Add(nodes[u.X + 1, u.Y]);

                if (u.Y + 1 < height && (fields[u.X, u.Y + 1].Placeable is Road || goals.Contains(fields[u.X, u.Y + 1])))
                    neighbors.Add(nodes[u.X, u.Y + 1]);

                foreach (var neighbor in neighbors)
                {
                    var v = neighbor.Field;

                    if (d[v.X, v.Y] <= d[u.X, u.Y] + 1)
                        continue;
                    
                    pi[v.X, v.Y] = u;
                    d[v.X, v.Y] = d[u.X, u.Y] + 1;
                    q.UpdatePriority(neighbor, d[v.X, v.Y]);
                }

                u = q.Dequeue().Field;
            }

            if (!goals.Contains(u))
                return result;

            //u = pi[u.X, u.Y]!; // The road next to the target

            while (u != s)
            {
                u = pi[u.X, u.Y]!;
                result.AddFirst(u);
            }

            return result;
        }

        private class FieldNode : FastPriorityQueueNode
        {
            public Field Field { get; }

            public FieldNode(Field field)
            {
                Field = field;
            }
        }
    }
}