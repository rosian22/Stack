using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace interview
{
    public class Stack<T>
    {
        private List<T> StackElements = new List<T>();

        public void Push(T elementToAdd)
        {
            lock (StackElements)
            {
                StackElements.Add(elementToAdd);
            }
        }

        public void Pop()
        {
            lock (StackElements)
            {
                var lastElement = StackElements.LastOrDefault();
                if (lastElement != null)
                {
                    StackElements.Remove(lastElement);
                }
            }
        }

        public override string ToString()
        {
            var listOfElementsAsString = new StringBuilder(100);

            foreach (var item in StackElements)
            {
                listOfElementsAsString.Append(item);
                listOfElementsAsString.Append(" ");
            }

            return listOfElementsAsString.ToString();
        }

        public void Reverse()
        {
            var reversedStack = new List<T>();
            lock (StackElements)
            {
                while (StackElements.Count != 0)
                {
                    var elementToRemove = StackElements.Last();
                    reversedStack.Add(elementToRemove);
                    StackElements.Remove(elementToRemove);
                }

                StackElements = reversedStack;
            }
        }

        public void ReverseFast()
        {
            lock (StackElements)
            {
                if (StackElements.Count > 0)
                {
                    T currentValue = StackElements.First();
                    StackElements.Remove(currentValue);
                    ReverseFast();
                    StackElements.Add(currentValue);
                }
            }
        }
    }



    class Program
    {

        const int measureValue1 = 5;
        const int measureValue2 = 50;
        const int measureValue3 = 5000;
        const int measureValue4 = 500000;

        static void Main(string[] args)
        {
            var stack = new Stack<int>();

            MesureReverseImplementations(stack, measureValue1);
            MesureReverseImplementations(stack, measureValue2);
            MesureReverseImplementations(stack, measureValue3);
            MesureReverseImplementations(stack, measureValue4);

            Console.ReadLine();
        }

        private static void MesureReverseImplementations(Stack<int> stack, int mesureValue)
        {
            for (int i = 0; i < mesureValue; i++)
            {
                stack.Push(i);
            }

            var swReverseFast = new Stopwatch();
            var swReverse = new Stopwatch();

            swReverseFast.Start();
            stack.ReverseFast();
            swReverseFast.Stop();

            swReverse.Start();
            stack.Reverse();
            swReverse.Stop();

            Console.WriteLine($"ReverseFast Time: {swReverseFast.Elapsed}");
            Console.WriteLine($"Reverse Time: {swReverse.Elapsed}");
            Console.WriteLine(string.Empty);
        }
    }
}
