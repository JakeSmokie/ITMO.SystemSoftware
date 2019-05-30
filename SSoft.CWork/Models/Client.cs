using System;

namespace SSoft.CWork {
    public class Client {
        private static int _seed = 228;

        public Client(string name, decimal cash) {
            Name = name;
            Cash = cash;
        }

        public string Name { get; }
        public decimal Cash { get; set; }
        public Random DecisionRandom { get; } = new Random(_seed += 2384);
    }
}