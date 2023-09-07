using Xunit;
using Moq;
using WebAppi_Diplom;
using WebApi_HW7;
using Microsoft.EntityFrameworkCore;

namespace Football_Tests
{
    public class TeamTests
    {
        [Fact]
        public void AddTeamToRepository_Success()
        {
            // Arrange

            var mockRepository = new Mock<TeamRepository>();
            mockRepository.Setup(repo => repo.AddTeam(It.IsAny<string>())).Verifiable();
            var service = new TeamService(mockRepository.Object);

            
            // Act
            service.AddTeam("Barselona");

            // Assert
            mockRepository.Verify(repo => repo.AddTeam("Barselona"), Times.Once);
        }

    }
}
