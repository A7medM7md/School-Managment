using School.xUnitTest.Dummy.Tests.TestModels;

namespace School.xUnitTest.MoqService
{
    public class CarMoqService : ICarMoqService
    {
        private readonly List<Car> cars;

        // Mock Data [Fake Table]
        // List<Car> cars = new List<Car>();

        public CarMoqService(List<Car> _cars)
        {
            cars = _cars;
        }


        public bool Add(Car? car)
        {
            if (car is null) return false;

            cars.Add(car);
            return true;
        }

        public bool Delete(int? id)
        {
            if (id is null) return false;

            var car = cars.Find(c => c.Id == id);

            if (car is null) return false; // This Case Is Already Handle As Return From Remove();

            return cars.Remove(car);
        }

        public List<Car> GetAll()
        {
            return cars;
        }
    }
}
