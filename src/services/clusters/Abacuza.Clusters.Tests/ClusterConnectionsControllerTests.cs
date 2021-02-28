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
#nullable disable
    public class ClusterConnectionsControllerTests
    {
        private IDataAccessObject _dao;
        private ClusterCollection _clusters;
        private ClusterConnectionsController _controller;
        private readonly ILogger<ClusterConnectionsController> _logger;
        private static readonly Guid _cluster1Guid = new Guid("{21BA40B4-E29E-43DF-A3C7-92579F68B36E}");
        private static readonly Guid _cluster2Guid = new Guid("{9FA1D1DC-D0F1-40E0-86C6-F13EFFB3DF79}");
        private static readonly Guid _clusterConnection1Guid = new Guid("{D71C5B22-3B9D-4006-BDDB-215A2BF344E3}");

        public ClusterConnectionsControllerTests()
        {
            var mockLogger = new Mock<ILogger<ClusterConnectionsController>>();
            _logger = mockLogger.Object;
        }

        [SetUp]
        public void Setup()
        {
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

            _dao = new InMemoryDataAccessObject();
            _dao.AddAsync(new ClusterConnectionEntity
            {
                Id = _clusterConnection1Guid,
                ClusterType = "cluster",
                Description = "fake cluster connection",
                Name = "FakeClusterConnection",
            });

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

        [Test]
        public async Task CreateConnectionWithDuplicatedNameTest()
        {
            var model = new ClusterConnectionEntity
            {
                ClusterType = "cluster",
                Name = "FakeClusterConnection"
            };

            var result = await _controller.CreateClusterConnectionAsync(model);
            Assert.IsInstanceOf<ConflictObjectResult>(result);
        }

        [Test]
        public async Task CreateConnectionSuccessfulTest()
        {
            var cnt = (await _dao.GetAllAsync<ClusterConnectionEntity>()).Count();
            var model = new ClusterConnectionEntity
            {
                ClusterType = "cluster",
                Name = "Connection 1"
            };

            var result = await _controller.CreateClusterConnectionAsync(model);
            var cnt2 = (await _dao.GetAllAsync<ClusterConnectionEntity>()).Count();
            Assert.AreEqual(cnt + 1, cnt2);
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
        }

        [Test]
        public async Task DeleteClusterConnectionWithInvalidIdTest()
        {
            var id = Guid.Empty;
            var result = await _controller.DeleteClusterConnectionAsync(id);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task DeleteClusterConnectionSuccessfulTest()
        {
            var id = _clusterConnection1Guid;
            var result = await _controller.DeleteClusterConnectionAsync(id);
            Assert.IsInstanceOf<OkResult>(result);
        }
    }
}