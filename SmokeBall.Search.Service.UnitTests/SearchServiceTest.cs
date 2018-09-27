using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmokeBall.Search.Models;
using SmokeBall.Search.Models.Interfaces;
using SmokeBall.Search.Service.Interfaces;

namespace SmokeBall.Search.Service.UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SearchServiceTest
    {
        private const string content = "<!DOCTYPE html><html><body><h1>Simple HTML Page</h1>" +
                                       "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit..</p></body></html>";

        [TestMethod]
        public async Task FindRankingsAsync_EmptySearchResult_ShouldReturnZero_Test()
        {
            var mockWebProxy = new Mock<IHttpWebProxy>();
            var mockParser = new Mock<IHtmlParser>();
            mockWebProxy.Setup(c => c.ExecuteGoogleSearchAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(content));
            var resultList = new List<ISearchResult>();
            mockParser.Setup(p => p.GetGoogleSearchResults(It.IsAny<string>())).Returns(resultList);

            var target = new SearchService(mockWebProxy.Object, mockParser.Object);
            var actual = await target.FindRankingsAsync("software", "www.microsoft.com");
            Assert.AreEqual("0", actual);
        }


        [TestMethod]
        public async Task FindRankingsAsync_A__B_Test()
        {
            var mockWebProxy = new Mock<IHttpWebProxy>();
            var mockParser = new Mock<IHtmlParser>();
            mockWebProxy.Setup(c => c.ExecuteGoogleSearchAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(content));
            var resultList = new List<ISearchResult>();
            for (var i = 1; i < 100; i++)
            {
                resultList.Add(new SearchResult {Position = i, Url = "Lorem ipsum dolor sit amet" });
            }

            resultList[5].Url = "https://www.microsoft.com.au";
            resultList[9].Url = "http://www.microsoft.com";
            resultList[15].Url = "https://doc.microsoft.com/en_au";
            resultList[55].Url = "http://www.microsoft.com/";
            mockParser.Setup(p => p.GetGoogleSearchResults(It.IsAny<string>())).Returns(resultList);

            var target = new SearchService(mockWebProxy.Object, mockParser.Object);
            var actual = await target.FindRankingsAsync("software", "www.microsoft.com");
            Assert.AreEqual("6, 10, 56", actual);
        }
    }
}