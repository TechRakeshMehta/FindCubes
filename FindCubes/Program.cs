using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

public class Program
{
    public static async Task Main()
    {
        try
        {
            var cancellationTokenSource = new CancellationTokenSource(1000);
            var token = cancellationTokenSource.Token;
            int degreeOfParallelism = 4;
            Task<int[]> task = Task<int[]>.Factory.StartNew((state) =>
            {
                var items = Enumerable.Range(1, 1000).ToArray();
                int degreeOfParallelism = Convert.ToInt32(state);
                static int map(int x) => x * x * x;
                var cubes = items.AsParallel().AsOrdered().WithCancellation(token).WithDegreeOfParallelism(Convert.ToInt32(state)).Select(map).ToArray();
                return cubes;
            }, cancellationToken: token, state: degreeOfParallelism);
            int[] cubes = await task;
            Console.WriteLine(string.Join(',' , cubes));
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        Console.ReadKey();
    }
}



