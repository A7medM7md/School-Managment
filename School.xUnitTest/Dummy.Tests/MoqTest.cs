using FluentAssertions;
using Moq;
using School.xUnitTest.Dummy.Tests.Models;
using School.xUnitTest.MoqService;

namespace School.xUnitTest.Dummy.Tests
{
    public class MoqTest
    {
        private readonly Mock<ICarMoqService> _carMoqService;

        public MoqTest()
        {
            _carMoqService = new Mock<ICarMoqService>();
        }

        [Fact]
        public void AddCar_Should_Return_True_When_Success()
        {
            // Arrange
            var car = new Car(1, "Toyota", "Red");

            _carMoqService.Setup(s => s.Add(It.IsAny<Car>())).Returns(true); // It.IsAny<Car>()) : Means Any Car Object With Any Data

            // Act
            var result = _carMoqService.Object.Add(car);

            // Assert
            result.Should().BeTrue();

            _carMoqService.Verify(s => s.Add(It.Is<Car>(c => c.Name == "Toyota")), Times.Once);
        }

        [Fact]
        public void RemoveCar_Should_Return_True_When_Deleted()
        {
            // Arrange
            var car = new Car(1, "Toyota", "Red");

            _carMoqService
                .Setup(s => s.Delete(It.IsAny<int>())) // Takes Any Int Value
                .Returns(true);

            // Act
            var result = _carMoqService.Object.Delete(1);

            // Assert
            result.Should().BeTrue();

            _carMoqService.Verify(s => s.Delete(1), Times.Once);
        }

        [Fact]
        public void GetAllCars_Should_Return_List_Of_Cars()
        {
            // Arrange
            var fakeCars = new List<Car>
            {
                new Car(1, "Toyota", "Red"),
                new Car(2, "BMW", "Blue")
            };

            _carMoqService
                .Setup(s => s.GetAll())
                .Returns(fakeCars);

            // Act
            var result = _carMoqService.Object.GetAll();

            // Assert
            result.Should().NotBeNullOrEmpty()
                .And.HaveCount(2)
                .And.Contain(c => c.Name == "BMW");

            _carMoqService.Verify(s => s.GetAll(), Times.Once);
        }

        [Fact]
        public void Test()
        {
            Thread.Sleep(5000);
        }
    }
}
