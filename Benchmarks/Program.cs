using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SimpleClass>();
        }
    }

    public class TestClass
    {
        public static void StaticCalculations(object arg)
        {
            var result = 0;
            var startString = arg.ToString();
            
            for (var i = 0; i < 5; i++)
            {
                result += startString.Length;
            }
        }
        
        public void Calculations(object arg)
        {
            var result = 0;
            var startString = arg.ToString();
            
            for (var i = 0; i < 5; i++)
            {
                result += startString.Length;
            }
        }

        public void Method()
        {
            Calculations("Da");
        }

        public static void StaticMethod()
        {
            StaticCalculations("Da");
        }

        public virtual void VirtualMethod()
        {
            Calculations("Da");
        }

        public void GenericMethod<T>(T arg)
            where T : TestClass
        {
            arg.Calculations("Da");
        }

        public void MethodWithTryCatch()
        {
            try
            {
                Calculations("Da");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void MethodWithDynamic(dynamic arg)
        {
            arg.Calculations("Da");
        }

        public void MethodWithReflection()
        {
            GetType().GetMethod("Calculations").Invoke(new TestClass(), new[] {"Da"});
        }
    }
    
    public class SimpleClass
    {
        public static TestClass TestClass = new TestClass();
        
        [Benchmark]
        public void Method()
        {
            TestClass.Method();
        }

        [Benchmark]
        public void StaticMethod()
        {
            TestClass.StaticMethod();
        }

        [Benchmark]
        public void VirtualMethod()
        {
            TestClass.VirtualMethod();
        }
        
        [Benchmark]
        public void GenericMethod()
        {
            TestClass.GenericMethod(TestClass);
        }

        [Benchmark]
        public void MethodWithTryCatch()
        {
            TestClass.MethodWithTryCatch();
        }

        [Benchmark]
        public void MethodWithDynamic()
        {
            TestClass.MethodWithDynamic(TestClass);
        }

        [Benchmark]
        public void MethodWithReflection()
        {
            TestClass.MethodWithReflection();
        }
    }
}