using System;
using System.Collections.Generic;

public class DijkstraAlgorithm
{
    public static Dictionary<int, double> Dijkstra(SimpleGraph graph, int source)
    {
        var dist = new Dictionary<int, double>();
        var prev = new Dictionary<int, int?>();

        var queue = new PriorityQueue<int, double>();

        // Inizializzazione
        foreach (var vertex in graph.GetVertices())
        {
            dist[vertex] = double.PositiveInfinity;
            prev[vertex] = null;
            queue.Enqueue(vertex, double.PositiveInfinity);
        }

        dist[source] = 0;
        queue.Enqueue(source, 0);

        while (queue.Count > 0)
        {
            int u = queue.Dequeue();

            foreach (var neighbor in graph.GetNeighbors(u))
            {
                double alt = dist[u] + graph.GetWeight(u, neighbor);
                if (alt < dist[neighbor])
                {
                    dist[neighbor] = alt;
                    prev[neighbor] = u;
                    queue.Enqueue(neighbor, alt); // Riaggiungiamo il vicino con nuova priorità
                }
            }
        }

        return dist;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        SimpleGraph g = new SimpleGraph(5);

        // Definizione grafo significativo
        g.SetWeight(1, 2, 1);
        g.SetWeight(1, 3, 2);
        g.SetWeight(2, 4, 1);
        g.SetWeight(3, 4, 2);
        g.SetWeight(3, 5, 2);
        g.SetWeight(4, 5, 1);

        int source = 1;

        // Applichiamo Dijkstra
        var distances = DijkstraAlgorithm.Dijkstra(g, source);

        // Stampiamo il risultato
        Console.WriteLine($"Distanze minime dal nodo {source}:");
        foreach (var pair in distances)
        {
            Console.WriteLine($"Nodo {pair.Key}: distanza = {pair.Value}");
        }
    }
}

public interface IGraph<VertexType>
{
    void SetWeight(VertexType u, VertexType v, double w);
    List<VertexType> GetVertices();
    List<VertexType> GetNeighbors(VertexType vertex);
    double GetWeight(VertexType u, VertexType v);
}

public class SimpleGraph : IGraph<int>
{
    private const double NF = double.PositiveInfinity;
    private double[,] Table;

    public SimpleGraph(int numVertices)
    {
        Table = new double[numVertices, numVertices];
        for (int i = 0; i < numVertices; i++)
        {
            for (int j = 0; j < numVertices; j++)
            {
                Table[i, j] = NF;
            }
        }
    }

    public List<int> GetVertices()
    {
        var result = new List<int>();
        for (int i = 0; i < Table.GetLength(0); i++)
        {
            result.Add(i + 1);
        }
        return result;
    }

    public List<int> GetNeighbors(int vertex)
    {
        var result = new List<int>();
        for (int i = 0; i < Table.GetLength(0); i++)
        {
            if (Table[vertex - 1, i] < NF && vertex - 1 != i)
            {
                result.Add(i + 1);
            }
        }
        return result;
    }

    public void SetWeight(int u, int v, double w)
    {
        Table[u - 1, v - 1] = w;
        Table[v - 1, u - 1] = w; // aggiunto per grafi non orientati
    }

    public double GetWeight(int u, int v)
    {
        return Table[u - 1, v - 1];
    }
}