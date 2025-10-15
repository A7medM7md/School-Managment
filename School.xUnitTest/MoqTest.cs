using FluentAssertions;
using School.xUnitTest.Models;
using School.xUnitTest.MoqService;

namespace School.xUnitTest
{
    public class MoqTest
    {
        CarMoqService _carMoqService = new CarMoqService();

        [Fact]
        public void AddCar()
        {
            /// Arrange
            var car = new Car(1, "Toyota", "Red");

            /// Act
            var result = _carMoqService.Add(car);

            /// Assert
            //Assert.True(result);

            // Or [Fluent Assertion]
            result.Should().BeTrue();

            // Extra Validations

            _carMoqService.GetAll().Should()
                .ContainSingle()
                .Which.Name.Should().Be("Toyota");
        }

        [Fact]
        public void RemoveCar()
        {
            // Arrange
            var car = new Car(1, "Toyota", "Red");
            _carMoqService.Add(car);

            // Act
            var result = _carMoqService.Delete(1);

            // Assert
            result.Should().BeTrue();

            _carMoqService.GetAll().Should().BeEmpty();

        }

        [Fact]
        public void GetAllCars()
        {
            // Arrange
            _carMoqService.Add(new Car(1, "Toyota", "Red"));
            _carMoqService.Add(new Car(2, "BMW", "Blue"));

            // Act
            var cars = _carMoqService.GetAll();

            // Assert
            cars.Should().NotBeNullOrEmpty()
                .And.HaveCount(2)
                .And.Contain(c => c.Name == "BMW");
        }
    }
}
