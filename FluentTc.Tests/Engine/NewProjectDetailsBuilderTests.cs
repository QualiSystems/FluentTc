using FluentAssertions;
using FluentTc.Engine;
using NUnit.Framework;

namespace FluentTc.Tests.Engine
{
    [TestFixture]
    public class NewProjectDetailsBuilderTests
    {
        [Test]
        public void GetDataXml_IdNameAndParent_XmlFormatted()
        {
            // Arrange
            var newProjectDetailsBuilder = new NewProjectDetailsBuilder();

            // Act
            newProjectDetailsBuilder.Name("New Project Name")
                .Id("newProjectId")
                .ParentProject(x => x.Id("parentProjectId"));

            // Assert
            newProjectDetailsBuilder.GetDataXml()
                .Should()
                .Be(
                    @"<newProjectDescription name='New Project Name' id='newProjectId'><parentProject locator='id:parentProjectId'/></newProjectDescription>");

        }

        [Test]
        public void GetDataXml_Name_XmlFormatted()
        {
            // Arrange
            var newProjectDetailsBuilder = new NewProjectDetailsBuilder();

            // Act
            newProjectDetailsBuilder.Name("New Project Name");

            // Assert
            newProjectDetailsBuilder.GetDataXml()
                .Should()
                .Be(
                    @"<newProjectDescription name='New Project Name'></newProjectDescription>");

        }
    }
}