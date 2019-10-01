using Faction.Foundation.Domain.v1;
using NUnit.Framework;
using ObjectGenerator.TestObjects;

namespace ObjectGenerator
{
    [TestFixture]
    public class TypeGeneratorTest
    {
        [Test]
        public void ShouldGenerateCombinationsForSimpleObject()
        {
            var mockTypeGenerator = new MockTypeGenerator();
            var genericDataResult = mockTypeGenerator.GenerateDefault<SimpleObject>(false);
            Assert.NotNull(genericDataResult);
        }

        [Test]
        public void ShouldGenerateCombinationsForObjectWithNesting()
        {
            var mockTypeGenerator = new MockTypeGenerator();
            var genericDataResult = mockTypeGenerator.GenerateDefault<NestedType>(false);
            Assert.NotNull(genericDataResult);
        }

        [Test]
        public void ShouldGenerateCombinationsForComplexObject()
        {
            var mockTypeGenerator = new MockTypeGenerator();
            var genericDataResult = mockTypeGenerator.GenerateDefault<MainType>(false);
            Assert.NotNull(genericDataResult);
        }

        [Test]
        public void ShouldGenerateCombinationsForObjectWithPrivateList()
        {
            var mockTypeGenerator = new MockTypeGenerator();
            var genericDataResult = mockTypeGenerator.GenerateDefault<DataResult<SimpleObject>>(false);
            Assert.NotNull(genericDataResult);
        }
    }
}
