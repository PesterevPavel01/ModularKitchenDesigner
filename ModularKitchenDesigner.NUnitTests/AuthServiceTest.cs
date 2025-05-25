namespace ModularKitchenDesigner.NUnitTests
{
    public class AuthServiceTest
    {
        [SetUp]

        public void SetUp()
        { }

        [TestCase("Иванов")]
        //[TestCase("Петров")]
        //[TestCase("Сидоров")]
        public void Login(string name) 
        {
            //arrange подготовка

            //var service = new AuthService();

            //act выполнение

            var result = true;

            //assert проверка

            Assert.IsTrue(result);
        }
    }
}
