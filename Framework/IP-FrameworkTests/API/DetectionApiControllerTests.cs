using Microsoft.VisualStudio.TestTools.UnitTesting;
using IP_Framework.API;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace IP_Framework.API.Tests
{
    [TestClass()]
    public class DetectionApiControllerTests
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }


        [TestMethod()]
        public void PostTest()
        {
           
            String[] eyeQuestions = { "\"Simti durere la nivelul ochilor?\"", "\"Ai roseata in ochi prezenta?\"" , "\"Ai experimentat recent o lacrimare intensa a ochilor?\"" };
            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Scheme).Returns("http");
            request.Setup(x => x.Host).Returns(HostString.FromUriComponent("http://localhost:5080"));
            request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api/v1/detectionapi/get-question"));
            String str = "{\"id\":1,\"status\":37.3}";
            var httpContext = Mock.Of<HttpContext>(_ =>
                _.Request == request.Object
            );
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new DetectionApiController()
            {
                ControllerContext = controllerContext,
            };
            String expected = "{@ \"id\": 1,@\"question\": \"Te rugam sa introduci temperatura corpului tau:\",@\"tip\": \"interval\"@}";
            expected = expected.Replace("@", "\n");
            //Act
            JObject json = JObject.Parse(str);
            string actual = controller.Post(json);
            String question = actual.Split(',')[1];
            String toBeSearched = "\"question\": ";
            question = question.Substring(question.IndexOf(toBeSearched) + toBeSearched.Length);
            if(question == "\"Te rugam sa introduci temperatura corpului tau:\"")
            {
                var secondStr = "{\"id\": 1, \"status\":37.3}";
                var secondJson = JObject.Parse(secondStr);
                var sendResponse = controller.PostResponse(secondJson);
               
                JObject statusEsential = JObject.Parse("{\"id\": 1, \"status\":1}");
                JObject statusOthers = JObject.Parse("{\"id\": 1, \"status\":0}");
                while (!sendResponse.Contains("verdict"))
                {
                     TestContext.WriteLine(sendResponse);
                    question = sendResponse.Split(',')[1];
                    question = question.Substring(question.IndexOf(toBeSearched) + toBeSearched.Length);
                    if (eyeQuestions.Contains(question))
                        sendResponse = controller.PostResponse(statusEsential);
                    else
                        sendResponse = controller.PostResponse(statusOthers);
                }
                string verdict = sendResponse.Split("\"")[3];
                actual = verdict;
            }
            expected = "conjunctivita";
            //Assert
                Assert.AreEqual(expected, actual);
        }

        
    }
}