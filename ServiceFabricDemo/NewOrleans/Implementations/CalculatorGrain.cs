using System.Threading.Tasks;
using Grains.Interfaces;
using Orleans;

namespace NewOrleans.Implementations
{
    public class CalculatorGrain : Grain, ICalculatorGrain
    {
        private readonly GrainObserverManager<ICalculatorObserver> _observers = new GrainObserverManager<ICalculatorObserver>();
        private double _current;

        public Task<double> Add(double value)
        {
            var result = _current += value;
            _observers.Notify(observer => observer.CalculationUpdated(result));
            return Task.FromResult(result);
        }

        public Task<double> Divide(double value)
        {
            var result = _current /= value;
            _observers.Notify(observer => observer.CalculationUpdated(result));
            return Task.FromResult(result);
        }

        public Task<double> Get()
        {
            return Task.FromResult(_current);
        }

        public Task<double> Multiply(double value)
        {
            var result = _current *= value;
            _observers.Notify(observer => observer.CalculationUpdated(result));
            return Task.FromResult(result);
        }

        public Task<double> Set(double value)
        {
            var result = _current = value;
            _observers.Notify(observer => observer.CalculationUpdated(result));
            return Task.FromResult(result);
        }

        public Task<double> Subtract(double value)
        {
            var result = _current -= value;
            _observers.Notify(observer => observer.CalculationUpdated(result));
            return Task.FromResult(result);
        }

        public Task Subscribe(ICalculatorObserver observer)
        {
            _observers.Subscribe(observer);
            return Task.FromResult(0);
        }
    }
}