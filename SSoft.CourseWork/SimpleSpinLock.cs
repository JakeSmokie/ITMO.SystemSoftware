using System.Threading;

namespace SSoft.CourseWork {
    public class SimpleSpinLock {
        private int _locked;

        public void Enter() {
            while (Interlocked.Exchange(ref _locked, 1) == 1) {
            }
        }

        public void Leave() {
            Volatile.Write(ref _locked, 0);
        }
    }
}