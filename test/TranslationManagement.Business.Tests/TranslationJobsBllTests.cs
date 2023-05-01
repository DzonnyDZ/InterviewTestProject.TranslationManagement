using External.ThirdParty.Services;
using Microsoft.Extensions.Logging;
using Moq;
using TranslationManagement.Business.JobReaders;
using TranslationManagement.Data;
using TranslationManagement.Data.Model;
using TranslationManagement.Test.Utilities;

namespace TranslationManagement.Business.Tests;

/// <summary>Tests for <see cref="TranslationJobsBll"/></summary>
[TestFixture]
public class TranslationJobsBllTests : BllTests
{
    private Mock<ITranslationJobsRepository> repo;
    private Mock<ITranslatorsRepository> transRepo;
    private Mock<INotificationService> notifSvc;
    private Mock<IJobFileReaderFactory> readerFact;
    private ILogger<TranslationJobsBll> bllLogger;

    public override void OneTimeSetUp()
    {
        base.OneTimeSetUp();
        bllLogger = TestLogger.Create<TranslationJobsBll>();
    }

    [SetUp]
    public void SetUp()
    {
        repo = new Mock<ITranslationJobsRepository>();
        transRepo = new Mock<ITranslatorsRepository>();
        notifSvc = new Mock<INotificationService>();
        readerFact = new Mock<IJobFileReaderFactory>();
    }

    [Test]
    [TestCase(TranslationJobsBll.JobStatus.New, TranslationJobsBll.JobStatus.New)]
    [TestCase(TranslationJobsBll.JobStatus.InProgress, TranslationJobsBll.JobStatus.InProgress)]
    [TestCase(TranslationJobsBll.JobStatus.Completed, TranslationJobsBll.JobStatus.Completed)]

    [TestCase(TranslationJobsBll.JobStatus.New, TranslationJobsBll.JobStatus.InProgress)]
    [TestCase(TranslationJobsBll.JobStatus.InProgress, TranslationJobsBll.JobStatus.Completed)]
    public async Task UpdateJobStatusAsync_OK(string from, string to)
    {
        //Arrange
        const int jobId = 235;
        const int translatorId = 6;
        repo.Setup(r => r.GetByIdAsync(jobId)).Returns(Task.FromResult(new TranslationJob { Id = jobId, CustomerName = "Bla", OriginalContent = "Bla bla", Status = from, Price = 65.3 }));
        repo.Setup(r => r.UpdateJobStatusAsync(jobId, to, It.IsAny<int?>())).Returns(Task.CompletedTask).Verifiable();
        var translator = new Translator { Id = translatorId, CreditCardNumber = "123", Name = "Jan Horák", HourlyRate = "35", Status = "Certified" };
        transRepo.Setup(r => r.GetByIdAsync(translatorId)).Returns(Task.FromResult(translator));
        var bll = new TranslationJobsBll(repo.Object, transRepo.Object, notifSvc.Object, Mapper, readerFact.Object, bllLogger);

        //Act
        await bll.UpdateJobStatusAsync(jobId, translatorId, to);

        //Assert
        repo.Verify(r => r.UpdateJobStatusAsync(jobId, to, It.IsAny<int?>()), from == to ? Times.Never : Times.Once);
    }

    [Test]
    [TestCase(TranslationJobsBll.JobStatus.New, TranslationJobsBll.JobStatus.Completed)]
    [TestCase(TranslationJobsBll.JobStatus.Completed, TranslationJobsBll.JobStatus.InProgress)]
    [TestCase(TranslationJobsBll.JobStatus.Completed, TranslationJobsBll.JobStatus.New)]

    [TestCase(TranslationJobsBll.JobStatus.InProgress, TranslationJobsBll.JobStatus.New)]
    [TestCase("Unknown", TranslationJobsBll.JobStatus.New)]
    public async Task UpdateJobStatusAsync_InvalidTransition_Throws(string from, string to)
    {
        //Arrange
        const int jobId = 235;
        const int translatorId = 6;
        repo.Setup(r => r.GetByIdAsync(jobId)).Returns(Task.FromResult(new TranslationJob { Id = jobId, CustomerName = "Bla", OriginalContent = "Bla bla", Status = from, Price = 65.3 }));
        repo.Setup(r => r.UpdateJobStatusAsync(jobId, to, It.IsAny<int?>())).Returns(Task.CompletedTask).Verifiable();
        var bll = new TranslationJobsBll(repo.Object, transRepo.Object, notifSvc.Object, Mapper, readerFact.Object, bllLogger);

        //Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () => await bll.UpdateJobStatusAsync(jobId, translatorId, to));
        repo.Verify(r => r.UpdateJobStatusAsync(jobId, to, It.IsAny<int?>()), Times.Never);
    }

    [Test]
    [TestCase("Unknown")]
    [TestCase("Xyz")]
    public void UpdateJobStatusAsync_InvalidNewStatus(string to)
    {
        //Arrange
        const int jobId = 235;
        const int translatorId = 6;
        repo.Setup(r => r.GetByIdAsync(jobId)).Returns(Task.FromResult(new TranslationJob { Id = jobId, CustomerName = "Bla", OriginalContent = "Bla bla", Status = "Unknown", Price = 65.3 })).Verifiable();
        repo.Setup(r => r.UpdateJobStatusAsync(jobId, to, It.IsAny<int?>())).Returns(Task.CompletedTask).Verifiable();
        var bll = new TranslationJobsBll(repo.Object, transRepo.Object, notifSvc.Object, Mapper, readerFact.Object, bllLogger);

        //Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () => await bll.UpdateJobStatusAsync(jobId, translatorId, to));
        Assert.That(ex.ParamName, Is.EqualTo("status"));
        repo.Verify(r => r.GetByIdAsync(jobId), Times.Never);
        repo.Verify(r => r.UpdateJobStatusAsync(jobId, to, It.IsAny<int?>()), Times.Never);
    }

    [Test]
    public async Task NotifyOnNewJob_PermanentFalse()
    {
        //Arrange
        var bll = new TranslationJobsBll(repo.Object, transRepo.Object, notifSvc.Object, Mapper, readerFact.Object, bllLogger);
        bll.ExponentialBackOffInitialDelay = 1;
        notifSvc.Setup(s => s.SendNotification(It.IsAny<string>())).Returns(Task.FromResult(false)).Verifiable();
        var entity = new TranslationJob { CustomerName = "DPD", Id = 6, OriginalContent = "Bla bla", Price = 3.6, Status = TranslationJobsBll.JobStatus.New };

        //Act
        var res = await bll.NotifyOnNewJobAsync(entity);

        //Assert
        Assert.That(res, Is.False);
        notifSvc.Verify(s => s.SendNotification(It.IsAny<string>()), Times.Exactly(5));
    }

    [Test]
    public async Task NotifyOnNewJob_PermanentFailure()
    {
        //Arrange
        var bll = new TranslationJobsBll(repo.Object, transRepo.Object, notifSvc.Object, Mapper, readerFact.Object, bllLogger);
        bll.ExponentialBackOffInitialDelay = 1;
        notifSvc.Setup(s => s.SendNotification(It.IsAny<string>())).Throws<ApplicationException>().Verifiable();
        var entity = new TranslationJob { CustomerName = "DPD", Id = 6, OriginalContent = "Bla bla", Price = 3.6, Status = TranslationJobsBll.JobStatus.New };

        //Act
        var res = await bll.NotifyOnNewJobAsync(entity);

        //Assert
        Assert.That(res, Is.False);
        notifSvc.Verify(s => s.SendNotification(It.IsAny<string>()), Times.Exactly(5));
    }

    [Test]
    public async Task NotifyOnNewJob_Success()
    {
        //Arrange
        var bll = new TranslationJobsBll(repo.Object, transRepo.Object, notifSvc.Object, Mapper, readerFact.Object, bllLogger);
        notifSvc.Setup(s => s.SendNotification(It.IsAny<string>())).Returns(Task.FromResult(true)).Verifiable();
        var entity = new TranslationJob { CustomerName = "DPD", Id = 6, OriginalContent = "Bla bla", Price = 3.6, Status = TranslationJobsBll.JobStatus.New };

        //Act
        var res = await bll.NotifyOnNewJobAsync(entity);

        //Assert
        Assert.That(res, Is.True);
        notifSvc.Verify(s => s.SendNotification(It.IsAny<string>()), Times.Exactly(1));
    }

    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    [TestCase(6)]
    public async Task NotifyOnNewJob_EventualSuccess(int when)
    {
        //Arrange
        var bll = new TranslationJobsBll(repo.Object, transRepo.Object, notifSvc.Object, Mapper, readerFact.Object, bllLogger);
        bll.ExponentialBackOffInitialDelay = 1;
        int i = 0;
        notifSvc.Setup(s => s.SendNotification(It.IsAny<string>())).Callback((string message) =>
        {
            if (++i < when && i % 2 == 0) throw new ApplicationException();
        }).Returns((string message) => Task.FromResult(i >= when)).Verifiable();
        var entity = new TranslationJob { CustomerName = "DPD", Id = 6, OriginalContent = "Bla bla", Price = 3.6, Status = TranslationJobsBll.JobStatus.New };

        //Act
        var res = await bll.NotifyOnNewJobAsync(entity);

        //Assert
        Assert.That(res, Is.EqualTo(when <= 5));
        notifSvc.Verify(s => s.SendNotification(It.IsAny<string>()), Times.Exactly(Math.Min(when, 5)));
    }
}
