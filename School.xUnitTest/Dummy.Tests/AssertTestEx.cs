using FluentAssertions;

namespace School.xUnitTest.Dummy.Tests
{
    public class AssertTestEx
    {
        [Fact] // Test Method
        public void Add_WhenGivenTwoNumbers_ReturnsTheirSum_Without_Fluent_Assertion()
        {
            // Arrange
            int x = 2;
            int y = 3;

            // Act
            int z = x + y;

            // Assert
            Assert.Equal(5, z);
        }

        [Fact]
        public void Add_WhenGivenTwoNumbers_ReturnsTheirSum_With_Fluent_Assertion()
        {
            // Arrange
            int x = 2;
            int y = 3;

            // Act
            int z = x + y;

            // Fluent Assert
            z.Should().Be(5, "sum of '{0} + {1}' not equals 7", x, y);
        }

        [Fact]
        public void String_ShouldStartAndEndWithExpectedValues()
        {
            // Arrange
            var name = "Ahmed";

            // Act + Assert
            name.Should().StartWith("Ah").And.EndWith("ed");
        }

        [Fact]
        public void String_ShouldBeWithLengthGreaterThan8()
        {
            // Arrange
            var password = "Ahmed123#";

            // Act + Assert
            password.Length.Should().BeGreaterThan(8);
        }

        [Fact]
        public void String_ShouldNotBeNullOrEmpty()
        {
            // Arrange
            var word = "welcome";

            // Act + Assert
            word.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Variable_ShouldBeOfTypeString()
        {
            // Arrange
            var word = "welcome";

            // Act + Assert
            word.Should().BeOfType<string>();
        }

        [Fact]
        public void Verify_ValueShouldBeTrue()
        {
            var res = 5 > 3;

            res.Should().BeTrue();
            //res.Should().NotBe(false);
        }

        [Fact]
        public void Verify_Number()
        {
            int num = 5;

            //num.Should().BePositive();
            //num.Should().BeNegative();
            //num.Should().BeGreaterThanOrEqualTo(5);
            num.Should().NotBeInRange(6, 8);
        }

        //[Fact]
        //public void Test()
        //{
        //    Thread.Sleep(5000);
        //}
    }
}