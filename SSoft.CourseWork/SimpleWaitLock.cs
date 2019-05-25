using System;
using System.Threading;

namespace SSoft.CourseWork {
    public class SimpleWaitLock : IDisposable {
        private readonly AutoResetEvent _free = new AutoResetEvent(true);

        public void Dispose() {
            _free?.Dispose();
        }

        public void Enter() {
            _free.WaitOne();
        }

        public void Leave() {
            _free.Set();
        }
    }
}