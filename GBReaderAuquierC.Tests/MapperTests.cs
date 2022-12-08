using GBReaderAuquierC.Domains;
using GBReaderAuquierC.Repositories;

namespace GBReaderAuquierC.Tests
{
    public class MapperTests
    {
        [Test]
        public void ConvertBookDtoWithPages()
        {
            BookDTO dto = new(
                "Title", 
                "Hello",
                "Me",
                "isbn",
                "imgPath",
                "1.2"
            );
            PageDTO p1 = new PageDTO("Helloé");
            p1.Choices.Add("coucou", "p2");
            PageDTO p2 = new PageDTO("Halloé");
            p2.Choices.Add("coucou", "p1");
            var pages = new List<PageDTO>();
            pages.Add(p1);
            pages.Add(p2);
            dto.Pages = pages;
            var page1 = new Page("Helloé");
            var page2 = new Page("Halloé");
            Dictionary<string, Page> choices1 = new();
            choices1.Add("coucou", page2);
            page1.Choices = choices1;
            Dictionary<string, Page> choices2 = new();
            page2.Choices = choices2;
            choices2.Add("coucou", page1);
            List<Page> temp = new List<Page>();
            temp.Add(page1);
            temp.Add(page2);
            Book result = new(
                "Title",
                "Me",
                "isbn",
                "Hello",
                "imgPath"
            );
            result.Pages = temp;
            Page resultP1 = new Page("Halloé");
            Page resultP2 = new Page("Halloé");
            resultP1.AddChoice("coucou", resultP2);
            resultP2.AddChoice("coucou", resultP1);
            Assert.That(result, Is.EqualTo(Mapper.ConvertToBook(dto)));
        }
    }
}