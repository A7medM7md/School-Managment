using School.xUnitTest.Models;

namespace School.xUnitTest.MoqService
{
    public class CarMoqService : ICarMoqService
    {
        // Mock Data [Fake Table]
        public List<Car> Cars = new List<Car>();


        public bool Add(Car? car)
        {
            if (car is null) return false;

            Cars.Add(car);
            return true;
        }

        public bool Delete(int? id)
        {
            if (id is null) return false;

            var car = Cars.Find(c => c.Id == id);

            if (car is null) return false; // This Case Is Already Handle As Return From Remove();

            return Cars.Remove(car);
        }

        public List<Car> GetAll()
        {
            return Cars;
        }
    }
}
