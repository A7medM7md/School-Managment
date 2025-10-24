namespace School.xUnitTest.Dummy.Tests.TestModels
{
    public class Car
    {
        public Car(int id, string? name, string? color)
        {
            Id = id;
            Name = name;
            Color = color;
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
    }
}
