using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parallelization
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("### Parallel ForEach");
            ParallelForEach_Demo();

            Console.WriteLine("### Parallel For");
            ParallelFor_Demo();

            Console.WriteLine("### Parallel Linq");
            ParallelLinq_Demo();

            Console.ReadLine();
        }

        private static void ParallelFor_Demo()
        {
            int iterations = 25;

            Parallel.For(
                fromInclusive: 0,
                toExclusive: iterations,
                body: (i) =>  { System.Console.WriteLine("parallel: " + i); } 
                );
        }

        private static void ParallelForEach_Demo()
        {
            int iterations = 25;
            // var range = Enumerable.Range(0, iterations);
            Parallel.ForEach
            (
                LoopGenerate(0, 25.0, 1),
                i => { Console.WriteLine("for each item: " + (double)(i / 10)); }
            );

        }
        static IEnumerable<double> LoopGenerate(double start, double end, double step)
        {
            for (double i = start; i < end; i += step)
            {
                yield return i;
            }
        }


        private static void ParallelLinq_Demo()
        {
            const string sentence = "the quick brown fox jumps over the lazy dog, lorem ipsum dolor amit sit non omnibus alequim costodius";
            char[] vowels = new[] { 'a', 'e', 'i', 'o', 'u' };

            var query = sentence.AsEnumerable()
                    .Where(x => { Thread.Sleep(100); return vowels.Any(v => v == x); })
                    .Select((vowel, index) => new { Index = index, Vowel = vowel })
                    .AsParallel()
                    .AsOrdered()
                    .WithDegreeOfParallelism(3)
                    .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism);

            Console.WriteLine(string.Join("\n", query));
        }
    }
}






