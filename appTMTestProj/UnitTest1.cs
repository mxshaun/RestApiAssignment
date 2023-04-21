using appTM.Controllers;
using appTM.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace appTMTestProj
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void ArtistGetById()
        {
            var controller = new ArtistController(new MusicDbContext(new DbContextOptions<MusicDbContext>()));

            var test = controller.GetById(1);
            Assert.IsTrue(test.IsCompletedSuccessfully);

            Assert.AreEqual(test.Result.ToString(), test.Result);
        }

        [TestMethod]
        public void Test1()
        {
            
        }
}
    }