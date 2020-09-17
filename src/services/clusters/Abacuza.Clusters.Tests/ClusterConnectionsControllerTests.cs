using Abacuza.Clusters.ApiService.Controllers;
using Abacuza.Clusters.ApiService.Models;
using Abacuza.Clusters.Common;
using Abacuza.Common.DataAccess;
using Abacuza.DataAccess.InMemory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Abacuza.Clusters.Tests
{
    public class ClusterConnectionsControllerTests
    {
        private IDataAccessObject _dao;
        private ClusterCollection _clusters;
        private ClusterConnectionsController _controller;
        private readonly ILogger<ClusterConnectionsController> _logger;
        private static readonly Guid _cluster1Guid = new Guid("{21BA40B4-E29E-43DF-A3C7-92579F68B36E}");
        private static readonly Guid _cluster2Guid = new Guid("{9FA1D1DC-D0F1-40E0-86C6-F13EFFB3DF79}");

        public ClusterConnectionsControllerTests()
        {
            var mockLogger = new Mock<ILogger<ClusterConnectionsController>>();
            _logger = mockLogger.Object;
        }

        [SetUp]
        public void Setup()
        {
            _dao = new InMemoryDataAccessObject();

            var mockCluster1 = new Mock<ICluster>();
            mockCluster1.Setup(x => x.Id).Returns(_cluster1Guid);
            mockCluster1.Setup(x => x.Name).Returns("cluster1");
            mockCluster1.Setup(x => x.Type).Returns("cluster");

            var mockCluster2 = new Mock<ICluster>();
            mockCluster2.Setup(x => x.Id).Returns(_cluster2Guid);
            mockCluster2.Setup(x => x.Name).Returns("cluster2");
            mockCluster2.Setup(x => x.Type).Returns("cluster");

            _clusters = new ClusterCollection
            {
                mockCluster1.Object,
                mockCluster2.Object
            };

            _controller = new ClusterConnectionsController(_logger, _clusters, _dao);
        }

        [Test]
        public async Task CreateConnectionWithInvalidClusterTypeTest()
        {
            var model = new ClusterConnectionEntity
            {
                ClusterType = "not a cluster"
            };

            var result = await _controller.CreateClusterConnectionAsync(model);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}