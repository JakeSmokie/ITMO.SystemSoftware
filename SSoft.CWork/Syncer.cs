using System;
using System.Threading;

namespace SSoft.CWork {
    public class Syncer {
        public Syncer(Action enter, Action exit) {
            Enter = () => {
                Console.Out.WriteLineAsync($"Поток {Thread.CurrentThread.ManagedThreadId} пытается войти в секцию");
                enter();
                Console.Out.WriteLineAsync($"Поток {Thread.CurrentThread.ManagedThreadId} вошёл в секцию");

            };
            Exit = () => {
                exit();
                Console.Out.WriteLineAsync($"Поток {Thread.CurrentThread.ManagedThreadId} выходит");
            };
        }

        public Action Enter { get; }
        public Action Exit { get; }

        public T Sync<T>(Func<T> action) {
            Enter();
            var result = action();
            Exit();

            return result;
        }
    }
}