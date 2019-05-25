using System;
using System.Threading;

namespace SSoft.CourseWork {
    public class SimpleHybridLock : IDisposable {
        private readonly AutoResetEvent _lock = new AutoResetEvent(false);
        private int _waiters;

        public void Dispose() {
            _lock.Dispose();
        }

        public void Enter() {
            if (Interlocked.Increment(ref _waiters) == 1) {
                return;
            }

            _lock.WaitOne();
        }

        public void Leave() {
            if (Interlocked.Decrement(ref _waiters) == 0) {
                return;
            }

            _lock.Set();
        }
    }
}