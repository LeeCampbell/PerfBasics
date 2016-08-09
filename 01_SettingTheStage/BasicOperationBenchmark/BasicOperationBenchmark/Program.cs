using System.Threading;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace BasicOperationBenchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var switcher = new BenchmarkSwitcher(new[] {
            //    typeof(IncrementBenchmarks),
            //    typeof(AdditionBenchmarks),
            //    typeof(MultiplicationBenchmarks),
            //    typeof(Division32BitBenchmarks),
            //    typeof(Division64BitBenchmarks),
            //});
            //switcher.Run(args);

            BenchmarkRunner.Run<IncrementBenchmarks>();
            BenchmarkRunner.Run<AdditionBenchmarks>();
            BenchmarkRunner.Run<MultiplicationBenchmarks>();
            BenchmarkRunner.Run<Division32BitBenchmarks>();
            BenchmarkRunner.Run<Division64BitBenchmarks>();
        }
    }

    class LocalConfig : ManualConfig
    {
        public LocalConfig()
        {
            //Add(Job.AllJits);
            Add(Job.LegacyJitX86, Job.LegacyJitX64, Job.RyuJitX64);
            Add(Job.Clr, Job.Core);
            //Add(Job.Default.With(Mode.Throughput));
            //Add(new MemoryDiagnoser());
            //Add(new InliningDiagnoser());
            Add(StatisticColumn.P0, StatisticColumn.Q1, StatisticColumn.P50, StatisticColumn.Q3,StatisticColumn.P90, StatisticColumn.P95, StatisticColumn.P100);
        }
    }

    [Config(typeof(LocalConfig))]
    public class IncrementBenchmarks
    {
        private int _intValue;
        private long _longValue;

        [Setup]
        public void Setup()
        {
            _intValue = 0;
            _longValue = 0;
        }

        [Benchmark(Baseline = true)]
        public int Increment32BitInteger()
        {
            return ++_intValue;
        }

        [Benchmark]
        public long Increment64BitInteger()
        {
            return ++_longValue;
        }

        [Benchmark]
        public long InterlockedIncrement32BitInteger()
        {
            return Interlocked.Increment(ref _intValue);
        }

        [Benchmark]
        public long InterlockedIncrement64BitInteger()
        {
            return Interlocked.Increment(ref _longValue);
        }
    }

    [Config(typeof(LocalConfig))]
    public class AdditionBenchmarks
    {
        private int _intValue;
        private long _longValue;

        [Params(2, 100)]
        public int Addend { get; set; }

        [Setup]
        public void Setup()
        {
            _intValue = 0;
            _longValue = 0;
        }
        [Benchmark]
        public int Add32BitInteger()
        {
            return _intValue += Addend;
        }

        [Benchmark]
        public long Add64BitInteger()
        {
            return _longValue += Addend;
        }

        [Benchmark]
        public long Add32BitIntegerAtomically()
        {
            return Interlocked.Add(ref _intValue, Addend);
        }

        [Benchmark]
        public long Add64BitIntegerAtomically()
        {
            return Interlocked.Add(ref _longValue, Addend);
        }
    }

    [Config(typeof(LocalConfig))]
    public class MultiplicationBenchmarks
    {
        private int _intValue;
        private long _longValue;

        [Params(2, 3)]
        public int Multiplier { get; set; }

        [Setup]
        public void Setup()
        {
            _intValue = 0;
            _longValue = 0;
        }
        [Benchmark]
        public int Multiply32BitInteger()
        {
            return _intValue *= Multiplier;
        }

        [Benchmark]
        public long Multiply64BitInteger()
        {
            return _longValue *= Multiplier;
        }

        [Benchmark]
        public long Multiply32BitIntegerAtomically()
        {
            return InterlockedMultiply(ref _intValue, Multiplier);
        }

        [Benchmark]
        public long Multiply64BitIntegerAtomically()
        {
            return InterlockedMultiply(ref _longValue, Multiplier);
        }
        private static long InterlockedMultiply(ref int location, int multiplier)
        {
            int original, result;
            do
            {
                original = Volatile.Read(ref location);
                result = original * multiplier;
            } while (Interlocked.CompareExchange(ref location, result, original) != original);
            return result;
        }
        private static long InterlockedMultiply(ref long location, long multiplier)
        {
            long original, result;
            do
            {
                original = Volatile.Read(ref location);
                result = original * multiplier;
            } while (Interlocked.CompareExchange(ref location, result, original) != original);
            return result;
        }
    }

    [Config(typeof(LocalConfig))]
    public class Division32BitBenchmarks
    {
        [Params(2, 10, 1000)]
        public int Numerator { get; set; }
        [Params(2, 3)]
        public int Denominator { get; set; }

        [Benchmark]
        public int Divide32BitInteger()
        {
            return Numerator / Denominator;
        }

        [Benchmark]
        public int Divide32BitIntegerAtomically()
        {
            var numerator = Numerator;
            return InterlockedDivide(ref numerator, Denominator);
        }
        
        private static int InterlockedDivide(ref int location, int denominator)
        {
            int original, result;
            do
            {
                original = Volatile.Read(ref location);
                result = original / denominator;
            } while (Interlocked.CompareExchange(ref location, result, original) != original);
            return result;
        }
    }

    [Config(typeof(LocalConfig))]
    public class Division64BitBenchmarks
    {
        [Params(2, 10, 1000)]
        public long Numerator { get; set; }
        [Params(2, 3)]
        public long Denominator { get; set; }

        [Benchmark]
        public long Divide64BitInteger()
        {
            return Numerator / Denominator;
        }

        [Benchmark]
        public long Divide32BitIntegerAtomically()
        {
            var numerator = Numerator;
            return InterlockedDivide(ref numerator, Denominator);
        }

        private static long InterlockedDivide(ref long location, long denominator)
        {
            long original, result;
            do
            {
                original = Volatile.Read(ref location);
                result = original / denominator;
            } while (Interlocked.CompareExchange(ref location, result, original) != original);
            return result;
        }
    }

}
