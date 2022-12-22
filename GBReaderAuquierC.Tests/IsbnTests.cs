using GBReaderAuquierC.Domains;

namespace GBReaderAuquierC.Tests
{
    public class IsbnTests
    {
        [TestCase("00")]
        [TestCase("")]
        [TestCase(null)]
        [TestCase("12301234567891")]
        public void whenIsbnLengthLessThan10OrMoreThan11ThenIsbnIsNotValid(string isbn)
        {
            Assert.False(Isbn.IsValid(isbn));
        }
        
        [TestCase("000000aa00")]
        [TestCase("000000000X")]
        public void whenIsbnContainsCharThenIsbnIsNotValid(string isbn)
        {
            Assert.False(Isbn.IsValid(isbn));
        }

        [Test]
        public void whenIsbnHasLengthOf10ButWrongControlNumThenIsbnIsNotValid()
        {
            Assert.False(Isbn.IsValid("1234567890"));
        }

        [TestCase("2200106696", true)]
        [TestCase("2200106698", false)]
        [TestCase("2200106785", true)]
        [TestCase("22001068410", true)]
        [TestCase("22001063011", true)]
        [TestCase("22001063015", false)]
        public void ValidControlNumber(string isbn, bool expected)
        {
            Assert.True(Isbn.IsValid(isbn) == expected);
        }

        [TestCase("2200106696", "2200106696")]
        [TestCase("2200106785", "2200106785")]
        [TestCase("22001068410", "220010684X")]
        [TestCase("22001063011", "2200106300")]
        public void ConvertForUser(string isbn, string expected)
        {
            Assert.AreEqual(expected, Isbn.ConvertForUser(isbn));
        }

        [TestCase("2200106696", "2200106696")]
        [TestCase("2200106785", "2200106785")]
        [TestCase("220010684X", "22001068410")]
        [TestCase("2200106300", "22001063011")]
        public void ConvertForSys(string isbn, string expected)
        {
            Assert.AreEqual(expected, Isbn.ConvertForSys(isbn));
        }
    }
}