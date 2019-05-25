using System;

namespace SSoft.CWork {
    public class Syncer {
        public Action Enter { get; }
        public Action Exit { get; }

        public Syncer(Action enter, Action exit) {
            Enter = enter;
            Exit = exit;
        }

        public T Sync<T>(Func<T> action) {
            Enter();
            var result = action();
            Exit();

            return result;
        }
    }
}