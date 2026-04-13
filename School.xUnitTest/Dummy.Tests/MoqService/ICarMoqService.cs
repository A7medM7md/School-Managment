using School.xUnitTest.Dummy.Tests.TestModels;

namespace School.xUnitTest.MoqService
{
    public interface ICarMoqService
    {
        public List<Car> GetAll();
        public bool Add(Car? car);
        public bool Delete(int? id);
    }
}
