using Moq;
using TranslationManagement.Data;

namespace TranslationManagement.Business.Tests;

/// <summary>Tests for <see cref="TranslatorsBll"/></summary>
[TestFixture]
public class TranslatorsBllTests : BllTests
{
    [Test]
    [TestCase("Applicant")]
    [TestCase("Certified")]
    [TestCase("Deleted")]
    public async Task SetStatusAsync_OK(string status)
    {
        //Arrange
        const int translatorId = 5;
        var repo = new Mock<ITranslatorsRepository>();
        repo.Setup(r => r.UpdateTranslatorStatusAsync(translatorId, status)).Returns(Task.CompletedTask).Verifiable();
        var bll = new TranslatorsBll(repo.Object, Mapper);

        //Act
        await bll.SetStatusAsync(translatorId, status);

        //Assert
        repo.Verify(r => r.UpdateTranslatorStatusAsync(translatorId, status), Times.Once);
    }

    [Test]
    [TestCase("Undeleted")]
    public void SetStatusAsync_Wrong_Throws(string status)
    {
        //Arrange
        const int translatorId = 5;
        var repo = new Mock<ITranslatorsRepository>();
        repo.Setup(r => r.UpdateTranslatorStatusAsync(translatorId, status)).Returns(Task.CompletedTask).Verifiable();
        var bll = new TranslatorsBll(repo.Object, Mapper);

        //Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () => await bll.SetStatusAsync(1, status));
        Assert.That(ex.ParamName, Is.EqualTo("status"));
        repo.Verify(r => r.UpdateTranslatorStatusAsync(translatorId, status), Times.Never);

    }
}
