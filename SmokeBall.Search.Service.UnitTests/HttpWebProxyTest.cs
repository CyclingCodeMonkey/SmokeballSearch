using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmokeBall.Search.Service.Interfaces;

namespace SmokeBall.Search.Service.UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class HttpWebProxyTest
    {
        [TestMethod]
        public async Task ExecuteGoogleSearchAsync_EmptySearchTerm_ShouldReturnEmptyPage_Test()
        {
            var mockHttpClient = new Mock<IHttpHandler>();

            var target = new HttpWebProxy(mockHttpClient.Object);
            var actual = await target.ExecuteGoogleSearchAsync("", 0);

            Assert.AreEqual(string.Empty, actual);
        }

        [TestMethod]
        public async Task ExecuteGoogleSearchAsync_ValieSearchTerm_ShouldReturnResultsPage_Test()
        {
            const string content = "<!DOCTYPE html><html><body><h1>Simple HTML Page</h1>" +
                                   "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit..</p></body></html>";
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(content)
            };
            var mockHttpClient = new Mock<IHttpHandler>();
            mockHttpClient.Setup(c => c.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(responseMessage));

            var target = new HttpWebProxy(mockHttpClient.Object);
            var actual = await target.ExecuteGoogleSearchAsync("software", 1000);

            Assert.AreEqual(content, actual);
        }

        [TestMethod]
        public async Task ExecuteGoogleSearchAsync_ValieSearchTerm_ShouldReturnException_Test()
        {
            const string content = "<!DOCTYPE html><html><body><h1>Simple HTML Page</h1>" +
                                   "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit..</p></body></html>";
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(content),
                ReasonPhrase = "Bad Gateway"
            };
            var mockHttpClient = new Mock<IHttpHandler>();
            mockHttpClient.Setup(c => c.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(responseMessage));

            var target = new HttpWebProxy(mockHttpClient.Object);
            Func<Task> act = async () => { await target.ExecuteGoogleSearchAsync("software"); };

            act.Should().Throw<Exception>().WithMessage("Bad Gateway");
        }
    }
}